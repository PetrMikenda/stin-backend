using System.ComponentModel.DataAnnotations.Schema;

namespace stin_api.Models
{
    public class Favorite
    {
        public Int32 id { get; set; }
        public string city { get; set; }
        public Int32 Users_id { get; set; }
    }
}
