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
        GameObject[] levels = Resources.LoadAll<GameObject>("Nomenclature/Groupes");
        LVM.isExamSession = false;
        LVM.debug = false;

        LVM.SetLevels(levels);
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().training = true;
        LVM.levelName = "Molécule ";
        LVM.LoadLevel(0);
    }

    
    public void LoadExamSession()
    {
        int exoNumber = 5;

        List<GameObject> shortList = new List<GameObject>();

        
        GameObject[] groups = Resources.LoadAll<GameObject>("Nomenclature/Groupes");
        GameObject[] names = Resources.LoadAll<GameObject>("Nomenclature/Names");

        shortList.AddRange(groups);
        shortList.AddRange(names);


        for (int i = groups.Length + names.Length; i > exoNumber; i--)
        {
            int index = (int)Random.Range(0, i);
            shortList.RemoveAt(index);
        }
        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        LVM.isExamSession = true;
        LVM.debug = false;

        LVM.SetLevels(shortList.ToArray());
        LVM.levelName = "Molécule ";
        LVM.LoadLevel(0);
    }

    public void LoadDebugSession()
    {

        List<GameObject> all = new List<GameObject>();


        GameObject[] groups = Resources.LoadAll<GameObject>("Nomenclature/Groupes");
        GameObject[] names = Resources.LoadAll<GameObject>("Nomenclature/Names");

        all.AddRange(groups);
        all.AddRange(names);
 

        LVM.SetLevels(all.ToArray());
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().debug = true;

        LVM.isExamSession = false;
        LVM.debug = true;

        LVM.levelName = "";
        LVM.LoadLevel(0);
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
