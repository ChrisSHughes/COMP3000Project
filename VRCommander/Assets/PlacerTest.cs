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
    public GameObject selectedBuilding;
    public GameObject ghostedSelectedBuilding;

    private InputAction selectCell;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();

    }
    // Start is called before the first frame update
    void Start()
    {
        ghostedSelectedBuilding = selectedBuilding;

        selectCell = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Activate");
        selectCell.Enable();
        selectCell.performed += OnSelectCell;
    }

    // Update is called once per frame
    private void Update()
    {
        OnHoverCell();
    }

    public void OnHoverCell()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            ShowGhost(hit.point);
        }
        else
        {
            ghostedSelectedBuilding.SetActive(false);
        }
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
        GameObject.Instantiate(selectedBuilding).transform.position = finalPosition;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
    }

    private void ShowGhost(Vector3 hitpoint)
    {
        ghostedSelectedBuilding.SetActive(true);
        var finalPosition = grid.GetNearestPointOnGrid(hitpoint);
        ghostedSelectedBuilding.transform.position = finalPosition;
    }
}
