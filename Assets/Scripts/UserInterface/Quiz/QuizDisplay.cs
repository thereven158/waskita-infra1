using System;
using Agate.WaskitaInfra1.Level;
using A3.Quiz;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class QuizDisplay : MonoBehaviour
    {
        private IQuiz _activeQuiz;
        private object _activeAnswer;
        private Action<IQuiz, object> _onConfirmAnswer;
        private Action _onClose;

        [SerializeField]
        private TMP_Text _questionText = default;

        [SerializeField]
        private ToggleSpritesDisplaySystem spriteOptionDisplaySystem = default;

        [SerializeField]
        private Button _confirmButton = default;

        [SerializeField]
        private Button _cancelButton = default;

        private void Awake()
        {
            spriteOptionDisplaySystem.Init();
            spriteOptionDisplaySystem.OnInteraction += sprite => _activeAnswer = sprite;
            spriteOptionDisplaySystem.OnInteraction += sprite => _confirmButton.gameObject.SetActive(true);
            _confirmButton.onClick.AddListener(ConfirmAnswer);
            _cancelButton.onClick.AddListener(Close);
        }

        public void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }

        public void Display(IQuiz quiz, Action<IQuiz,object> onConfirmAnswer, Action onClose)
        {
            ToggleDisplay(true);
            _onConfirmAnswer = onConfirmAnswer;
            _onClose = onClose;
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

        private void ConfirmAnswer()
        {
            _onConfirmAnswer?.Invoke(_activeQuiz, _activeAnswer);
            Close();
        }

        public void Close()
        {
            ToggleDisplay(false);
            _onClose?.Invoke();
        }
    }
}