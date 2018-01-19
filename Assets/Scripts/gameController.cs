using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
    protected List<GameObject> accepteurs = new List<GameObject>();
    protected List<GameObject> doublets = new List<GameObject>();
    public int failCount; // nombre d'echec sur le level en cours
    public int step;    // Pour les réactions mutli étapes, n° de l'étape
    public int stepNumber; // Pour les réactions mutli étapes, nombre d'étapes

    protected LevelManager LVM;
    public Transform Tips;
    protected Animator anim;
    protected GameObject[] Buttons;
    public GameObject FloatingButtons;
    protected GameObject ResetButton;
    GameObject ValidateButton;
    GameObject ClearButton;
    protected GameObject Controls;

    public GameObject Help;
    public bool training=false;

    virtual public void Start()
    {
        
    }

    

    virtual public void Awake () { 
        LVM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Tips = transform.Find("Tips");
        bool gameActive = LVM.Game.activeSelf;
        LVM.Game.SetActive(true);
        FloatingButtons = GameObject.FindGameObjectWithTag("Controls");
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
        Controls= GameObject.FindGameObjectWithTag("Controls");
        ResetButton = GameObject.FindGameObjectWithTag("Reset");
        //ResetButton = GameObject.Find("FloatingButtons/Reset");
        LVM.Game.SetActive(gameActive);


        transform.localPosition = new Vector3(0, 0, 0);

        transform.GetComponent<Image>().enabled = false;
        anim = GetComponent<Animator>();

        // Enlève le texte pour ne pas être visible sur le jeu
        Transform sol = transform.Find("Solutions");
        if (sol && sol.GetComponent<Text>())
            sol.GetComponent<Text>().enabled = false;
    }

	
	virtual public void LateUpdate () {

    }

    virtual public void Validate()
    {
        
    }

    virtual public int Score() {
        // Evalue le score  entre 0 et 100;
        return 100;
    }

    virtual public void ShowCorrection() {
        // Corrige l'exercice
    }

    virtual public void UnselectElements() {
        foreach (ElementManager em in gameObject.GetComponentsInChildren<ElementManager>())
        {
           em.unSelectElement();
        }
    }

    virtual public void ResetElements()
    {
        foreach (ElementManager em in gameObject.GetComponentsInChildren<ElementManager>())
        {
            em.reset();
        }
    }

    virtual public IEnumerator WarningAnimation(string message)
    {
        transform.parent.parent.GetComponent<resize>().ReZoom();
        ClickableDisable();
        UnselectElements();
        transform.parent.parent.Find("Warning/Text").transform.GetComponent<Text>().text = message; // "Réaction incomplète";
        transform.parent.parent.Find("Warning").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.0f);
        ClickableEnable();
        ShowTip();
    }

    virtual public IEnumerator FailAnimation()
    {
        transform.parent.parent.GetComponent<resize>().ReZoom();
        ClickableDisable();
        UnselectElements();
        transform.parent.parent.Find("Fail").GetComponent<Animator>().SetTrigger("FailTrigger");
        yield return new WaitForSeconds(1.5f);
        //ClearLevel();
        ClickableEnable();
        ShowTip();
    }


    virtual public void WinLevel(){

        Debug.Log("gagné");

        if (LVM.completedLevel < LVM.currentLevel + 1)
            {
                LVM.completedLevel = LVM.currentLevel + 1;
            }

        transform.parent.parent.Find("Check").GetComponent<Animator>().SetTrigger("SuccessTrigger");

        //ResetButton = GameObject.FindGameObjectWithTag("Reset");
        //ResetButton.SetActive(true);
    }

   


    virtual public void ResetLevel() {

        SetupLevel(true);

        ClearLevel();
    }

    virtual public void ClearLevel()
    {
        Debug.Log("Clear Level");
        ResetElements(); // déselectionne les éléments

    }

    virtual public void SetupLevel(bool playable)  // Set up the level interactable or just to show correction
    {
        Debug.Log("reset base");
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

        failCount = 0;
        step = 0;

        ShowTip();

        if (Help != null)
        {
            LVM.Game.transform.Find("Panel/ShowHelp").gameObject.SetActive(true);
            Transform helpFrame = LVM.Game.transform.Find("HelpFrame");
            Help.transform.SetParent(helpFrame);
            Help.transform.localPosition = Vector3.zero;
        }
        else
        {
            Transform helpButton = LVM.Game.transform.Find("Panel/ShowHelp");
            if (helpButton) helpButton.gameObject.SetActive(false);
        }
    }

    public void ShowTip()
    {
        if (!Tips) return; // Si pas de Tips pas de Tips !

        for(int i = 0; i < Tips.childCount; i++)
        {
            Transform tip= Tips.GetChild(i);
            TipManager tm = tip.GetComponent<TipManager>();

            if (!tm || (tm.ShowInStep != step && tm.ShowInStep >= 0))
            {
                tip.gameObject.SetActive(false);
                return;
            }

            if (failCount >= tm.ShowAfterNFail && ( failCount < tm.HideAfterNFail || tm.HideAfterNFail < 0))
            {
                tip.gameObject.SetActive(true);
            }
            else
            {
                tip.gameObject.SetActive(false);
            }

        }
    }

    public void RemoveTip()
    {
        if (!Tips) return; // Si pas de Tips pas de Tips !

        for (int i = 0; i < Tips.childCount; i++)
        {
            Transform tip = Tips.GetChild(i);
            tip.gameObject.SetActive(false);
        }
    }


    public void ClickableDisable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(false);
        }

    }

    public void ClickableEnable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        foreach (GameObject ob in Buttons)
        {
            ob.SetActive(true);
        }
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LVM.ActivitiesSelector.SetActive(true);
            LVM.Game.SetActive(false);
        }
    }

   

}

