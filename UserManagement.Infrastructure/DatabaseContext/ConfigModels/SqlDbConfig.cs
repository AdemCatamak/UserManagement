using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Infrastructure.DatabaseContext.Exceptions;

namespace UserManagement.Infrastructure.DatabaseContext.ConfigModels
{
    public class SqlDbConfig
    {
        public int SelectedIndex { get; set; }
        public List<SqlDbOption> SqlDbOptions { get; set; } = new List<SqlDbOption>();

        public SqlDbOption SelectedDbOption()
        {
            if (SqlDbOptions == null)
                throw new ArgumentNullException(nameof(SqlDbOptions));

            if (!SqlDbOptions.Any())
                throw new ArgumentException($"{nameof(SqlDbOptions)} is empty");

            var dbOption = SqlDbOptions.FirstOrDefault(o => o.Index == SelectedIndex);

            if (dbOption == null)
                throw new DbOptionNotFoundException(SelectedIndex);

            return dbOption;
        }
    }

    public class SqlDbOption
    {
        public int Index { get; set; }
        public SqlDbTypes SqlDbType { get; set; }
        public string ConnectionStr { get; set; } = string.Empty;
    }

    public enum SqlDbTypes
    {
        SqlServer = 1
    }
}