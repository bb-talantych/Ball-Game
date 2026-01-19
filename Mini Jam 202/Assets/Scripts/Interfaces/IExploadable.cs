using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExploadable 
{
    Rigidbody GetRb
    {  get; }
    void OnExplosion();
}
