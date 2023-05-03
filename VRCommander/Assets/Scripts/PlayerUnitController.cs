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
        Debug.Log("Back pressed");
        if (UnitsSelected)
        {
            SelectedUnitsList[0].GetComponent<UnitHealthController>().ShowUI(false);
            SelectedUnitsList.Clear();
            UnitsSelected = false;
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
        Debug.Log("Trigger Pressed");

        if (UnitsSelected)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitGround))
            {
                Debug.Log("RayCast Hit");
                if (hitGround.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Debug.Log("Ground Hit");
                    for (int i = 0; i < SelectedUnitsList.Count; i++)
                    {
                        Vector3 movePoint = grid.GetNearestPointOnGrid(hitGround.point);
                        TankController tankController = SelectedUnitsList[i].GetComponent<TankController>();
                        tankController.SetDestination(movePoint);
                    }
                    return;
                }
                if (hitGround.collider.gameObject.tag == "RedUnit")
                {
                    GameObject targetTank = hitGround.collider.gameObject;
                    TankController targetTankController = hitGround.collider.gameObject.GetComponent<TankController>();
                    for (int i = 0; i < SelectedUnitsList.Count; i++)
                    {
                        TankController tankController = SelectedUnitsList[i].GetComponent<TankController>();
                        if (Vector3.Distance(tankController.gameObject.transform.position, targetTank.transform.position) <= tankController.rangeFinder.radius)
                        {
                            tankController.target = targetTank;
                            tankController.moveTowards = false;
                            tankController.CanShoot = true;
                            StartCoroutine(tankController.ShootProjectile(targetTankController.HitPoint, tankController.BulletSpawn));
                        }
                        else
                        {
                            tankController.target = targetTank;
                            tankController.moveTowards = true;
                            tankController.isChasing = true;
                            tankController.CanShoot = false;
                        }
                    }
                    return;
                }
                if (hitGround.collider.gameObject.tag == "RedStructure")
                {
                    GameObject targetStructure = hitGround.collider.gameObject;
                    StructureController targetStructureController = hitGround.collider.gameObject.GetComponent<StructureController>();

                    for (int i = 0; i < SelectedUnitsList.Count; i++)
                    {
                        TankController tankController = SelectedUnitsList[i].GetComponent<TankController>();
                        if (Vector3.Distance(tankController.gameObject.transform.position, targetStructure.transform.position) <= tankController.rangeFinder.radius)
                        {
                            tankController.target = targetStructure;
                            tankController.moveTowards = false;
                            tankController.CanShoot = true;
                            StartCoroutine(tankController.ShootProjectile(targetStructureController.HitPoint, tankController.BulletSpawn));
                        }
                        else
                        {
                            tankController.target = targetStructure;
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
        Debug.Log("Grab Pressed");
        if (UnitsSelected == true)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit HitCheck1))
            {
                if (HitCheck1.collider.gameObject.tag == "Ground")
                {
                    SelectedUnitsList[0].GetComponent<UnitHealthController>().ShowUI(false);
                    SelectedUnitsList.Clear();
                    return;
                }

                if (HitCheck1.collider.gameObject.tag == "BlueUnit")
                {
                    if(SelectedUnitsList != null)
                    {
                        SelectedUnitsList[0].GetComponent<UnitHealthController>().ShowUI(false);
                    }

                    SelectedUnitsList.Clear();
                    SelectedUnitsList.Add(HitCheck1.collider.gameObject);
                    UnitsSelected = true;
                    SelectedUnitsList[0].GetComponent<UnitHealthController>().ShowUI(true);
                    return;
                }
            }
        }

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit HitCheck2))
        {
            if (HitCheck2.collider.gameObject.tag == "BlueUnit")
            {
                SelectedUnitsList.Clear();
                SelectedUnitsList.Add(HitCheck2.collider.gameObject);
                UnitsSelected = true;
                SelectedUnitsList[0].GetComponent<UnitHealthController>().ShowUI(true);
                Debug.Log("Unit Selected");
                return;
            }
        }
    }
}
