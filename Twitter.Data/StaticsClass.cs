namespace Twitter.Data
{
    public static class StaticsClass
    {
        public static string _token { get; set; }
        public static string _dbName { get; set; } = "Twitter";
        public static string _tableName { get; set; } = "searchHistories";
        public static int Version { get; set; } = -1;

        public static void GetStoreVersion()
        {
        }
    }
}