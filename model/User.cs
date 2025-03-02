using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.model
{
    // This class represents a User entity with three properties: Id, Username, and Password.
public class User
{
    // Property to store the unique identifier for the user.
    public int Id { get; set; }

    // Property to store the username of the user. 
    public required string Username { get; set; }

    // Property to store the password of the user. 
    public required string Password { get; set; }
}

}