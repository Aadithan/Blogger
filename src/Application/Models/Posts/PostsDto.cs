using MyBlog.Application.Common.Mappings;
using MyBlog.Domain.Entities;

namespace MyBlog.Application.Models;


public class PostsDto : IMapFrom<Posts>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string PostContent { get; set; }
}
