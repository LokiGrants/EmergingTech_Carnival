using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTestGame_Manager : MiniGameManager<BallThrowTestGame_Manager>
{
    public GameObject ballPrefab;
    public Transform ballPosition;
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
        score++;
    }

    protected override void AfterWhile(float totalGameTime)
    {
        gameOver = true;
        Debug.Log("Basket Score " + score);
        Destroy(currentBall);

        ScoreManager.Instance.AddCurrentScore(score * 10);
    }
}
