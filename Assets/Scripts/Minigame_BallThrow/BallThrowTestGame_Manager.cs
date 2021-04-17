using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTestGame_Manager : MiniGameManager<BallThrowTestGame_Manager>
{
    public GameObject ballPrefab;
    public Transform ballPosition;
    public GameObject animationHitPrefab;
    public string musicToPlay;
    public int scoreValue;
    public float timeForBallReset;

    private int score;
    private bool gameOver = true;
    private GameObject currentBall;

    private void Start()
    {
        //StartBallThrow();
    }

    [ContextMenu("Start BallThrow")]
    public void StartBallThrow()
    {
        if (GameMenuManager.Instance.IsPlayingMinigame())
            return;

        GameMenuManager.Instance.MinigameHasStarted();

        AudioManager.instance.PauseSound();
        AudioManager.instance.PlaySound(musicToPlay);

        currentBall = Instantiate(ballPrefab, ballPosition);
        currentBall.GetComponent<BallController>().timeForReset = timeForBallReset;
        gameOver = false;
        StartMinigame();
    }

    public void Respawn()
    {
        if (!gameOver)
        {
            currentBall = Instantiate(ballPrefab, ballPosition);
            currentBall.GetComponent<BallController>().timeForReset = timeForBallReset;
        }
    }

    public void OnBasketHit()
    {
        GameObject go = Instantiate(animationHitPrefab, currentBall.transform.position, Quaternion.identity);
        go.GetComponentInChildren<AnimationDataAndController>().ScoreValueChange(scoreValue.ToString());

        score += scoreValue;
    }

    protected override void BeforeYield(float totalGameTime)
    {
        GameMenuManager.Instance.textAnimation.ChangeText(Mathf.Floor(totalGameTime).ToString("00"));
    }

    protected override void AfterWhile(float totalGameTime)
    {
        GameMenuManager.Instance.MinigameHasEnded();

        gameOver = true;
        Debug.Log("Basket Score " + score);
        Destroy(currentBall);

        ScoreManager.Instance.AddCurrentScore(score); 
        
        AudioManager.instance.UnpauseSound();
        AudioManager.instance.StopSound(musicToPlay);
    }
}
