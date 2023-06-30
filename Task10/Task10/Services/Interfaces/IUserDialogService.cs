using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task10.Models;

namespace Task10.Services.Interfaces
{
    internal interface IUserDialogService
    {
        bool AddEdit(object item);

        void ShowInformation(string message, string caption);
        
        void ShowWarning(string message, string caption);
        
        void ShowError(string message, string caption);

        bool Confirm(string message, string caption, bool exclamation = false);
    }
}
