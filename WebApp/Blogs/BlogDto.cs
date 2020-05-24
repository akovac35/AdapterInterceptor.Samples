// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using System.ComponentModel.DataAnnotations;

namespace WebApp.Blogs
{
    public class BlogDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Url is too long.")]
        public string Url { get; set; }
    }
}
