using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionSelectorManager : MonoBehaviour {

    public GameObject levelList;
    public GameObject button;
    public GameObject Playground;
    public GameObject levelManager;
    public GameObject Game;
    public GameObject LevelSelector;
    LevelManager LVM;

    bool started = false;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);

        LVM = levelManager.GetComponent<LevelManager>();

        CreateReactionSelector();

        started = true;
    }

    public void Update()
    {
        /*int cpLevel = LVM.completedLevel;

        for (int i = 0; i < levelList.transform.childCount; i++)
        {
            if (LVM.debug || i < cpLevel+1)
                levelList.transform.GetChild(i).GetComponent<Button>().interactable=true;
            else
                levelList.transform.GetChild(i).GetComponent<Button>().interactable = false;

        }*/


        if (Input.GetKeyDown(KeyCode.Escape)) {
            BackToLevelSelector();
        }
    }



    private void OnEnable()
    {
        if (started)
        {
            if (LVM.LevelName().Contains("Tuto") || LVM.LevelName().Contains("Dida"))
                BackToLevelSelector();
            else
                CreateReactionSelector();


        }
    }

    public void CreateReactionSelector() {

        foreach (Transform child in levelList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < LVM.reactions[LVM.currentLevel].Length; i++)
        {
            GameObject go = Instantiate(button);
            go.transform.SetParent(levelList.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.Find("Text").transform.GetComponent<Text>().text = (i + 1).ToString();

            int level = i;
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(delegate () { LVM.LoadReaction(level); });
        }

        /*Text title = transform.Find("Panel").transform.Find("Text").GetComponent<Text>();
        title.text = LVM.LevelName();//+ " - Sélectionner une réaction";*/

        //transform.Find("Panel").
        transform.GetComponent<Image>().color = LVM.LevelColor[LVM.currentLevel];

    }

    public void BackToLevelSelector()
    {
        gameObject.SetActive(false);
        LevelSelector.SetActive(true);
        Game.SetActive(false);
    }

}
