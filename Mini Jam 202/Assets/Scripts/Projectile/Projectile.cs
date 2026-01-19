using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    private Collider bulletCollider;
    private bool canMove = true;

    [SerializeField]
    float explosionRadius = 25;
    [SerializeField]
    float explosionForce = 25;
    [SerializeField]
    float effectDispayTime = 1;

    [SerializeField]
    private GameObject explosionVFX;
    [SerializeField]
    private GameObject explosionSFX;

    private void Start()
    {
        GameManager.Event_ResetGame += OnResetGame;
        PlayerController.Instance.Event_TouchedBullet += OnTouchedBullet;
    }
    private void OnDestroy()
    {
        GameManager.Event_ResetGame -= OnResetGame;
        PlayerController.Instance.Event_TouchedBullet -= OnTouchedBullet;
    }

    private void FixedUpdate()
    {
        if(canMove)
            rb.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Stuff"))
            return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Manager"))
            return;

        Debug.Log(this.name + ": triggered with " + other.name);
        canMove = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        bulletCollider.isTrigger = false;

        Expload();

        StartCoroutine(WaitCor(1));

    }

    void Expload()
    {

        if (explosionSFX != null)
        {
            GameObject effect = Instantiate(explosionSFX, transform.position, Quaternion.identity);
            Destroy(effect, effectDispayTime);
        }

        if (explosionVFX != null)
        {
            GameObject effect = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            explosionVFX.transform.localScale = Vector3.one * explosionRadius;
            Destroy(effect, effectDispayTime);
        }

        StartCoroutine(Shake(.4f, .7f));

        Collider[] overlappedColliders = Physics.OverlapSphere(rb.position, explosionRadius);
        foreach (Collider c in overlappedColliders)
        {
            var exploableObj = c.GetComponentInParent<IExploadable>();
            if (exploableObj != null)
            {
                exploableObj.GetRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                exploableObj.OnExplosion();
            }
        }
    }
    void OnTouchedBullet(object sender, EventArgs e)
    {
        Debug.Log(this.name + ": triggered explosion");
        Destroy(this.gameObject);
    }

    IEnumerator WaitCor(float _time)
    {
        yield return new WaitForSeconds(_time);

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = CameraHolder.Instance.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            CameraHolder.Instance.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        CameraHolder.Instance.transform.localPosition = originalPos;
    }

    void OnResetGame(object sender, EventArgs e)
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionRadius);
    }
}
