using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardAsset)), CanEditMultipleObjects]
public class CardAssetCustomInspector : Editor
{
    public SerializedProperty 
    CharacterAsset_prop,
    Description_prop,
    Tags_prop,
    Rarity_prop,
    CardImage_prop,
    ManaCost_prop,
    TypeOfCard_prop,
    TokenCard_prop,
    OverrideLimit_prop,
    MaxHealth_prop,
    Attack_prop,
    AttacksForOneTurn_prop,
    Charge_prop,
    Taunt_prop,
    CreatureScriptName_prop,
    SpecialCreatureAmount_prop ,
    SpellScriptName_prop,
    SpecialSpellAmount_prop,
    Targets_prop
    ;

    void OnEnable () {
        // Setup the SerializedProperties

        // all the common general fields
        CharacterAsset_prop = serializedObject.FindProperty ("CharacterAsset");  
        Description_prop = serializedObject.FindProperty ("Description");
        Tags_prop = serializedObject.FindProperty("Tags");
        Rarity_prop = serializedObject.FindProperty ("Rarity"); 
        CardImage_prop = serializedObject.FindProperty("CardImage");
        ManaCost_prop =  serializedObject.FindProperty ("ManaCost"); 
        TokenCard_prop = serializedObject.FindProperty("TokenCard");
        OverrideLimit_prop = serializedObject.FindProperty("OverrideLimitOfThisCardInDeck");
        TypeOfCard_prop = serializedObject.FindProperty ("TypeOfCard");

        // all the creature fields:
        MaxHealth_prop = serializedObject.FindProperty ("MaxHealth"); 
        Attack_prop = serializedObject.FindProperty ("Attack"); 
        AttacksForOneTurn_prop = serializedObject.FindProperty ("AttacksForOneTurn"); 
        Charge_prop =  serializedObject.FindProperty ("Charge"); 
        Taunt_prop =  serializedObject.FindProperty ("Taunt"); 
        CreatureScriptName_prop = serializedObject.FindProperty ("CreatureScriptName"); 
        SpecialCreatureAmount_prop = serializedObject.FindProperty ("SpecialCreatureAmount"); 

        // all the spell fields:
        SpellScriptName_prop = serializedObject.FindProperty ("SpellScriptName");
        SpecialSpellAmount_prop = serializedObject.FindProperty ("SpecialSpellAmount");
        Targets_prop = serializedObject.FindProperty ("Targets");
    
    }

    public override void OnInspectorGUI() {
        serializedObject.Update ();

        EditorGUILayout.PropertyField( CharacterAsset_prop );
        EditorGUILayout.PropertyField( Description_prop );
        EditorGUILayout.PropertyField( Tags_prop);
        EditorGUILayout.PropertyField( Rarity_prop );
        EditorGUILayout.PropertyField( CardImage_prop );
        EditorGUILayout.PropertyField( ManaCost_prop);
        EditorGUILayout.PropertyField( TokenCard_prop );
        EditorGUILayout.PropertyField( OverrideLimit_prop);
        EditorGUILayout.PropertyField( TypeOfCard_prop );


        TypesOfCards st = (TypesOfCards) TypeOfCard_prop.enumValueIndex;

        switch( st ) 
        {
            case TypesOfCards.Creature:            
                EditorGUILayout.PropertyField( MaxHealth_prop );
                EditorGUILayout.PropertyField( Attack_prop  );
                EditorGUILayout.PropertyField( AttacksForOneTurn_prop );
                EditorGUILayout.PropertyField( Charge_prop);
                EditorGUILayout.PropertyField( Taunt_prop);
                EditorGUILayout.PropertyField( CreatureScriptName_prop);
                EditorGUILayout.PropertyField( SpecialCreatureAmount_prop);
                break;

            case TypesOfCards.Spell:            
                EditorGUILayout.PropertyField( SpellScriptName_prop );
                EditorGUILayout.PropertyField( SpecialSpellAmount_prop );
                EditorGUILayout.PropertyField( Targets_prop);
                break;
        }

        serializedObject.ApplyModifiedProperties ();
    }
	
}
