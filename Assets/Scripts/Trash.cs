using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("destroyed: " + other.gameObject);
        Placeable placeable = other.GetComponentInParent<Placeable>();
        if (placeable)
        {
            Destroy(placeable);
            Destroy(placeable.gameObject);
        } else
        {
            Destroy(other.gameObject);
        }
        
    }

}
