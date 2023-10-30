using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Comment : MonoBehaviour
{
    public Camera m_Camera;
    public GameObject panelPrefab;
    private Vector3 panelPosition;
    private Vector3 panelRotation;
    public Vector2 panelOffset;
    private RectTransform panelTransform;
    private GameObject panelInstance;
    public GameObject commentWindow;
    public GameObject viewWindow;
    public GameObject optionWindow;
    public GameObject weatherWindow;
    public GameObject shareWindow;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

             RaycastHit hit;
             Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

             if (Physics.Raycast(ray, out hit))
             {
                 if (hit.transform != null)
                 {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        commentWindow.SetActive(true);
                        viewWindow.SetActive(false);
                        optionWindow.SetActive(false);
                        weatherWindow.SetActive(false);
                        shareWindow.SetActive(false);
                        Time. timeScale = 0;
                        Vector2 screenPoint = m_Camera.WorldToScreenPoint(hit.point);
                        if (panelInstance != null)
                        {
                            Destroy(panelInstance);
                        }
                        
                        panelPosition = new Vector3(hit.point.x, hit.point.y + 10, hit.point.z);
                        panelInstance = Instantiate(panelPrefab, panelPosition, Quaternion.Euler(82, -24, -14));
                    }
                 }
             }
        }
    }

    public void Resume()
    {
        Time. timeScale = 1;
    }

    public void Freeze()
    {
        Time. timeScale = 0;
    }

    private void CreatePanel(Vector2 screenPoint)
    {
        GameObject panelObject = Instantiate(panelPrefab, transform);
        panelTransform = panelObject.GetComponent<RectTransform>();
        panelTransform.anchoredPosition = screenPoint + panelOffset;
        panelTransform.anchorMin = new Vector2(0.5f, 0.5f);
        panelTransform.anchorMax = new Vector2(0.5f, 0.5f);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
    }
}
