using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Thumbnail { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public User? User { get; set; }
        public Category? Category { get; set; }

        
    }
}
