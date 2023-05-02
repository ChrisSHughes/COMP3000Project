using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerUnitController : MonoBehaviour
{
    public enum Controller
    {
        RightHand
    }

    public Controller targetController;

    public Grid grid;
    public XRRayInteractor rayInteractor;
    public InputActionAsset inputAction;
    public GameController gameController;

    public List<GameObject> SelectedUnitsList = new List<GameObject>();
    public bool UnitsSelected = false;

    public GameObject SelectedBuilding;
    public bool BuildingSelected;

    private void OnEnable()
    {
        Debug.Log("enabling Unit controller");
        gameController.trigger.performed += onSelect;
        gameController.cancel.performed += Back;
        gameController.grip.performed += Grab;
    }

    private void OnDisable()
    {
        Debug.Log("disabling Unit controller");
        gameController.trigger.performed -= onSelect;
        gameController.cancel.performed -= Back;
        gameController.grip.performed -= Grab;
    }

    // Update is called once per frame
    void Update()
    {
        OnHoverCall();
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (UnitsSelected)
        {
            UnitsSelected = false;
            SelectedUnitsList.Clear();
            Debug.Log("Cleared Unit List");
            return;
        }

        if (BuildingSelected)
        {
            BuildingSelected = false;
            SelectedBuilding = null;
            Debug.Log("Cleared Building List");
        }
    }

    public void OnHoverCall()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("Structure")) || (hit.collider.gameObject.layer == LayerMask.NameToLayer("Unit")))
            {
                //Debug.Log("hovered over: " + hit.collider.gameObject.name);
            }
        }
    }

    /// <summary>
    /// these were originally on layers, and i was checking the layers to see what the player can interact with, which i thought was wrong. unity has a system for this in the background
    /// i need to be checking tags because the player will be interacting with specific objects.
    /// </summary>



    void onSelect(InputAction.CallbackContext context)
    {
        if (UnitsSelected)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitGround))
            {
                if (hitGround.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    for (int i = 0; i < SelectedUnitsList.Count; i++)
                    {
                        Vector3 movePoint = grid.GetNearestPointOnGrid(hitGround.point);
                        TankController tankController = GetComponent<TankController>();
                        tankController.SetDestination(movePoint);
                    }
                }
                if (hitGround.collider.gameObject.tag == "RedUnit")
                {
                    GameObject targetTank = hitGround.collider.gameObject;
                    for (int i = 0; i < SelectedUnitsList.Count; i++)
                    {
                        TankController tankController = GetComponent<TankController>();
                        if(Vector3.Distance(tankController.gameObject.transform.position,targetTank.transform.position) <= tankController.rangeFinder.radius)
                        {
                            tankController.target = targetTank;
                            tankController.moveTowards = false;
                            tankController.CanShoot = true;
                            StartCoroutine(tankController.ShootProjectile(targetTank.transform, tankController.BulletSpawn));
                        }
                        else
                        {
                            tankController.target = targetTank;
                            tankController.moveTowards = true;
                            tankController.isChasing = true;
                            tankController.CanShoot = false;
                        }
                    }
                }
            }
        }
    }

    void Grab(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "BlueUnit")
            {
                SelectedUnitsList.Clear();
                SelectedUnitsList.Add(raycastHit.collider.gameObject);
                UnitsSelected = true;
                Debug.Log("Unit Selected");
                return;
            }
        }
    }

}
