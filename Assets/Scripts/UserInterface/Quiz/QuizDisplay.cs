using System;
using Agate.WaskitaInfra1.Level;
using A3.Quiz;
using Agate.SpriteSheet;
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
        public event Action OnInteraction;

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
        
        [SerializeField]
        private SpriteSheetTogglesDisplaySystem spriteSheetOptionsDisplaySystem = default;

        [SerializeField]
        private TextSpriteSheetTogglesDisplaySystem textSpriteSheetOptionsDisplaySystem = default;
        
        private Toggle _activeToggle;

        [SerializeField]
        private Button _confirmButton = default;

        [SerializeField]
        private Button _cancelButton = default;

        private TMP_Text HeaderText => _headerText;

        public void Init()
        {
            spriteOptionDisplaySystem.Init();
            spriteOptionDisplaySystem.OnInteraction += sprite => OnOptionInteraction(sprite);

            stringOptionsDisplaySystem.Init();
            stringOptionsDisplaySystem.OnInteraction += str => OnOptionInteraction(str);

            imgTextOptionsDisplaySystem.Init();
            imgTextOptionsDisplaySystem.OnInteraction += imgText => OnOptionInteraction(imgText);
            
            spriteSheetOptionsDisplaySystem.Init();
            spriteSheetOptionsDisplaySystem.OnInteraction += spriteSheet => OnOptionInteraction(spriteSheet);
            
            textSpriteSheetOptionsDisplaySystem.Init();
            textSpriteSheetOptionsDisplaySystem.OnInteraction += textSpriteSheet => OnOptionInteraction(textSpriteSheet);

            _confirmButton.onClick.AddListener(ConfirmAnswer);
            _cancelButton.onClick.AddListener(Close);
        }

        public void OnOptionInteraction(object data)
        {
            OnInteraction?.Invoke();
            _confirmButton.interactable = true;
            _activeAnswer = data;
        }

        private void ToggleDisplay(bool toggle)
        {
            gameObject.SetActive(toggle);
        }

        public void Display(IQuiz quiz, Action<IQuiz, object> onConfirmAnswer, Action onClose, object answer = null,
            string header = "Quiz")
        {
            HeaderText.text = header;
            
            _onConfirmAnswer = onConfirmAnswer;
            _onClose = onClose;
            _confirmButton.interactable = false;

            _activeAnswer = null;
            _activeQuiz = quiz;
            _activeToggle = null;
            
            spriteOptionDisplaySystem.Reset();
            stringOptionsDisplaySystem.Reset();
            imgTextOptionsDisplaySystem.Reset();
            spriteSheetOptionsDisplaySystem.Reset();
            textSpriteSheetOptionsDisplaySystem.Reset();
            
            imgTextOptionsDisplaySystem.gameObject.SetActive(false);
            textSpriteSheetOptionsDisplaySystem.gameObject.SetActive(false);
            if (_activeQuiz.Question is IMessageQuestion msgQuestion)
                _questionText.text = msgQuestion.Message;

            switch (_activeQuiz.Question)
            {
                // how to abstract these thing
                case IMultipleChoiceQuestion<Sprite> iQuestion:
                    spriteOptionDisplaySystem.PopulateDisplay(iQuestion.AnswerOptions);
                    _activeToggle = spriteOptionDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
                case IMultipleChoiceQuestion<string> txtQuestion:
                    stringOptionsDisplaySystem.PopulateDisplay(txtQuestion.AnswerOptions);
                    _activeToggle = stringOptionsDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
                case IMultipleChoiceQuestion<ScriptableImgText> imgTextQuestion:
                    imgTextOptionsDisplaySystem.gameObject.SetActive(true);
                    imgTextOptionsDisplaySystem.PopulateDisplay(imgTextQuestion.AnswerOptions);
                    _activeToggle = imgTextOptionsDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
                case IMultipleChoiceQuestion<ISpriteSheet> spritesQuestion:
                    spriteSheetOptionsDisplaySystem.PopulateDisplay(spritesQuestion.AnswerOptions);
                    _activeToggle = spriteSheetOptionsDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
                case IMultipleChoiceQuestion<ITextSpriteSheet> textSpriteSheetQuestion:
                    textSpriteSheetOptionsDisplaySystem.gameObject.SetActive(true);
                    textSpriteSheetOptionsDisplaySystem.PopulateDisplay(textSpriteSheetQuestion.AnswerOptions);
                    _activeToggle = textSpriteSheetOptionsDisplaySystem.GetDisplayWith(answer)?.Toggle;
                    break;
            }
            
            ToggleDisplay(true);

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
            OnInteraction?.Invoke();
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