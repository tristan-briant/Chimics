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

        GameObject[] levels = Resources.LoadAll<GameObject>("Mecanismes/Training");
        LVM.isExamSession = false;
        LVM.debug = false;
        LVM.SetLevels(levels);
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void LoadExamSession()
    {
        int exoNumber = 5;
        

        List<GameObject> shortList = new List<GameObject>();

        GameObject[] exam = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        GameObject[] doublets = Resources.LoadAll<GameObject>("Mecanismes/Doublets");


        shortList.AddRange(exam);
        shortList.AddRange(doublets);


        for (int i = shortList.Count; i > exoNumber; i--) {
            int index = (int) Random.Range(0, i);
            shortList.RemoveAt(index);
        }

        LVM.scoreBoard.GetComponent<ScoreBoardManager>().ResetBoard();
        LVM.isExamSession = true;
        LVM.debug = false;
        LVM.SetLevels(shortList.ToArray());
        LVM.levelName = "Réaction";
        LVM.LoadLevel(0);
    }

    public void LoadDebugSession()
    {
       
        transform.parent.Find("WaitScreen").gameObject.SetActive(true);
        StartCoroutine(LoadDebugSessionWait());
        //gameObject.SetActive(false);
        /*List<GameObject> all = new List<GameObject>();


        GameObject[] exam = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        GameObject[] tuto = Resources.LoadAll<GameObject>("Mecanismes/Tutorial");
        GameObject[] training = Resources.LoadAll<GameObject>("Mecanismes/Training");
        GameObject[] doublets = Resources.LoadAll<GameObject>("Mecanismes/Doublets");

        all.AddRange(tuto);
        all.AddRange(training);
        all.AddRange(exam);
        all.AddRange(doublets);


        LVM.SetLevels(all.ToArray());
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().debug = true;

        LVM.isExamSession = false;
        LVM.debug = true;

        LVM.levelName = "level";

        transform.parent.Find("WaitScreen").gameObject.SetActive(false);
        LVM.LoadLevel(0);
 */
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

    IEnumerator LoadDebugSessionWait()
    {
        yield return null;
        List<GameObject> all = new List<GameObject>();

        int k = 0;
        GameObject[] exam = Resources.LoadAll<GameObject>("Mecanismes/Exam");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text=k.ToString(); k++;
        yield return null;


        GameObject[] tuto = Resources.LoadAll<GameObject>("Mecanismes/Tutorial");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text = k.ToString(); k++;
        yield return null;

        GameObject[] training = Resources.LoadAll<GameObject>("Mecanismes/Training");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text = k.ToString(); k++;
        yield return null;

        GameObject[] doublets = Resources.LoadAll<GameObject>("Mecanismes/Doublets");
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text = k.ToString(); k++;
        yield return null;


        all.AddRange(tuto);
        all.AddRange(training);
        all.AddRange(exam);
        all.AddRange(doublets);

        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text = k.ToString(); k++;
        yield return null;



        LVM.SetLevels(all.ToArray());
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().debug = true;

        LVM.isExamSession = false;
        LVM.debug = true;

        LVM.levelName = "level";
        transform.parent.Find("WaitScreen").GetComponentInChildren<Text>().text = k.ToString(); k++;
        yield return null;



        transform.parent.Find("WaitScreen").gameObject.SetActive(false);
        LVM.LoadLevel(0);
        
    }


}
