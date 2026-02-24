using Microsoft.EntityFrameworkCore;
using wot_api.Classes;
using wot_api.Data;
using wot_api.DTO;
using wot_api.Entities;
using wot_api.Services;

namespace wot_api.Tests.Services;

public class UserAuthServiceTests
{
    private static UserAuthService CreateService(DataContext context)
    {
        var authService = new AuthService(
            "this-is-a-test-secret-key-with-32chars",
            "wot-test-issuer",
            "wot-test-audience");
        var dataProtection = new DataProtection();
        return new UserAuthService(context, authService, dataProtection);
    }

    private static DataContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new DataContext(options);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsValidationFailure_ForWeakPassword()
    {
        await using var context = CreateContext(nameof(RegisterAsync_ReturnsValidationFailure_ForWeakPassword));
        var sut = CreateService(context);

        var result = await sut.RegisterAsync(new RegisterUserRequestDTO
        {
            Username = "bob",
            Email = "bob@example.com",
            Password = "abc12"
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(AuthErrorType.Validation, result.ErrorType);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsValidationFailure_ForDuplicateEmail()
    {
        await using var context = CreateContext(nameof(RegisterAsync_ReturnsValidationFailure_ForDuplicateEmail));
        context.Users.Add(new User
        {
            Username = "existing",
            Email = "duplicate@example.com",
            Password = "hash",
            Salt = new byte[16],
            UserTypeId = UserType.FreeUser
        });
        await context.SaveChangesAsync();

        var sut = CreateService(context);
        var result = await sut.RegisterAsync(new RegisterUserRequestDTO
        {
            Username = "new",
            Email = "duplicate@example.com",
            Password = "Valid@123"
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(AuthErrorType.Validation, result.ErrorType);
        Assert.Equal("User already exists.", result.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_ReturnsUnauthorized_ForInvalidCredentials()
    {
        await using var context = CreateContext(nameof(LoginAsync_ReturnsUnauthorized_ForInvalidCredentials));
        var sut = CreateService(context);

        var result = await sut.LoginAsync(new LoginUserRequestDTO
        {
            Email = "missing@example.com",
            Password = "Wrong@123"
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(AuthErrorType.Unauthorized, result.ErrorType);
        Assert.Equal("Invalid email or password", result.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_ReturnsToken_ForValidCredentials()
    {
        await using var context = CreateContext(nameof(LoginAsync_ReturnsToken_ForValidCredentials));
        var dataProtection = new DataProtection();
        var seededUser = new User
        {
            Username = "alice",
            Email = "alice@example.com",
            Password = "Valid@123",
            UserTypeId = UserType.FreeUser
        };
        var hash = dataProtection.HashPassword(seededUser);
        seededUser.Password = hash.HashPassword;
        seededUser.Salt = hash.Salt;
        context.Users.Add(seededUser);
        await context.SaveChangesAsync();

        var sut = CreateService(context);
        var result = await sut.LoginAsync(new LoginUserRequestDTO
        {
            Email = "alice@example.com",
            Password = "Valid@123"
        });

        Assert.True(result.IsSuccess);
        Assert.Equal(AuthErrorType.None, result.ErrorType);
        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrWhiteSpace(result.Value!.Token));
    }
}
