using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    public UnitController unitController;
    public BuildController buildContrller;


    public enum Controller
    {
        RightHand, LeftHand
    }

    public Controller targetController;

    public InputActionAsset inputAction;
    public InputAction trigger;
    public InputAction cancel;

    private void Awake()
    {
        Debug.Log("Mapping buttons");

        cancel = inputAction.FindActionMap("XRI UI").FindAction("Cancel");
        trigger = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Activate");
        
        Debug.Log("Enabling buttons");

        cancel.Enable();
        trigger.Enable();

        unitController.enabled = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
