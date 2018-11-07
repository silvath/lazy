using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class TableVO
    {
        public List<TableColumnVO> Columns { set; get; }
        public List<TableRowVO> Rows { set; get; }
        public TableVO(string content = null)
        {
            this.Columns = new List<TableColumnVO>();
            this.Rows = new List<TableRowVO>();
            if (!string.IsNullOrEmpty(content))
                this.Initialize(content);
        }

        private void Initialize(string content)
        {
            string[] lines = content.Split("\r\n");
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (!this.HasColumns())
                {
                    this.InitializeColumns(line);
                    continue;
                }
                if (!this.HasColumnsSize())
                {
                    this.InitializeColumnsSizes(line);
                    continue;
                }
                this.InitializeRow(line);
            }
        }

        private bool HasColumns()
        {
            return (this.Columns.Count > 0);
        }

        private void InitializeColumns(string line)
        {
            string[] lineSplits = line.Split(" ");
            foreach (string lineSplit in lineSplits)
            {
                if (string.IsNullOrEmpty(lineSplit))
                    continue;
                TableColumnVO column = new TableColumnVO();
                column.Name = lineSplit;
                this.Columns.Add(column);
            }
        }

        private bool HasColumnsSize()
        {
            foreach (TableColumnVO column in this.Columns)
                if (column.Size > 0)
                    return (true);
            return (false);
        }

        private void InitializeColumnsSizes(string line)
        {
            string[] lineSplits = line.Split(" ");
            int index = 0;
            int start = 0;
            foreach (string lineSplit in lineSplits)
            {
                if (string.IsNullOrEmpty(lineSplit))
                {
                    start++;
                    continue;
                }
                int size = lineSplit.Length;
                TableColumnVO column = this.Columns[index];
                column.Start = start;
                column.Size = size;
                index++;
                start = start + size;
                start++;
            }
        }

        private void InitializeRow(string line)
        {
            TableRowVO row = new TableRowVO(this);
            for (int i = 0; i < Columns.Count; i++)
            {
                TableColumnVO column = this.Columns[i];
                string cell = i < (this.Columns.Count - 1) ? line.Substring(column.Start, column.Size) : line.Substring(column.Start);
                row.Cells.Add(cell.Trim());
            }
            this.Rows.Add(row);
        }
    }
}
