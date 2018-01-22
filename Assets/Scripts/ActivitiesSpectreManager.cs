using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ActivitiesSpectreManager : MonoBehaviour {

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
        GameObject[] levels = Resources.LoadAll<GameObject>("Spectre/Tutorial");
        LVM.isExamSession = false;

        LVM.SetLevels(levels);
        LVM.levelName = "Didacticiel";
        LVM.LoadLevel(0);

    }

    public void LoadIRSession()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Spectre/IR");
        LVM.isExamSession = false;
        LVM.SetLevels(levels);
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().training = true;
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void LoadGroupeSession()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Nomenclature/Groupes");
        LVM.isExamSession = false;
        LVM.SetLevels(levels);
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().training = true;
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    
    public void LoadExamSession()
    {
        int exoNumber = 5;

        List<GameObject> shortList = new List<GameObject>();

        
        GameObject[] ir = Resources.LoadAll<GameObject>("Spectre/IR");
        GameObject[] rmn = Resources.LoadAll<GameObject>("Spectre/RMN");

        shortList.AddRange(ir);
        shortList.AddRange(rmn);


        for (int i = shortList.Count; i > exoNumber; i--)
        {
            int index = (int)Random.Range(0, i);
            shortList.RemoveAt(index);
        }
        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        LVM.isExamSession = true;
        LVM.SetLevels(shortList.ToArray());
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void LoadDebugSession()
    {

        List<GameObject> all = new List<GameObject>();


        GameObject[] ir = Resources.LoadAll<GameObject>("Spectre/IR");
        GameObject[] rmn = Resources.LoadAll<GameObject>("Spectre/RMN");

        all.AddRange(ir);
        all.AddRange(rmn);


        LVM.SetLevels(all.ToArray());
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().debug = true;

        LVM.levelName = "level";
        LVM.LoadLevel(0);
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("Themes");
    }

}
