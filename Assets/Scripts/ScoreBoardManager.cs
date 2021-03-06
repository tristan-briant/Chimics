﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {

    public GameObject playground;
    public GameObject levelManager;
    LevelManager LVM;

    public GameObject ScoreButton;
    public GameObject reviewButton;
    public GameObject CorrectionButton;

    bool started = false;
    bool corrected = false;


    private void Awake()
    {
        LVM = levelManager.GetComponent<LevelManager>();
    }

    private void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        started = true;
    }


    private void OnEnable()
    {
        if(!started) return;

        if (!corrected)
        {
            int score = 0;
            LVM.Game.SetActive(true);

            for (int i = 0; i < LVM.levels.Length; i++)
            {
                Transform level = LVM.levels[i];
                level.gameObject.SetActive(true);
                score += level.GetComponent<GameController>().Score();
                //level.GetComponent<GameController>().ShowCorrection();
                //level.GetComponent<GameController>().corrected = true;
                level.GetComponent<GameController>().ClickableDisable();
                level.gameObject.SetActive(false);
            }

            LVM.Game.SetActive(false);

            score = score / (LVM.levels.Length);
            
            ScoreButton.transform.Find("Text").GetComponent<Text>().text = "Score : " + score + "%";
        }
    }


    public void Correction()
    {

        int score = 0;
        LVM.Game.SetActive(true);

        for (int i = 0; i < LVM.levels.Length; i++)
        {
            Transform level = LVM.levels[i];
            level.gameObject.SetActive(true);
            score += level.GetComponent<GameController>().Score();
            level.GetComponent<GameController>().ShowCorrection();
            level.GetComponent<GameController>().corrected = true;
            level.GetComponent<GameController>().ClickableDisable();
            level.gameObject.SetActive(false);
        }

        LVM.Game.SetActive(false);

        score = score / (LVM.levels.Length );

        //ScoreButton.GetComponent<Button>().interactable = false;
        ScoreButton.transform.Find("Text").GetComponent<Text>().text = "Score : " + score + "%";

        reviewButton.transform.Find("Text").GetComponent<Text>().text = "Voir la correction";
        corrected = true;
    }

    public void ResetBoard()
    {
        corrected = false;
        //ScoreButton.GetComponent<Button>().interactable = true;
        CorrectionButton.GetComponent<Button>().interactable = true;
        //ScoreButton.transform.Find("Text").GetComponent<Text>().text = "Corriger la série";

        reviewButton.transform.Find("Text").GetComponent<Text>().text = "Se relire";
    }


    public void Review()
    {
        LVM.LoadLevel(0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LVM.ActivitiesSelector.SetActive(true);
            LVM.Game.SetActive(false);
            LVM.scoreBoard.SetActive(false);
        }
    }
}
