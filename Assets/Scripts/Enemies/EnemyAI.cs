using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Transform[] patrolTargets;

    [SerializeField] private Seeker seeker;

    [SerializeField] private new Rigidbody2D rigidbody;

    [SerializeField] private Transform graphics;

    [SerializeField] private Vector3
        graphicsScale;

    [SerializeField] private float
        speed = 10f,
        nextWaypointDistance = 3f,
        repathRate = 3f;

    private Path _path;
    
    private int 
        _currentWaypoint = 0,
        _index;

    private IAstarAI _agent;

    private bool _reachedEndOfPath;

    private float
        _lastRepath = float.NegativeInfinity;

    private void Start()
    {
        if (seeker == null) seeker = GetComponent<Seeker>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
        if (_agent == null) _agent = GetComponent<IAstarAI>();
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
    }

    private void Patrol()
    {
        if (patrolTargets.Length == 0) return;

        var search = false;

        if (_agent.reachedEndOfPath && !_agent.pathPending)
        {
            _index++;
            search = true;
        }

        _index = _index % patrolTargets.Length;
        _agent.destination = patrolTargets[_index].position;
        
        if (search) _agent.SearchPath();
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

        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (_currentWaypoint + 1 < _path.vectorPath.Count)
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
        var velocity = dir * speed * speedFactor;

        velocity *= 500 * Time.deltaTime;

        rigidbody.AddForce(velocity);

        if (rigidbody.velocity.x >= 0.01f) graphics.localScale = graphicsScale;
        else if (rigidbody.velocity.x <= -0.01f) graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);
        
    }
}