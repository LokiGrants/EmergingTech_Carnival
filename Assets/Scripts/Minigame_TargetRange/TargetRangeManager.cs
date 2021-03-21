using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TargetRangeManager : MiniGameManager<TargetRangeManager>
{
    public List<Transform> spawnParents;
    public Hand leftHand, rightHand;
    public ItemPackageSpawner itemPackageSpawnerBow;
    public GrabTypes grabTypeBow;

    private float score;
    private Hand selectedHand;

    [ContextMenu("Start Target Range")]
    void StartTargetRange()
    {
        score = 0;

        Debug.Log("Give VR bow to player here");
        if (selectedHand == null)
            selectedHand = rightHand;
        HandBow();
        StartMinigame();
        //Turn 4 on
    }

    void ChangeHand(Hand hand)
    {
        selectedHand = hand;
    }

    void HandBow()
    {
        itemPackageSpawnerBow.CallSpawnAndAttachObject(selectedHand, grabTypeBow);
    }

    void UnhandBow()
    {
        itemPackageSpawnerBow.CallTakeBackItem(selectedHand);
    }

    protected override void BeforeYield(float totalGameTime)
    {
        Debug.Log("Time Left " + Mathf.Floor(totalGameTime));
        
    }

    protected override void AfterYield(float totalGameTime)
    {
        //Turn all target off
    }

    protected override void AfterWhile(float totalGameTime)
    {
        Debug.Log("Total hits: " + score);
        UnhandBow();
    }

    public void OnTargetHit()
    {
        score += 10;
        Debug.Log("At least it's hit");
        //Turn another one on
    }
}
