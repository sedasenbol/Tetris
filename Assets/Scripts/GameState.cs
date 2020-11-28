using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public enum State
    {
        Start,
        OnPlay,
        Paused,
        GameOver,
    }

    private int score = 0;
    private State currentState = State.Start;

    public int Score { get { return score; } set { score = value; } }
    public State CurrentState { get { return currentState; } set { currentState = value; } }
}

