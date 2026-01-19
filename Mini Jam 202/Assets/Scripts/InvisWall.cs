using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisWall : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer m_Renderer;
    void Awake()
    {
        m_Renderer.enabled = false;
    }
}
