using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelProperties : MonoBehaviour
{
    public string world_name = "default";
    public bool unlocked = false;
    public Object scene;
    public float unlock_light_distance = 0;
    public float unlock_light_height = 5;
    public float unlock_light_intensity = 100;
    public float front_light_distance = 13;
    public float front_light_height = -1.5f;
    public float front_light_intensity = 300;
    public float spot_angle = 35;
    public Color unlock_color = Color.white;
    public Color front_color = Color.white;
   
    private void Start()
    {
        if (unlocked)
        {
            GameObject unlocked_light = new GameObject("Unlock_light");
            unlocked_light.transform.position = transform.position;

            unlocked_light.transform.parent = transform.parent;
            unlocked_light.transform.LookAt(transform.parent);
            unlocked_light.transform.Translate(Vector3.back * unlock_light_distance + Vector3.up * unlock_light_height);
            unlocked_light.transform.localRotation = Quaternion.Euler(90, 0, 0);
            Light unlock_light_comp = unlocked_light.AddComponent<Light>();
            unlock_light_comp.type = LightType.Spot;
            unlock_light_comp.intensity = unlock_light_intensity;
            unlock_light_comp.color = unlock_color;
            unlock_light_comp.spotAngle = spot_angle;

            GameObject front_light = new GameObject("Front_light");
            front_light.transform.position = transform.position;

            front_light.transform.parent = transform.parent;
            front_light.transform.LookAt(transform.parent);
            front_light.transform.Translate(Vector3.back * front_light_distance + Vector3.up * front_light_height);
            Light light_comp = front_light.AddComponent<Light>();
            light_comp.type = LightType.Spot;
            light_comp.intensity = front_light_intensity;
            light_comp.spotAngle = spot_angle;
        }
    }
}
