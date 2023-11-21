using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class ImageTracking : MonoBehaviour{
    [SerializeField] ARTrackedImageManager imageManagerRef;
    [SerializeField] GameObject OBJECT;
    [SerializeField] TextMeshProUGUI SelectedEnemyRef;
    GameObject spawnedObject;

    [Header("Debugging")]
    [SerializeField] DebugStripBehaviour debugStripRef;

    void OnEnable()
    {
        imageManagerRef.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        imageManagerRef.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs){
        foreach(var trackedImage in eventArgs.added){
            if(trackedImage.referenceImage.name == "Doom"){
                spawnedObject = Instantiate(OBJECT, trackedImage.transform.position.normalized * 6, trackedImage.transform.rotation);
                 debugStripRef.log(trackedImage.transform.position.ToString());
               
            }
        }

        foreach (var updatedTrackedImage in eventArgs.updated){
            if (updatedTrackedImage.referenceImage.name == "Doom"){
                spawnedObject.transform.position = updatedTrackedImage.transform.position.normalized * 6;
                spawnedObject.transform.rotation = updatedTrackedImage.transform.rotation;
                debugStripRef.log(updatedTrackedImage.transform.position.ToString());
            }
        
        }

    }


}