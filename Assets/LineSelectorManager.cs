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

    public void ResetLineSelector()
    {
        isSelected = false;
        isIdentified = false;
        ID = null;
        liaisons.Clear();

        Color c = new Color(0,0,0,30/255.0f);
        
        transform.Find("Name").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        transform.Find("Mask").GetComponent<Image>().color = new Color(0, 0, 0, 30 / 255.0f);

    }

    public void AddElement(GameObject element)
    {
        liaisons.Add(element);
    }

    public void UnSelectElement()
    {
        isSelected = false;

        GetComponent<Animator>().SetBool("selected", isSelected);
    }

    public void SelectElement()
    {

        bool s = isSelected;
       
        foreach (LineSelectorManager ls in transform.parent.GetComponentsInChildren<LineSelectorManager>()) {
            if (ls != this)
                ls.UnSelectElement();
        }

        isSelected = !s; //toggle selection
        GetComponent<Animator>().SetBool("selected", isSelected);
    }

    public void SetID(GameObject id)
    {
        foreach (Transform child in transform.Find("Name"))
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
            name.transform.SetParent(transform.Find("Name"), false);
            name.transform.localPosition = Vector3.zero;
            name.transform.localScale = 0.5f * Vector3.one;

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
            transform.Find("Name").GetComponent<Image>().color = c;
            c.a = 100 / 255.0f;
            transform.Find("Mask").GetComponent<Image>().color = c;

            ID = id;
        }
    }


}
