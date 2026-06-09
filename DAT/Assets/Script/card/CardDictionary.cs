using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardDatabase", menuName = "ScriptableObjects/CardDatabase")]
public class CardDictionary : ScriptableObject
{
    public List<CardData> cardList;
}