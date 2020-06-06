using A3.CodePattern;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.QuestionList
{
    public class QuestionListItemDisplayPool : ObjectPool<QuestionListItemDisplay>
    {
        [SerializeField]
        private QuestionListItemDisplay _objectToPool = default;

        protected override QuestionListItemDisplay ObjectToPool => _objectToPool;
    }
}