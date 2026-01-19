using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void OnButtonClick()
    {
        Debug.Log("Exit Button Pressed");
        Application.Quit();
    }
}
