using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{/// <summary>
///  Non-generated class
/// </summary>
    public class BlogPostModel
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [DisplayName("Cím")]
        [Required(ErrorMessage = "A mező kitöltése kötelező!")]
        [StringLength(70, ErrorMessage = "Maximum 70 karakter lehetséges!")] 
        public string Title { get; set; }

        [DisplayName("Szöveg")]
        [Required(ErrorMessage = "A mező kitöltése kötelező!")]
        [StringLength(int.MaxValue, MinimumLength = 30, ErrorMessage = "Minimum 30 karakterből álljon a poszt!")]
        public string Text { get; set; }

        [DisplayName("Betűméret")]
        [Range(8, 40, ErrorMessage = "Túl kicsi vagy túl nagy a betűméret")]
        public Nullable<int> FontSize { get; set; }

        public Nullable<int> FontColorId { get; set; }
        public Nullable<int> BackgroundColorId { get; set; }


        public byte[] Image { get; set; }
        // Make the byte[] visible.
        
        [DisplayName("Image")]
        public HttpPostedFileBase DataInHttpPostedFileBase { get; set; }

        [DisplayName("Image")]
        public string SourceString { get; set; }

        public bool IsVisible { get; set; }

        
        public virtual Color Color { get; set; }  //--> BackgroundColor(BackgroundColorId)
        public virtual Color Color1 { get; set; }  //--> FontColor(FontColorId)
        public virtual User User { get; set; }
    }
}