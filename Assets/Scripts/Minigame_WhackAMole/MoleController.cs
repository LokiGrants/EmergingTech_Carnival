using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    [HideInInspector]
    public bool canNotPlay;
    [HideInInspector]
    public float mole_yPosAboveGround;
    [HideInInspector]
    public float mole_yPosUnderGround;
    [HideInInspector]
    public float mole_movementTime;
    [HideInInspector]
    public float mole_minTimeUp;
    [HideInInspector]
    public float mole_maxTimeUp;

    public void MoveMole()
    {
        StartCoroutine(MoveUpAndDownObject());
    }

    public IEnumerator MoveUpAndDownObject()
    {
        canNotPlay = true;
        Coroutine coroutine = StartCoroutine(
            MoveObject(transform.position, 
            new Vector3(transform.position.x, mole_yPosAboveGround, transform.position.z)));
        yield return new WaitUntil(() => transform.position.y >= mole_yPosAboveGround);
        StopCoroutine(coroutine);

        yield return new WaitForSeconds(
            Mathf.FloorToInt(
                Random.Range(mole_minTimeUp * 1000000, mole_maxTimeUp * 1000000) / 1000000));

        coroutine = StartCoroutine(
            MoveObject(transform.position, 
            new Vector3(transform.position.x, mole_yPosUnderGround, transform.position.z)));
        yield return new WaitUntil(() => transform.position.y <= mole_yPosUnderGround);
        canNotPlay = false;
    }

    public IEnumerator MoveObject(Vector3 initialPos, Vector3 finalPos)
    {
        float timeElapsed = 0;

        while (timeElapsed < mole_movementTime)
        {
            float newY = Mathf.Lerp(initialPos.y, finalPos.y, timeElapsed / mole_movementTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = finalPos;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HammerTime")
        {
            StopAllCoroutines();
            canNotPlay = false;
            //Particles
            //Insta move down
            transform.position = new Vector3(transform.position.x, mole_yPosUnderGround, transform.position.z);
            WhackMole_Manager.Instance.OnMoleHit();
        }
    }
}
