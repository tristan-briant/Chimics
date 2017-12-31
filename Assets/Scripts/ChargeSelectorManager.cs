using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChargeSelectorManager : MonoBehaviour {

    public int Charge;
    int ChargeMax = 3;
    public GameObject Scroller;
    float CellHeigh;
    float StartTime;
    float ScrollTime = 0.2f;
    int OldCharge;
    public bool validated;
    GameObject BtnUp, BtnDown;

    public void Awake()
    {
        BtnUp = transform.Find("ButtonUp").gameObject;
        BtnDown = transform.Find("ButtonDown").gameObject;
    }

    public void Start()
    {
        Reset();
        CellHeigh = Scroller.transform.GetComponent<GridLayoutGroup>().cellSize.y;
    }


    public void ChangeCharge(int increment)
    {
        OldCharge = Charge;
        Charge = Mathf.Clamp(Charge + increment, -ChargeMax, ChargeMax);
        StartTime = Time.time;
    }


    private void Update()
    {
        float x = Mathf.Clamp01((Time.time-StartTime) / ScrollTime);
        float r = x * Charge + (1 - x) * OldCharge;

        Scroller.transform.localPosition = new Vector3(0, -CellHeigh * r, 0);

    
        int c = ChargeMax;
        foreach (Transform child in Scroller.transform)
        {
            float alpha;
            if (!validated)
            {
                if (c == r) alpha = 1;
                else
                   alpha = Mathf.Clamp01(0.3f / Mathf.Abs(c - r) + 0.1f);
                   
            }
            else
            {
                if (c == Charge) alpha = 1;
                else alpha = 0;
            }
            child.GetComponent<CanvasGroup>().alpha = alpha;
            c--;
        }


    }

    public void Reset()
    {
        OldCharge = Charge = 0;
        Scroller.transform.localPosition = new Vector3(0, 0, 0);
        StartTime = Time.time;
        validated = false;
    }

    public void ValidateChoice(bool val)
    {
        validated = val;
        BtnUp.SetActive(!val);
        BtnDown.SetActive(!val);
    }



}
