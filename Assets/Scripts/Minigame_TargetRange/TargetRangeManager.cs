using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TargetRangeManager : MiniGameManager<TargetRangeManager>
{
    public List<GameObject> targetList;
    public GameObject animationHitPrefab;
    public int howManyTargetsPerTime = 1; 
    public string musicToPlay;
    public Hand leftHand, rightHand;
    public ItemPackageSpawner itemPackageSpawnerBow;
    public GrabTypes grabTypeBow;

    public int scoreValue;

    public float dissolveTime = .5f;
    public float dissolveStepTime = .05f;

    private int score;
    private Hand selectedHand;
    private GameObject spawnedBow;

    private void Start()
    {
        //StartTargetRange();
    }

    [ContextMenu("Start Target Range")]
    public void StartTargetRange()
    {
        if (GameMenuManager.Instance.IsPlayingMinigame())
            return;

        GameMenuManager.Instance.MinigameHasStarted();

        AudioManager.instance.PauseSound();
        AudioManager.instance.PlaySound(musicToPlay);

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
        spawnedBow = itemPackageSpawnerBow.CallSpawnAndAttachObject(selectedHand, grabTypeBow);
        if (spawnedBow != null)
        {
            StartCoroutine(BowDissolve());
        }
    }

    IEnumerator BowDissolve(bool isDissolveOut = false)
    {
        float cutoffValue = 1f;

        if (isDissolveOut)
        {
            cutoffValue = 0f;
        }

        List<Renderer> childRenderer = spawnedBow.GetComponentsInChildren<Renderer>().ToList();

        if (isDissolveOut)
        {
            for (float f = 0; f <= dissolveTime; f += dissolveStepTime)
            {
                cutoffValue = f / dissolveTime;
                foreach (Renderer r in childRenderer)
                {
                    r.material.SetFloat("_Cutoff", cutoffValue);
                }
                yield return new WaitForSeconds(dissolveStepTime);
            }

            itemPackageSpawnerBow.CallTakeBackItem(selectedHand);
        }
        else
        {
            for (float f = 0; f <= dissolveTime + dissolveStepTime; f += dissolveStepTime)
            {
                cutoffValue = 1 - (f / dissolveTime);
                foreach (Renderer r in childRenderer)
                {
                    r.material.SetFloat("_Cutoff", cutoffValue);
                }
                yield return new WaitForSeconds(dissolveStepTime);
            }
        }
    }

    void UnhandBow()
    {
        if (spawnedBow != null)
        {
            StartCoroutine(BowDissolve(true));
        }
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
        GameMenuManager.Instance.MinigameHasEnded();

        Debug.Log("Total hits: " + score);
        UnhandBow();
        TurnOffTargets();

        ScoreManager.Instance.AddCurrentScore(score);

        AudioManager.instance.UnpauseSound();
        AudioManager.instance.StopSound(musicToPlay);
    }

    public void OnTargetHit(Transform hitObject)
    {
        GameObject go = Instantiate(animationHitPrefab, hitObject.position, Quaternion.identity);
        go.GetComponentInChildren<AnimationDataAndController>().ScoreValueChange(scoreValue.ToString());

        score += scoreValue;
        var targetSubset = targetList.Where(x => !x.activeSelf).ToList();
        Debug.Log("targetList " + targetList.Count);
        if (targetSubset.Count > 0)
        {
            targetSubset[Mathf.FloorToInt(Random.Range(0, targetSubset.Count * 1000000) / 1000000)].SetActive(true);
        }
    }
}
