using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButton : MonoBehaviour
{
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.grey;
    public Color disabledColor = Color.black;
    public WhichMinigame whichIsMe;

    public enum WhichMinigame
    {
        WHACKA,
        BASKET,
        TARGET,
        SIMON
    }
}
