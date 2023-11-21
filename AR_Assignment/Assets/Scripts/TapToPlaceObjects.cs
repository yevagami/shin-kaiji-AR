using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlaceObjects : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject objectToSpawn;

    GameObject spawnObject;
    ARRaycastManager arRaycastManager;
    Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if there was no Touch event on Screen
        if(!GetTouchPosition(out Vector2 touchPosition)) return;
        // Touch event occured
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)){
            Pose hitPose = hits[0].pose;
            if (spawnObject == null){
                spawnObject = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
            }
            else{

                spawnObject.transform.position = hitPose.position;
            }
        }
        
    }

    bool GetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = Vector2.zero;
        return false;
    }
}
