using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PathingAgent : MonoBehaviour
{
    [HideInInspector] public float NEXT_WAYPOINT_DISTANCE { get; private set; } = 1f;
    [HideInInspector] public bool reachedEndOfPath = false;
    [HideInInspector] public Seeker seeker;
    [HideInInspector] public Path path;
    [HideInInspector] public int currentWaypoint = 0;
    [HideInInspector] public Rigidbody2D rb;

    [SerializeField] Transform jumpDetector;

    private bool isRefreshing;
    private Transform target;
    private float reactionTime;
    private float speed;
    private float jumpHeight;

    private void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        seeker= GetComponent<Seeker>();
    }

    public void SetTarget(Transform target)
    {
        this.target= target;
    }

    public void UpdateMovement()
    {
        if (path == null) { return; }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = new Vector2((path.vectorPath[currentWaypoint].x - rb.position.x), 0).normalized;
        Vector2 force;
        if (IsGrounded())
        {
            force = direction * speed * Time.deltaTime;
        } else
        {
            force = direction * (speed/3) * Time.deltaTime;
        }
        

        RaycastHit2D hit = Physics2D.Raycast(jumpDetector.position, -Vector2.up, 0.2f);
        if (path.vectorPath[currentWaypoint].y > rb.position.y+0.5f 
            && Mathf.Abs(path.vectorPath[currentWaypoint].x - rb.position.x) < 1 && IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < NEXT_WAYPOINT_DISTANCE)
        {
            currentWaypoint += 1;
        }
    }

    public void PathRefreshing(bool refresh)
    {
        isRefreshing = refresh;
        StartCoroutine(RefreshPath());
    }

    void OnPathComplete(Path p)
    {
        Debug.Log("Got path");
        if (!p.error)
        {
            Debug.Log("Set path");
            path = p;
            currentWaypoint = 0;
        }
    }

    private IEnumerator RefreshPath()
    {
        while (isRefreshing)
        {
            yield return new WaitForSeconds(reactionTime);
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }
    }

    public void SetRefreshRate(float seconds)
    {
        this.reactionTime = seconds;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetJumpHeight(float height)
    {
        this.jumpHeight = height;
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(jumpDetector.position, -Vector2.up, 0.2f);
        return hit.collider != null && hit.transform.gameObject.layer == 7;
    }
}
