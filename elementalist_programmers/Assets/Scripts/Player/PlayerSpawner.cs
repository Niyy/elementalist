using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public void SpawnPlayer(GameObject prefab, string control_scheme, InputDevice pair_with_device)
    {
        PlayerInput.Instantiate(prefab, -1, control_scheme, -1, pair_with_device);
    }
}
