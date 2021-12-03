using System.ComponentModel.DataAnnotations;

namespace Demo.Pages
{
    public class Model
    {
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = default!;

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = default!;
    }
}