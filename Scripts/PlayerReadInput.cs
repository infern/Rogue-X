using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadInput : MonoBehaviour
{
    #region Variables

    [Header("Components")]    /********/
    [SerializeField]
    PlayerComponents player;
    private InputMap keyMap;

    #endregion

    #region Base Methods
    void Awake()
    {
        keyMap = new InputMap();
    }
    void OnEnable()
    {
        keyMap.Enable();
    }
    void OnDisable()
    {
        keyMap.Disable();
    }

     void Update()
    {
        RunInput();
        JumpInput();
        AttackScript();
        PauseScript();
        SpecialScript();
    }

    #endregion

    #region Action Methods

    void RunInput()
    {
        Vector2 direction = keyMap.I.run.IsPressed() ? keyMap.I.run.ReadValue<Vector2>() : Vector2.zero;
        player.movementScript.direction = direction;
     }

    void JumpInput()
    {
        if (keyMap.I.jump.WasPerformedThisFrame()) player.jumpScript.ButtonDown();
        else if (keyMap.I.jump.WasReleasedThisFrame()) player.jumpScript.ButtonUp();

        if (keyMap.I.jump.IsPressed()) player.jumpScript.ButtonHeld();

    }


    void AttackScript()
    {
        if (keyMap.I.attack.WasPerformedThisFrame()) player.attackScript.ButtonDown();
    }

    void SpecialScript()
    {
        if (keyMap.I.special.WasPerformedThisFrame()) player.specialScript.ButtonDown();
    }
    void PauseScript()
    {
        if (keyMap.I.pause.WasPerformedThisFrame()) GameController.Instance.Pause();
    }
    #endregion

}
