using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTestGame_Manager : BallThrowManager<BallThrowTestGame_Manager>
{
    public GameObject ballPrefab;
    public Transform ballPosition;
    public float timeForBallReset;

    protected int basketScore;

    [ContextMenu("Start BallThrow")]
    void StartBallThrow()
    {
        Debug.Log("Balls left " + livesCount);
        var newGameObject = Instantiate(ballPrefab, ballPosition);
        newGameObject.GetComponent<BallController>().timeForReset = timeForBallReset;
    }

    public void Respawn()
    {
        livesCount--;

        if (livesCount > 0)
        {
            Debug.Log("Balls left " + livesCount);
            var newGameObject = Instantiate(ballPrefab, ballPosition);
            newGameObject.GetComponent<BallController>().timeForReset = timeForBallReset;
        } else
        {
            Debug.Log("<color=red>ZERO BALLS LEFT</color>");
        }
    }

    public void OnBasketHit()
    {
        basketScore += 100;
        Debug.Log("Basket Score " + basketScore);
    }
}
