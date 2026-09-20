namespace Identity.UnitTests.ApplicationTests
{
    public class UserRegistrationCommandTest
    {
        private static readonly UserRegisterCommand Command = new("John", "Bushmaker", "john.bush@gmail.com", "password123");

        private readonly UserRegisterCommandHandler _handler;
        private readonly UserManager<User> _userManagerMock;
        private readonly IIdentityDbContext _identityDbContextMock;
        private readonly ITokenProvider _tokenProviderMock;
        private readonly IMapper _mapperMock;


        public UserRegistrationCommandTest()
        {
            var userStoreMock = Substitute.For<IUserStore<User>>();
            _userManagerMock = Substitute.For<UserManager<User>>(userStoreMock, null, null, null, null, null, null, null, null);
            _identityDbContextMock = Substitute.For<IIdentityDbContext>();
            _tokenProviderMock = Substitute.For<ITokenProvider>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new UserRegisterCommandHandler(_userManagerMock, _identityDbContextMock, _tokenProviderMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserAlreadyExists()
        {
            // Arrange
            var existingUser = new User(Command.FirstName, Command.LastName, Command.Email);
            _userManagerMock.FindByEmailAsync(Command.Email).Returns(existingUser);
            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>((() => _handler.Handle(Command, CancellationToken.None)));

            // Assert
            Assert.Equal("User with this email already exists.", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldSaveChanges_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            _userManagerMock.FindByEmailAsync(Command.Email).Returns((User)null);
            var userDto = new UserDto(Guid.NewGuid(), Command.FirstName, Command.LastName, Command.Email, "", MemberTier.Bronze, "", false, DateTime.UtcNow, DateTime.UtcNow);
            _mapperMock.Map<UserDto>(Arg.Any<User>()).Returns(userDto);
            _tokenProviderMock.GenerateAccessToken(Arg.Any<User>()).Returns("access_token");
            _tokenProviderMock.GenerateRefreshToken().Returns("refresh_token");
            _userManagerMock.CreateAsync(Arg.Do<User>(u => u.Id = Guid.NewGuid()), Command.Password).Returns(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(Command, CancellationToken.None);

            // Assert
            await _identityDbContextMock.Received(1).SaveChangesAsync(CancellationToken.None);
            Assert.Equal(userDto, result.User);
            Assert.Equal("access_token", result.AccessToken);
            Assert.Equal("refresh_token", result.RefreshToken);
        }
    }
}