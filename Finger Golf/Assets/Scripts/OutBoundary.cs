using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutBoundary : MonoBehaviour
{
    [SerializeField] Ball ball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            ball.StopAllCoroutines();
            StartCoroutine(ball.DelayedTeleport());
        }
    }
}
