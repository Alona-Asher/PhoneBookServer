public class ContactsRepository : IContactsRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ContactsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> AddContact(Contact contact)
    {
        var contactEntity = ContactsMapper.MapToContactEntity(contact);
        await _dbContext.Contacts.AddAsync(contactEntity);
        await _dbContext.SaveChangesAsync();
        return contact.Id;
    }
    public async Task<List<Contact>> GetPaginatedContacts(int pageNumber, int pageSize)
    {
        var contactsEntities = await _dbContext.Contacts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        var contacts = new List<Contact>();
        foreach (var entity in contactsEntities)
        {
            contacts.Add(ContactsMapper.MapToContact(entity));
        }
        return contacts;
    }
    public async Task<Contact?> GetContact(Guid contactId)
    {
        var contactEntity = await _dbContext.Contacts.FindAsync(contactId);
        if (contactEntity == null) return null;
        return ContactsMapper.MapToContact(contactEntity);
    }
    public async Task UpdateContact(Contact updatedContact)
    {
        var existingContact = await _dbContext.Contacts.FindAsync(updatedContact.Id);
        if (existingContact == null)
        {
            throw new Exception("Contact was not found");
        }
        existingContact.FirstName = updatedContact.FirstName;
        existingContact.LastName = updatedContact.LastName;
        existingContact.Phone = updatedContact.Phone;
        existingContact.Address = updatedContact.Address;
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteContact(Guid contactId)
    {
        var contact = await _dbContext.Contacts.FindAsync(contactId);
        if (contact == null)
        {
            throw new Exception("Contact was not found");
        }
        _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync();
    }
}