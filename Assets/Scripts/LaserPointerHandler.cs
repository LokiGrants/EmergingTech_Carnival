using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;
using System.Linq;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class LaserPointerHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    private void Awake()
    {
        if (laserPointer == null)
            GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerClick += PointerClickCallback;
        laserPointer.PointerIn += PointerInCallback;
        laserPointer.PointerOut += PointerOutCallback;
    }

    public void PointerClickCallback(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("MenuButton"))
            e.target.GetComponent<Button>().onClick.Invoke(); //invoke the button we just clicked
    }

    public void PointerInCallback(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("MenuButton"))
            e.target.GetComponent<Button>().image.color = e.target.GetComponent<LaserButton>().highlightColor;
    }

    public void PointerOutCallback(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("MenuButton"))
            e.target.GetComponent<Button>().image.color = e.target.GetComponent<LaserButton>().defaultColor;
    }
}
