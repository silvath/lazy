using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy.Views
{
    public class ListViewObject<T> : ListView
    {
        private Dictionary<T,string> _source;
        public T Selection
        {
            get
            {
                if (this.SelectedItem < 0)
                    return (default(T));
                return (GetObject(this.SelectedItem));
            }
        }
        public ListViewObject(Rect rect, Dictionary<T, string> source) : base(rect, FlattenValue(source))
        {
            this._source = source;
        }

        private static List<T> FlattenKey(Dictionary<T, string> source)
        {
            List<T> list = new List<T>();
            foreach (KeyValuePair<T, string> entry in source)
                list.Add(entry.Key);
            return (list);
        }

        private static List<string> FlattenValue(Dictionary<T, string> source)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<T, string> entry in source)
                list.Add(entry.Value);
            return (list);
        }

        private T GetObject(int index)
        {
            return (FlattenKey(this._source)[index]);
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
