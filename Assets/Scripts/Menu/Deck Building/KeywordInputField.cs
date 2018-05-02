using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordInputField : MonoBehaviour {

    private InputField iField;

    void Awake()
    {
        iField = GetComponent<InputField>();
    }

    public void Clear()
    {
        iField.text = "";
        DeckBuildingScreen.Instance.CollectionBrowserScript.Keyword = iField.text;
    }

    public void EnterSubmit()
    {
        DeckBuildingScreen.Instance.CollectionBrowserScript.Keyword = iField.text;
    }
}
