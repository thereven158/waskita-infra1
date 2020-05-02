using System;
using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;
using as3mbus.Selfish.Source;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class ChecklistDataListDisplay : MonoBehaviour
    {
        [SerializeField]
        private ChecklistDataDisplayPool _pool = default;

        public event Action<IQuiz> OnDataInteraction;

        public void Init()
        {
            _pool.Init();
        }

        private void AddData(IQuiz data, bool interactable = true)
        {
            ChecklistDataDisplay display = _pool.GetPooledObject();
            display.Button.interactable = interactable;
            display.gameObject.SetActive(true);
            display.Display(data);
            display.OnInteraction = OnDataDisplayInteraction;
        }

        private void PopulateLevelList(IEnumerable<IQuiz> checklists)
        {
            int i = 0;
            foreach (IQuiz levelData in checklists)
            {
                AddData(levelData);
                i++;
            }
        }

        public void OpenList(IEnumerable<IQuiz> checklists)
        {
            gameObject.SetActive(true);
            _pool.ResetPool();
            PopulateLevelList(checklists);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnDataDisplayInteraction(IQuiz data)
        {
            OnDataInteraction?.Invoke(data);
        }
    }
}
