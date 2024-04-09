using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    public TMP_Text storyText;
    public GameObject choicePanel;
    public TMP_Text choice1Text;
    public TMP_Text choice2Text;
    public GameObject resultPanel;
    public TMP_Text resultText;

    private bool isStoryDone = false;

    void Start()
    {
        StartStory();
    }

    public void StartStory()
    {
        StartCoroutine(ShowStoryCoroutine());
    }

    private IEnumerator ShowStoryCoroutine()
    {
        yield return TypeText("Once upon a time, in a distant land...");
        yield return new WaitForSeconds(1f);

        yield return TypeText("You are part of a brave expedition seeking...");
        yield return new WaitForSeconds(1f);

        yield return TypeText("As you journeyed through the forest, you came across...");
        yield return new WaitForSeconds(1f);

        choicePanel.SetActive(true);
        choice1Text.text = "Investigate the mysterious sound";
        choice2Text.text = "Continue on the path";

        isStoryDone = true;
    }

    private IEnumerator TypeText(string text)
    {
        storyText.text = "";
        foreach (char c in text)
        {
            storyText.text += c;
            yield return new WaitForSeconds(0.05f); // Adjust the speed here
        }
    }

    public void PlayerChoice(int choice)
    {
        choicePanel.SetActive(false);

        if (!isStoryDone) return;

        switch (choice)
        {
            case 1:
                StartCoroutine(InvestigateSound());
                break;
            case 2:
                StartCoroutine(ContinueOnPath());
                break;
            default:
                Debug.LogError("Invalid choice!");
                break;
        }
    }

    private IEnumerator InvestigateSound()
    {
        yield return TypeText("You decided to investigate the sound...");
        yield return new WaitForSeconds(1f);

        int randomNumber = UnityEngine.Random.Range(1, 11);
        if (randomNumber <= 5)
        {
            yield return TypeText("While investigating the sound you see a Gods Eye in the distance...");
            yield return new WaitForSeconds(1f);

            yield return TypeText("It notices you and charges at you...");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            yield return TypeText("You found a robot in the forest!");
            yield return new WaitForSeconds(1f);

            yield return TypeText("You brought the robot back to your settlement...");
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("SettlementPhase");
        }

        resultPanel.SetActive(true);
    }

    private IEnumerator ContinueOnPath()
    {
        yield return TypeText("You decided to continue on the path...");
        yield return new WaitForSeconds(1f);

        int randomNumber = UnityEngine.Random.Range(1, 11);
        if (randomNumber <= 5)
        {
            resultText.text = "Nothing eventful happened...";
        }
        else
        {
            resultText.text = "One of your team members got injured...";
            // Handle the event where a team member gets injured
        }

        resultPanel.SetActive(true);
    }

    public void LoadSettlementPhase()
    {
        SceneManager.LoadScene("SettlementPhase");
    }
}
