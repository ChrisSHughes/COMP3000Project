using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private InputAction select;

    public List<GameObject> selectedUnits = new List<GameObject>();
    public bool SelectedUnits = false;

    void OnEnable()
    {
        Debug.Log("Unit Controller enabled");
        select = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Activate");
        select.Enable();
        select.performed += onSelect;
    }

    // Update is called once per frame
    void Update()
    {
        OnHoverCall();
    }


    public void OnHoverCall()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("Structure")) || (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlueUnit")))
            {
                Debug.Log("hovered over: " + hit.collider.gameObject.name);
            }
        }
    }



    void onSelect(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("BlueUnit"))
            {
                selectedUnits.Add(hit.collider.gameObject);
            }

            if (SelectedUnits == false)
            {
                //if unit, select unit
                //if building, show
            }
            else if (SelectedUnits == true)
            {
                //if unit, or building, remove array elements, select new thing

                //if new target is the floor, move them

                //if new target is enemy, attack.
            }
        }

    }
}
