using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseProject.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Content { get; set; }
        public BlogPostStatus Status { get; set; }
        public DateTime Date { get; set; }

        public string? bloggerName { get; set; }
        public string? bloggerSurname { get; set; }
        public string? AuthorId { get; set; }
    }

    public enum BlogPostStatus
    {
        YAYINDA,
        Yayında_Değil
    }
}





