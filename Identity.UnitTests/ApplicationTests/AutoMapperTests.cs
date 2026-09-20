namespace Identity.UnitTests.ApplicationTests
{
    public class AutoMapperTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;
        public AutoMapperTests()
        {
            var configExpr = new MapperConfigurationExpression();
            configExpr.AddProfile<IdentityMapper>();
            _configuration = new MapperConfiguration(configExpr, NullLoggerFactory.Instance);

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void Configuration_ShouldBeValid()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_User_To_UserDto_Correctly()
        {
            // Arrange
            var user = new User("John", "Doe", "john.doe@example.com");

            // Act
            var userDto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(user.FirstName, userDto.FirstName);
            Assert.Equal(user.LastName, userDto.LastName);
            Assert.Equal(user.Email, userDto.Email);
        }
    }
}