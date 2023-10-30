using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameManager gameManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
