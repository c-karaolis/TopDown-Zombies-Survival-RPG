using Foxlair.Character;
using Foxlair.Tools.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    protected CharacterStates _state;
    public StateMachine<CharacterStates.MovementStates> MovementState;
    public StateMachine<CharacterStates.CharacterConditions> ConditionState;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
