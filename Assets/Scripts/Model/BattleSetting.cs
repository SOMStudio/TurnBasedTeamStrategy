namespace Model
{
    [System.Serializable]
    public class ActionPoint
    {
        public string action;
        public int distance;
        public int point;
    }
    
    [System.Serializable]
    public struct BattleSetting
    {
        public ActionPoint[] moveActionPoint;
        public ActionPoint[] attackActionPoint;
    }
}
