using System.Collections.Generic;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using UnityEngine;

namespace Agate.WaskitaInfra1
{
    [CreateAssetMenu(fileName = "PlayerGameData", menuName = "WaskitaInfra1/PlayerData")]
    public class ScriptablePlayerGameData : ScriptableObject, IPlayerGameData
    {
        public string username;
        public string authToken;

        [SerializeField]
        private ScriptableGameProgress _gameProgress;

        [SerializeField]
        private ScriptableLevelProgress _levelProgress;
        
        public PlayerAccountData GetAccountData()
        {
            return new PlayerAccountData()
            {
                Username = username,
                AuthenticationToken = authToken
            };
        }

        public IGameProgressData GetProgressData()
        {
            return _gameProgress;
        }

        public ILevelProgressData LevelProgressData()
        {
            return _levelProgress;
        }
    }

    
}