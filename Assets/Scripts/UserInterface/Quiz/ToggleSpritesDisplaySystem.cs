using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class ToggleSpritesDisplaySystem : MonoBehaviour
    {
        [SerializeField]
        private ToggleSpriteDisplayPool _togglePool = default;

        [SerializeField]
        private ToggleGroup _toggleGroup = default;
        
        public event Action<Sprite> OnInteraction;

        public void Init()
            => _togglePool.Init();

        public void DisplayData(Sprite data)
        {
            ToggleSpriteDataDisplay display = _togglePool.GetPooledObject();
            _toggleGroup.allowSwitchOff = true;
            display.Toggle.group = _toggleGroup;
            display.gameObject.SetActive(true);
            display.Display(data);
            display.OnInteraction = OnDataDisplayInteraction;
        }

        public void PopulateDisplay(IEnumerable<Sprite> datas)
        {
            foreach (Sprite data in datas)
                DisplayData(data);
        }

        private void OnDataDisplayInteraction(Sprite obj)
        {
            OnInteraction?.Invoke(obj);
            _toggleGroup.allowSwitchOff = false;
        }

        public void Reset()
            => _togglePool.ResetPool();

        public void Shuffle()
            => _togglePool.Shuffle();
    }
}