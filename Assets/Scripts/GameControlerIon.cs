using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControlerIon : GameController {

    //LevelManager LVM;
    //GameObject Controls;
 
    public int solution = 0;
    //public int failCount = 0;
    bool started = false;
    public ChargeSelectorManager Selector;
    public GameObject BtnValidate;

    /*public void Awake()
    {
        transform.localPosition = new Vector3(0, 0, 0);

        transform.GetComponent<Image>().enabled = false;

        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Controls = GameObject.FindGameObjectWithTag("Controls");
    }*/

    public override void ResetLevel()
    {
        base.ResetLevel();

        transform.Find("ChargeSelector").GetComponent<ChargeSelectorManager>().Reset();
        transform.parent.parent.GetComponent<resize>().InitResize(transform);
        BtnValidate.SetActive(true);
        Selector.ValidateChoice(false);
        Selector.Reset();

        ResetButton.SetActive(false);
        Controls.SetActive(false);

        
    }


    /*private void Start()
    {
        started = true;
    }

    private void OnEnable()
    {
        if (!started) return;


        Transform t = transform.parent.parent.Find("Check");
        Animator anim = t.GetComponent<Animator>();
        if (anim.isActiveAndEnabled) anim.SetTrigger("reset");

        t = transform.parent.parent.Find("Fail");
        anim = t.GetComponent<Animator>();
        if (anim.isActiveAndEnabled) anim.SetTrigger("reset");

        t = transform.parent.parent.Find("Warning");
        anim = t.GetComponent<Animator>();
        if (anim.isActiveAndEnabled) anim.SetTrigger("reset");

        Controls.SetActive(false);
        failCount = 0;
        transform.Find("ChargeSelector").GetComponent<ChargeSelectorManager>().Reset();
        transform.parent.parent.GetComponent<resize>().InitResize(transform);
        BtnValidate.SetActive(true);
        Selector.ValidateChoice(false);
        Selector.Reset();

    }*/

    override public void Validate()
    {
        Selector.ValidateChoice(true);
        if (Selector.Charge == solution) {
            WinLevel();
        }
        else
        {
            failCount++;
            StartCoroutine(FailAnimation());
        }
    }

    override public IEnumerator FailAnimation()
    {
        BtnValidate.SetActive(false);

        transform.parent.parent.GetComponent<resize>().ReZoom();
        transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.5f);
        BtnValidate.SetActive(true);
        Selector.ValidateChoice(false);


        //ShowTip();
    }

    override public void WinLevel()
    {
        BtnValidate.SetActive(false);

        Debug.Log("gagné");
        if (LVM.completedLevel < LVM.currentLevel + 1)
        {
            LVM.completedLevel = LVM.currentLevel + 1;
            Debug.Log("level+1");
        }

        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("SuccessTrigger");
    }

    /*public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LVM.ReactionSelector.SetActive(true);
            LVM.Game.SetActive(false);
        }
    }*/

}
