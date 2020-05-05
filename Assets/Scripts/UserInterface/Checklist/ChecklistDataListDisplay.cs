using System;
using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class ChecklistDataListDisplay : MonoBehaviour
    {
        [SerializeField]
        private ChecklistDataDisplayPool _pool = default;

        public Action<IChecklistItem> OnDataInteraction;

        public void Init()
        {
            _pool.Init();
        }

        public void AddData(CheckListViewData data, bool interactable = true)
        {
            ChecklistDataDisplay display = _pool.GetPooledObject();
            display.Button.interactable = interactable;
            display.gameObject.SetActive(true);
            display.Display(data);
            display.OnInteraction = OnDataDisplayInteraction;
        }

        private void OnDataDisplayInteraction(CheckListViewData data)
        {
            OnDataInteraction?.Invoke(data.Item);
        }

        public void Reset()
        {
            _pool.ResetPool();
        }
    }
}
