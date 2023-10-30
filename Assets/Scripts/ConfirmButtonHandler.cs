using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButtonHandler : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void ConfirmedSearch()
    {
        gameManager.ConfirmedSearch();
    }
}
