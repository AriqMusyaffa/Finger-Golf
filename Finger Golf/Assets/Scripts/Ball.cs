using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public Vector3 Position => rb.position;
    public bool IsMoving => rb.velocity != Vector3.zero;
    public bool IsTeleporting => isTeleporting;
    Vector3 lastPosition;
    bool isTeleporting;
    bool onSlope;
    float slopeTime;

    AudioSource ballAudio;
    [SerializeField] AudioClip wall_SFX;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            lastPosition = transform.position;
        }

        ballAudio = GetComponent<AudioSource>();
        ballAudio.volume = SaveLoad.soundVolume;
        slopeTime = 0;
    }

    internal void AddForce(Vector3 force)
    {
        rb.isKinematic = false;
        lastPosition = transform.position;
        rb.AddForce(force, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (!onSlope)
        {
            rb.drag = 1;
        }

        if (rb.velocity != Vector3.zero && rb.velocity.magnitude < 0.1f)
        {
            if (!onSlope)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            else
            {
                rb.drag = 0;
                slopeTime += Time.deltaTime;
            }
        }

        if (slopeTime > 8)
        {
            onSlope = false;
            slopeTime = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Out")
        {
            StopAllCoroutines();
            StartCoroutine(DelayedTeleport());
        }

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Log")
        {
            ballAudio.clip = wall_SFX;
            ballAudio.Play();
        }

        if (collision.gameObject.tag == "Slope")
        {
            onSlope = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Slope")
        {
            onSlope = false;
        }
    }

    public IEnumerator DelayedTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(3);
        rb.isKinematic = true;
        transform.position = lastPosition;
        isTeleporting = false;
    }
}
