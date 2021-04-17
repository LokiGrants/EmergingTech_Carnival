using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimonSaysManager : MiniGameManager<SimonSaysManager>
{
    public List<GameObject> simonButtons;
    public GameObject animationHitPrefab;
    public string musicToPlay;
    public int startingAmount = 3;
    public int scoreValue;

    public List<Color> buttonColors;
    public Color baseColor;

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
        GameMenuManager.Instance.textAnimation.ChangeText(Mathf.Floor(totalGameTime).ToString("00"));
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
            go.GetComponentInChildren<Renderer>().material.color = baseColor;
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
                GameObject go = Instantiate(animationHitPrefab, simonButton.transform.position, Quaternion.identity);
                go.GetComponentInChildren<AnimationDataAndController>().ScoreValueChange(scoreValue.ToString());

                score += scoreValue;
                selectedButtons.Remove(simonButton);
                simonButton.GetComponentInChildren<Renderer>().material.color = baseColor;
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
            go.GetComponentInChildren<Renderer>().material.color = buttonColors[Random.Range(0, buttonColors.Count)];
        }
    }
}
