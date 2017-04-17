using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour {

    public GameObject levelList;
    public GameObject button;
    public GameObject Playground;
    public GameObject levelManager;
    levelManager LVM;

    // Use this for initialization
    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);

        LVM = levelManager.GetComponent<levelManager>();

        //GameObject button = levelList.transform.GetChild(0).gameObject;

        foreach (Transform child in levelList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i=0; i < Playground.transform.childCount; i++)
        {
            GameObject go=Instantiate(button);
            go.transform.SetParent(levelList.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.FindChild("Text").transform.GetComponent<Text>().text = "Réaction " + (i + 1);

            int level = i;
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(delegate () { LVM.loadLevel(level); });
            
        }

    }

    public void Update()
    {
        int cpLevel = LVM.completedLevel;

        for (int i = 0; i < levelList.transform.childCount; i++)
        {
            if (LVM.debug || i < cpLevel+1)
                levelList.transform.GetChild(i).GetComponent<Button>().interactable=true;
            else
                levelList.transform.GetChild(i).GetComponent<Button>().interactable = false;

        }
    }


   /* public void loadLevel(int levelNumber)
    {
        if (levelNumber >= Playground.transform.childCount)
            return;

        for (int i = 0; i < Playground.transform.childCount; i++) 
        {
            Transform child=Playground.transform.GetChild(i);
            if (i == levelNumber)
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

        gameObject.SetActive(false); // desactive le menu

        levelManager.GetComponent<levelManager>().currentLevel = levelNumber;

    }*/


}
