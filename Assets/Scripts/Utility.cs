using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    /*------------------Singleton---------------------->>*/
    private static Utility _instance;

    public static Utility Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    /*<<------------------Singleton-----------------------*/
}
