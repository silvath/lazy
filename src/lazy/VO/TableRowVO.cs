using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class TableRowVO
    {
        public List<string> Cells { set; get; }
        public TableRowVO()
        {
            this.Cells = new List<string>();
        }
    }
}
