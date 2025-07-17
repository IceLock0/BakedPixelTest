using System;
using _Project.Scripts.UI.Inventory.Weight;
using _Project.Scripts.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Inventory
{
    public class InventoryUIView : MonoBehaviour
    {
        private InventoryCellUIView[] _cells;

        private InventoryWeightUIView _inventoryWeightUIView;

        public event Action<int> UnblockCellClicked;
        public event Action<int, BasePopupUIView> PopupCreated;

        public void Construct(InventoryCellUIView[] cells, InventoryWeightUIView inventoryWeightUIView)
        {
            _cells = cells;
            _inventoryWeightUIView = inventoryWeightUIView;

            foreach (var cell in _cells)
            {
                cell.UnblockClicked += OnUnblockCellClicked;
                cell.PopupCreated += OnPopupCreated;
            }
        }

        public void SetInventoryData(float weight) =>
            _inventoryWeightUIView.SetWeight(weight);

        public void SetCellData(int index, Image icon, int amount) =>
            _cells[index].SetItem(icon, amount);

        public void SetCellData(int index) =>
            _cells[index].CleanItem();

        public void SetCellAvailable(int index, bool isAvailable) =>
            _cells[index].SetAvailable(isAvailable);


        private void OnUnblockCellClicked(InventoryCellUIView cell) =>
            UnblockCellClicked?.Invoke(GetCellIndex(cell));

        private void OnPopupCreated(InventoryCellUIView cell, BasePopupUIView popup) =>
            PopupCreated?.Invoke(GetCellIndex(cell), popup);

        private int GetCellIndex(InventoryCellUIView cell)
        {
            for (var i = 0; i < _cells.Length; i++)
            {
                if (_cells[i] == cell)
                    return i;
            }

            return -1;
        }

        private void OnDisable()
        {
            foreach (var cell in _cells)
            {
                cell.UnblockClicked += OnUnblockCellClicked;
                cell.PopupCreated -= OnPopupCreated;
            }
        }
    }
}