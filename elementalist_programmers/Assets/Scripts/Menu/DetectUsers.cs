using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

// Note: I think you only need one prefab for all players.
public class DetectUsers : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    public GameObject[] prefab;
    string controlScheme = "PlayerControls";

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        InputUser.listenForUnpairedDeviceActivity = 4;
        InputUser.onUnpairedDeviceUsed += ListenForUnpairedDevices;
    }

    void ListenForUnpairedDevices(InputControl control, InputEventPtr arg)
    {
        Debug.Log("Found unpaired device " + control);
        if ((control.device is Gamepad || control.device is Keyboard))
        {
            InputDevice pair_with_device = control.device;
            if (playerInputManager.playerCount < 4)
            {
                playerInputManager.playerPrefab = prefab[(playerInputManager.playerCount)];
                playerInputManager.JoinPlayer(-1, -1, controlScheme, pair_with_device);
            }
        }
    }

    private void OnApplicationQuit()
    {
        InputUser.listenForUnpairedDeviceActivity = 0;
    }

    public void DisableListening()
    {
        InputUser.listenForUnpairedDeviceActivity = 0;
    }

}