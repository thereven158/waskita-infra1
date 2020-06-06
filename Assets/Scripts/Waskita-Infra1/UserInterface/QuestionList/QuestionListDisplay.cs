using Agate.WaskitaInfra1.Level;
using System;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.QuestionList
{
    public class QuestionListDisplay : MonoBehaviour
    {
        [SerializeField]
        private QuestionListItemDisplayPool _pool = default;

        public Action<IQuestion> OnDataInteraction;

        public void Init()
        {
            _pool.Init();
        }

        public void AddData(QuestionListItemViewData data, bool interactable = true)
        {
            QuestionListItemDisplay display = _pool.GetPooledObject();
            display.Button.interactable = interactable;
            display.gameObject.SetActive(true);
            display.OnInteraction = OnDataDisplayInteraction;
            display.Display(data);
        }

        private void OnDataDisplayInteraction(QuestionListItemViewData data)
        {
            OnDataInteraction?.Invoke(data.Item);
        }

        public void Reset()
        {
            _pool.ResetPool();
        }
    }
}
