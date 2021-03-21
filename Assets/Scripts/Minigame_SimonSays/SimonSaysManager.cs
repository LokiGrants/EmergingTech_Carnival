using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysManager : MiniGameManager<SimonSaysManager>
{
    public List<GameObject> SimonButtons;

    private int score;
    private bool canTouch;
    private GameObject pressedButton;

    private bool isShowingOrder;
    private Coroutine showOrderCoroutine;

    private void Start()
    {
        //StartSimon();
    }

    [ContextMenu("Start Simon Says")]
    void StartSimon()
    {
        score = 0;

        StartMinigame();
    }

    protected override void BeforeYield(float totalGameTime)
    {
        if (!canTouch && !isShowingOrder)
        {
            isShowingOrder = true;
            showOrderCoroutine = StartCoroutine(ShowOrder());
        }
        Debug.Log("Time Left " + Mathf.Floor(totalGameTime));
    }

    protected override void AfterYield(float totalGameTime)
    {
    }

    protected override void AfterWhile(float totalGameTime)
    {
        Debug.Log("Total hits: " + score);
    }

    IEnumerator ShowOrder()
    {
        yield return new WaitForSecondsRealtime(0.1f);
    }

    public void OnButtonDown(GameObject simonButton)
    {
        if (canTouch)
        {
            pressedButton = simonButton;
        }
    }

    public void OnButtonUp(GameObject simonButton)
    {
        Debug.Log("Click click");
        if (canTouch)
        {
            if (pressedButton == simonButton)
            {
                CheckOrder(simonButton);
                //Check if button == to next in line and keep going until correct order. Then repeat lights, add a new button to the order.
                //cooldown button?? Dunno if that is necessary but we want to avoid triggering it multiple times by mistake.
            }
        }

        pressedButton = null;
    }

    private void CheckOrder(GameObject simonButton)
    {

    }

    public void OnOrderPerfect()
    {
        score += 1;
        Debug.Log("At least it's hit");
    }
}
