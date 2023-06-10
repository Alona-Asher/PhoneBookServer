# API Documentation  
The Phone Book Web Server provides the following APIs:  

__Add Contact__  
Description: Adds a new contact.  
HTTP Method: POST  
Endpoint: /contacts  
Request Body: ContactDetails object (JSON format), for example:   
{  
    "FirstName": "alona",  
    "LastName": "Asher",  
    "Phone": "005484833",  
    "Address": "Tel-Aviv"  
}  
Response:  
Status Code: 200 (OK) - Returns the added contact, including its new contactId.  
Status Code: 400 (Bad Request) - Returned when the contact details are not valid.  
Status Code: 500 (Internal Server Error) - Returned when an unexpected error occurs.  

__Get Paginated Contacts__  
Description: Retrieves a paginated list of contacts (up to 10 contacts per page).  
HTTP Method: GET  
Endpoint: /contacts?pageNumber={pageNumber}&pageSize={pageSize}  
Parameters:  
pageNumber (optional): The page number of contacts to retrieve. Default is 1.  
pageSize (optional): The number of contacts to retrieve. Default is 10.   
Response:  
Status Code: 200 (OK) - Returns the paginated list of up to 10 contacts.  
Status Code: 400 (Bad Request) - Returned when the pageNumber is less than 1.  
Status Code: 500 (Internal Server Error) - Returned when an unexpected error occurs.  

__Get Contact__  
Description: Retrieves a specific contact by its ID.  
HTTP Method: GET  
Endpoint: /contacts/{contactId}  
Parameters:  
contactId: The unique ID of the contact (GUID).  
Response:  
Status Code: 200 (OK) - Returns the contact.  
Status Code: 500 (Internal Server Error) - Returned when an unexpected error occurs.  

__Update Contact__  
Description: Updates an existing contact.  
HTTP Method: PUT  
Endpoint: /contacts  
Request Body: Contact object (JSON format), for example:   
{  
    "id": "b6a89a9f-217c-4630-a7a7-07d9c8d4b437",  
    "firstName": "alona",  
    "lastName": "Asher",  
    "phone": "01654646",  
    "address": "Haifa"  
}  
Response:  
Status Code: 200 (OK) - Returns a success message.  
Status Code: 400 (Bad Request) - Returned when the contact details are not valid.  
Status Code: 500 (Internal Server Error) - Returned when an unexpected error occurs.   

__Delete Contact__  
Description: Deletes a contact by its ID.  
HTTP Method: DELETE  
Endpoint: /contacts/{contactId}  
Parameters:  
contactId: The unique ID of the contact.  
Response:  
Status Code: 200 (OK) - Returns a success message.  
Status Code: 400 (Bad Request) - Returned when the contactId is invalid.  
Status Code: 500 (Internal Server Error) - Returned when an unexpected error occurs.  

# Database 
The app is using an __in-memory MySQL database__ (in lack of real credentials).   
Table name: "Contacts"  
__Schema:__
- Id (Guid) Primary Key
- FirstName (string) MaxLength(50) - can't be null
- LastName (string) MaxLength(50) - can't be null
- Phone (string) MaxLength(50) - can't be null
- Address (string) MaxLength(50) - allowed to be null

# Getting Started
To run the Phone Book Web Server, follow these steps:
1. Ensure that .NET SDK is installed on your system. Official .NET website: https://dotnet.microsoft.com/download
2. Clone the repository or download the source code.
3. Using IDE (Visual Studio is preferred):  
3.1. Open the solution in your preferred IDE.   
3.2. click "Build" to compile the solution.  
3.2. Click "Run" to start the application.  
4. Using CLI commands:  
4.1. __dotnet restore__  
4.2. __dotnet build__  
4.3. __dotnet run__  
