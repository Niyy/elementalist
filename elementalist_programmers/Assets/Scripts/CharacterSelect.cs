using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] prefab;
    PlayerInputManager playerInputManager;
    InputDevice pairWithDevice;
    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = new PlayerInputManager();
        playerInputManager.playerPrefab = prefab[1];
        playerInputManager.JoinPlayer(-1, -1, null, pairWithDevice: Keyboard.current);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
