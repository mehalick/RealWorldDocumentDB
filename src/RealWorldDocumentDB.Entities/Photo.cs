using System;

namespace RealWorldDocumentDB.Entities
{
    public class Photo : EntityBase
    {
        public string Caption { get; set; }
        public Uri ImageUri { get; set; }
        public double FileSize { get; set; }
    }
}