using MyBlog.Application.Common.Exceptions;
using MyBlog.Application.Common.Mailing;
using MyBlog.Application.Identity.Users;
using MyBlog.Domain.Enums;

namespace MyBlog.Infrastructure.Identity;

internal partial class UserService
{   
    public async Task<string> CreateAsync(CreateUserRequest request, string origin)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        await _userManager.AddToRoleAsync(user, "Basic");

        var messages = new List<string> { string.Format(_t["User {0} Registered."], user.UserName) };

        //Todo confirm if account is valid and then proceed.
        if (!string.IsNullOrEmpty(user.Email))
        {
            // send verification email
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri
            };
            var mailRequest = new MailRequest(
                new List<string> { user.Email },
                _t["Confirm Registration"],
                _templateService.GenerateEmailTemplate("email-confirmation", eMailModel));
            await _mailService.SendAsync(mailRequest, CancellationToken.None);
            messages.Add(_t[$"Please check {user.Email} to verify your account!"]);
        } 

        return string.Join(Environment.NewLine, messages);
    }

    public async Task UpdateAsync(UpdateUserRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        string currentImage = user.PictureUrl ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.PictureUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image);
            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                _fileStorage.Remove(Path.Combine(root, currentImage));
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user); 
      
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
        }
    }
}
