using UnityEngine;
using System;

[Serializable]
public class CraftRecipe
{
    public int materialA; // 合成するカード１
    public int materialB; // 合成するカード２
    public int result; // 合成後のカード
}
