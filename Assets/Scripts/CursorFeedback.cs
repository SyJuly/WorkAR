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

    [SerializeField]
    GameObject cameraFeedback;

    [SerializeField]
    GameObject sortColorFeedback;

    private HoloToolkit.Unity.InputModule.Cursor cursor;

    private GameObject activeFeedback;

    private Renderer sortColorRenderer;

    private bool cameraFeedbackActivated = false;

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
        sortColorRenderer = sortColorFeedback.GetComponent<Renderer>();
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
            default: ChangeFeedback(null); break;
        }
    }

    public void ToggleCameraModeFeedback(bool cameraOn)
    {
        if (cameraOn)
        {
            ChangeFeedback(cameraFeedback);
            cameraFeedbackActivated = true;
        } else if(!cameraOn)
        {
            cameraFeedbackActivated = false;
            ChangeFeedback(null);
        }
        
    }

    public void ToggleSortModeFeedback(Material sortColor)
    {
        if (sortColor != null)
        {
            ChangeFeedback(sortColorFeedback);
            sortColorRenderer.material = sortColor;
        } else
        {
            ChangeFeedback(null);
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

    private void ChangeFeedback(GameObject feedbackGO)
    {
        if (!cameraFeedbackActivated)
        {
            ActivateFeedback(feedbackGO);
        }
    }
}
