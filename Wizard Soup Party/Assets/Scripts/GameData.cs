using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    private HashSet<string> collectedIngredients = new();

    public bool HasIngredient(string name) => collectedIngredients.Contains(name);
    public int IngredientCount => collectedIngredients.Count;

    [SerializeField] private int requiredIngredientsToFinish = 5; // or however many you have

    public bool HasAllIngredients()
    {
        return collectedIngredients.Count >= requiredIngredientsToFinish;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CollectIngredient(string name)
    {
        if (collectedIngredients.Add(name))
        {
            Debug.Log($"Collected ingredient: {name}");

        }
        GameData.Instance.DebugPrintIngredients();
    }


    public void DebugPrintIngredients()
    {
        if (collectedIngredients.Count == 0)
        {
            Debug.Log("No ingredients collected yet.");
            return;
        }
        
        Debug.Log("Collected Ingredients:");
        foreach (string ingredient in collectedIngredients)
        {
            Debug.Log($"- {ingredient}");
        }
    }

}
