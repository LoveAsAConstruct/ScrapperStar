using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Statio : MonoBehaviour
{
    bool interactible = false;
    public GameObject interactibleText;
    public bool UIOpen = false;
    public InputActionAsset input;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        input.FindActionMap("Player").FindAction("Interact").performed += Interact;
    }

    // Update is called once per frame
    public void Interact(InputAction.CallbackContext context)
    {
        canvas.gameObject.GetComponent<CustomizeCanvas>().UpdateCanvas();
        print("interact");
        if(interactible){
            UIOpen = !UIOpen;
            canvas.gameObject.SetActive(UIOpen);
            if(UIOpen){
                Time.timeScale=0;
            }
            else{
                Time.timeScale = 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            interactible = true;
            interactibleText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {  
        if(other.gameObject.tag == "Player"){
            interactible = false;
            interactibleText.SetActive(false);
        }
    }
}
