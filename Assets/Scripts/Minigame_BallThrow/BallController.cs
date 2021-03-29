using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [HideInInspector]
    public float timeForReset = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Basket")
        {
            BallThrowTestGame_Manager.Instance.OnBasketHit();
        } else if (other.tag == "Minigame_Basket_BallDestroyer")
        {
            StopAllCoroutines();
            Destroy(gameObject);
            BallThrowTestGame_Manager.Instance.Respawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Minigame_Basket_BallDestroyer")
        {
            StopAllCoroutines();
            Destroy(gameObject);
            BallThrowTestGame_Manager.Instance.Respawn();
        }
    }

    public void OnBallReleased()
    {
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(timeForReset);
        Destroy(gameObject); //destroy the gameObject

        BallThrowTestGame_Manager.Instance.Respawn();
    }
}
