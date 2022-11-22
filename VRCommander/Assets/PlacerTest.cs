using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlacerTest : MonoBehaviour
{
    public enum Controller
    {
        RightHand
    }

    public Controller targetController;

    public Grid grid;
    public XRRayInteractor rayInteractor;
    public InputActionAsset inputAction;

    private InputAction selectCell;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();

    }
    // Start is called before the first frame update
    void Start()
    {
        selectCell = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Activate");
        selectCell.Enable();
        selectCell.performed += OnSelectCell;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void OnSelectCell(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            PlaceCubeNear(hit.point);
        }
    }

    private void PlaceCubeNear(Vector3 hitPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(hitPoint);
        GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
    }
}
