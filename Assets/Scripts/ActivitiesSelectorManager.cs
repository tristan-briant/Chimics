using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
        rect.localPosition = new Vector3(0, 0, 0);
    }

    public void LoadTutorial()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Tutorial");
        LVM.isExamSession = false;

        LVM.SetLevels(levels);
        LVM.levelName = "Didacticiel";
        LVM.LoadLevel(0);
        
    }

    public void LoadTrainingSession() {

        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Training");
        LVM.isExamSession = false;
        LVM.SetLevels(levels);
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void LoadExamSession()
    {
        int exoNumber = 5;

        List<GameObject> shortList = new List<GameObject>();

        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        shortList.AddRange(levels);

        for (int i = levels.Length; i > exoNumber; i--) {
            int index = (int) Random.Range(0, i);
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


        GameObject[] exam = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        GameObject[] tuto = Resources.LoadAll<GameObject>("Mecanismes/Tutorial");
        GameObject[] training = Resources.LoadAll<GameObject>("Mecanismes/Training");

        all.AddRange(tuto);
        all.AddRange(training);
        all.AddRange(exam);


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
