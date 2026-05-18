using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    [SerializeField] private List<CraftRecipe> recipeList;

    private Dictionary<int, int> recipeDictionary = new Dictionary<int, int>();

    void Start()
    {
        foreach (var recipe in recipeList)
        {
            int key = CreateRecipeKey(recipe.materialA, recipe.materialB);

            if (!recipeDictionary.ContainsKey(key))
            {
                recipeDictionary.Add(key, recipe.result);
            }
        }
    }

    private int CreateRecipeKey(int item1, int item2)
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

    public int CombineItems(int input1, int input2)
    {
        int searchKey = CreateRecipeKey(input1, input2);

        if (recipeDictionary.TryGetValue(searchKey, out int craftResult))
        {
            return craftResult;
        }

        return -1;
    }
}
