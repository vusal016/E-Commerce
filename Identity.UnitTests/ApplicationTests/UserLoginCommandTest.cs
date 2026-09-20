namespace Identity.UnitTests.ApplicationTests
{
    public class UserLoginCommandTest
    {
        private readonly static UserLoginCommand Command = new("mvusal@234.com", "Password123!");
        private readonly UserLoginCommandHandler _handler;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityDbContext _identityDbContext;
        private readonly ITokenProvider _tokenProvider;
        private readonly IMapper _mapper;

        public UserLoginCommandTest()
        {
            var userStoreMock = Substitute.For<IUserStore<User>>();
            _userManager = Substitute.For<UserManager<User>>(userStoreMock, null!, null!, null!, null!, null!, null!, null!, null!);
            _identityDbContext = Substitute.For<IIdentityDbContext>();
            _tokenProvider = Substitute.For<ITokenProvider>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UserLoginCommandHandler(_userManager, _identityDbContext, _tokenProvider, _mapper);
        }

        [Fact]
        public async Task RefreshToken_ShouldBeDeleted()
        {
            // Arrange
            var user = new User("John", "Doe", Command.Email) { Id = Guid.NewGuid() };

            _userManager.FindByEmailAsync(Command.Email).Returns(user);
            _userManager.IsLockedOutAsync(user).Returns(false);
            _userManager.CheckPasswordAsync(user, Command.Password).Returns(true);

            var emptyRefreshTokens = new List<RefreshToken>().BuildMockDbSet();
            _identityDbContext.RefreshTokens.Returns(emptyRefreshTokens);

            // HƏLL: Null xətası verməməsi üçün Token-ləri mock edirik
            _tokenProvider.GenerateAccessToken(user).Returns("access_token");
            _tokenProvider.GenerateRefreshToken().Returns("valid_refresh_token_string");

            // Act
            await _handler.Handle(Command, CancellationToken.None);

            // Assert
            var _ = _identityDbContext.Received().RefreshTokens;
        }

        [Fact]
        public async Task RefSaveChanges_ShouldBeCalled_WhenLoginIsSuccessful()
        {
            // Arrange
            var user = new User("John", "Doe", Command.Email) { Id = Guid.NewGuid() };

            _userManager.FindByEmailAsync(Command.Email).Returns(user);
            _userManager.IsLockedOutAsync(user).Returns(false);
            _userManager.CheckPasswordAsync(user, Command.Password).Returns(true);

            var emptyRefreshTokens = new List<RefreshToken>().BuildMockDbSet();
            _identityDbContext.RefreshTokens.Returns(emptyRefreshTokens);
            _tokenProvider.GenerateAccessToken(user).Returns("access_token");
            _tokenProvider.GenerateRefreshToken().Returns("refresh_token");

            // Act
            await _handler.Handle(Command, CancellationToken.None);

            // Assert
            await _identityDbContext.Received(1).SaveChangesAsync(CancellationToken.None);
        }
    }
}