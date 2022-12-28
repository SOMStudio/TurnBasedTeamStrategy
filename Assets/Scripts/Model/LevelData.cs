namespace Model
{
    [System.Serializable]
    public struct LevelData
    {
        public string name;
        public string scene;
        public string sprite;
        public int x;
        public int y;
    }
    
    [System.Serializable]
    public struct LevelList
    {
        public LevelData[] level;
    }
}
