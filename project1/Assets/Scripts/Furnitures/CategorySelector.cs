using UnityEngine;

public class CategorySelector : MonoBehaviour
{
    public PrefabSelectionUI selectionUI;
    public GameObject categoryDropdownUI;
    public GameObject prefabSelectionUI;

    private string selectedCategory = string.Empty;

    public void HandleInput(int val)
    {
        if (val == 1) selectedCategory = "Bathroom";
        else if (val == 2) selectedCategory = "Beds";
        else if (val == 3) selectedCategory = "Cabinets+Racks";
        else if (val == 4) selectedCategory = "Lights";
        else if (val == 5) selectedCategory = "Mirrors";
        else if (val == 6) selectedCategory = "Modular Kitchen";
        else if (val == 7) selectedCategory = "Sofas+Chairs";
        else if (val == 8) selectedCategory = "Tables";
        else if (val == 9) selectedCategory = "Vases";
        else return;

        selectionUI.PopulateUI(selectedCategory);
        categoryDropdownUI.SetActive(false);
        prefabSelectionUI.SetActive(true);
    }
}
