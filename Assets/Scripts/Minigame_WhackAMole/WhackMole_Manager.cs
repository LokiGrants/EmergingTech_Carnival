using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WhackMole_Manager : MiniGameManager<WhackMole_Manager>
{
    public List<MoleController> moles;
    public float minTimeBetweenMole = 2f;
    public float maxTimeBetweenMole = 5f;

    public float mole_yPosAboveGround;
    public float mole_yPosUnderGround;
    public float mole_movementTime;
    public float mole_minTimeUp;
    public float mole_maxTimeUp;

    public Hand leftHand, rightHand;
    public ItemPackageSpawner itemPackageSpawnerHammer;
    public GrabTypes grabTypeHammer;

    private int whacks;
    private float timeForNextMole = 0;
    private Hand selectedHand;

    private void Start()
    {
        StartWhacka();
    }

    [ContextMenu("Start Whacka")]
    void StartWhacka()
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

        if (selectedHand == null)
            selectedHand = rightHand;
        HandHammer();

        Debug.Log("Give VR hammer to player here");
        StartMinigame();
    }

    void ChangeHand(Hand hand)
    {
        selectedHand = hand;
    }

    void HandHammer()
    {
        itemPackageSpawnerHammer.CallSpawnAndAttachObject(selectedHand, grabTypeHammer);
    }

    void UnhandHammer()
    {
        itemPackageSpawnerHammer.CallTakeBackItem(selectedHand);
    }

    protected override void BeforeYield(float totalGameTime)
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
    }

    protected override void AfterYield(float totalGameTime)
    {
        timeForNextMole -= stepTime;
    }

    protected override void AfterWhile(float totalGameTime)
    {
        Debug.Log("Total hits: " + whacks);
    }

    public void OnMoleHit()
    {
        whacks += 1;
        selectedHand.TriggerHapticPulse(1000);
        Debug.Log("At least it's hit");
    }
}
