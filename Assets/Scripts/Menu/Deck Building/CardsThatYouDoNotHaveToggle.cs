using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsThatYouDoNotHaveToggle : MonoBehaviour {

    private Toggle t;

    void Awake()
    {
        t = GetComponent<Toggle>();
    }

    public void ValueChanged()
    {
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowingCardsPlayerDoesNotOwn = t.isOn;
    }

    public void SetValue (bool val)
    {
        if (t!=null)
            t.isOn = val;
    }
}
