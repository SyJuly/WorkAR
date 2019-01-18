using HoloToolkit.Unity.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        Placeable placeable = other.GetComponentInParent<Placeable>();
        InteractionModel model = other.GetComponentInParent<InteractionModel>();
        if (placeable)
        {
            Debug.Log("destroyed: " + placeable.gameObject);
            Destroy(placeable);
            Destroy(placeable.gameObject);
        } else if (model)
        {
            Debug.Log("destroyed: " + model.gameObject);
            Destroy(model.transform.parent.gameObject);
            Destroy(GameObject.FindObjectOfType<BoundingBox>().gameObject);
        }
        
    }

}
