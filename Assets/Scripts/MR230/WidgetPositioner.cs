using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine.XR.WSA;

/// <summary>
/// Called by PlaySpaceManager after planes have been generated from the Spatial Mapping Mesh.
/// This class will create a collection of prefab objects that have the 'Placeable' component and
/// will attempt to set their initial location on planes that are close to the user.
/// </summary>
public class WidgetPositioner : Singleton<WidgetPositioner>
{
    [Tooltip("A collection of Placeable space object prefabs to generate in the world.")]
    public List<GameObject> widgetPrefabs;

    int test = 0;

    /// <summary>
    /// Generates a collection of Placeable objects in the world and sets them on planes that match their affinity.
    /// </summary>
    /// <param name="horizontalSurfaces">Horizontal surface planes (floors, tables).</param>
    /// <param name="verticalSurfaces">Vertical surface planes (walls).</param>
    public void GenerateItemsInWorld(List<GameObject> horizontalSurfaces, List<GameObject> verticalSurfaces)
    {
        Debug.Log("GenerateItemsInWorld: " + test);
        test++;
        List<GameObject> horizontalObjects = new List<GameObject>();
        List<GameObject> verticalObjects = new List<GameObject>();

        foreach (GameObject widgetPrefab in widgetPrefabs)
        {
            Placeable placeable = widgetPrefab.GetComponent<Placeable>();
            if (placeable.PlacementSurface == PlacementSurfaces.Horizontal)
            {
                horizontalObjects.Add(widgetPrefab);
            }
            else
            {
                verticalObjects.Add(widgetPrefab);
            }
        }

        if (horizontalObjects.Count > 0)
        {
            CreateWidgets(horizontalObjects, horizontalSurfaces, PlacementSurfaces.Horizontal);
        }

        if (verticalObjects.Count > 0)
        {
            CreateWidgets(verticalObjects, verticalSurfaces, PlacementSurfaces.Vertical);
        }
    }

    /// <summary>
    /// Creates and positions a collection of Placeable space objects on SurfacePlanes in the environment.
    /// </summary>
    /// <param name="spaceObjects">Collection of prefab GameObjects that have the Placeable component.</param>
    /// <param name="surfaces">Collection of SurfacePlane objects in the world.</param>
    /// <param name="surfaceType">Type of objects and planes that we are trying to match-up.</param>
    private void CreateWidgets(List<GameObject> widgets, List<GameObject> surfaces, PlacementSurfaces surfaceType)
    {
        List<GameObject> widgetsGOs = new List<GameObject>();
        foreach (GameObject widget in widgets)
        {
            GameObject widgetGO = Instantiate(widget);
            widgetsGOs.Add(widgetGO);
        }
        InstantiateWidgetsWithCalculatedPlacement(widgetsGOs, surfaces, surfaceType);
    }


