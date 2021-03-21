using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TargetRangeManager : MiniGameManager<TargetRangeManager>
{
    public List<GameObject> targetList;
    public GameObject prefabTarget;
    public int howManyTargetsPerTime = 1;
    public Hand leftHand, rightHand;
    public ItemPackageSpawner itemPackageSpawnerBow;
    public GrabTypes grabTypeBow;

    private float score;
    private Hand selectedHand;

    private void Start()
    {
        StartTargetRange();
    }

    [ContextMenu("Start Target Range")]
    void StartTargetRange()
    {
        score = 0;

        Debug.Log("Give VR bow to player here");
        if (selectedHand == null)
            selectedHand = rightHand;
        HandBow();
        for (int i = 0; i < howManyTargetsPerTime; i++)
        {
            var targetSubset = targetList.Where(x => !x.activeSelf).ToList();
            if (targetSubset.Count > 0)
            {
                targetSubset[Mathf.FloorToInt(Random.Range(0, targetSubset.Count * 1000000) / 1000000)].SetActive(true);
            }
        }
        StartMinigame();
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

    void TurnOffTargets()
    {
        foreach (GameObject go in targetList.Where(x => x.activeSelf))
        {
            go.SetActive(false);
        }
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
        TurnOffTargets();
    }

    public void OnTargetHit()
    {
        score += 10;
        Debug.Log("At least it's hit");
        var targetSubset = targetList.Where(x => !x.activeSelf).ToList();
        if (targetSubset.Count > 0)
        {
            targetSubset[Mathf.FloorToInt(Random.Range(0, targetSubset.Count * 1000000) / 1000000)].SetActive(true);
        }
    }
}
