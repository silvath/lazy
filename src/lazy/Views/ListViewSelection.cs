using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy.Views
{
    public class ListViewSelection : ListView
    {
        private List<string> _source;
        public string Selection
        {
            get
            {
                if (this.SelectedItem < 0)
                    return (null);
                return (_source[this.SelectedItem]);
            }
        }
        public ListViewSelection(Rect rect, List<string> source) : base(rect, source)
        {
            this._source = source;
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            if (kb.Key == Key.Enter)
            {
                Application.RequestStop();
                return (true);
            }
            return base.ProcessKey(kb);
        }
    }
}
