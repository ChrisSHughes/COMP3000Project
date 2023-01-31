using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildController : MonoBehaviour
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
    Vector3 lastPos;

    bool GhostActive = false;
    bool isBuilding = false;
    bool buildable = false;

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
        ghostedSelectedBuilding = Instantiate(ghostedSelectedBuilding);
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
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Vector3 finalPosition = grid.GetNearestPointOnGrid(hit.point);
                ghostedSelectedBuilding.transform.position = finalPosition;

                if(CheckValid(finalPosition) == true)
                {
                    ShowGhost(hit.point);
                    buildable = true;
                }
                else
                {
                    HideGhost();
                    buildable = false;
                }
            }
        }
        else
        {
            HideGhost();
            buildable = false;
        }
    }

    public void OnSelectCell(InputAction.CallbackContext context)
    {
        if (buildable == true)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    PlaceCubeNear(hit.point);
                }
            }
        }
    }

    private void PlaceCubeNear(Vector3 hitPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(hitPoint);

        if (selectedBuilding.GetComponent<StructureController>() != null)
        {
            Debug.Log("SCRIPT EXISTS");
            GameObject currentBuilding;
            currentBuilding = Instantiate(selectedBuilding);
            currentBuilding.transform.position = finalPosition;
            StructureController structCon = selectedBuilding.GetComponent<StructureController>();

            structCon.origin = finalPosition;
            currentBuilding.SetActive(true);

            grid.dictCoords[finalPosition] = currentBuilding;
            Vector3 tile;
            for (int x = 0; x < structCon.sizex; x++)
            {
                for (int z = 0; z < structCon.sizez; z++)
                {
                    tile = new Vector3(finalPosition.x + x, finalPosition.y, finalPosition.z - z);
                    Debug.Log("tile " + tile);
                    grid.dictCoords[tile] = currentBuilding;

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = tile;
                    cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }

            foreach (KeyValuePair<Vector3, GameObject> pair in grid.dictCoords) // pair is variable for both parts of dict. you cna use pair.key and pair.value to change things respectively.
            {
                if (pair.Value != null)
                {
                    Debug.Log("coord: " + pair.Key + " taken? = " + pair.Value);

                }
            }

        }
    }


    public bool CheckValid(Vector3 finalPos)
    {
        StructureController structCon = selectedBuilding.GetComponent<StructureController>();
        bool valid = true;
        foreach(KeyValuePair<Vector3, GameObject> pair in grid.dictCoords)
        {
            if (pair.Key.Equals(finalPos))
            {
                if(pair.Value != null)
                {
                    valid = false;
                    return valid;
                }
            }
        }

        for (int x = 0; x < structCon.sizex; x++)
        {
            for (int z = 0; z < structCon.sizez; z++)
            {
                Vector3 tile = new Vector3(finalPos.x + x, finalPos.y, finalPos.z - z);

                foreach (KeyValuePair<Vector3, GameObject> pair in grid.dictCoords)
                {
                    if (pair.Key.Equals(tile))
                    {
                        if (pair.Value != null)
                        {
                            valid = false;
                            return valid;
                        }
                        else
                        {
                            valid = false;
                            return valid;
                        }
                    }
                }
            }
        }

        return valid;
    }

    private void ShowGhost(Vector3 hitpoint)
    {
        //Debug.Log("hit a thing with hover");
        ghostedSelectedBuilding.SetActive(true);

        // check dictionary for current vec location - if it has a building, no buildy.
    }

    private void HideGhost()
    {
        ghostedSelectedBuilding.SetActive(false);
    }
}
