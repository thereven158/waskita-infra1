using System;
using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class LevelProgressCheckListDisplay : MonoBehaviour
    {
        [SerializeField]
        private ChecklistDataListDisplay _checklistDataListDisplay;

        private Action<IChecklistItem> _onDataInteraction;

        private void Awake()
        {
            _checklistDataListDisplay.Init();
            _checklistDataListDisplay.OnDataInteraction = OnDataInteraction;
        }

        public void Open(ILevelProgressData progressData, Action<IChecklistItem> onDataInteraction)
        {
            _onDataInteraction = onDataInteraction;
            gameObject.SetActive(true);
            List<IChecklistItem> checklistItems = progressData.Level.Quizzes;
            for (int i = 0; i < checklistItems.Count; i++)
                _checklistDataListDisplay.AddData(progressData.CheckListViewAt(i));
        }

        public void Close()
        {
            _checklistDataListDisplay.Reset();
            _onDataInteraction = null;
            gameObject.SetActive(false);
            
        }

        private void OnDataInteraction(IChecklistItem item)
        {
            _onDataInteraction?.Invoke(item);
        }
    }
}