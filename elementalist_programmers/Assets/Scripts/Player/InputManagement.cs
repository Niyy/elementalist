using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class InputManagement : MonoBehaviour
{
    public struct Controllers
    {
        public Gamepad pad;
        public Keyboard key;
    }
    InputUser[] players;
    Controllers[] controllers;
    public GameObject[] prefab;
    string controlScheme = "PlayerControls";

    private void Start()
    {
        players = new InputUser[4];
        controllers = new Controllers[4];
        

        InputUser.listenForUnpairedDeviceActivity = 4;

        InputUser.onChange += OnControlsChanged;
        InputUser.onUnpairedDeviceUsed += ListenForUnpairedDevices;

        for (var i = 0; i < players.Length; i++)
        {
            players[i] = InputUser.CreateUserWithoutPairedDevices();
        }
        
    }

    public void OnControlsChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        /*if (change == InputUserChange.DevicePaired)
        {
            var playerId = players.ToList().IndexOf(user);
            Debug.Log(playerId);
            if (device is Gamepad)
            {
                controllers[playerId].pad = device as Gamepad;
            }
            else if (device is Keyboard)
            {
                controllers[playerId].key = device as Keyboard;
            }
        }
        else if (change == InputUserChange.DeviceUnpaired)
        {
            var playerId = players.ToList().IndexOf(user);
            controllers[playerId].key = null;
            controllers[playerId].pad = null;
        }*/
    }

    void ListenForUnpairedDevices(InputControl control, InputEventPtr arg)
    {
        if (control.device is Gamepad)
        {
            InputDevice pair_with_device = control.device;
            for (var i = 0; i < players.Length; i++)
            {
                // find a user without a paired device
                if (players[i].pairedDevices.Count == 0)
                {
                    // pair the new Gamepad device to that user
                    players[i] = InputUser.PerformPairingWithDevice(control.device, players[i]);
                    //players[i].ActivateControlScheme(controlScheme);
                    PlayerInput p1 = PlayerInput.Instantiate(prefab[i], -1, controlScheme, -1, pair_with_device);
                    return;
                }
            }
        }
        if (control.device is Keyboard)
        {
            InputDevice pair_with_device = control.device;
            for (var i = 0; i < players.Length; i++)
            {
                // find a user without a paired device
                if (players[i].pairedDevices.Count == 0)
                {
                    // pair the new Gamepad device to that user
                    players[i] = InputUser.PerformPairingWithDevice(control.device, players[i]);
                    PlayerInput.Instantiate(prefab[i], -1, controlScheme, -1, pair_with_device);
                    return;
                }
            }
        }
    }
}
