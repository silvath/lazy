using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class TableVO
    {
        public List<TableColumnVO> Columns { set; get; }
        public List<TableRowVO> Rows { set; get; }
        public TableVO()
        {
            this.Columns = new List<TableColumnVO>();
            this.Rows = new List<TableRowVO>();
        }
    }
}
