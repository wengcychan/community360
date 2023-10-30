using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Esri.GameEngine.Geometry;

public class GameManager : MonoBehaviour
{
    public bool searchAddress;
    public double searchLatitude;
    public double searchLongitude;
    private static GameObject instance;
    private Geocoder geocoder;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        searchAddress = false;
    }

    public void SearchAddress()
    {
        SceneManager.LoadScene("SearchHome");
    }

    public void ConfirmedSearch()
    {
        searchAddress = true;
        GameObject geocoderObject = GameObject.Find("Geocoder");
        geocoder = geocoderObject.GetComponent<Geocoder>();

        ArcGISPoint markerPosition = geocoder.GetMarkerPosition();

        searchLatitude = markerPosition.X;
        searchLongitude = markerPosition.Y;

        SceneManager.LoadScene("HelsinkiScene");
    }

}
