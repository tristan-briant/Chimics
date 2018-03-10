using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        List<GameObject> levels=new List<GameObject>();

        if (Resources.Load("Nomenclature/Names/" + nameGroupe + "/List"))
        {
            Debug.Log("list");
            TextAsset tt = Resources.Load("Nomenclature/Names/" + nameGroupe + "/List") as TextAsset;

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
            levels.AddRange(Resources.LoadAll<GameObject>("Nomenclature/Names/" + nameGroupe));
            LVM.SetLevels(levels.ToArray());

        }
        LVM.isExamSession = false;
        LVM.training = true;
        LVM.debug = false;

        LVM.levelName = "Molécule ";
        //gameObject.SetActive(false);
        //LVM.LoadLevel(0);
    }


    public void BackToMenu()
    {
        transform.parent.Find("ActivitiesSelector").gameObject.SetActive(true);
        gameObject.SetActive(false);
        
    }

}
