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

        public event Action<LevelData> OnDataInteraction;

        public void Init()
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

        public void OpenList(IEnumerable<LevelData> levels, int unlockedLevel = 0)
        {
            gameObject.SetActive(true);
            _pool.ResetPool();
            PopulateLevelList(levels, unlockedLevel);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnDataDisplayInteraction(LevelData data)
        {
            OnDataInteraction?.Invoke(data);
        }
    }
}