using System.ComponentModel.DataAnnotations;

namespace MyBlog.Domain.Entities;

public partial class Posts
{
    [Key]
    public int PostRowID { get; set; }

    public Guid BlogID { get; set; }

    public Guid PostID { get; set; }

    [StringLength(255)]
    public string Title { get; set; }

    public string Description { get; set; }

    public string PostContent { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    [StringLength(50)]
    public string Author { get; set; }

    public bool? IsPublished { get; set; }

    public bool? IsCommentEnabled { get; set; }

    public int? Raters { get; set; }

    public float? Rating { get; set; }

    [StringLength(255)]
    public string Slug { get; set; }

    public bool IsDeleted { get; set; }
}