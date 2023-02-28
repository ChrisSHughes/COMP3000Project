using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UnitController : MonoBehaviour
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
    public bool SelectedUnitsBool = false;

    public GameObject SelectedBuilding;
    public bool SelectedBuildingBool;

    private void OnEnable()
    {
        Debug.Log("enabling Unit controller");
        gameController.trigger.performed += onSelect;
        gameController.cancel.performed += Back;
    }

    private void OnDisable()
    {
        Debug.Log("disabling Unit controller");
        gameController.trigger.performed -= onSelect;
        gameController.cancel.performed -= Back;
    }

    // Update is called once per frame
    void Update()
    {
        OnHoverCall();
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (SelectedUnitsBool)
        {
            SelectedUnitsBool = false;
            SelectedUnitsList.Clear();
            Debug.Log("Cleared Unit List");
        }

        if (SelectedBuildingBool)
        {
            SelectedBuildingBool = false;
            SelectedBuilding = null;
            Debug.Log("Cleared Building List");
        }
    }

    public void OnHoverCall()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("Structure")) || (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlueUnit")))
            {
                //Debug.Log("hovered over: " + hit.collider.gameObject.name);
            }
        }
    }



    void onSelect(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitUnit))
        {
            if (hitUnit.collider.gameObject.layer == LayerMask.NameToLayer("BlueUnit"))
            {
                SelectedUnitsList.Clear();
                SelectedUnitsList.Add(hitUnit.collider.gameObject);
                SelectedUnitsBool = true;
                Debug.Log("Unit Selected");
                return;
            }
        }

        if (SelectedUnitsBool)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitGround))
            {
                if (hitGround.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    for (int i = 0; i < SelectedUnitsList.Count; i++)
                    {
                        NavMeshAgent agent = SelectedUnitsList[i].GetComponent<NavMeshAgent>();
                        Vector3 movePoint = grid.GetNearestPointOnGrid(hitGround.point);
                        agent.Move(movePoint);
                    }
                }
            }
        }

    }
}
