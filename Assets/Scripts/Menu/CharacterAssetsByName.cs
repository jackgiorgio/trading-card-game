using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAssetsByName : MonoBehaviour {

    public static CharacterAssetsByName Instance;
    private CharacterAsset[] allCharacterAssets; 
    private Dictionary<string, CharacterAsset> AllCharactersDictionary = new Dictionary<string, CharacterAsset>();

    void Awake()
    {
        Instance = this;
        allCharacterAssets = Resources.LoadAll<CharacterAsset>("");

        foreach (CharacterAsset ca in allCharacterAssets)
            if(!AllCharactersDictionary.ContainsKey(ca.name))
                AllCharactersDictionary.Add(ca.name, ca);
    }

    public CharacterAsset GetCharacterByName(string name)
    {
        if (AllCharactersDictionary.ContainsKey(name))
            return AllCharactersDictionary[name];
        else
            return null;
    }
}
