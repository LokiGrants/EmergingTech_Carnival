using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTestGame_Manager : MiniGameManager<BallThrowTestGame_Manager>
{
    public GameObject ballPrefab;
    public Transform ballPosition;
    public float timeForBallReset;

    private int basketScore;
    private bool gameOver = true;

    private void Start()
    {
        StartBallThrow();
    }

    [ContextMenu("Start BallThrow")]
    void StartBallThrow()
    {
        var newGameObject = Instantiate(ballPrefab, ballPosition);
        newGameObject.GetComponent<BallController>().timeForReset = timeForBallReset;
        gameOver = false;
        StartMinigame();
    }

    public void Respawn()
    {
        if (!gameOver)
        {
            var newGameObject = Instantiate(ballPrefab, ballPosition);
            newGameObject.GetComponent<BallController>().timeForReset = timeForBallReset;
        }
    }

    public void OnBasketHit()
    {
        basketScore += 100;
    }

    protected override void AfterWhile(float totalGameTime)
    {
        gameOver = true;
        Debug.Log("Basket Score " + basketScore);
    }
}
