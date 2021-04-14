using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimonSaysManager : MiniGameManager<SimonSaysManager>
{
    public List<GameObject> simonButtons;
    public string musicToPlay;
    public int startingAmount = 3;
    public int scoreValue;

    private List<GameObject> selectedButtons;
    private int score;
    private bool isGameOn;

    private void Start()
    {
        //StartSimon();
    }

    [ContextMenu("Start Simon Says")]
    public void StartSimon()
    {
        if (GameMenuManager.Instance.IsPlayingMinigame())
            return;

        GameMenuManager.Instance.MinigameHasStarted();

        AudioManager.instance.PauseSound();
        AudioManager.instance.PlaySound(musicToPlay);

        selectedButtons = new List<GameObject>();
        score = 0;
        isGameOn = true;

        for (int i = 0; i < startingAmount; i++)
        {
            AddSelectedButton();
        }

        StartMinigame();
    }

    protected override void BeforeYield(float totalGameTime)
    {
        Debug.Log("Time Left " + Mathf.Floor(totalGameTime));
    }

    protected override void AfterYield(float totalGameTime)
    {
    }

    protected override void AfterWhile(float totalGameTime)
    {
        GameMenuManager.Instance.MinigameHasEnded();

        Debug.Log("Total hits: " + score);
        foreach(GameObject go in selectedButtons)
        {
            go.GetComponentInChildren<Renderer>().material.color = new Color(1f, 1f, 1f);
        }
        selectedButtons.Clear();

        ScoreManager.Instance.AddCurrentScore(score);

        AudioManager.instance.UnpauseSound();
        AudioManager.instance.StopSound(musicToPlay);
    }

    public void OnButtonPressed(GameObject simonButton)
    {
        if (isGameOn)
        {
            if (selectedButtons.Contains(simonButton))
            {
                score += scoreValue;
                selectedButtons.Remove(simonButton);
                simonButton.GetComponentInChildren<Renderer>().material.color = new Color(1f, 1f, 1f);
                AddSelectedButton();
            }
        }
    }

    private void AddSelectedButton()
    {
        if (isGameOn)
        {
            List<GameObject> simonButtonsMinusSelected = simonButtons.Except(selectedButtons).ToList();
            GameObject go = simonButtonsMinusSelected[Random.Range(0, simonButtonsMinusSelected.Count)];
            selectedButtons.Add(go);
            go.GetComponentInChildren<Renderer>().material.color = new Color(.25f, .25f, 1f);
        }
    }
}
