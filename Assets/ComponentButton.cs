using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentButton : MonoBehaviour
{
    public enum ComponentType { thruster};
    public ComponentType component;
    public GameObject WorldObj;
    CustomizeCanvas customize;
    // Start is called before the first frame update
    void Start()
    {
        customize = Object.FindObjectOfType<CustomizeCanvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pressed()
    {
        customize.selected = gameObject.GetComponent<ComponentButton>();
        customize.ColorUpdate();
        customize.EditorUpdate();
    }
}
