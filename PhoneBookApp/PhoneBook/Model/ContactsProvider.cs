public class ContactsProvider: IContactsProvider
{
    private readonly IContactsRepository _contactRepository;

    public ContactsProvider(IContactsRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Contact> AddContactAsync(Contact contact)
    {
        await _contactRepository.AddContact(contact);
        return contact;
    }

    public async Task DeleteContactAsync(Guid contactId)
    {
        await _contactRepository.DeleteContact(contactId);
    }

    public async Task<PagedResult<Contact>> GetPaginatedContactsAsync(int pageNumber, int pageSize)
    {
        var contactList = await _contactRepository.GetPaginatedContacts(pageNumber, pageSize);
        return new PagedResult<Contact>
        {
            Items = contactList,
            Page = pageNumber,
            PageSize = contactList.Count
        };
    }

    public async Task<Contact?> GetContactAsync(Guid contactId)
    {
        return await _contactRepository.GetContact(contactId);
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        await _contactRepository.UpdateContact(contact);
    }
}
