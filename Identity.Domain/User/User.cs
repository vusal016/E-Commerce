namespace Identity.Domain.User
{
    public sealed class User: IdentityUser<Guid>, IAuiditEntity
    {
        private User()
        {

        }
        public User(string firstName, string lastName, string email)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            Email=email;
        }
        public string FirstName { get;private set; }
        public string LastName { get;private set; }
        public MemberTier MemberTier { get;private set; }
        public string AvatarUrl { get;private set; }
        public bool IsGuest { get;private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        private void SetFirstName(string firstName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName,"First name cannot be empty.");
            FirstName = firstName;      
        }
        private void SetLastName(string lastName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName,"Last name cannot be empty.");
            LastName = lastName;
        }
        public void SetMemberTier(MemberTier memberTier)
        {
            if (!Enum.IsDefined(memberTier))
                throw new ArgumentException("Invalid member tier.");        
            MemberTier = memberTier;
        }
        public void SetAvatarUrl(string avatarUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(avatarUrl, "Avatar URL cannot be empty.");
            AvatarUrl = avatarUrl;
        }
    }
}