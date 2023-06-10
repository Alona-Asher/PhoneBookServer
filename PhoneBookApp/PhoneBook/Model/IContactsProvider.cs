public interface IContactsProvider
{
    Task<PagedResult<Contact>> GetPaginatedContactsAsync(int pageNumber, int pageSize);
    Task<Contact?> GetContactAsync(Guid contactId);
    Task<Contact> AddContactAsync(Contact contact);
    Task UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(Guid contactId);
}