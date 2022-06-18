﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("Posts", Schema = "Social"), Index(nameof(CreatedOn))]
    public class Post 
    {
        public int Id { get; set; }
        [MaxLength(400)]
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public Forum Forum { get; set; }
        public int ForumId { get; set; }
        public User PostedBy { get; set; }
        public int PostedById { get; set; }
    }
}
