using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSelectorManager : MonoBehaviour {

    public bool isSelected;
    public bool isIdentified;
    public Color color;
    public GameObject ID;
    List<GameObject> liaisons = new List<GameObject>();
    GameObject unkwown;

    private void Awake()
    {
        color = transform.Find("Etiquette/Name").GetComponent<Image>().color;
        unkwown = transform.Find("Etiquette/Name/Unknown").gameObject;
        unkwown.transform.SetParent(this.transform);

        ResetLineSelector();
    }

    public void ResetLineSelector()
    {
        isSelected = false;
        isIdentified = false;

        if (ID != null)
        {
            ID.GetComponent<Image>().enabled = false;
            ID.GetComponent<Button>().interactable = true;

            ID = null;
        }

        liaisons.Clear();

        Color c = color;
        c.a = 0.25f;

        transform.Find("Etiquette/Name").GetComponent<Image>().color = c;
        foreach (Transform child in transform.Find("Etiquette/Name"))
        {
            Destroy(child.gameObject);
        }
        //transform.Find("Mask").GetComponent<Image>().color = new Color(0, 0, 0, 30 / 255.0f);
        unkwown.SetActive(true);
    }

    public void AddElement(GameObject element)
    {
        liaisons.Add(element);
    }

    public void UnSelectElement()
    {
        isSelected = false;

        transform.Find("Etiquette").GetComponent<Animator>().SetBool("selected", isSelected);
    }

    public void SelectElement()
    {

        bool s = isSelected;
       
        foreach (LineSelectorManager ls in transform.parent.GetComponentsInChildren<LineSelectorManager>()) {
            if (ls != this)
                ls.UnSelectElement();
        }

        isSelected = !s; //toggle selection
        transform.Find("Etiquette").GetComponent<Animator>().SetBool("selected", isSelected);
    }

    public void SetID(GameObject id)
    {
        foreach (Transform child in transform.Find("Etiquette/Name"))
        {
            Destroy(child.gameObject);
        }

        if (id.transform.Find("Data/Name") == null){
           
            UnSelectElement();
            if (ID != null)
            {
                ID.GetComponent<Image>().enabled = false;
                ID.GetComponent<Button>().interactable = true;
            }
            ResetLineSelector();
        }
        else {
            GameObject name = Object.Instantiate<GameObject>(id.transform.Find("Data/Name").gameObject);
            name.transform.SetParent(transform.Find("Etiquette/Name"), false);
            name.transform.localPosition = Vector3.zero;
            name.transform.localScale = 0.5f * Vector3.one;
            name.GetComponent<Image>().enabled = false;
            unkwown.SetActive(false);

            if (ID != null)
            {
                ID.GetComponent<Image>().enabled = false;
                ID.GetComponent<Button>().interactable = true;
            }

            id.GetComponent<Button>().interactable = false;
            id.GetComponent<Image>().enabled = true;
            id.GetComponent<Image>().color = color;
            Color c = color;
            //color = c;
            //c.a = 150 / 255.0f;
            transform.Find("Etiquette/Name").GetComponent<Image>().color = c;
            //c.a = 100 / 255.0f;
            //transform.Find("Mask").GetComponent<Image>().color = c;

            ID = id;
        }
    }


}
