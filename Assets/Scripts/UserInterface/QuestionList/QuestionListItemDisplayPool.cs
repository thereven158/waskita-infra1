using A3.CodePattern;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class QuestionListItemDisplayPool : ObjectPool<QuestionListItemDisplay>
    {
        [SerializeField]
        private QuestionListItemDisplay _objectToPool;

        protected override QuestionListItemDisplay ObjectToPool => _objectToPool;
    }
}