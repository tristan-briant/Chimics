using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public int currentLevel;
    public int currentReaction;

    public int completedLevel=0;
    public int maxLevel;
    public GameObject Playground;
    public GameObject ActivitiesSelector;
    public GameObject scoreBoard;
    public GameObject Game;

    [Header("Debug Mode")]
    public bool debug = false;

    public Transform[] levels;
    public string[] LevelNames;
    public string levelName;
    public Color[] LevelColor;
    public LevelParameters[] Parameters;
    public bool isExamSession = false;
    public bool training = false;

    private void Start()
    {  
        ActivitiesSelector.SetActive(true);
        scoreBoard.SetActive(false);
        Game.SetActive(false);
        GameObject.Find("/Canvas/WaitScreen").transform.localPosition = Vector3.zero;
        GameObject.Find("/Canvas/WaitScreen").gameObject.SetActive(false);

    }

    public void SetLevels(GameObject[] ls) {

        if (levels != null)
            foreach (Transform child in levels)
            {
                Destroy(child.gameObject);
            }

        levels = new Transform[ls.Length];

        int n = 0;
        foreach (GameObject level in ls)
        {
            Debug.Log(level);
            Transform lv = Instantiate<GameObject>(level).transform;
            lv.SetParent(Playground.transform);
            lv.localScale = new Vector3(1, 1, 1);
            lv.localPosition = new Vector3(0, 0, 0);
            lv.gameObject.SetActive(false);
            

            levels[n++] = lv;
        }

        LoadLevel(0);

    }

    public void SetLevels(string[] lnames)
    {


        if (levels != null)
            foreach (Transform child in levels)
            {
                Destroy(child.gameObject);
            }


        StartCoroutine(LoadSessionWait(lnames));

    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber >= levels.Length)
            return;
        //ActivitiesSelector.SetActive(false); // desactive le menu

        Game.SetActive(true);
        scoreBoard.SetActive(false);

        if(levelNumber>=0)  // if levelnumber =-1 reload level
            currentLevel = levelNumber;

        foreach (Transform child in Playground.transform)
            child.gameObject.SetActive(false);

        Transform lv = levels[currentLevel];

        lv.gameObject.SetActive(true);
        GameController gc = lv.GetComponent<GameController>();
        //gc.ResetLevel();
        gc.SetupLevel(true);
       
        SetTitlePanel();
    }

    public Transform CurrentLevel()
    {
        return levels[currentLevel];
    }

    public void LoadNextLevel()
    {

        if (currentLevel < levels.Length - 1)
        {
            currentLevel++;
            LoadLevel(currentLevel);
        }
        else
        {
            currentLevel = 0;
            LoadLevel(currentLevel);
        }

    }

    public void LoadPreviousLevel()
    {
        if (0 <currentLevel)
        {
            currentLevel--;
            LoadLevel(currentLevel);
        }
        else
        {
            currentLevel = levels.Length - 1;
            LoadLevel(currentLevel);
        }

    }

    public void LoadScoreBoard()
    {
        ActivitiesSelector.SetActive(false);
        scoreBoard.SetActive(true);
        Game.SetActive(false);
    }

    public string LevelName() {
        // Return the name of the current level

        return LevelNames[currentLevel];
    }

    void SetTitlePanel()
    {
        if (debug) levelName = CurrentLevel().name;
        string title = levelName + " " + (currentLevel + 1);

        Game.transform.Find("Panel/Title").GetComponent<Text>().text = title;

        if (!debug)
        {
            if (currentLevel == levels.Length - 1)
            {
                if (isExamSession)
                {
                    Game.transform.Find("Panel/NextLevel").gameObject.SetActive(false);
                    Game.transform.Find("Panel/End").gameObject.SetActive(true);
                }
                else
                {
                    Game.transform.Find("Panel/NextLevel").GetComponent<Button>().interactable = false;
                    Game.transform.Find("Panel/End").gameObject.SetActive(false);
                }
            }
            else
            {
                Game.transform.Find("Panel/NextLevel").gameObject.SetActive(true);
                Game.transform.Find("Panel/NextLevel").GetComponent<Button>().interactable = true;
                Game.transform.Find("Panel/End").gameObject.SetActive(false);
            }

            if (currentLevel == 0)
                Game.transform.Find("Panel/PreviousLevel").GetComponent<Button>().interactable = false;
            else
                Game.transform.Find("Panel/PreviousLevel").GetComponent<Button>().interactable = true;
        }
        else
        {
            Game.transform.Find("Panel/NextLevel").gameObject.SetActive(true);
            Game.transform.Find("Panel/NextLevel").GetComponent<Button>().interactable = true;
            Game.transform.Find("Panel/PreviousLevel").GetComponent<Button>().interactable = true;
            Game.transform.Find("Panel/End").gameObject.SetActive(false);

        }

    }


    IEnumerator LoadSessionWait(string[] lnames)
    {
        Transform waitScreen = GameObject.Find("/Canvas").transform.Find("WaitScreen");
        waitScreen.gameObject.SetActive(true);
        waitScreen.GetComponentInChildren<Slider>().value = 0;
        yield return null;

        List<GameObject> all = new List<GameObject>();

        levels = new Transform[lnames.Length];
        int total = lnames.Length;
        int n = 0;

        foreach(string name in lnames)
        {

            GameObject level = Resources.Load(name) as GameObject;
           
            Transform lv = Instantiate<GameObject>(level).transform;
            lv.SetParent(Playground.transform);
            lv.localScale = new Vector3(1, 1, 1);
            lv.localPosition = new Vector3(0, 0, 0);
            lv.gameObject.SetActive(false);
            lv.GetComponent<GameController>().training = training;
            lv.GetComponent<GameController>().debug = debug;

            waitScreen.GetComponentInChildren<Slider>().value = (float)n/total ;
            yield return null;
            levels[n++] = lv;

        }

        waitScreen.gameObject.SetActive(false);
        LoadLevel(0);

    }


}
