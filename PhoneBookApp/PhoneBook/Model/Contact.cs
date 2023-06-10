public class Contact
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public static Contact FromContactDetails(ContactDetails contactDetails)
    {
        return new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = contactDetails.FirstName,
            LastName = contactDetails.LastName,
            Phone = contactDetails.Phone,
            Address = contactDetails.Address
        };
    }

    public bool IsValidContact()
    {
        // logic to validate the Contact properties
        return (Id != Guid.Empty && Id.GetType() == typeof(Guid))
            && (!string.IsNullOrEmpty(FirstName) && FirstName.GetType() == typeof(string))
            && (!string.IsNullOrEmpty(LastName) && LastName.GetType() == typeof(string))
            && (!string.IsNullOrEmpty(Phone) && Phone.GetType() == typeof(string))
            && (Address == null || Address.GetType() == typeof(string));
            // address can be null
    }
}

public class ContactDetails
{    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public bool IsValidContact()
    {
        // logic to validate the ContactDetails properties
        return (!string.IsNullOrEmpty(FirstName) && FirstName.GetType() == typeof(string))
            && (!string.IsNullOrEmpty(LastName) && LastName.GetType() == typeof(string))
            && (!string.IsNullOrEmpty(Phone) && Phone.GetType() == typeof(string))
            && (Address == null || Address.GetType() == typeof(string));
            // address can be null
    }
}

