using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;
using System.Linq;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

//THIS SCRIPT IS FOR MULTIPLE HANDS

public class LaserPointerHandler : MonoBehaviour
{
   // private SteamVR_LaserPointer laserPtrRef;
    private List<SteamVR_LaserPointer> laserPtrRefs;

    private void Awake()
    {
      //  laserPtrRef = FindObjectOfType<SteamVR_LaserPointer>();
      //  laserPtrRef.PointerClick += PointerClickCallback;

        laserPtrRefs = FindObjectsOfType<SteamVR_LaserPointer>().ToList(); //convert array to list
     
        foreach (SteamVR_LaserPointer x in laserPtrRefs)
            x.PointerClick += PointerClickCallback;
        
        // alternative to doing foreach above:::
        // laserPtrRefs.ForEach(x=>x.PointerClick += PointerClickCallback);
    }

    public void PointerClickCallback(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<Button>() != null)
            e.target.GetComponent<Button>().onClick.Invoke(); //invoke the button we just clicked
    }
}
