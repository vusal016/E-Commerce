namespace Identity.Domain.AddressAggregate
{
    public sealed class Address : AuditEntity
    {
        private Address()
        {
        }

        public Address(Guid userId, string label, string firstName, string lastName, string addressLine1, string? addressLine2, string city, string state, string postalCode, string country, string phone, bool isDefault)
        {
            SetUserId(userId);
            SetLabel(label);
            SetFirstName(firstName);
            SetLastName(lastName);
            SetAddressLine1(addressLine1);
            AddressLine2 = addressLine2;
            SetCity(city);
            SetState(state);
            SetPostalCode(postalCode);
            SetCountry(country);
            SetPhone(phone);
            IsDefault = isDefault;
        }

        public Guid UserId { get; private set; }
        public string Label { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string Phone { get; private set; }
        public bool IsDefault { get; private set; }

        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");
            UserId = userId;
        }
        private void SetLabel(string label)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(label, "Label cannot be empty.");
            Label = label;
        }
        private void SetFirstName(string firstName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, "First name cannot be empty.");
            FirstName = firstName;
        }
        private void SetLastName(string lastName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, "Last name cannot be empty.");
            LastName = lastName;
        }
        private void SetAddressLine1(string addressLine1)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine1, "Address line 1 cannot be empty.");
            AddressLine1 = addressLine1;
        }
        private void SetCity(string city)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(city, "City cannot be empty.");
            City = city;
        }
        private void SetState(string state)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(state, "State cannot be empty.");
            State = state;
        }
        private void SetPostalCode(string postalCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(postalCode, "Postal code cannot be empty.");
            PostalCode = postalCode;
        }
        private void SetCountry(string country)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(country, "Country cannot be empty.");
            Country = country;
        }
        private void SetPhone(string phone)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(phone, "Phone cannot be empty.");
            Phone = phone;
        }
    }
}
