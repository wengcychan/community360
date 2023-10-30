using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonHandler : MonoBehaviour
{
    public GameObject optionA;
    public GameObject optionB;

    // // Start is called before the first frame update
    void Start()
    {
        optionA = GameObject.Find("OptionA");
        optionB = GameObject.Find("OptionB");
        optionA.SetActive(false);
        optionB.SetActive(false);
    }

    public void OptionA()
    {
        optionA.SetActive(true);
        optionB.SetActive(false);
    }

    public void OptionB()
    {
        optionB.SetActive(true);
        optionA.SetActive(false);
    }

    public void WithoutDevelopment()
    {
        optionA.SetActive(false);
        optionB.SetActive(false);
    }  
}
