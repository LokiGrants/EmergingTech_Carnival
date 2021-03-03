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
