using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ActivitiesNomencatureManager : MonoBehaviour {

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
        rect.localPosition = new Vector3(0, 0, 0);
    }

    public void LoadTutorial()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Nomenclature/Tutorial");
        LVM.isExamSession = false;
        LVM.debug = true;

        LVM.SetLevels(levels);
        LVM.levelName = "Didacticiel";
        LVM.LoadLevel(0);

    }


    public void LoadGroupeSession()
    {
        List<GameObject> levels = new List<GameObject>();


        Debug.Log("list");
        TextAsset tt = Resources.Load("Nomenclature/Groupes/List") as TextAsset;

        List<string> fLines = new List<string>();
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));

        List<string> fLinesNoempty = new List<string>();
        foreach (string l in fLines)
            if (l != "")
                fLinesNoempty.Add(l);

        LVM.SetLevels(fLinesNoempty.ToArray());

        LVM.isExamSession = false;
        LVM.debug = false;

        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().training = true;

        LVM.levelName = "Molécule ";
        LVM.training = true;
    }

    
    public void LoadExamSession()
    {
        int exoNumber = 5;

        List<string> shortList = new List<string>();
        List<string> fLines = new List<string>();
        TextAsset tt;

        tt = Resources.Load("Nomenclature/Names/Alcane/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        tt = Resources.Load("Nomenclature/Names/Alcool/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        tt = Resources.Load("Nomenclature/Names/Cetone/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        
        List<GameObject> levels = new List<GameObject>();
        
        foreach (string l in fLines)
            if (l != "")
                shortList.Add(l);
        
        LVM.isExamSession = false;
        LVM.debug = false;

        for (int i = shortList.Count; i > exoNumber; i--)
        {
            int index = (int)Random.Range(0, i);
            shortList.RemoveAt(index);
        }
        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        LVM.isExamSession = true;
        LVM.debug = false;

        LVM.SetLevels(shortList.ToArray());
        LVM.levelName = "Molécule ";
    }

    public void LoadDebugSession()
    {
        List<string> shortList = new List<string>();
        List<string> fLines = new List<string>();
        TextAsset tt;

        tt = Resources.Load("Nomenclature/Names/Alcane/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        tt = Resources.Load("Nomenclature/Names/Alcool/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));
        tt = Resources.Load("Nomenclature/Names/Cetone/List") as TextAsset;
        fLines.AddRange(tt.text.Split(new char[] { '\r', '\n' }));

        List<GameObject> levels = new List<GameObject>();

        foreach (string l in fLines)
            if (l != "")
                shortList.Add(l);

        LVM.isExamSession = false;
        LVM.debug = false;
        
        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        //LVM.isExamSession = true;
        LVM.debug = true;

        LVM.SetLevels(shortList.ToArray());
        LVM.levelName = "Molécule ";
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("Themes");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }


}
