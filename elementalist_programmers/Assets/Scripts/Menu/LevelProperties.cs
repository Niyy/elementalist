using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelProperties : MonoBehaviour
{
    public bool unlocked = false;
    public Object scene;
    public float light_distance = 1;
    public float light_height = 1;
    public float light_intensity = 1;
    private void Start()
    {
        if (unlocked)
        {
            GameObject unlocked_light = new GameObject("Unlock_light");
            unlocked_light.transform.position = transform.position;

            unlocked_light.transform.parent = transform.parent;
            unlocked_light.transform.LookAt(transform.parent);
            unlocked_light.transform.Translate(Vector3.back * light_distance + Vector3.up * light_height);
            Light light_comp = unlocked_light.AddComponent<Light>();
            light_comp.intensity = light_intensity;
        }
    }
}
