using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScoreBoard : GameController {

    public GameObject ScoreButton;
    public GameObject reviewButton;

    public override void SetupLevel(bool playable)
    {
        base.SetupLevel(playable);
        FloatingButtons.transform.Find("Clear").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Reset").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Validate").gameObject.SetActive(false);
        FloatingButtons.transform.Find("Correction").gameObject.SetActive(false);
        FloatingButtons.transform.Find("NextStep").gameObject.SetActive(false);
        FloatingButtons.transform.Find("PreviousStep").gameObject.SetActive(false);
    }

    public void Correction()
    {

        int score = 0;

        for (int i = 0; i < LVM.levels.Length - 1; i++) 
        {
            Transform level = LVM.levels[i];
            level.gameObject.SetActive(true);
            score+=level.GetComponent<GameController>().Score();
            level.GetComponent<GameController>().ShowCorrection();
            level.GetComponent<GameController>().corrected = true;
            level.GetComponent<GameController>().ClickableDisable();
            level.gameObject.SetActive(false);
        }

        score = score / (LVM.levels.Length - 1);

        ScoreButton.GetComponent<Button>().interactable=false;
        ScoreButton.transform.Find("Text").GetComponent<Text>().text = "Score : " + score + "%";

        reviewButton.transform.Find("Text").GetComponent<Text>().text = "Voir la correction";

    }

    public void Review() {
        LVM.LoadLevel(0);
    }

}
