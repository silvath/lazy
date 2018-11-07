using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class WorkItemVO
    {
        public int Code { set; get; }
        public string Name { set; get; }
        public string TaskID
        {
            get
            {
                return (string.Format("T{0}", this.Code));
            }
        }
    }
}
