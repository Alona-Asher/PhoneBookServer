public class DataLayerTests
{
    private readonly IContactsRepository _contactsRepository;

    public DataLayerTests()
    {
        _contactsRepository = CreateTestRepository();
    }

    [Fact]
    public void MapToContactEntity_ReturnsContactEntity()
    {
        var contact = CreateNewContact(Guid.NewGuid(), "Taylor", "Swift", "123456789", "New-York");
        var result = ContactsMapper.MapToContactEntity(contact);
        Assert.NotNull(result);
        Assert.Equal(contact.Id, result.Id);
        Assert.Equal(contact.FirstName, result.FirstName);
        Assert.Equal(contact.LastName, result.LastName);
        Assert.Equal(contact.Phone, result.Phone);
        Assert.Equal(contact.Address, result.Address);
    }

    [Fact]
    public void MapToContact_ReturnsContact()
    {
        var contactEntity = CreateNewContactEntity(Guid.NewGuid(), "Katy", "Perry", "123456789", "Los-Angeles");
        var result = ContactsMapper.MapToContact(contactEntity);
        Assert.NotNull(result);
        Assert.Equal(contactEntity.Id, result.Id);
        Assert.Equal(contactEntity.FirstName, result.FirstName);
        Assert.Equal(contactEntity.LastName, result.LastName);
        Assert.Equal(contactEntity.Phone, result.Phone);
        Assert.Equal(contactEntity.Address, result.Address);
    }

    [Fact]
    public async Task GetContact_ExistingContact_ReturnsContact()
    {
        var contactId = Guid.NewGuid();
        var newContact = CreateNewContact(contactId, "John", "Doe", "05433333", "Givataim");     
        await _contactsRepository.AddContact(newContact);

        var result = await _contactsRepository.GetContact(contactId);
        Assert.NotNull(result);
        Assert.IsType<Contact>(result);
        Assert.Equivalent(result, newContact);
    }

    [Fact]
    public async Task GetContact_NonExistingContact_ReturnsNull()
    {
        var contactId = Guid.NewGuid();
        var result = await _contactsRepository.GetContact(contactId);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPaginatedContacts_ReturnsPaginatedContacts()
    {
        int pageNumber = 1;
        int pageSize = 10;
        var expectedContacts = new List<Contact>() { };
        for (var i = 1; i <= pageSize; i++)
        {
            expectedContacts.Add(CreateNewContact(Guid.NewGuid(), $"name{i}", $"lastName{i}", $"phone{i}", $"address{i}"));
        }
        foreach (var contact in expectedContacts)
        {
            await _contactsRepository.AddContact(contact);
        }
        
        var result = await _contactsRepository.GetPaginatedContacts(pageNumber, pageSize);
        Assert.NotNull(result);
        Assert.IsType <List<Contact>>(result);
        Assert.Equal(expectedContacts.Count, result.Count);
        for (int i = 0; i < expectedContacts.Count; i++)
        {
            Assert.Equivalent(expectedContacts[i], result[i]);
        }
    }

    [Fact]
    public async Task UpdateContact_SuccessfullyUpdatesDetails()
    {
        var contactId = Guid.NewGuid();
        var oldContact = CreateNewContact(contactId, "oldName", "oldLastName", "oldPhone", "oldAddress");
        var updatedContact = CreateNewContact(contactId, "newName", "newLastName", "newPhone", "newAddress");
        await _contactsRepository.AddContact(oldContact);
        var exception = await Record.ExceptionAsync(async () => await _contactsRepository.UpdateContact(updatedContact));
        Assert.Null(exception);
        Assert.Equivalent(updatedContact, await _contactsRepository.GetContact(contactId));
    }

    private static Contact CreateNewContact (Guid id, string firstName, string lastName, string phone, string address)
    {
        return new Contact
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Address = address
        };
    }
    private static ContactEntity CreateNewContactEntity(Guid id, string firstName, string lastName, string phone, string address)
    {
        return new ContactEntity
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Address = address
        };
    }
    private static IContactsRepository CreateTestRepository()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDatabase")
    .Options;
        ApplicationDbContext testDBContext = new ApplicationDbContext(options);
        return new ContactsRepository(testDBContext);
    }
}