using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour, IExploadable
{
    public static PlayerController Instance
    { get; private set; }

    [SerializeField]
    private Rigidbody rb;

    public Rigidbody GetRb => rb;
    

    Vector2 inputDir;

    [SerializeField]
    private float maxSpeed = 18.5f;
    [SerializeField]
    private float maxAcceleration = 1.35f;
    [SerializeField]
    private float maxDecceleration = 0.35f;
    [SerializeField]
    private float maxAirAcceleration = 1f;
    [SerializeField]
    private float shootForce = 25;
    [SerializeField]
    private Transform BulletStartPos;
    [SerializeField]
    private GameObject Bullet;

    private bool hasBullet = true;
    private bool onGround = false;
    Vector3 velocity, desiredVelocity = Vector3.zero;
    private Vector3 forceDir;

    [SerializeField]
    private GameObject laser;

    public event EventHandler Event_TouchedBullet;

    void Awake()
    {
        Instance = this;
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        hasBullet = true;
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        inputDir = new Vector2(x, y).normalized;

        desiredVelocity = new Vector3(inputDir.x, 0, inputDir.y) * maxSpeed;
        forceDir = -(transform.forward).normalized;
        if (Input.GetMouseButtonDown(0) && hasBullet)
        {
            SetVelocityY(Vector3.zero);
            rb.AddForce(forceDir * shootForce, ForceMode.Impulse);

            Instantiate(Bullet, BulletStartPos.position, Quaternion.LookRotation(BulletStartPos.forward));
            hasBullet = false;
            laser.SetActive(hasBullet);
        }
    }

    void FixedUpdate()
    {
        SetVelocityY(velocity);

        float maxSpeedChange;
        if (inputDir.sqrMagnitude > 0.05)
            maxSpeedChange = maxAcceleration;
        else
            maxSpeedChange = maxDecceleration;

        maxSpeedChange = onGround ? maxSpeedChange : maxAirAcceleration;

        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        onGround = false;
    }
    void SetVelocityY(Vector3 _velocity)
    {
        Vector3 vel = new Vector3(_velocity.x, rb.velocity.y, _velocity.z);

        rb.AddForce(vel, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forceDir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Bullet"))
        {
            onGround = true;
            return;
        }

        Debug.Log("Touched Bullet");
        hasBullet = true;
        laser.SetActive(hasBullet);

        Event_TouchedBullet?.Invoke(this, EventArgs.Empty);
        
    }
    void OnCollisionStay()
    {
        onGround = true;
    }

    public void OnExplosion()
    {

    }
}
