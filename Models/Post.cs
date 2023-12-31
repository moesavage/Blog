using Microsoft.AspNet.Identity;
using System;

namespace Blog.Models
{
     public class Post {
        public int PostId { get; set; }

        //public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        //public int UserId { get; set; } 
        public User Author { get; set; }// Foreign key for UserId

        public string ImageUrl { get; set; }
        public StatusType Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
     }

    public enum StatusType
    {
        Published,
        Draft,
        Archived,
        // Add other statuses as needed
    }

    public class PostViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; } // Assuming you want to select a category
        public string ImageUrl { get; set; }
        public StatusType Status { get; set; } // Assuming you want to set the post status
    }
}
