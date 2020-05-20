namespace Agate.Waskita.Responses
{
    [System.Serializable]
    public class CurrentProject
    {
        public int proyekId;
        public string proyekName;
        public int currentDay;
        public int currentEnergy;
        public int pointI;
        public int pointP;
        public int pointT;
        public int pointEx;
        public string[] npc;
    }
}
