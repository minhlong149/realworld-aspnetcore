using Application.DTOs;
using Application.Users.Queries.GetUser;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.UnitTests.Users.Queries.GetUser;

public class GetUserHandlerTest
{
    private readonly Mock<ITokenClaimsService> _tokenClaimsServiceMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task WhenEmailNotExists_ShouldThrowInvalidCredentialsException()
    {
        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((UserEntity?)null);

        var handler = new GetUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var request = new GetUserRequest() { Email = "root@mail.com", Password = "password" };

        Func<Task> getUser = async () => await handler.Handle(request, CancellationToken.None);

        await getUser.Should()
            .ThrowAsync<InvalidCredentialsException>()
            .WithMessage(UserConstants.InvalidCredentials);
    }

    [Fact]
    public async Task WhenPasswordNotMatch_ShouldThrowInvalidCredentialsException()
    {
        var userEntity = new UserEntity() { Username = "root", Email = "root@mail.com", Password = "hashedPassword" };

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(userEntity.Email))
            .ReturnsAsync(userEntity);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.VerifyHashedPassword(It.IsAny<string>(), userEntity.Password))
            .Returns(false);

        var handler = new GetUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var request = new GetUserRequest() { Email = "root@mail.com", Password = "password" };

        Func<Task> getUser = async () => await handler.Handle(request, CancellationToken.None);

        await getUser.Should()
            .ThrowAsync<InvalidCredentialsException>()
            .WithMessage(UserConstants.InvalidCredentials);
    }

    [Fact]
    public async Task WhenUserExists_ShouldReturnUserDto()
    {
        var userEntity = new UserEntity() { Username = "root", Email = "root@mail.com", Password = "hashedPassword" };
        
        var userDto = new UserDto()
        {
            Username = userEntity.Username, Email = userEntity.Email, Image = userEntity.Image, Bio = userEntity.Bio
        };

        const string rawPassword = "password";

        const string userJwt = "header.payload.signature";

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(userEntity.Email))
            .ReturnsAsync(userEntity);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.VerifyHashedPassword(rawPassword, userEntity.Password))
            .Returns(true);

        _mapperMock
            .Setup(mapper => mapper.Map<UserDto>(userEntity))
            .Returns(userDto);

        _tokenClaimsServiceMock
            .Setup(tokenClaimsService => tokenClaimsService.GetToken(userEntity))
            .Returns(userJwt);

        var handler = new GetUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var request = new GetUserRequest() { Email = userEntity.Email, Password = rawPassword };

        var user = await handler.Handle(request, CancellationToken.None);

        user.Should().BeEquivalentTo(userDto with { Token = userJwt });
    }
}
