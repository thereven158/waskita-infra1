using System;
using System.Collections.Generic;

namespace GameAction
{
    public class GameActionSystem
    {
        private Dictionary<Type, object> _component;

        public T GetActionComponent<T>() where T : class
        {
            if (!_component.ContainsKey(typeof(T))) return null;
            return (T)_component[typeof(T)];
        }

        public virtual void Init(params object[] components)
        {
            foreach (object cmpn in components)
                RegisterComponent(cmpn);
        }

        private void RegisterComponent(object cmpn)
        {
            if (cmpn == null) return;
            if (_component == null) _component = new Dictionary<Type, object>();
            if (_component.ContainsKey(cmpn.GetType())) return;
            _component.Add(cmpn.GetType(), cmpn);
        }

        public void InvokeAction(IGameActionData data)
        {
            data.Init(this);
            if (!data.Ready) throw new Exception("Action Not Ready");
            data.Invoke();
        }

    }
}