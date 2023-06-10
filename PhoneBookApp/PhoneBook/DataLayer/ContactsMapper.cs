public class ContactsMapper
{
    public static ContactEntity MapToContactEntity(Contact contact)
    {
        return new ContactEntity
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Phone = contact.Phone,
            Address = contact.Address
        };
    }

    public static Contact MapToContact(ContactEntity contactEntity)
    {
        return new Contact
        {
            Id = contactEntity.Id,
            FirstName = contactEntity.FirstName,
            LastName = contactEntity.LastName,
            Phone = contactEntity.Phone,
            Address = contactEntity.Address
        };
    }
}