using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public static CameraHolder Instance
    { get; private set; }
    void Awake()
    {
        Instance = this;
    }
}
