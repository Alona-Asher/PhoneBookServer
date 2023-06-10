public interface IContactsRepository
{
    Task<List<Contact>> GetPaginatedContacts(int pageNumber, int pageSize);
    Task<Contact?> GetContact(Guid contactId);
    Task<Guid> AddContact(Contact contact);
    Task UpdateContact(Contact updatedContact);
    Task DeleteContact(Guid contactId);
}