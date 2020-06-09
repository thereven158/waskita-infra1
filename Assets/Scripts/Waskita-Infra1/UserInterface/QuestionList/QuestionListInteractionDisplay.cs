using A3.UserInterface;
using Agate.WaskitaInfra1.Level;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.QuestionList
{
    public class QuestionListInteractionDisplay : DisplayBehavior
    {
        [SerializeField]
        private QuestionListDisplay _questionListDisplay = default;

        [SerializeField]
        private Button _finishButton = default;

        [SerializeField]
        private Button _abortButton = default;

        [SerializeField]
        private Scrollbar _verticalScrollBar = default;

        private Action<IQuestion> _onDataInteraction;
        private Action _onFinishButton;
        private Action _onAbortButton;


        public void Open(IQuestionListViewData viewData, IQuestionListInteractionData interaction)
        {
            _questionListDisplay.Reset();
            _onDataInteraction = interaction.OnDataInteraction;
            _onFinishButton = interaction.OnFinishButton;
            _onAbortButton = interaction.OnAbortButton;
            _finishButton.interactable = viewData.FinishButtonInteractable;
            gameObject.SetActive(true);
            foreach (QuestionListItemViewData itemViewData in viewData.ItemDatas)
                _questionListDisplay.AddData(itemViewData);
        }

        public override void Init()
        {
            _questionListDisplay.Init();
            _questionListDisplay.OnDataInteraction = OnDataInteraction;
            _finishButton.onClick.AddListener(OnFinishButton);
            _abortButton.onClick.AddListener(OnAbortButton);
        }

        public override void Close()
        {
            _verticalScrollBar.value = 1;
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

        private void OnAbortButton()
        {
            _onAbortButton?.Invoke();
        }
    }

    public interface IQuestionListViewData
    {
        IEnumerable<QuestionListItemViewData> ItemDatas { get; }
        bool FinishButtonInteractable { get; }
    }

    public interface IQuestionListInteractionData
    {
        Action<IQuestion> OnDataInteraction { get; }
        Action OnFinishButton { get; }
        Action OnAbortButton { get; }
    }

    public struct QuestionListViewData : IQuestionListViewData
    {
        public IEnumerable<QuestionListItemViewData> ItemDatas { get; set; }
        public bool FinishButtonInteractable { get; set; }
    }

    public struct QuestionListInteractionData : IQuestionListInteractionData
    {
        public Action<IQuestion> OnDataInteraction { get; set; }
        public Action OnFinishButton { get; set; }
        public Action OnAbortButton { get; set; }
    }
}