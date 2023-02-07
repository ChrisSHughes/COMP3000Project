using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationController : MonoBehaviour
{
    public enum Controller
    {
        RightHand,
        LeftHand
    }

    public Controller targetController;

    static private bool teleportIsActive = false; // this bool is used so both controllers can not be actively teleporting at one time

    public InputActionAsset inputAction;  // controller input actions
    public XRRayInteractor rayInteractor;  // the ray interactor, we can disable the ray and enable it later
    public TeleportationProvider teleportProvider;  // the provider, usually on the floor gameobject so we can teleport the player

    private InputAction thumbstick;  // grabs the thumbstick input action on startup
    private InputAction teleportActivate;  // if the teleport is active
    private InputAction teleportCancel;  // if the teleport has been cancelled


    // Start is called before the first frame update
    void Start()
    {
        rayInteractor.enabled = false; // only activates on button press

        // these below, will find the action map for the controller being used for teleport active/cancel

        teleportActivate = inputAction.FindActionMap("XRI " + targetController.ToString() + " Locomotion").FindAction("Teleport Mode Activate"); // finds the input action for activating teleportation
        teleportActivate.Enable();
        teleportActivate.performed += OnTeleportActivate;  // once this action has been performed, run the method for activating teleportation

        teleportCancel = inputAction.FindActionMap("XRI " + targetController.ToString() + " Locomotion").FindAction("Teleport Mode Cancel");
        teleportCancel.Enable();
        teleportCancel.performed += OnTeleportCancel; // same as above, but for cancel

        thumbstick = inputAction.FindActionMap("XRI " + targetController.ToString() + " Locomotion").FindAction("Move");
        
    }

    private void OnDestroy()
    {
        teleportActivate.performed -= OnTeleportActivate;
        teleportCancel.performed -= OnTeleportCancel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!teleportIsActive) // if the teleport is NOT active then return, do not continue the update
        {
            return;
        }

        if (!rayInteractor.enabled)  // if the ray is NOT enabled just return, do not continue the update
        {
            return;
        }

        if (thumbstick.triggered) // if the thumbstick is moved
        {
            return;
        }

        if(!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) // if the ray isn't hitting anything, don't continue to render the ray
        {
            
            rayInteractor.enabled = false;
            teleportIsActive = false;
            return;
        }

        // if all the above conditions pass, we can move on with teleporting the player

        TeleportRequest teleportReq = new TeleportRequest()
        {
            
            destinationPosition = hit.point,
        };

        teleportProvider.QueueTeleportRequest(teleportReq);

        rayInteractor.enabled = false;
        teleportIsActive = false;

    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        if (!teleportIsActive)
        {
            rayInteractor.enabled = true;
            teleportIsActive = true;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if(teleportIsActive && rayInteractor.enabled == true)
        {
            rayInteractor.enabled = false;
            teleportIsActive = false;
        }
    }
}
