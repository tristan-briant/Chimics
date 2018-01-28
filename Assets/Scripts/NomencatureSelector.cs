using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NomencatureSelector : MonoBehaviour {

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
        gameObject.SetActive(false);
    }

    public void LoadNameSession(string nameGroupe)
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Nomenclature/Names/"+ nameGroupe);
        LVM.isExamSession = false;
        LVM.debug = false;

        LVM.SetLevels(levels);
        foreach (Transform lv in LVM.levels)
            lv.GetComponent<GameController>().training = true;
        LVM.levelName = "Molécule ";
        //gameObject.SetActive(false);
        LVM.LoadLevel(0);
    }


    public void BackToMenu()
    {
        transform.parent.Find("ActivitiesSelector").gameObject.SetActive(true);
        gameObject.SetActive(false);
        
    }

}
