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

        LVM.SetLevels(levels);
        LVM.levelName = "Didacticiel";
        LVM.LoadLevel(0);
        
    }

    public void LoadTrainingSession() {

        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Training");

        LVM.SetLevels(levels);
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void LoadExamSession()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        GameObject[] shortlist = new GameObject[1];
        shortlist[0] = levels[0];
        LVM.SetLevels(shortlist);
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Themes");
    }

}
