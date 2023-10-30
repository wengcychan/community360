using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Geometry;
using Esri.HPFramework;
using System;
using Unity.Mathematics;

public class CameraPosition : MonoBehaviour
{
    Vector3 overviewPosition;
    Vector3 pedestrianPosition;
    public GameManager gameManager;
    public float cameraRotateSpeed;
    public float rotateSpeed;
    public float moveSpeed;
    bool pedestrian;
    private Camera cam;
    double latitude;
    double longitude;
    float desiredHeading;
    ArcGISLocationComponent arcGISLocationComponent;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        arcGISLocationComponent = GetComponent<ArcGISLocationComponent>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        overviewPosition = new Vector3 (2780664, 139, 8442081);
        pedestrianPosition = new Vector3 (97, 5, -885);
        pedestrian = false;
        if (gameManager.GetComponent<GameManager>().searchAddress == false)
        {
            transform.position = overviewPosition;
        }
        else
        {
            desiredHeading = 66;

            latitude = gameManager.GetComponent<GameManager>().searchLatitude;
            longitude = gameManager.GetComponent<GameManager>().searchLongitude;

            ArcGISPoint newLocation = new ArcGISPoint(latitude, longitude, 50, arcGISLocationComponent.Position.SpatialReference);
            arcGISLocationComponent.Position = newLocation;

            ArcGISRotation newRotation = new ArcGISRotation(desiredHeading, 90, 0);
            arcGISLocationComponent.Rotation = newRotation;
            gameManager.GetComponent<GameManager>().searchAddress = false;
        }
    }

    public void Overview()
    {
        transform.position = overviewPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0); 
        pedestrian = false;
    }


    public void Pedestrian()
    {
        transform.position = pedestrianPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0); 
        pedestrian = true;
    }

    public void SelectLocation(string buttonName)
    {
        if (buttonName == "sornaistenkatu")
        {
            latitude = 24.971623155448274;
            longitude = 60.19237376066364;
        }
        else if (buttonName == "hermannin")
        {
            latitude = 24.96942122058029;
            longitude = 60.19257124268198;
        }
        else
        {
            latitude = 24.974918469054852;
            longitude = 60.19322473706929;
        }

        desiredHeading = 66;

        ArcGISPoint newLocation = new ArcGISPoint(latitude, longitude, 50, arcGISLocationComponent.Position.SpatialReference);
        arcGISLocationComponent.Position = newLocation;

        ArcGISRotation newRotation = new ArcGISRotation(desiredHeading, 90, 0);
        arcGISLocationComponent.Rotation = newRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left") || Input.GetKey("right"))
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float yaw = horizontalInput * cameraRotateSpeed * Time.deltaTime;
            transform.Rotate(0, yaw, 0, Space.Self);
        }

        if (pedestrian == true)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
}
