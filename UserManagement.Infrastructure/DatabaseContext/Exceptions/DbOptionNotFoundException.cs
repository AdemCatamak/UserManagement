using System;

namespace UserManagement.Infrastructure.DatabaseContext.Exceptions
{
    public class DbOptionNotFoundException : Exception
    {
        public DbOptionNotFoundException(int selectedIndex) : base($"SqlDbOption could not found. [SelectedIndex : {selectedIndex}]")
        {
        }
    }
}