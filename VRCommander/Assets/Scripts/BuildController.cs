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

    public MeshRenderer[] ghostedSelectedBuildingMesh;

    public Material invalid;
    public Material valid;

    bool GhostActive = false;
    public bool isBuilding = false;
    bool buildable = false;

    private InputAction selectCell;
    public StructureDatabase structureDatabase;

    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("Building Controller enabled");
        selectCell = inputAction.FindActionMap("XRI " + targetController.ToString() + " Interaction").FindAction("Activate");
        selectCell.Enable();
        selectCell.performed += OnSelectCell;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isBuilding == true)
        {
            OnHoverCell();
        }
    }

    public void OnHoverCell()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("hitting ground");
                Vector3 finalPosition = grid.GetNearestPointOnGrid(hit.point);
                ghostedSelectedBuilding.transform.position = finalPosition;

                if(CheckValid(finalPosition) == true)
                {
                    Debug.Log("Valid placement");
                    ShowGhost(hit.point);
                    buildable = true;
                }
                else
                {
                    Debug.Log("Not Valid 1");
                    buildable = false;
                    ShowGhost(hit.point);
                }
            }
        }
        else
        {
            Debug.Log("Not Valid 2");
            buildable = false;
            ShowGhost(hit.point);
        }
    }

    public void OnSelectCell(InputAction.CallbackContext context)
    {
        if ((buildable == true) && (isBuilding == true))
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    PlaceCubeNear(hit.point);
                    isBuilding = false;
                    Destroy(ghostedSelectedBuilding);
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

                    // debugging for spawning a cube in each used tile.
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = tile;
                    //cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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

        for (int x = 0; x < structCon.sizex; x++)
        {
            for (int z = 0; z < structCon.sizez; z++)
            {
                bool coordsFound = false;

                Vector3 tile = new Vector3(finalPos.x + x, finalPos.y, finalPos.z - z);

                foreach (KeyValuePair<Vector3, GameObject> pair in grid.dictCoords)
                {
                    if (pair.Key.Equals(tile))
                    {
                        coordsFound = true;

                        if (pair.Value != null)
                        {
                            valid = false;
                            return valid;
                        }
                    }
                }
                if (!coordsFound)
                {
                    valid = false;
                    return valid;
                }
            }
        }

        return valid;
    }

    private void ShowGhost(Vector3 hitpoint)
    {
        // shows the ghost if it is possible to build
        if(buildable == true)
        {
            for (int i = 0; i < ghostedSelectedBuildingMesh.Length; i++)
            {
                ghostedSelectedBuildingMesh[i].material = valid;
            }
        }
        else
        {
            for (int i = 0; i < ghostedSelectedBuildingMesh.Length; i++)
            {
                ghostedSelectedBuildingMesh[i].material = invalid;
            }
        }

    }

    public void SetBuilding(int building)
    {
        isBuilding = true;
        if(ghostedSelectedBuilding != null)
        {
            Destroy(ghostedSelectedBuilding);
        }
        selectedBuilding = structureDatabase.blueStructures[building].building;
        ghostedSelectedBuilding = Instantiate(structureDatabase.ghostBlueStructures[building].building);
        ghostedSelectedBuilding.transform.position = new Vector3(50, -8, 50);
        ghostedSelectedBuildingMesh = ghostedSelectedBuilding.GetComponentsInChildren<MeshRenderer>();
    }
}
