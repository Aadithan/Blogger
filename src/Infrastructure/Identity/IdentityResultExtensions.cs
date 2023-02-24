using MyBlog.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace MyBlog.Infrastructure.Identity;

internal static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public static List<string> GetErrors(this IdentityResult result, IStringLocalizer T) =>
        result.Errors.Select(e => T[e.Description].ToString()).ToList();
}
