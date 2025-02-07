using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_LockControls : MonoBehaviour
{
    private InputReader inputReader;
    private CharacterManager characterManager;
    private PauseManager pauseManager;

    public void LockControls()
    {
        inputReader = InputReader.Instance;
        characterManager = CharacterManager.Instance;
        pauseManager = PauseManager.Instance;

        inputReader.enableAxisInput = false;
        characterManager.canMove = false;
        inputReader.enableJump = false;
        inputReader.enableBomb = false;
        inputReader.enableDash = false;
        inputReader.enableYoyo = false;
        inputReader.enableCameraSwitch = false;
        pauseManager.canBePaused = false;

        characterManager.isThrowing = false;
        characterManager.altThrowing = false;
    }
}