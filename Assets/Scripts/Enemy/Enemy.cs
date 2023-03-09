using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyModel;
    enum State {patrolling, chasing, searching, attacking, retreating};
    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.patrolling;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.patrolling: // Green
                state = State.chasing;
                break;
            case State.chasing: // Orange
                state = State.searching;
                state = State.attacking;
                break;
            case State.searching: // Yellow
                state = State.chasing;
                state = State.retreating;
                break;
            case State.attacking: // Red
                state = State.chasing;
                break;
            case State.retreating: // White
                state = State.patrolling;
                state = State.chasing;
                break;
        }
    }
}
