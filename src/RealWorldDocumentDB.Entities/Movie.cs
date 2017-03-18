using System;
using Newtonsoft.Json;

namespace RealWorldDocumentDB.Entities
{
    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id", Order = -4)]
        public string Id { get; set; }

        [JsonProperty(Order = -3)]
        public string Type => GetType().ToString();

        [JsonProperty(Order = -2)]
        public DateTime CreatedUtc { get; set; }
    }

    public class Movie : EntityBase
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Metascore { get; set; }
    }

    public class Photo : EntityBase
    {
        public string Caption { get; set; }
        public Uri ImageUri { get; set; }
        public double FileSize { get; set; }
    }
}
