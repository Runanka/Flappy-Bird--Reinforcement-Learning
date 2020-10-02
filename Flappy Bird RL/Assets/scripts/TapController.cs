using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    Rigidbody2D rigid_body;
    Quaternion downRotation;
    Quaternion forwardRotation;

    private void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);

    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
    }

    void OnGameStarted()
    {
        rigid_body.velocity = Vector3.zero;
        rigid_body.simulated = true;
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigid_body.velocity = Vector3.zero;
            rigid_body.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "ScoreZone")
        {
            OnPlayerScored();
        }
        if(col.gameObject.tag == "DeadZone")
        {
            rigid_body.simulated = false;
            OnPlayerDied();
        }
    }
}
