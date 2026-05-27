using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    [SerializeField] private List<CraftRecipe> recipeList; // レシピのリストUnityの画面上にを作成

    public int craftFrag = 0; // カード選択の状態を管理
    public int material1; // 素材１

    private Dictionary<int, int> recipeDictionary = new Dictionary<int, int>(); // レシピを収めるレシピ本を作成

    void Start()
    {
        foreach (var recipe in recipeList) // レシピ本の索引を作成
        {
            int key = CreateRecipeKey(recipe.materialA, recipe.materialB);

            if (!recipeDictionary.ContainsKey(key))
            {
                recipeDictionary.Add(key, recipe.result); // 索引を登録
            }
        }

        material1 = 0;
    }

    private int CreateRecipeKey(int item1, int item2) // レシピの索引用の鍵を作成
    {
        if (item1 < item2)
        {
            return item1 * 100 + item2;
        }
        else
        {
            return item2 * 100 + item1;
        }
    }

    public int CraftItems(int input1, int input2) // 鍵とレシピをもとに合成、カードの配列内の位置を示す数値を返す
    {
        int searchKey = CreateRecipeKey(input1, input2);

        if (recipeDictionary.TryGetValue(searchKey, out int craftResult))
        {
            return craftResult;
        }
        else
        {
            Debug.Log("なにかが違うようだ……？");

            craftFrag = 0;
        }

        return -1;
    }

    public void CraftFragManager() // 状態をクラフト中に設定
    {

        if (craftFrag == 0)
        {
            craftFrag = 1;

            Debug.Log("クラフト待機状態");
        }
        else
        {
            craftFrag = 0;

            Debug.Log("待機状態解除");
        }
    }

    public void SettingMaterial1(int cardID) // 素材１の設定
    {
        material1 = cardID;

        craftFrag = 2; // 状態を選択待機中に設定

        Debug.Log("素材２選択待機状態");
    }
}
