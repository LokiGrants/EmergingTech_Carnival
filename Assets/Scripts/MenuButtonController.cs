using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtonController : MonoBehaviour
{
    public UnityEvent didntBuyEvent, buyEvent;

    public void OnClick()
    {
        didntBuyEvent.Invoke();
        GameMenuOpener.Instance.confirmPurchaseButton.onClick.RemoveAllListeners();
        GameMenuOpener.Instance.confirmPurchaseButton.onClick.AddListener(delegate { buyEvent.Invoke(); });
    }
}
