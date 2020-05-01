using System;
using Agate.WaskitaInfra1.Level;
using as3mbus.Selfish.Source;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class QuizDisplay : MonoBehaviour
    {
        private IQuiz _activeQuiz;
        private object _activeAnswer;
        public Action<IQuiz, object> OnAnswerConfirm;
        public Action OnCancel;

        [SerializeField]
        private TMP_Text _questionText = default;

        [SerializeField]
        private ToggleSpritesDisplaySystem spriteOptionDisplaySystem = default;

        [SerializeField]
        private Button _confirmButton = default;

        [SerializeField]
        private Button _cancelButton = default;

        public void Init()
        {
            spriteOptionDisplaySystem.Init();
            spriteOptionDisplaySystem.OnInteraction += sprite => _activeAnswer = sprite;
            spriteOptionDisplaySystem.OnInteraction += sprite => _confirmButton.gameObject.SetActive(true);
            _confirmButton.onClick.AddListener(ConfirmAnswer);
            _cancelButton.onClick.AddListener(() => ToggleDisplay(false));
        }

        public void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }

        public void DisplayQuiz(IQuiz quiz)
        {
            _activeAnswer = null;
            _confirmButton.gameObject.SetActive(false);
            _activeQuiz = quiz;
            spriteOptionDisplaySystem.Reset();
            switch (_activeQuiz.Question)
            {
                case ImagesQuestion iQuestion:
                    _questionText.text = iQuestion.QuestionText;
                    spriteOptionDisplaySystem.PopulateDisplay(iQuestion.Options);
                    break;
            }
        }

        public void ConfirmAnswer()
        {
            ToggleDisplay(false);
            OnAnswerConfirm?.Invoke(_activeQuiz, _activeAnswer);
        }
    }
}