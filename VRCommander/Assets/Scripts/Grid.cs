using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float size = 2f;
    public GameObject node;
    //public List<GameObject> coOrds = new List<GameObject>();

    public Dictionary<Vector2, bool> dictCoords = new Dictionary<Vector2, bool>();

    public (int, int, bool) TupleCoords = (0, 0, false);
    

    public void Start()
    {
        DrawGridNodes();
        foreach(KeyValuePair<Vector2, bool> pair in dictCoords)
        {
            Debug.Log("coord: " + pair.Key + " taken? = " + pair.Value);
        }
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

        for (float x = 0; x < 200; x += size)
        {
            //Debug.Log("x = " + x);
            for (float z = 0; z < 200; z += size)
            {
                //Debug.Log("z = " + z);
                //node = Instantiate(node,new Vector3(x + 1, 0f, z + 1), Quaternion.identity);
                //node.name = "x: " + x + ", z: " + z;
                //coOrds.Add(node);
                //node.transform.SetParent(transform);

                Vector2 coord = new Vector2(x, z);
                dictCoords.Add(coord, false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        //for(float x = 0; x < 200; x += size)
        //{
        //    for(float z = 0; z < 200; z += size)
        //    {
        //        var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
        //        Gizmos.DrawSphere(point, 0.5f);
        //    }
        //}
    }
}
