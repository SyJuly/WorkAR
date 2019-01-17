using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCube : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;

    void Update()
    {
        transform.Rotate(Vector3.down * Time.deltaTime * speed, Space.World);
    }
}
