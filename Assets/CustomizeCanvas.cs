using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomizeCanvas : MonoBehaviour
{
    public Image shipImage;
    public GameObject ThrusterButtonRoot;
    public GameObject PlayerThrusterRoot;
    public GameObject ThrusterButtonPrefab;
    List<GameObject> ThrusterButtons = new List<GameObject>();
    List<GameObject> Buttons = new List<GameObject>();
    public ComponentButton selected;
    public Slider Xslider;
    public Slider Yslider;
    public Slider rotSlider;
    public TMPro.TMP_InputField Xinput;
    public TMPro.TMP_InputField Yinput;
    public TMPro.TMP_InputField Rotinput;
    public TMPro.TextMeshProUGUI titleText;
    public bool inputOpen = true;
    public GameObject ThrusterOptions;
    public Slider powerslider;
    public TMPro.TMP_InputField powerinput;
    public TMPro.TMP_Dropdown BindingDropdown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SliderInput()
    {
        print("slider Input");
        if(selected != null && inputOpen)
        {

            float Xpos = Xslider.value/10;
            float Ypos = Yslider.value/10;
            float Rot = rotSlider.value*45;
            Xinput.text = Xpos.ToString();  
            Yinput.text = Ypos.ToString();
            Rotinput.text = Rot.ToString();
            Vector2 buttonPos = new Vector2(shipImage.rectTransform.rect.width * Xpos, shipImage.rectTransform.rect.height * Ypos);
            selected.gameObject.GetComponent<RectTransform>().anchoredPosition = buttonPos;
            selected.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, Rot);
            selected.WorldObj.transform.localPosition = new Vector2(Xpos, Ypos);
            selected.WorldObj.transform.localRotation = Quaternion.Euler(0, 0, Rot);

            if(selected.component == ComponentButton.ComponentType.thruster)
            {
                selected.WorldObj.GetComponent<Thruster>().sensitivity = powerslider.value;
            }
        }
    }
    public void DropdownInput()
    {
        if(selected != null)
        {
            if(selected.component == ComponentButton.ComponentType.thruster)
            {
                Thruster thruster = selected.WorldObj.GetComponent<Thruster>();
                if (BindingDropdown.value == 0)
                {
                    thruster.SetBinding(Thruster.Binding.forward);
                }
                if (BindingDropdown.value == 1)
                {
                    thruster.SetBinding(Thruster.Binding.left);
                }
                if (BindingDropdown.value == 2)
                {
                    thruster.SetBinding(Thruster.Binding.right);
                }
                if (BindingDropdown.value == 3)
                {
                    thruster.SetBinding(Thruster.Binding.forward);
                }
            }
        }
    }
    public void EditorUpdate()
    {
        if (selected != null)
        {
            inputOpen = false;
            Vector2 pos = selected.gameObject.GetComponent<RectTransform>().anchoredPosition/shipImage.rectTransform.rect.size*10;
            print(pos);
            print(selected.gameObject.GetComponent<RectTransform>().rotation.eulerAngles.z/45);
            rotSlider.value = selected.gameObject.GetComponent<RectTransform>().rotation.eulerAngles.z / 45;
            Yslider.value = pos.y;
            Xslider.value = pos.x;
            titleText.text = selected.component.ToString();
            Yinput.text = pos.y.ToString();
            Xinput.text = pos.x.ToString();
            Rotinput.text = (rotSlider.value * 45).ToString();
            inputOpen = true;

            if (selected.component == ComponentButton.ComponentType.thruster)
            {
                Thruster thruster = selected.WorldObj.GetComponent<Thruster>();
                powerslider.value = thruster.sensitivity;
                if(thruster.bound == Thruster.Binding.forward)
                {
                    BindingDropdown.value = 0;
                }
                else if (thruster.bound == Thruster.Binding.left)
                {
                    BindingDropdown.value = 1;
                }
                else if (thruster.bound == Thruster.Binding.right)
                {
                    BindingDropdown.value = 2;
                }
            }
        }
    }
    public void ColorUpdate()
    {
        foreach(GameObject button in Buttons)
        {
            if(selected != null)
            {
                if (selected.gameObject == button)
                {
                    print("yellowed");
                    button.GetComponent<Image>().color = Color.yellow;
                }
                else
                {
                    button.GetComponent<Image>().color = Color.green;
                }
            }
           
        }
    }
    public void UpdateCanvas(){
        foreach(GameObject button in Buttons) {
            Destroy(button);
        }
        Buttons = new List<GameObject>();
        ThrusterButtons = new List<GameObject>();
        shipImage.sprite=GameObject.Find("shippng").GetComponent<SpriteRenderer>().sprite;
        for(int i = 0; i<PlayerThrusterRoot.transform.childCount; i++)
        {
            GameObject child = PlayerThrusterRoot.transform.GetChild(i).gameObject;
            
            Vector2 childpos = child.transform.localPosition;
            Vector2 buttonPos = new Vector2(shipImage.rectTransform.rect.width * childpos.x / 1f, shipImage.rectTransform.rect.height * childpos.y / 1f);
            GameObject button = Instantiate(ThrusterButtonPrefab, ThrusterButtonRoot.transform);
            print(buttonPos);
            button.GetComponent<RectTransform>().anchoredPosition += buttonPos;
            //button.transform.Translate(buttonPos);
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "T" + i.ToString();
            button.GetComponent<ComponentButton>().WorldObj = child;
            button.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, child.transform.localRotation.eulerAngles.z);
            ThrusterButtons.Add(button);
            Buttons.Add(button);

        }
        ColorUpdate();
    }
}
