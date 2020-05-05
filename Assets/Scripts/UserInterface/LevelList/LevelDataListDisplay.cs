using System;
using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.LevelList
{
    public class LevelDataListDisplay : MonoBehaviour
    {
        [SerializeField]
        private LevelDataDisplayPool _pool = default;

        private Action<LevelData> _onDataInteraction;
        public event Action OnInteraction;

        private void Awake()
        {
            _pool.Init();
        }

        private void AddData(LevelData data, bool interactable = true )
        {
            LevelDataListItemDisplay listItemDisplay = _pool.GetPooledObject();
            listItemDisplay.Button.interactable = interactable;
            listItemDisplay.gameObject.SetActive(true);
            listItemDisplay.Display(data);
            listItemDisplay.OnInteraction = OnDataDisplayInteraction;
        }

        private void PopulateLevelList(IEnumerable<LevelData> levels, int unlockedLevel = 0)
        {
            int i = 0;
            foreach (LevelData levelData in levels)
            {
                AddData(levelData, unlockedLevel < 1 || i < unlockedLevel);
                i++;
            }
        }

        public void OpenList(IEnumerable<LevelData> levels, Action<LevelData> onDataInteraction, int unlockedLevel = 0)
        {
            gameObject.SetActive(true);
            _pool.ResetPool();
            PopulateLevelList(levels, unlockedLevel);
            _onDataInteraction = onDataInteraction;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _onDataInteraction = null;
        }

        private void OnDataDisplayInteraction(LevelData data)
        {
            OnInteraction?.Invoke();
            _onDataInteraction?.Invoke(data);
        }
    }
}