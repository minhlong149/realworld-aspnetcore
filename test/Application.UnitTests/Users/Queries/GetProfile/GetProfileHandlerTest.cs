using Application.DTOs;
using Application.Users.Queries.GetProfile;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Application.UnitTests.Users.Queries.GetProfile;

public class GetProfileHandleTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task WhenUserExists_ShouldReturnProfile()
    {
        var userEntity = new UserEntity() { Username = "root", Email = "root@mail.com", Password = "password" };
        
        var profileDto = new ProfileDto()
        {
            Username = userEntity.Username, Email = userEntity.Email, Image = userEntity.Image, Bio = userEntity.Bio
        };

        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByUsernameAsync(userEntity.Username))
            .ReturnsAsync(userEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<ProfileDto>(userEntity))
            .Returns(profileDto);

        var handler = new GetProfileHandler(_userRepositoryMock.Object, _mapperMock.Object);
        var request = new GetProfileRequest(userEntity.Username);

        var profile = await handler.Handle(request, CancellationToken.None);
        profile.Should().BeEquivalentTo(profileDto);
    }

    [Fact]
    public async Task WhenUserNotExists_ShouldThrowNotFoundException()
    {
        _userRepositoryMock
            .Setup(userRepository => userRepository.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((UserEntity?)null);

        var handler = new GetProfileHandler(_userRepositoryMock.Object, _mapperMock.Object);
        var request = new GetProfileRequest("notExistUsername");

        Func<Task> getProfile = async () => await handler.Handle(request, CancellationToken.None);

        await getProfile.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(UserConstants.UserNotFound);
    }
}
