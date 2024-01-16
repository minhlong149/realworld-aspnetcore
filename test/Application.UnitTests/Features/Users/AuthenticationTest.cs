using Application.DTOs;
using Application.Features.Users;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.UnitTests.Features.Users;

public class AuthenticationHandlerTest
{
    private readonly Mock<ITokenClaimsService> _tokenClaimsServiceMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task WhenEmailNotExists_ShouldThrowInvalidCredentialsException()
    {
        var command = new Authentication() { Email = "root@mail.com", Password = "password" };

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(command.Email))
            .ReturnsAsync((UserEntity?)null);

        var handler = new AuthenticationHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        Func<Task> authentication = async () => await handler.Handle(command, CancellationToken.None);

        await authentication.Should()
            .ThrowAsync<InvalidCredentialsException>()
            .WithMessage(UserConstants.InvalidCredentials);
    }

    [Fact]
    public async Task WhenPasswordNotMatch_ShouldThrowInvalidCredentialsException()
    {
        var command = new Authentication() { Email = "root@mail.com", Password = "password" };

        var userEntity = new UserEntity()
        {
            Username = command.Email, Email = command.Email, Password = command.Password
        };

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(command.Email))
            .ReturnsAsync(userEntity);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.VerifyHashedPassword(command.Password, userEntity.Password))
            .Returns(false);

        var handler = new AuthenticationHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        Func<Task> authentication = async () => await handler.Handle(command, CancellationToken.None);

        await authentication.Should()
            .ThrowAsync<InvalidCredentialsException>()
            .WithMessage(UserConstants.InvalidCredentials);
    }

    [Fact]
    public async Task WhenUserExists_ShouldReturnUserDto()
    {
        var command = new Authentication() { Email = "root@mail.com", Password = "password" };

        var userEntity = new UserEntity()
        {
            Username = command.Email, Email = command.Email, Password = command.Password
        };

        var userDto = new UserDto()
        {
            Username = userEntity.Username, Email = userEntity.Email, Image = userEntity.Image, Bio = userEntity.Bio
        };

        const string userJwt = "header.payload.signature";

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(command.Email))
            .ReturnsAsync(userEntity);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.VerifyHashedPassword(command.Password, userEntity.Password))
            .Returns(true);

        _mapperMock
            .Setup(mapper => mapper.Map<UserDto>(userEntity))
            .Returns(userDto);

        _tokenClaimsServiceMock
            .Setup(tokenClaimsService => tokenClaimsService.GetToken(userEntity))
            .Returns(userJwt);

        var handler = new AuthenticationHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var user = await handler.Handle(command, CancellationToken.None);
        
        user.Should().BeEquivalentTo(userDto with { Token = userJwt });
    }
}
