using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Structure")
        {
            Debug.Log(other.gameObject.name + " has overlapped");
        }
    }
}
