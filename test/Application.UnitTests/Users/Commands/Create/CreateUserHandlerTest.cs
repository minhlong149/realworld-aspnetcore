using Application.DTOs;
using Application.Users.Commands.Create;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.UnitTests.Users.Commands.Create;

public class CreateUserHandlerTest
{
private readonly Mock<ITokenClaimsService> _tokenClaimsServiceMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task WhenUsernameAlreadyExists_ThrowsConflictException()
    {
        var userEntity = new UserEntity() { Email = "root@mail.com", Username = "root", Password = "password" };

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByUsernameAsync(userEntity.Username))
            .ReturnsAsync(userEntity);
        
        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(userEntity.Email))
            .ReturnsAsync((UserEntity?)null);

        var handler = new CreateUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var command = new CreateUserRequest()
        {
            Username = userEntity.Username, Email = userEntity.Email, Password = userEntity.Password
        };

        Func<Task> registration = async () => await handler.Handle(command, CancellationToken.None);

        await registration.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage(UserConstants.UsernameInUse);

        _userRepositoryMock.Verify(
            userRepository => userRepository.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task WhenEmailAlreadyExists_ThrowsConflictException()
    {
        var userEntity = new UserEntity() { Email = "root@mail.com", Username = "root", Password = "password" };

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByUsernameAsync(userEntity.Username))
            .ReturnsAsync((UserEntity?)null);

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(userEntity.Email))
            .ReturnsAsync(userEntity);

        var handler = new CreateUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
        
        var command = new CreateUserRequest()
        {
            Username = userEntity.Username, Email = userEntity.Email, Password = userEntity.Password
        };

        Func<Task> registration = async () => await handler.Handle(command, CancellationToken.None);

        await registration.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage(UserConstants.EmailInUse);

        _userRepositoryMock.Verify(
            userRepository => userRepository.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task WhenUserNotExists_ShouldCreateAndReturnUser()
    {
        var newUser = new UserDto { Email = "root@mail.com", Username = "root" };

        const string userJwt = "header.payload.signature";

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((UserEntity?)null);

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((UserEntity?)null);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>()))
            .Returns("hashedPassword");

        _mapperMock
            .Setup(mapper => mapper.Map<UserDto>(It.IsAny<UserEntity>()))
            .Returns(newUser);

        _tokenClaimsServiceMock
            .Setup(tokenClaimsService => tokenClaimsService.GetToken(It.IsAny<UserEntity>()))
            .Returns(userJwt);

        var handler = new CreateUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
        
        var command = new CreateUserRequest() { Email = newUser.Email, Username = newUser.Username, Password = "password" };

        var createdUser = await handler.Handle(command, CancellationToken.None);

        createdUser.Should().BeEquivalentTo(newUser with { Token = userJwt });
        
        _userRepositoryMock.Verify(
            userRepository => userRepository.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
