[ApiController]
[Route("contacts")]
public class ContactsController : ControllerBase
{ 
	private readonly IContactsProvider _contactsProvider;
    private const int _maxPageSize = 10;
    private readonly ILogger _logger;

    public ContactsController(IContactsProvider contactsProvider)
	{
        _contactsProvider = contactsProvider;
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        _logger = loggerFactory.CreateLogger<Program>();
    }

    // Post contacts [body = json ContactDetails object]
    [HttpPost]
    public async Task<ActionResult<Contact>> AddContact([FromBody] ContactDetails contactDetails)
    {
        if (!contactDetails.IsValidContact())
        {
            _logger.LogError("Got invalid contact datails from client");
            return StatusCode(400, "Contact datails are not valid"); 
        }
        try
        {
            var contact = await _contactsProvider.AddContactAsync(Contact.FromContactDetails(contactDetails));
            return Ok(contact); 
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error occured", ex.Message);
            return StatusCode(500, ex.Message);           
        }
    }
	
    // Get contacts?pageNumber={pageNumber}&pageSize={pageSize}
    [HttpGet]
    public async Task<ActionResult<PagedResult<Contact>>> GetPaginatedContacts(int pageNumber = 1, int pageSize = _maxPageSize)
    {
        if (pageNumber < 1) {
            _logger.LogError("Got invalid pageNumber from client");
            return StatusCode(400, "PageNumber should be greater than 0"); 
        }
        try
        {
            var contactsPage = await _contactsProvider.GetPaginatedContactsAsync(pageNumber, Math.Min(_maxPageSize, pageSize));
            return Ok(contactsPage);
        } 
        catch (Exception ex)
        {
            _logger.LogError("Internal server error occured", ex.Message);
            return StatusCode(500, ex.Message);   
        }
    }

    // Get contacts/{contactId}
    [HttpGet("{contactId}")]
    public async Task<ActionResult<Contact>> GetContact(Guid contactId)
    {
        try
        {
            var result = await _contactsProvider.GetContactAsync(contactId);
            if (result == null)
            {
                _logger.LogError($"Contact - {contactId} was not found in repository");
                return StatusCode(500, "Contact was not found");
            }
            return Ok(result);
        }
        catch (Exception ex)    
        {
            _logger.LogError("Internal server error occured", ex.Message);
            return StatusCode(500, ex.Message);        
        }
    }

    // Put contacts [body = json Contact object]
    [HttpPut()]
    public async Task<ActionResult> UpdateContact([FromBody] Contact updatedContact)
    {
        if (!updatedContact.IsValidContact())
        {
            _logger.LogError("Got invalid contact datails from client");
            return StatusCode(400, "Contact datails are not valid");
        }
        try
        {
            await _contactsProvider.UpdateContactAsync(updatedContact);
            return Ok("Contact updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error occured", ex.Message);
            return StatusCode(500, ex.Message);          
        }
    }

    // Delete contacts/{contactId}
    [HttpDelete("{contactId}")]
    public async Task<ActionResult> DeleteContact(Guid contactId)
    {
        if (contactId == Guid.Empty)
        { 
            _logger.LogError("Got invalid contactId from client");
            return StatusCode(400, "Invalid ContactId");
        }
        try
        {
            await _contactsProvider.DeleteContactAsync(contactId);
            return Ok("Contact deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error occured", ex.Message);
            return StatusCode(500, ex.Message);        
        }
    }
}


