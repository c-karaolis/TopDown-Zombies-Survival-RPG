using Foxlair.Character;
using Foxlair.Tools.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public StateMachine CharacterMovementStateMachine;
    public State[] CharacterStates;

    public GameObject characterStatesGameObject;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        LoadAvailableCharacterStates();
    }

    private void LoadAvailableCharacterStates()
    {
        CharacterStates = characterStatesGameObject.GetComponentsInChildren<State>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
