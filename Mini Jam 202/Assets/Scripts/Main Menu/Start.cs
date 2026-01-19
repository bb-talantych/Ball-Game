using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    public string levelName;

    public void OnButtonClick()
    {
        Debug.Log("Start Button Pressed");
        SceneManager.Instance.LoadScene(levelName);
    }
}
