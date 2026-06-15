using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    // レシピのリストをインスペクターで設定できるようにする
    [SerializeField] private List<CraftRecipe> recipeList;

    // 素材１
    public int material1;

    // レシピを格納する辞書を作成
    private Dictionary<int, int> recipeDictionary = new Dictionary<int, int>();

    public static CraftManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

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

    void Start()
    {

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

    public int CraftCards(int input1, int input2) // 鍵とレシピをもとに合成、カードの配列内の位置を示す数値を返す
    {
        int searchKey = CreateRecipeKey(input1, input2);

        if (recipeDictionary.TryGetValue(searchKey, out int craftResult))
        {
            return craftResult;
        }

        return -1;
    }
}
