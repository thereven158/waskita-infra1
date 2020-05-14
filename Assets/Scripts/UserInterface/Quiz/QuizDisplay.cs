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
        private TMP_Text _headerText = default;

        [SerializeField]
        private TMP_Text _questionText = default;

        [SerializeField]
        private SpriteTogglesDisplaySystem spriteOptionDisplaySystem = default;

        [SerializeField]
        private StringTogglesDisplaySystem stringOptionsDisplaySystem = default;

        [SerializeField]
        private ImgTextTogglesDisplaySystem imgTextOptionsDisplaySystem = default;

        private Toggle _activeToggle;

        [SerializeField]
        private Button _confirmButton = default;

        [SerializeField]
        private Button _cancelButton = default;

        public TMP_Text HeaderText => _headerText;

        private void Awake()
        {
            spriteOptionDisplaySystem.Init();
            spriteOptionDisplaySystem.OnInteraction += sprite => _activeAnswer = sprite;
            spriteOptionDisplaySystem.OnInteraction += sprite => _confirmButton.interactable = true;

            stringOptionsDisplaySystem.Init();
            stringOptionsDisplaySystem.OnInteraction += str => _activeAnswer = str;
            stringOptionsDisplaySystem.OnInteraction += str => _confirmButton.interactable = true;

            imgTextOptionsDisplaySystem.Init();
            imgTextOptionsDisplaySystem.OnInteraction += imgText => _activeAnswer = imgText;
            imgTextOptionsDisplaySystem.OnInteraction += imgText => _confirmButton.interactable = true;

            _confirmButton.onClick.AddListener(ConfirmAnswer);
            _cancelButton.onClick.AddListener(Close);
        }

        private void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }

        public void Display(IQuiz quiz, Action<IQuiz, object> onConfirmAnswer, Action onClose, object answer = null,
            string header = "Quiz")
        {
            HeaderText.text = header;
            ToggleDisplay(true);
            _onConfirmAnswer = onConfirmAnswer;
            _onClose = onClose;
            _activeAnswer = null;
            _confirmButton.interactable = false;
            _activeQuiz = quiz;
            _activeToggle = null;
            spriteOptionDisplaySystem.Reset();
            stringOptionsDisplaySystem.Reset();
            imgTextOptionsDisplaySystem.Reset();
            imgTextOptionsDisplaySystem.gameObject.SetActive(false);
            if (_activeQuiz.Question is IMessageQuestion msgQuestion)
                _questionText.text = msgQuestion.Message;

            switch (_activeQuiz.Question)
            {
                case IMultipleChoiceQuestion<Sprite> iQuestion:
                    spriteOptionDisplaySystem.PopulateDisplay(iQuestion.AnswerOptions);
                    _activeToggle = spriteOptionDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
                case IMultipleChoiceQuestion<string> txtQuestion:
                    stringOptionsDisplaySystem.PopulateDisplay(txtQuestion.AnswerOptions);
                    _activeToggle = stringOptionsDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
                case IMultipleChoiceQuestion<ScriptableImgText> txtQuestion:
                    imgTextOptionsDisplaySystem.gameObject.SetActive(true);
                    imgTextOptionsDisplaySystem.PopulateDisplay(txtQuestion.AnswerOptions);
                    _activeToggle = imgTextOptionsDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
            }

            if (_activeToggle == null) return;
            _activeToggle.isOn = true;
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

    public static class QuizDisplayExtension
    {
        public static void Display(this QuizDisplay display, IQuestion question, Action<IQuiz, object> onConfirmAnswer,
            Action onClose, object answer = null)
        {
            display.Display(question.Quiz, onConfirmAnswer, onClose, answer, question.DisplayName);
        }
    }
}