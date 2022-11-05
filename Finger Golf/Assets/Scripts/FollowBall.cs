using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] float speed = 1;
    [SerializeField] GameManager GM;
    [SerializeField] GameObject flag;
    bool active = true;

    void Update()
    {
        if (active)
        {
            if (transform.position == ball.Position)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, ball.Position, Time.deltaTime * speed);

            // Jika sudah dekat, langsung teleport
            if (!GM.GameWin)
            {
                if (ball.IsMoving)
                {
                    return;
                }

                if (Vector3.Distance(transform.position, ball.Position) < 0.1f)
                {
                    transform.position = ball.Position;
                }
            }
            else
            {
                transform.position = flag.transform.position;
                active = false;
            }
        }
    }
}
