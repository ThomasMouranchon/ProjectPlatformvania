using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_UnlockControls : MonoBehaviour
{
    private InputReader inputReader;
    private CharacterManager characterManager;
    private PauseManager pauseManager;

    public void UnlockControls()
    {
        inputReader = InputReader.Instance;
        characterManager = CharacterManager.Instance;
        pauseManager = PauseManager.Instance;

        inputReader.enableAxisInput = true;
        characterManager.canMove = true;
        inputReader.enableJump = true;
        inputReader.enableBomb = true;
        inputReader.enableDash = true;
        inputReader.enableYoyo = true;
        inputReader.enableCameraSwitch = true;
        pauseManager.canBePaused = true;

        characterManager.isThrowing = true;
        characterManager.altThrowing = true;
    }
}