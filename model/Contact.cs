using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.model
{
    public class Contact
    {
        // Property to store the unique identifier for the entity.
        public int Id { get; set; }

        // Property to store the user identifier for the entity.
        public int UserId { get; set; }

        // Property to store the name of the entity. 
        public required string Name { get; set; }

        // Property to store the phone number of the entity. 
        public required string PhoneNo { get; set; }
    }
}