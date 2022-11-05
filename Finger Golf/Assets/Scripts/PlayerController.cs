using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject arrow;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_Text shootCountText;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] float shootForce;

    Vector3 lastMousePosition;
    float ballDistance;
    bool isShooting;
    Vector3 forceDir;
    [SerializeField] float forceFactor;
    int shootCount = 0;
    bool resetArrow;

    [SerializeField] GameManager GM;
    [SerializeField] GameObject flag;
    bool flagFocus = false;
    float lastRotY;

    public int ShootCount { get => shootCount; }

    AudioSource ballAudio;
    [SerializeField] AudioClip hit1, hit2, hit3;

    void Start()
    {
        ballDistance = Vector3.Distance(cam.transform.position, ball.Position) + 1;
        arrow.SetActive(false);
        shootCountText.text = "Shoot Count : " + shootCount;
        line.enabled = false;
        ballAudio = ball.GetComponent<AudioSource>();
        ballAudio.volume = SaveLoad.soundVolume;
    }

    void Update()
    {
        if (transform.position != ball.Position)
        {
            transform.position = ball.Position;
            aim.gameObject.SetActive(true);
        }

        if (!ball.IsMoving && !ball.IsTeleporting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, ballDistance, ballLayer))
                {
                    isShooting = true;
                    arrow.SetActive(true);
                    line.enabled = true;
                }
            }

            // Shooting mode
            if (!GM.GameWin)
            {
                if (Input.GetMouseButton(0) && isShooting)
                {
                    var ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, ballDistance * 2, rayLayer))
                    {
                        var forceVector = ball.Position - hit.point;
                        forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                        forceDir = forceVector.normalized;
                        var forceMagnitude = forceVector.magnitude;
                        forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 5);
                        forceFactor = forceMagnitude / 5;
                    }

                    // Arrow
                    transform.LookAt(transform.position + forceDir);
                    if (arrow.transform.localScale.z < 1)
                    {
                        arrow.transform.localScale = new Vector3(Mathf.Clamp(3 * forceFactor, 0.5f, 1), 1, 3 * forceFactor);
                    }
                    else
                    {
                        arrow.transform.localScale = new Vector3(1, 1, 3 * forceFactor);
                    }

                    // Aim
                    var rect = aim.GetComponent<RectTransform>();
                    rect.anchoredPosition = Input.mousePosition;

                    // Line
                    var ballScrPos = cam.WorldToScreenPoint(ball.Position);
                    line.SetPositions(new Vector3[]
                    {
                    ballScrPos,
                    Input.mousePosition,
                    });
                }
            }
        }

        // Camera mode
        if (!GM.GameWin)
        {
            if (Input.GetMouseButton(0) && !isShooting)
            {
                var current = cam.ScreenToViewportPoint(Input.mousePosition);
                var last = cam.ScreenToViewportPoint(lastMousePosition);
                var delta = current - last;

                cameraPivot.transform.RotateAround(ball.Position, Vector3.up, delta.x * camSensitivity.x);
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, -delta.y * camSensitivity.y);

                var angle = Vector3.SignedAngle(Vector3.up, cam.transform.up, cam.transform.right);

                if (!GM.GameWin)
                {
                    if (angle < -3)
                    {
                        cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, -3 - angle);
                    }
                    else if (angle > 65)
                    {
                        cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 65 - angle);
                    }
                }
            }
        }
        else
        {
            if (!flagFocus)
            {
                cam.transform.localPosition = new Vector3(0, 10, -8);
                cameraPivot.transform.rotation = Quaternion.Euler(-33, cameraPivot.transform.eulerAngles.y, 0);
                flagFocus = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            ball.AddForce(forceDir * shootForce * forceFactor);
            shootCount++;
            shootCountText.text = "Shoot Count : " + shootCount;
            forceFactor = 0;
            forceDir = Vector3.zero;
            isShooting = false;
            arrow.SetActive(false);
            aim.gameObject.SetActive(false);
            line.enabled = false;

            var randomHit = Random.value;
            if (randomHit < 0.33f)
            {
                ballAudio.clip = hit1;
            }
            else if (randomHit < 0.66f)
            {
                ballAudio.clip = hit2;
            }
            else
            {
                ballAudio.clip = hit3;
            }
            ballAudio.Play();
        }

        lastMousePosition = Input.mousePosition;
    }
}
