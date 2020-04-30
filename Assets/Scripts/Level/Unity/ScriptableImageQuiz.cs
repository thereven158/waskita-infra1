using System;
using System.Collections.Generic;
using Agate.SugiSuma.Quiz;
using as3mbus.Selfish.Source;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(fileName = "ImageQuiz", menuName = "WaskitaInfra1/Quiz/Image", order = 0)]
    public class ScriptableImageQuiz : ScriptableQuiz
    {
        [SerializeField]
        private ImagesQuestion _question = default;

        private ImageQuiz _quiz;

        public override IQuiz Quiz
        {
            get
            {
                _quiz = _quiz ?? new ImageQuiz(_question);
                return _quiz;
            }
        }
    }

    [Serializable]
    public struct ImagesQuestion
    {
        public string QuestionText;
        public Sprite Answer;
        public List<Sprite> Options;
    }

    public class ImageQuiz : Quiz<ImagesQuestion, Sprite>
    {
        public override bool IsCorrect(Sprite answer)
        {
            return answer == Question.Answer;
        }

        public override ImagesQuestion Question { get; }

        public ImageQuiz(ImagesQuestion question)
        {
            Question = question;
        }
    }
}