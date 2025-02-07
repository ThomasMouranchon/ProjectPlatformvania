using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_LockControls : MonoBehaviour
{
    public bool enableControls;
    private InputReader inputReader;
    private CharacterManager characterManager;
    private PauseManager pauseManager;

    public void LockControls()
    {
        inputReader = InputReader.Instance;
        characterManager = CharacterManager.Instance;
        pauseManager = PauseManager.Instance;

        inputReader.enableAxisInput = enableControls;
        characterManager.canMove = enableControls;
        inputReader.enableJump = enableControls;
        inputReader.enableBomb = enableControls;
        inputReader.enableDash = enableControls;
        inputReader.enableYoyo = enableControls;
        inputReader.enableCameraSwitch = enableControls;
        pauseManager.canBePaused = enableControls;
    }
}