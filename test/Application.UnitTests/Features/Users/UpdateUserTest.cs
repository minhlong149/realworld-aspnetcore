using Application.DTOs;
using Application.Features.Users;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.UnitTests.Features.Users;

public class UpdateUserHandlerTest
{
    private readonly Mock<ITokenClaimsService> _tokenClaimsServiceMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task WhenUserNotFound_ShouldThrowNotFoundException()
    {
        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((UserEntity?)null);

        var updateUserHandler = new UpdateUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var request = new UpdateUser(Guid.NewGuid());

        Func<Task> updateUser = async () => await updateUserHandler.Handle(request, CancellationToken.None);

        await updateUser.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(UserConstants.UserNotFound);

        _userRepositoryMock.Verify(
            userRepository => userRepository.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task WhenUserExists_ShouldUpdateUser()
    {
        var userToUpdate = new UserEntity
        {
            Id = Guid.NewGuid(), Email = "root@mail.com", Username = "root", Password = "password"
        };

        var request = new UpdateUser(userToUpdate.Id)
        {
            Email = "admin@mail.com",
            Username = "admin",
            Password = "password",
            Bio = "Lorem Ipsum",
            Image = "https://picsum.photos/300/200"
        };

        var updateUser = new UserDto
        {
            Email = request.Email, Username = request.Username, Bio = request.Bio, Image = request.Image
        };

        const string userJwt = "header.payload.signature";

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByIdAsync(request.Id))
            .ReturnsAsync(userToUpdate);

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByEmailAsync(request.Email))
            .ReturnsAsync((UserEntity?)null);

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByUsernameAsync(request.Username))
            .ReturnsAsync((UserEntity?)null);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>()))
            .Returns("hashedPassword");

        _mapperMock
            .Setup(mapper => mapper.Map<UserDto>(userToUpdate))
            .Returns(updateUser);

        _tokenClaimsServiceMock
            .Setup(tokenClaimsService => tokenClaimsService.GetToken(userToUpdate))
            .Returns(userJwt);

        var handler = new UpdateUserHandler(
            _tokenClaimsServiceMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );

        var updatedUser = await handler.Handle(request, CancellationToken.None);

        updatedUser.Should().BeEquivalentTo(updateUser with { Token = userJwt });

        _userRepositoryMock.Verify(
            userRepository => userRepository.UpdateAsync(userToUpdate, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
