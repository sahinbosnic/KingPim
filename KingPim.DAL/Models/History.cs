using System;
using System.Collections.Generic;
using System.Text;

namespace KingPim.DAL.Models
{
    public class History
    {
        public int Id { get; set; }
        public int? CatalogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public DateTime Timestamp { get; set; }

        public History()
        {
            Timestamp = DateTime.Now;
        }
    }
}
