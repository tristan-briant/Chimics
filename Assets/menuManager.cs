using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour {

    public GameObject levelList;
    public GameObject button;
    public GameObject Playground;
    public GameObject levelManager;
    public GameObject level;
    levelManager LVM;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);

        LVM = levelManager.GetComponent<levelManager>();

        foreach (Transform child in levelList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i=0; i < LVM.maxLevel; i++)
     
        {
            GameObject go=Instantiate(button);
            go.transform.SetParent(levelList.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.Find("Text").transform.GetComponent<Text>().text = "Réaction " + (i + 1);

            int level = i;
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(delegate () { LVM.LoadLevel(level); });
            
        }

        level.gameObject.SetActive(false);
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


        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }


}
