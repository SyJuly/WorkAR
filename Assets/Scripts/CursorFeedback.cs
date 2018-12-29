using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFeedback : MonoBehaviour {

    [SerializeField]
    GameObject moveFeedback;

    [SerializeField]
    GameObject scaleFeedback;

    [SerializeField]
    GameObject rotateFeedback;

    private HoloToolkit.Unity.InputModule.Cursor cursor;

    private GameObject activeFeedback;

    /*------------------Singleton---------------------->>*/
    private static CursorFeedback _instance;

    public static CursorFeedback Instance { get { return _instance; } }


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

    private void Start()
    {
        activeFeedback = scaleFeedback;
        cursor = InputManager.Instance.gameObject.GetComponent<SimpleSinglePointerSelector>().Cursor;
    }

    private void Update()
    {
        transform.position = cursor.transform.position;
        transform.rotation = cursor.transform.rotation;
    }

    public void ActivateManipulationModeFeedback(ManipulationMode mode)
    {
        switch (mode){
            case ManipulationMode.Scale: ActivateFeedback(scaleFeedback); break;
            case ManipulationMode.Rotate: ActivateFeedback(rotateFeedback); break;
            case ManipulationMode.Move: ActivateFeedback(moveFeedback); break;
            default: ActivateFeedback(null); break;
        }
    }

    private void ActivateFeedback(GameObject feedbackGO)
    {
        activeFeedback.SetActive(false);
        if (feedbackGO != null)
        {
            feedbackGO.SetActive(true);
            activeFeedback = feedbackGO;
        }
    }
}
