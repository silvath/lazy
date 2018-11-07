using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class TableRowVO
    {
        private TableVO _table = null;
        public List<string> Cells { set; get; }
        public TableRowVO(TableVO table)
        {
            this._table = table;
            this.Cells = new List<string>();
        }

        public string GetValue(string columnName)
        {
            int index = this._table.Columns.FindIndex(c => c.Name == columnName);
            if (index < 0)
                return (string.Empty);
            return (this.GetValue(index));
        }

        public string GetValue(int index)
        {
            return (this.Cells[index]);
        }
    }
}
