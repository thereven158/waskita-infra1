using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public abstract class DataTogglesDisplaySystem<TData> : MonoBehaviour
    {
        protected abstract DataToggleDisplayPool<TData> TogglePool { get; }

        [SerializeField]
        private ToggleGroup _toggleGroup = default;

        public event Action<TData> OnInteraction;

        public void Init()
            => TogglePool.Init();

        public void DisplayData(TData data)
        {
            DataToggleDisplayBehavior<TData> displayBehavior = TogglePool.GetPooledObject();
            _toggleGroup.allowSwitchOff = true;
            displayBehavior.Toggle.group = _toggleGroup;
            displayBehavior.gameObject.SetActive(true);
            displayBehavior.Display(data);
            displayBehavior.OnInteraction = OnDataDisplayInteraction;
        }

        public void PopulateDisplay(IEnumerable<TData> datas)
        {
            foreach (TData data in datas)
                DisplayData(data);
        }

        public DataToggleDisplayBehavior<TData> GetDisplayWith(object data)
        {
            return !(data is TData) ? default : GetDisplayWith((TData) data);
        }
        
        public DataToggleDisplayBehavior<TData> GetDisplayWith(TData data)
        {
            foreach (DataToggleDisplayBehavior<TData> toggleDisplayBehavior in TogglePool.CurrentActiveObjectList())
                if (toggleDisplayBehavior._actualData.Equals(data)) return toggleDisplayBehavior;
            return default;
        }

        protected virtual void OnDataDisplayInteraction(TData obj)
        {
            OnInteraction?.Invoke(obj);
            _toggleGroup.allowSwitchOff = false;
        }

        public virtual void Reset()
            => TogglePool.ResetPool();

        public void Shuffle()
            => TogglePool.Shuffle();
    }
}