using System;
using System.Collections.Generic;
using A3.UserInterface;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class QuestionListInteractionDisplay : DisplayBehavior
    {
        [SerializeField]
        private QuestionListDisplay _questionListDisplay;

        [SerializeField]
        private Button _finishButton = default;

        private Action<IQuestion> _onDataInteraction;
        private Action _onFinishButton;


        public void Open(ILevelProgressData progressData, Action<IQuestion> onDataInteraction, Action onFinish)
        {
            _onDataInteraction = onDataInteraction;
            _onFinishButton = onFinish;
            _finishButton.interactable = progressData.IsChecklistDone();
            gameObject.SetActive(true);
            List<IQuestion> checklistItems = progressData.Level.Questions;
            for (int i = 0; i < checklistItems.Count; i++)
                _questionListDisplay.AddData(progressData.CheckListViewAt(i));
        }
        public void Open(LevelEvaluationData evalData, Action onFinish)
        {
            _onDataInteraction = null;
            _onFinishButton = onFinish;
            _finishButton.interactable = true;
            gameObject.SetActive(true);
            List<IQuestion> checklistItems = evalData.Level.Questions;
            for (int i = 0; i < checklistItems.Count; i++)
                _questionListDisplay.AddData(evalData.CheckListViewAt(i));
        }

        public override void Init()
        {
            Debug.Log("Init");
            _questionListDisplay.Init();
            _questionListDisplay.OnDataInteraction = OnDataInteraction;
            _finishButton.onClick.AddListener(OnFinishButton);
        }

        public override void Open()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            _questionListDisplay.Reset();
            _onDataInteraction = null;
            gameObject.SetActive(false);
        }

        public override bool IsOpen => gameObject.activeSelf;

        private void OnDataInteraction(IQuestion item)
        {
            _onDataInteraction?.Invoke(item);
        }

        private void OnFinishButton()
        {
            _onFinishButton?.Invoke();
        }
    }
}