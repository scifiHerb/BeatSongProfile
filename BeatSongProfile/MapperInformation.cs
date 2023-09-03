using System;
using System.Collections.Generic;


namespace MapperInformation
{
    public class DiffStats
    {
        public int total { get; set; }
        public int easy { get; set; }
        public int normal { get; set; }
        public int hard { get; set; }
        public int expert { get; set; }
        public int expertPlus { get; set; }
    }

    public class Stats
    {
        public int totalUpvotes { get; set; }
        public int totalDownvotes { get; set; }
        public int totalMaps { get; set; }
        public int rankedMaps { get; set; }
        public double avgBpm { get; set; }
        public double avgScore { get; set; }
        public double avgDuration { get; set; }
        public DateTime firstUpload { get; set; }
        public DateTime lastUpload { get; set; }
        public DiffStats diffStats { get; set; }
    }

    public class FollowData
    {
        public int followers { get; set; }
        public object follows { get; set; }
        public object following { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string hash { get; set; }
        public string avatar { get; set; }
        public Stats stats { get; set; }
        public FollowData followData { get; set; }
        public string type { get; set; }
        public bool admin { get; set; }
        public bool curator { get; set; }
        public bool verifiedMapper { get; set; }
        public string playlistUrl { get; set; }
    }
}