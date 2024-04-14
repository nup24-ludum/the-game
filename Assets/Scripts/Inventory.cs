using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public enum ItemTy {
        COOKIE,
        MIRRORSHARD,
    };

    [Serializable]
    public struct CraftingRecipe {
        public ItemTy[] items;
        public ItemTy outcome;
    }

    [SerializeField] private bool doCrafting;
    [SerializeField] private List<CraftingRecipe> crafting = new();
    [SerializeField] private List<ItemTy> items;

    public bool CraftRecipe(int idx) {
        if (idx >= crafting.Count) {
            return false;
        }

        CraftingRecipe recipe = crafting[idx];
        if (!items.All(item => recipe.items.Contains(item))) {
            return false;
        }

        foreach (ItemTy item in recipe.items) {
            items.Remove(item);
        }

        items.Add(recipe.outcome);

        return true;
    }
}
