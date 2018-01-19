using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : GameController
{

    public GameObject ReadMoreButton;
    bool started = false;

    public override void LateUpdate()
    {
    }


    /*void Awake()
    {
        anim = GetComponent<Animator>();
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        ResetButton = GameObject.Find("FloatingButtons/Reset");
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        transform.GetComponent<Image>().enabled = false;
        transform.localPosition = new Vector3(0, 0, 0);

    }*/


    /*override public void Start()
    {
        started = true;
    }

    private void OnEnable()
    {
        if (!started) return;

        ResetLevel();
    }*/

    public void ReadMore()
    {
        Debug.Log("More !");
        if (anim.isActiveAndEnabled)
            anim.SetTrigger("Next");
    }

    public void ReadMoreToNext()
    {
        Button btn = ReadMoreButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(LVM.LoadNextLevel);
    }

    public void LoadNext()
    {
        LVM.LoadNextLevel();
    }


    /*override public void ResetLevel()
    {

        // On reset l'animation
        anim = GetComponent<Animator>();
        if (anim && anim.isActiveAndEnabled)
            anim.SetTrigger("reset");

        // si on load le level on enlève un éventuel check

        if (transform.parent)
        {
            Transform t = transform.parent.parent.Find("Check");
            Animator a = t.GetComponent<Animator>();
            if (a.isActiveAndEnabled) a.SetTrigger("reset");

            t = transform.parent.parent.Find("Fail");
            a = t.GetComponent<Animator>();
            if (a.isActiveAndEnabled) a.SetTrigger("reset");

            t = transform.parent.parent.Find("Warning");
            a = t.GetComponent<Animator>();
            if (a.isActiveAndEnabled) a.SetTrigger("reset");

            transform.parent.parent.GetComponent<resize>().InitResize(transform);

        }

        Button btn = ReadMoreButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(ReadMore);

        ResetButton.SetActive(false);

        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(false);
        }

    }*/

    public override void SetupLevel(bool playable)
    {
        base.SetupLevel(playable);

        FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        FloatingButtons.transform.Find("NextStep").gameObject.SetActive(false);
        FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Correction").gameObject.SetActive(false);
    }


}