    private void InstantiateWidgetsWithCalculatedPlacement(List<GameObject> widgetsGOs, List<GameObject> surfaces, PlacementSurfaces surfaceType)
    {
        List<int> UsedPlanes = new List<int>();

        // Sort the planes by distance to user.
        surfaces.Sort((lhs, rhs) =>
        {
            Vector3 headPosition = Camera.main.transform.position;
            Collider rightCollider = rhs.GetComponent<Collider>();
            Collider leftCollider = lhs.GetComponent<Collider>();

            // This plane is big enough, now we will evaluate how far the plane is from the user's head.  
            // Since planes can be quite large, we should find the closest point on the plane's bounds to the 
            // user's head, rather than just taking the plane's center position.
            Vector3 rightSpot = rightCollider.ClosestPointOnBounds(headPosition);
            Vector3 leftSpot = leftCollider.ClosestPointOnBounds(headPosition);

            return Vector3.Distance(leftSpot, headPosition).CompareTo(Vector3.Distance(rightSpot, headPosition));
        });

        List<GameObject> widgetsToPlace = new List<GameObject>();

        foreach (GameObject existingWidget in widgetsGOs)
        {
            if (existingWidget.GetComponent<WorldAnchor>() != null)
            {
                widgetsToPlace.Add(existingWidget);
            } else
            {
                existingWidget.transform.parent = transform;
            }
        }


        foreach (GameObject item in widgetsToPlace)
        {
            int index = -1;
            Collider collider = item.GetComponent<Collider>();

            if (surfaceType == PlacementSurfaces.Vertical)
            {
                index = FindNearestPlane(surfaces, collider.bounds.size, UsedPlanes, true);
            }
            else
            {
                index = FindNearestPlane(surfaces, collider.bounds.size, UsedPlanes, false);
            }

            // If we can't find a good plane we will put the object floating in space.
            Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 2.0f + Camera.main.transform.right * (Random.value - 1.0f) * 2.0f;
            Quaternion rotation = Quaternion.identity;

            // If we do find a good plane we can do something smarter.
            if (index >= 0)
            {
                Debug.Log("Place widget at wall: " + item.name);
                UsedPlanes.Add(index);
                GameObject surface = surfaces[index];
                SurfacePlane plane = surface.GetComponent<SurfacePlane>();
                position = surface.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
                position = AdjustPositionWithSpatialMap(position, plane.SurfaceNormal);
                rotation = Camera.main.transform.localRotation;

                if (surfaceType == PlacementSurfaces.Vertical)
                {
                    // Vertical objects should face out from the wall.
                    rotation = Quaternion.LookRotation(surface.transform.forward, Vector3.up);
                }
                else
                {
                    // Horizontal objects should face the user.
                    rotation = Quaternion.LookRotation(Camera.main.transform.position);
                    rotation.x = 0f;
                    rotation.z = 0f;
                }
            }

            //Vector3 finalPosition = AdjustPositionWithSpatialMap(position, surfaceType);
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.transform.parent = transform;
        }
    }

    /// <summary>
    /// Attempts to find a the closest plane to the user which is large enough to fit the object.
    /// </summary>
    /// <param name="planes">List of planes to consider for object placement.</param>
    /// <param name="minSize">Minimum size that the plane is required to be.</param>
    /// <param name="startIndex">Index in the planes collection that we want to start at (to help avoid double-placement of objects).</param>
    /// <param name="isVertical">True, if we are currently evaluating vertical surfaces.</param>
    /// <returns></returns>
    private int FindNearestPlane(List<GameObject> planes, Vector3 minSize, List<int> usedPlanes, bool isVertical)
    {
        int planeIndex = -1;

        for (int i = 0; i < planes.Count; i++)
        {
            if (usedPlanes.Contains(i))
            {
                continue;
            }

            Collider collider = planes[i].GetComponent<Collider>();
            if (isVertical && (collider.bounds.size.x < minSize.x || collider.bounds.size.y < minSize.y))
            {
                // This plane is too small to fit our vertical object.
                continue;
            }
            else if (!isVertical && (collider.bounds.size.x < minSize.x || collider.bounds.size.y < minSize.y))
            {
                // This plane is too small to fit our horizontal object.
                continue;
            }

            return i;
        }

        return planeIndex;
    }

    /// <summary>
    /// Adjusts the initial position of the object if it is being occluded by the spatial map.
    /// </summary>
    /// <param name="position">Position of object to adjust.</param>
    /// <param name="surfaceNormal">Normal of surface that the object is positioned against.</param>
    /// <returns></returns>
    private Vector3 AdjustPositionWithSpatialMap(Vector3 position, Vector3 surfaceNormal)
    {
        Vector3 newPosition = position;
        RaycastHit hitInfo;
        float distance = 0.5f;

        // Check to see if there is a SpatialMapping mesh occluding the object at its current position.
        if (Physics.Raycast(position, surfaceNormal, out hitInfo, distance, SpatialMappingManager.Instance.LayerMask))
        {
            // If the object is occluded, reset its position.
            newPosition = hitInfo.point;
        }

        return newPosition;
    }
}