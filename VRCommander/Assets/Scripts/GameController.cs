using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    public PlayerUnitController unitController;
    public PlayerBuildController buildContrller;


    public enum Controller
    {
        RightHand, LeftHand
    }

    public Controller targetController;

    public InputActionAsset inputAction;
    public InputAction trigger;
    public InputAction cancel;
    public InputAction grip;

    private void Awake()
    {
        Debug.Log("GameController: Mapping buttons");

        cancel = inputAction.FindActionMap("XRI UI").FindAction("Cancel");
        trigger = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Activate");
        grip = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Select");
        
        Debug.Log("GameController: Enabling buttons");

        cancel.Enable();
        trigger.Enable();
        grip.Enable();

        unitController.enabled = true;

    }
}
