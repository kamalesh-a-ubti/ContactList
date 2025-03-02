using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.model.Enums
{
    public enum MenuOptions
    {
        // enum options to select the actions to perform in the contact list
        AddContact = 1,
        ViewContacts,
        UpdateContact,
        DeleteContact,
        SearchContact,
        Exit
    }
}