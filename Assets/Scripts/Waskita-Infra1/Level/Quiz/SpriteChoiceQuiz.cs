using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class SpriteChoiceQuiz : ScriptableMultipleChoiceQuiz<Sprite>
    {
        [SerializeField]
        private SpriteChoiceQuestion _question = default;

        protected override BasicMultipleChoiceQuestion<Sprite> Question => _question;
    }

    [Serializable]
    public class SpriteChoiceQuestion : BasicMultipleChoiceQuestion<Sprite> { };
}