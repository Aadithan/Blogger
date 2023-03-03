using AutoMapper;
using AutoMapper.QueryableExtensions; 
using MyBlog.Application.Common.Mappings;

namespace MyBlog.Application.Models;

public class GetPostsRequest : PaginationFilter, IRequest<PaginationResponse<PostsDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPostsRequestHandler : IRequestHandler<GetPostsRequest, PaginationResponse<PostsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper; 

    public GetPostsRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<PostsDto>> Handle(GetPostsRequest request, CancellationToken cancellationToken)
    {
        var test = _context.Posts;

        return await _context.Posts 
            .OrderBy(x => x.Title)
            .ProjectTo<PostsDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}