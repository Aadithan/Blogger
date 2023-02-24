using MyBlog.Application.Common.Interfaces;  
using Moq;
using NUnit.Framework;

namespace MyBlog.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{ 
    private Mock<ICurrentUser> _currentUserService = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    { 
        _currentUserService = new Mock<ICurrentUser>();
        _identityService = new Mock<IIdentityService>();
    } 
}
