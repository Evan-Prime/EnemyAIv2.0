using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum State 
    {
        patrolling, 
        chasing, 
        searching, 
        attacking, 
        retreating
    };
    State state;
    NavMeshAgent agent;

    GameObject player;
    bool playerInSight;
    int detectionRange = 6;
    int attackRange = 2;

    int currentPatrolNode;
    int numOfPatrolNodes;

    Vector3 currentNodePosition;
    Vector3 lastPosition;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindWithTag("Player");

        state = State.patrolling;
        currentPatrolNode = 0;
        numOfPatrolNodes = transform.parent.transform.GetChild(1).transform.childCount;
        agent.SetDestination(transform.parent.transform.GetChild(1).transform.GetChild(currentPatrolNode).transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerInSight();

        switch (state)
        {
            case State.patrolling: // Green
                
                GetComponent<MeshRenderer>().material.color = Color.green;
                
                currentNodePosition = transform.parent.transform.GetChild(1).transform.GetChild(currentPatrolNode).transform.position;

                agent.SetDestination(transform.parent.transform.GetChild(1).transform.GetChild(currentPatrolNode).transform.position);

                if (playerInSight == true)
                {
                    lastPosition = transform.position;
                    state = State.chasing;
                    timer = 5;
                }

                if (new Vector3(transform.position.x, 0, transform.position.z) == new Vector3(currentNodePosition.x, 0, currentNodePosition.z))
                {
                    currentPatrolNode++;

                    if (currentPatrolNode == numOfPatrolNodes)
                        currentPatrolNode = 0;
                }
                break;
            case State.chasing: // Magenta
                GetComponent<MeshRenderer>().material.color = Color.magenta;
                timer -= Time.deltaTime;

                agent.SetDestination(player.transform.position);

                if (playerInSight == true)
                {
                    timer = 5;
                    agent.SetDestination(player.transform.position);
                    if (CanPunch() == true)
                    {
                        state = State.attacking;
                    }
                }
                
                if (timer <= 0)
                {
                    state = State.searching;
                    timer = 3;
                }
                break;
            case State.searching: // Yellow
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                timer -= Time.deltaTime;

                if (playerInSight == true)
                {
                    state = State.chasing;
                    timer = 5;
                }
                
                if (timer <= 0)
                {
                    state = State.retreating;
                }
                break;
            case State.attacking: // Red
                GetComponent<MeshRenderer>().material.color = Color.red;
                if (CanPunch() == false)
                {
                    state = State.chasing;
                }
                break;
            case State.retreating: // White
                GetComponent<MeshRenderer>().material.color = Color.white;


                if (playerInSight == false)
                {
                    agent.SetDestination(lastPosition);
                    if (Vector3.Distance(transform.position, lastPosition) <= attackRange)
                    {
                        state = State.patrolling;
                    }
                }
                
                if (playerInSight == true)
                {
                    state = State.chasing;
                    timer = 5;
                }
                break;
        }
    }

    void IsPlayerInSight()
    {
        playerInSight = false;

        if(Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            RaycastHit detect;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out detect))
            {
                if (detect.collider.tag == "Player")
                {
                    playerInSight = true;
                }
            }
        }
    }

    bool CanPunch()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
