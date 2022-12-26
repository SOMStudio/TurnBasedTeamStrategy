namespace Model
{
    [System.Serializable]
    public struct PlayerData
    {
        public string name;
        public int health;
        public int damage;
        public int attackRange;
        public int actionPoint;
        public int[] moveActionList;
        public int[] attackActionList;
    }

    [System.Serializable]
    public struct PlayerList
    {
        public PlayerData[] player;
    }
}
