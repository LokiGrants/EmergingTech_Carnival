using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhackMole_Manager : Singleton<WhackMole_Manager>
{
    public float timeOfGameplayInSeconds;
    public List<MoleController> moles;
    public float minTimeBetweenMole = 2f;
    public float maxTimeBetweenMole = 5f;
    public float stepTime = 0.1f;

    public float mole_yPosAboveGround;
    public float mole_yPosUnderGround;
    public float mole_movementTime;
    public float mole_minTimeUp;
    public float mole_maxTimeUp;

    private int whacks;
    private float timeForNextMole = 0;

    [ContextMenu("Start Whacka")]
    void StartMinigame()
    {
        whacks = 0;

        foreach (MoleController mc in moles)
        {
            mc.mole_yPosAboveGround = mole_yPosAboveGround;
            mc.mole_yPosUnderGround = mole_yPosUnderGround;
            mc.mole_movementTime = mole_movementTime;
            mc.mole_minTimeUp = mole_minTimeUp;
            mc.mole_maxTimeUp = mole_maxTimeUp;
        }

        Debug.Log("Give VR hammer to player here");
        StartCoroutine(MiniGameController(timeOfGameplayInSeconds));
    }

    IEnumerator MiniGameController(float totalGameTime)
    {
        while (totalGameTime > 0)
        {
            Debug.Log("Time Left " + Mathf.Floor(totalGameTime));
            if (timeForNextMole <= 0)
            {
                timeForNextMole = Mathf.FloorToInt(Random.Range(minTimeBetweenMole * 1000000, maxTimeBetweenMole * 1000000) / 1000000);
                var moleSubset = moles.Where(x => !x.canNotPlay).ToList();
                if (moleSubset.Count > 0)
                {
                    moleSubset[Mathf.FloorToInt(Random.Range(0, moleSubset.Count * 1000000) / 1000000)].MoveMole();
                }
            }

            yield return new WaitForSeconds(stepTime);
            timeForNextMole -= stepTime;
            totalGameTime -= stepTime;
        }

        Debug.Log("Total hits: " + whacks);
    }

    public void OnMoleHit()
    {
        whacks += 1;
    }
}
