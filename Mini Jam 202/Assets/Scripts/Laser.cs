using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform startPos;

    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, startPos.position);

        lineRenderer.SetPosition(1, startPos.forward * 100);
    }
}
