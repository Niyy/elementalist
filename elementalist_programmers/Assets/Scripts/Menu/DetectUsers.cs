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
    public GameObject playerPrefab;
    public GameObject[] play_test_characters;
    public bool play_testing = false;
    string controlScheme = "PlayerControls";
    InputDevice Keyboard = null;

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
                if (control.device is Keyboard)
                {
                    Keyboard = pair_with_device;
                }
                if (play_testing && play_test_characters.Length > playerInputManager.playerCount)
                {
                    playerPrefab = play_test_characters[playerInputManager.playerCount];
                }
                playerInputManager.playerPrefab = playerPrefab;
                
                playerInputManager.JoinPlayer(-1, -1, controlScheme, pair_with_device);
            }
        }
        if (control.device is Mouse && Keyboard != null)
        {
            InputDevice pair_with_device = control.device;
            InputUser.PerformPairingWithDevice(pair_with_device, InputUser.FindUserPairedToDevice(Keyboard).Value, InputUserPairingOptions.None);
        }
    }

    private void OnApplicationQuit()
    {
        InputUser.listenForUnpairedDeviceActivity = 0;
        InputUser.onUnpairedDeviceUsed -= ListenForUnpairedDevices;
    }

    public void DisableListening()
    {
        InputUser.listenForUnpairedDeviceActivity = 0;
        InputUser.onUnpairedDeviceUsed -= ListenForUnpairedDevices;
    }

}