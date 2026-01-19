using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IExploadable
{
    [SerializeField]
    private Rigidbody rb;
    public Rigidbody GetRb => rb;

    [SerializeField]
    private float moveSpeed = 2;

    public static EventHandler Event_PlayerTouched;

    private void Start()
    {
        GameManager.Event_ResetGame += OnResetGame;
    }
    private void OnDestroy()
    {
        GameManager.Event_ResetGame -= OnResetGame;
    }


    private void FixedUpdate()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 moveDir = playerPos - transform.position;
        moveDir.Normalize();
        SetVelocityY(moveDir * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(this.name + "collided with player");
            Event_PlayerTouched?.Invoke(this, EventArgs.Empty);
        }
    }

    void SetVelocityY(Vector3 _velocity)
    {
        Vector3 vel = new Vector3(_velocity.x, rb.velocity.y, _velocity.z);

        rb.AddForce(vel, ForceMode.Acceleration);
    }

    public void OnExplosion()
    {
        EnemySpawner.Instance.RemoveEnemy();
        Destroy(this.gameObject);
    }

    void OnResetGame(object sender, EventArgs e)
    {
        OnExplosion();
    }
}
