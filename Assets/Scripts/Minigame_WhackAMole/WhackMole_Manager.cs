using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WhackMole_Manager : MiniGameManager<WhackMole_Manager>
{
    public List<MoleController> moles;
    public GameObject animationHitPrefab;
    public float positionCorrectionAnimation;
    public string musicToPlay;
    public float minTimeBetweenMole = 2f;
    public float maxTimeBetweenMole = 5f;
    public int scoreValue;

    public float mole_yPosAboveGround;
    public float mole_yPosUnderGround;
    public float mole_movementTime;
    public float mole_minTimeUp;
    public float mole_maxTimeUp;

    public Hand leftHand, rightHand;
    public ItemPackageSpawner itemPackageSpawnerHammer;
    public GrabTypes grabTypeHammer;
    public float dissolveTime = .5f;
    public float dissolveStepTime = .05f;

    private int score;
    private float timeForNextMole = 0;
    private Hand selectedHand;
    private GameObject spawnedHammer;

    private void Start()
    {
        //StartWhacka();
    }

    [ContextMenu("Start Whacka")]
    public void StartWhacka()
    {
        if (GameMenuManager.Instance.IsPlayingMinigame())
            return;

        GameMenuManager.Instance.MinigameHasStarted();

        AudioManager.instance.PauseSound();
        AudioManager.instance.PlaySound(musicToPlay);

        score = 0;

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

        StartMinigame();
    }

    void ChangeHand(Hand hand)
    {
        selectedHand = hand;
    }

    void HandHammer()
    {
        spawnedHammer = itemPackageSpawnerHammer.CallSpawnAndAttachObject(selectedHand, grabTypeHammer);
        if (spawnedHammer != null)
        {
            StartCoroutine(HammerDissolve());
        }
    }

    IEnumerator HammerDissolve(bool isDissolveOut = false)
    {
        float cutoffValue = 1f;

        if (isDissolveOut)
        {
            cutoffValue = 0f;
        }

        List<Renderer> childRenderer = spawnedHammer.GetComponentsInChildren<Renderer>().ToList();

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

            itemPackageSpawnerHammer.CallTakeBackItem(selectedHand);
        } else
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

    void UnhandHammer()
    {
        if (spawnedHammer != null)
        {
            StartCoroutine(HammerDissolve(true));
        }
    }

    protected override void BeforeYield(float totalGameTime)
    {
        GameMenuManager.Instance.textAnimation.ChangeText(Mathf.Floor(totalGameTime).ToString("00"));

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
        GameMenuManager.Instance.MinigameHasEnded();

        Debug.Log("Total hits: " + score);
        UnhandHammer();
        //Hide clowns

        ScoreManager.Instance.AddCurrentScore(score);

        AudioManager.instance.UnpauseSound();
        AudioManager.instance.StopSound(musicToPlay);
    }

    public void OnMoleHit(Transform hitObject)
    {
        Vector3 newPos = new Vector3(hitObject.position.x, hitObject.position.y + positionCorrectionAnimation, hitObject.position.z);
        GameObject go = Instantiate(animationHitPrefab, newPos, Quaternion.identity);
        go.GetComponentInChildren<AnimationDataAndController>().ScoreValueChange(scoreValue.ToString());

        score += scoreValue;
        selectedHand.TriggerHapticPulse(1000);
        Debug.Log("At least it's hit");
    }
}
