using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;

namespace Agate.WaskitaInfra1
{
    public interface IPlayerGameData
    {
        PlayerAccountData GetAccountData();
        IGameProgressData GetProgressData();
        ILevelProgressData LevelProgressData();
    }
}