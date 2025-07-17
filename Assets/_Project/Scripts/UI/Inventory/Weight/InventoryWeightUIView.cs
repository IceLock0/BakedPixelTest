using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Inventory.Weight
{
    public class InventoryWeightUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _weight;

        public void SetWeight(float weight) =>
            _weight.text = $"{weight:f}";
    }
}