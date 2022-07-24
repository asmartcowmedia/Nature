using System;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    [SerializeField] private GameObject circle;

    [SerializeField] private Seeker seeker;

    [SerializeField] private new Rigidbody2D rigidbody;

    [SerializeField] private Transform graphics;

    [SerializeField] public Vector3
        graphicsScale;

    [SerializeField] private float
        speed = 10f,
        nextWaypointDistance = 3f,
        repathRate = 3f,
        stopRange = 3f,
        distanceToFollowPlayer = 10f;

    [SerializeField] private bool
        drawCustomGizmo;

    [SerializeField] private new AudioManager audio;

    private Path _path;
    
    private int 
        _currentWaypoint = 0,
        _index;

    private bool _reachedEndOfPath;

    private float
        _lastRepath = float.NegativeInfinity;

    private void Start()
    {
        if (seeker == null) seeker = GetComponent<Seeker>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        target = FindObjectOfType<CharacterController>().transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Camera.main.GetComponent<AudioManager>().TriggerCombat();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Camera.main.GetComponent<AudioManager>().TriggerAmbiance();
        }
    }

    private void OnPathComplete(Path p)
    {
        p.Claim(this);
        
        if (!p.error)
        {
            _path?.Release(this);
            _path = p;
            _currentWaypoint = 0;
        }
        else p.Release(this);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= distanceToFollowPlayer)
        {
            FollowPlayer();
        }
        
        DrawCircle();
    }

    private void DrawCircle()
    {
        switch (drawCustomGizmo)
        {
            case true:
                circle.SetActive(true);
                circle.transform.localScale = new Vector3(distanceToFollowPlayer + 4, distanceToFollowPlayer + 4, 0);
                break;
            case false:
                circle.SetActive(false);
                break;
        }
    }

    private void FollowPlayer()
    {
        if (Time.time > _lastRepath + repathRate && seeker.IsDone())
        {
            _lastRepath = Time.time;
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
        
        if (_path == null)
            return;

        _reachedEndOfPath = false;

        float distanceToWaypoint;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (distanceToTarget <= stopRange)
                {
                    _reachedEndOfPath = true;
                    break;
                }
                else if (_currentWaypoint + 1 < _path.vectorPath.Count)
                {
                    _currentWaypoint++;
                }
                else
                {
                    _reachedEndOfPath = true;
                    break;
                }
            }
            else break;
        }

        var speedFactor = _reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        var dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
        var velocity = dir * (speed * speedFactor);

        velocity *= 500 * Time.deltaTime;

        rigidbody.AddForce(velocity);

        if (rigidbody.velocity.x >= .5f) graphics.localScale = graphicsScale;
        else if (rigidbody.velocity.x <= -.5f) graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);
        
    }
}