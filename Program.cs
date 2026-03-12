using System.ComponentModel;
using System.Text.Json;
using ContactManager;

const string FileName = "contacts.json";
List<Contact> contacts = LoadContacts();

// Main application loop
while (true)
{
    Console.WriteLine("\n--- Contact Manager ---");
    Console.WriteLine("1. Add Contact");
    Console.WriteLine("2. View All Contacts");
    Console.WriteLine("3. Search Contacts");
    Console.WriteLine("4. Delete a Contact");
    Console.WriteLine("5. Exit");
    Console.Write("Choose an option: ");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddContact();
            break;
        case "2":
            ViewContacts();
            break;
        case "3":
            SearchContacts();
            break;
        case "4":
            DeleteContact();
            break;
        case "5":
            SaveContacts();
            Console.WriteLine("Contacts saved. Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

List<Contact> LoadContacts()
{
    if (File.Exists(FileName))
    {
        string json = File.ReadAllText(FileName);

        // handle the case if the file is empty
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Contact>();
        }

        return JsonSerializer.Deserialize<List<Contact>>(json);
    }
    // Return an empty list if the file doesn't exist
    return new List<Contact>();
}

void SaveContacts()
{
    var options = new JsonSerializerOptions { WriteIndented = true };
    string json = JsonSerializer.Serialize(contacts, options);
    File.WriteAllText(FileName, json);
}

void AddContact()
{
    Console.Write("Enter name: ");
    string name = Console.ReadLine();

    Console.Write("Enter email: ");
    string email = Console.ReadLine();

    Console.Write("Enter phone: ");
    string phone = Console.ReadLine();

    // Check if a contact with the same email already exists
    if (contacts.Any(c => c.Email == email))
    {
        Console.WriteLine("Contact already included.");
    }
    else
    {
        contacts.Add(new Contact(name, email, phone));
        Console.WriteLine("Contact added successfully.");
    }
}

void ViewContacts()
{
    Console.WriteLine("\n--- All Contacts ---");
    // Clean way to check if the list is empty
    if (!contacts.Any())
    {
        Console.WriteLine("No contacts found.");
        return;
    }

    // Use LINQ to sort contacts by name before displaying
    var sortedContacts = contacts.OrderBy(c => c.Name);

    foreach (var contact in sortedContacts)
    {
        Console.WriteLine($"Name: {contact.Name}, Email: {contact.Email}, Phone: {contact.Phone}");
    }
}

void SearchContacts()
{
    Console.Write("Enter search term (for name): ");
    string searchTerm = Console.ReadLine();

    // Use LINQ to filter contacts
    var results = contacts.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    Console.WriteLine("\n--- Search Results ---");
    if (!results.Any())
    {
        Console.WriteLine("No matching contacts found.");
    }
    else
    {
        foreach (var contact in results)
        {
            Console.WriteLine($"Name: {contact.Name}, Email: {contact.Email}, Phone: {contact.Phone}");
        }
    }
}

void DeleteContact()
{
    Console.Write("Enter the email of the contact to delete: ");
    string emailDelete = Console.ReadLine();

    var contactDelete = contacts.FirstOrDefault(c => c.Email == emailDelete);

    if (contactDelete != null)
    {
        contacts.Remove(contactDelete);
        Console.WriteLine("Contact removed successfully");
    }
    else
    {
        Console.WriteLine("Contact not found in the list.");
    }
}