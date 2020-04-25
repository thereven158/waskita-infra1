
namespace Agate.WaskitaInfra1.PlayerAccount
{
    public struct PlayerAccountData
    {
        public string Username { get; set; }
        public string AuthenticationToken { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Username);
        }
    }
}