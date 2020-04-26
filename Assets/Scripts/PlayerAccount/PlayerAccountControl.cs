using System;

namespace Agate.WaskitaInfra1.PlayerAccount
{
    public class PlayerAccountControl
    {
        public PlayerAccountData Data { get; private set; }
        public event Action<PlayerAccountData> OnDataChange;

        public void SetData(PlayerAccountData data)
        {
            Data = data;
            OnDataChange?.Invoke(Data);
        }

        public void ClearData()
        {
            SetData(new PlayerAccountData());
        }
}
}