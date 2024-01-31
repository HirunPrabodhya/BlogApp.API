using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Thumbnail { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UpdateDate { get; set; } = null;
        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
