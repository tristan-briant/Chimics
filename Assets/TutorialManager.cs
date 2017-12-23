using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    Animator anim;
    GameObject[] Buttons;
    GameObject ResetButton;
    public GameObject ReadMoreButton;
    levelManager LVM;

    void Awake () {
        anim = GetComponent<Animator>();
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        ResetButton = GameObject.FindGameObjectWithTag("Reset");
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();

        transform.GetComponent<Image>().enabled = false;
        transform.localPosition = new Vector3(0, 0, 0);

    }

    private void OnEnable()
    {
        Button btn = ReadMoreButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(ReadMore);
    }

    public void ReadMore() {
        anim.SetTrigger("Next");
    }

    public void ReadMoreToNext() {

        Button btn = ReadMoreButton.GetComponent<Button>();
        //btn.onClick.AddListener(delegate () { LVM.LoadNextReaction(); });
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(LVM.LoadNextReaction);
    }

}
