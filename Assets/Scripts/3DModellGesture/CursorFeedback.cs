// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Academy
{
    /// <summary>
    /// CursorFeedback class takes GameObjects to give cursor feedback
    /// to users based on different states.
    /// </summary>
    public class CursorFeedback : MonoBehaviour
    {
        [Tooltip("Drag the GameObject to display when a scroll enabled Interactible is detected.")]
        [SerializeField]
        private GameObject scrollDetectedGameObject;

        [Tooltip("Drag the GameObject to display when a pathing enabled Interactible is detected.")]
        [SerializeField]
        private GameObject pathingDetectedGameObject;

        private HoloToolkit.Unity.InputModule.Cursor cursor;

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
            cursor = GetComponent<HoloToolkit.Unity.InputModule.Cursor>();
        }
        /*<<------------------Singleton-----------------------*/

        public GameObject targetedGestureActionObj;

        private bool IsNavigationFocused
        {
            get
            {
                targetedGestureActionObj = cursor.GetTargetedObject();
                if (targetedGestureActionObj != null)
                {
                    GestureAction gestureAction = targetedGestureActionObj.GetComponent<GestureAction>();
                    if (gestureAction != null)
                    {
                        return gestureAction.IsNavigationEnabled;
                    }
                    else
                    {
                        gestureAction = targetedGestureActionObj.transform.root.GetComponent<GestureAction>();
                        if (gestureAction != null)
                        {
                            return gestureAction.IsNavigationEnabled;
                        }
                    }
                }

                return false;
            }
        }

        private bool IsManipulationFocused
        {
            get
            {
                targetedGestureActionObj = cursor.GetTargetedObject();
                if (targetedGestureActionObj != null)
                {
                    GestureAction gestureAction = targetedGestureActionObj.GetComponent<GestureAction>();
                    if (gestureAction != null)
                    {
                        return !gestureAction.IsNavigationEnabled;
                    }
                    else
                    {
                        gestureAction = targetedGestureActionObj.transform.root.GetComponent<GestureAction>();
                        if (gestureAction != null)
                        {
                            return !gestureAction.IsNavigationEnabled;
                        }
                    }
                }

                return false;
            }
        }

        private void Update()
        {
            UpdatePathDetectedState();

            UpdateScrollDetectedState();
        }

        private void UpdatePathDetectedState()
        {
            if (pathingDetectedGameObject == null)
            {
                return;
            }

            if (!IsManipulationFocused)
            {
                pathingDetectedGameObject.SetActive(false);
                return;
            }

            pathingDetectedGameObject.SetActive(true);
        }

        private void UpdateScrollDetectedState()
        {
            if (scrollDetectedGameObject == null)
            {
                return;
            }

            if (!IsNavigationFocused)
            {
                scrollDetectedGameObject.SetActive(false);
                return;
            }

            scrollDetectedGameObject.SetActive(true);
        }
    }
}