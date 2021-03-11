using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRangeManager : MiniGameManager<TargetRangeManager>
{
    public List<Transform> spawnParents;
    private float score;

    [ContextMenu("Start Target Range")]
    void StartTargetRange()
    {
        score = 0;

        Debug.Log("Give VR hammer to player here");
        StartMinigame();
    }
}
