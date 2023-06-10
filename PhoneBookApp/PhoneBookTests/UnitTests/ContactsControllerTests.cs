public class ContactsControllerTests
{
    private readonly ContactsController _contactController;
    private readonly Mock<IContactsProvider> _contactsProviderMock;

    public ContactsControllerTests()
    {
        _contactsProviderMock = new Mock<IContactsProvider>();
        _contactController = new ContactsController(_contactsProviderMock.Object);
    }

    [Theory]
    [InlineData("Alona", "Asher", "05864842", "Tel-Aviv")]
    [InlineData("Maya", "Cohen", "054333333", "Bat-Yam")]
    [InlineData("Dani", "Israeli", "1234567", null)] // address can be null
    public async Task AddContact_ValidContactDetails_ReturnsOkResult(
        string firstName, string lastName, string phone, string address)
    {
        var contactDetails = CreateNewContactDetails(firstName, lastName, phone, address);
        _contactsProviderMock.Setup(x => x.AddContactAsync(It.IsAny<Contact>()))
        .ReturnsAsync((Contact contact) =>
        {
            return new Contact { Id = contact.Id, FirstName = contact.FirstName, 
                LastName = contact.LastName, Phone= contact.Phone, Address = contact.Address };
        });
        var result = await _contactController.AddContact(contactDetails);
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.IsType<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;
        var newContact = okResult.Value as Contact;
        Assert.Equal(newContact.FirstName, firstName);
        Assert.Equal(newContact.LastName, lastName);
        Assert.Equal(newContact.Phone, phone);
        Assert.Equal(newContact.Address, address);
    }

    [Theory]
    [InlineData(null, "Asher", "05864842", "Tel-Aviv")]
    [InlineData("", "Asher", "05864842", "Tel-Aviv")]
    [InlineData("Dana", null, "054333333", "Bat-Yam")]
    [InlineData("Alona", "Asher", null, "Givataim")]
    public async Task AddContact_InvalidContactDetails_ReturnsBadRequest(
        string firstName, string lastName, string phone, string address)
    {
        var contactDetails = CreateNewContactDetails(firstName, lastName, phone, address);       
        var result = await _contactController.AddContact(contactDetails);
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(400, ((ObjectResult)result.Result).StatusCode);
    }

    [Fact]
    public async Task GetPaginatedContacts_ValidPageNumber_ReturnsOkResult()
    {
        int pageNumber = 1;
        int pageSize = 10;
        var expectedContactsPage = new PagedResult<Contact>()
        {
            Items = new List<Contact>(),
            Page = pageNumber,
            PageSize = pageSize
        };
        _contactsProviderMock.Setup(c => c.GetPaginatedContactsAsync(pageNumber, pageSize))
            .ReturnsAsync(expectedContactsPage);
        var result = await _contactController.GetPaginatedContacts(pageNumber);
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.Equal(expectedContactsPage, (PagedResult<Contact>)okResult.Value);
    }

    [Fact]
    public async Task GetPaginatedContacts_InvalidPageNumber_ReturnsBadRequest()
    {
        int pageNumber = 0;
        var result = await _contactController.GetPaginatedContacts(pageNumber);
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(400, ((ObjectResult)result.Result).StatusCode);
    }

    [Fact]
    public async Task GetPaginatedContacts_ExceptionThrown_ReturnsInternalServerError()
    {
        int pageNumber = 1;
        int pageSize = 10;
        _contactsProviderMock.Setup(c => c.GetPaginatedContactsAsync(pageNumber, pageSize))
            .Throws(new Exception("An error occurred"));
        var result = await _contactController.GetPaginatedContacts(pageNumber);
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(500, ((ObjectResult)result.Result).StatusCode);
    }

    [Fact]
    public async Task AddContact_ExceptionThrown_ReturnsInternalServerError()
    {
        var contactDetails = CreateNewContactDetails("Alona","Asher","0548888888","Tel-Aviv");
        _contactsProviderMock.Setup(x => x.AddContactAsync(It.IsAny<Contact>()))
            .ThrowsAsync(new Exception());
        var result = await _contactController.AddContact(contactDetails);
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(500, ((ObjectResult)result.Result).StatusCode);       
    }

    [Fact]
    public async Task UpdateContact_ValidContactDetails_ReturnsOkResult()
    {
        var updatedContact = CreateNewContact(Guid.NewGuid(), "John", "Doe", "123456789", "Haifa");        
        _contactsProviderMock.Setup(x => x.UpdateContactAsync(updatedContact)).Returns(Task.CompletedTask);
        var validResult = await _contactController.UpdateContact(updatedContact); ;
        Assert.NotNull(validResult);
        Assert.IsType<OkObjectResult>(validResult);
    }

    [Fact]
    public async Task UpdateContact_InalidContactDetails_ReturnsBadRequest()
    {
        var invalidContact = CreateNewContact(Guid.NewGuid(), "John", "Doe", ""/* Invalid phone number */, "Haifa");       
        var result = await _contactController.UpdateContact(invalidContact);
        Assert.NotNull(result);
        Assert.Equal(400, ((ObjectResult)result).StatusCode);
    }

    [Fact]
    public async Task DeleteContact_ValidContactId_ReturnsOkResult()
    {
        var deletedContact = Guid.NewGuid();
        _contactsProviderMock.Setup(x => x.DeleteContactAsync(deletedContact))
            .Returns(Task.CompletedTask);

        var validResult = await _contactController.DeleteContact(deletedContact); 
        Assert.NotNull(validResult);
        Assert.IsType<OkObjectResult>(validResult);
    }

    [Fact]
    public async Task DeleteContact_InalidContactId_ReturnsBadRequest()
    {
        var invalidContact = Guid.Empty;
        var result = await _contactController.DeleteContact(invalidContact);
        Assert.NotNull(result);
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, ((ObjectResult)result).StatusCode);
    }

    private static Contact CreateNewContact(Guid id, string firstName, string lastName, string phone, string address)
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
    private static ContactDetails CreateNewContactDetails(string firstName, string lastName, string phone, string address)
    {
        return new ContactDetails
        {
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Address = address
        };
    }
}
