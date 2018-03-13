using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ActivitiesSelectorManager : MonoBehaviour {

    public GameObject playground;
    public GameObject levelManager;
    LevelManager LVM;

    private void Awake()
    {
        LVM = levelManager.GetComponent<LevelManager>();
    }

    private void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        transform.parent.Find("WaitScreen").localPosition = Vector3.zero;
         transform.parent.Find("WaitScreen").gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void LoadTutorial()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Tutorial");
        LVM.isExamSession = false;
        LVM.debug = false;
        LVM.SetLevels(levels);
        LVM.levelName = "Didacticiel";
        LVM.LoadLevel(0);
        
    }

    public void LoadTrainingSession() {

        List<GameObject> levels = new List<GameObject>();

        if (Resources.Load("Mecanismes/Training/List"))
        {
            Debug.Log("list");
            TextAsset tt = Resources.Load("Mecanismes/Training/List") as TextAsset;

            List<string> fLines = new List<string>();
            fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));

            List<string> fLinesNoempty = new List<string>();
            foreach (string l in fLines)
                if (l != "")
                    fLinesNoempty.Add(l);

            LVM.SetLevels(fLinesNoempty.ToArray());
        }
        else
        {
            levels.AddRange(Resources.LoadAll<GameObject>("Mecanismes/Training/List"));
            LVM.SetLevels(levels.ToArray());

        }

        
        LVM.isExamSession = false;
        LVM.debug = false;
        LVM.training = true;
        
        LVM.levelName = "Réaction";
    }

    public void LoadExamSession()
    {
        int exoNumber = 5;

        List<string> shortList = new List<string>();
        List<string> fLines = new List<string>();
        TextAsset tt;

        tt = Resources.Load("Mecanismes/Exam/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        tt = Resources.Load("Mecanismes/Doublets/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));


        List<GameObject> levels = new List<GameObject>();

        foreach (string l in fLines)
            if (l != "")
                shortList.Add(l);

        LVM.isExamSession = true;
        LVM.debug = false;

        for (int i = shortList.Count; i > exoNumber; i--)
        {
            int index = (int)Random.Range(0, i);
            shortList.RemoveAt(index);
        }
        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        LVM.isExamSession = true;
        LVM.debug = false;
        LVM.training = false;

        LVM.SetLevels(shortList.ToArray());
        LVM.levelName = "Réaction ";

    }

    public void LoadDebugSession()
    {
        /*List<string> shortList = new List<string>();
        List<string> fLines = new List<string>();
        TextAsset tt;

        tt = Resources.Load("Mecanismes/Exam/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        tt = Resources.Load("Mecanismes/Doublets/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));


        List<GameObject> levels = new List<GameObject>();

        foreach (string l in fLines)
            if (l != "")
                shortList.Add(l);
                */


        List<string> levelList = new List<string>();

        levelList.AddRange(GetLevelList("Mecanismes/Exam"));
        levelList.AddRange(GetLevelList("Mecanismes/Doublets"));

        LVM.isExamSession = true;
        LVM.debug = true;
        
        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        //LVM.isExamSession = true;
        //LVM.debug = false;

        LVM.SetLevels(levelList.ToArray());
        LVM.levelName = "Réaction ";

    }


    string[] GetLevelList(string path)
    {
        List<string> fLines = new List<string>();
        List<string> shortList = new List<string>();

        TextAsset tt = Resources.Load(path + "/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));

        foreach (string l in fLines)
            if (l != "")
                shortList.Add(l);

        return shortList.ToArray();

    }


    public void BackToMenu()
    {
        gameObject.SetActive(false);

    //  SceneManager.LoadScene("Themes");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    IEnumerator LoadDebugSessionWait()
    {
        yield return null;
        List<GameObject> all = new List<GameObject>();

        int k = 0;
        GameObject[] exam = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text+="."; yield return null;
        GameObject[] tuto = Resources.LoadAll<GameObject>("Mecanismes/Tutorial");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text += "."; yield return null;
        GameObject[] training = Resources.LoadAll<GameObject>("Mecanismes/Training");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text += "."; yield return null;
        GameObject[] doublets = Resources.LoadAll<GameObject>("Mecanismes/Doublets");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text += "."; yield return null;
        all.AddRange(tuto);
        all.AddRange(training);
        all.AddRange(exam);
        all.AddRange(doublets);

        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text += "."; yield return null;

        LVM.SetLevels(all.ToArray());
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().debug = true;

        LVM.isExamSession = false;
        LVM.debug = true;
        LVM.levelName = "level";
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text += "."; yield return null;
        
        transform.parent.Find("WaitScreen").gameObject.SetActive(false);
        LVM.LoadLevel(0);
        
    }


}
