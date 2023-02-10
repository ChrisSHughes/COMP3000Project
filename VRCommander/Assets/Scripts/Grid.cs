using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float size = 0;
    public int cells = 0;
    public List<GameObject> coOrds = new List<GameObject>();

    public Dictionary<Vector3, GameObject> dictCoords = new Dictionary<Vector3, GameObject>();

    

    public void Start()
    {
        size = GetComponent<Grid>().size;
        DrawGridNodes();
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)xCount * size, (float)yCount * size, (float)zCount * size);

        result += transform.position;
        return result ;
    }

    public void DrawGridNodes()
    {

        for (float x = 0; x < 100; x += size)
        {
            //Debug.Log("x = " + x);
            for (float z = 0; z < 100; z += size)
            {
                Vector3 coord = new Vector3(x, 0, z);
                dictCoords.Add(coord, null);
                cells++;
            }
        }
    }

    private void OnDrawGizmos()
    {
    //    Gizmos.color = Color.magenta;
    //    for (float x = 0; x < 100; x += size)
    //    {
    //        for (float z = 0; z < 100; z += size)
    //        {
    //            var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
    //            Gizmos.DrawSphere(point, 0.25f);
    //        }
    //    }
    }
}
