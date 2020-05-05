using System;
using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class LevelProgressCheckListDisplay : MonoBehaviour
    {
        [SerializeField]
        private ChecklistDataListDisplay _checklistDataListDisplay;

        [SerializeField]
        private Button _finishButton = default;

        private Action<IChecklistItem> _onDataInteraction;
        private Action _onFinishButton;

        private void Awake()
        {
            _checklistDataListDisplay.Init();
            _checklistDataListDisplay.OnDataInteraction = OnDataInteraction;
            _finishButton.onClick.AddListener(OnFinishButton);
        }

        public void Open(ILevelProgressData progressData, Action<IChecklistItem> onDataInteraction, Action onFinish)
        {
            _onDataInteraction = onDataInteraction;
            _onFinishButton = onFinish;
            _finishButton.interactable = progressData.IsChecklistDone();
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

        private void OnFinishButton()
        {
            _onFinishButton?.Invoke();
        }
    }
}