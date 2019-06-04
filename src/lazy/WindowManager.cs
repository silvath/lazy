using lazy.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy
{
    public class WindowManager
    {
        private static TextField _textField = null;
        private static Window _windowLog = null;
        private static ListView _listViewLog = null;
        private static List<string> _logs = new List<string>();
        public static string ShowDialogText(string title, string text, string defaultValue = "")
        {
            string response = "";
            var dialog = new Dialog(title, 50, 10, new Button("Ok") { Clicked = () => { response = _textField.Text.ToString(); Application.RequestStop(); } }, new Button("Cancel") { Clicked = () => { Application.RequestStop(); } });
            Label label = new Label(1, 1, text);
            dialog.Add(label);
            _textField = new TextField(1, 1, 40, "");
            _textField.Text = defaultValue;
            dialog.Add(_textField);
            _textField.EnsureFocus();
            Application.Run(dialog);
            return (response);
        }

        public static string ShowDialogList(string title, List<string> list)
        {
            var dialog = new Dialog(title, 50, 22, new Button("Close", true) { Clicked = () => { Application.RequestStop(); } });
            ListViewSelection listView = new ListViewSelection(new Rect(1,1,45,16),list);
            dialog.Add(listView);
            Application.Run(dialog);
            return (listView.Selection);
        }

        public static T ShowDialogObjects<T>(string title, Dictionary<T,string> objects)
        {
            var dialog = new Dialog(title, 50, 22, new Button("Close", true) { Clicked = () => { Application.RequestStop(); } });
            ListViewObject<T> listView = new ListViewObject<T>(new Rect(1, 1, 45, 16), objects);
            dialog.Add(listView);
            Application.Run(dialog);
            return (listView.Selection);
        }

        public static bool ShowDialogFilesNotStaged(string title, Dictionary<string,string> files)
        {
            var dialog = new Dialog(title, 100, 30, new Button("Close", true) { Clicked = () => { Application.RequestStop(); } });
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> entry in files)
                list.Add(entry.Key);
            ListViewSelection listView = new ListViewSelection(new Rect(1, 1, 95, 24), list);
            dialog.Add(listView);
            Application.Run(dialog);
            return (false);
        }

        public static bool ShowDialogFilesNotCommited(string title, Dictionary<string, string> files)
        {
            var dialog = new Dialog(title, 100, 30, new Button("Close", true) { Clicked = () => { Application.RequestStop(); } });
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> entry in files)
                list.Add(entry.Key);
            ListViewSelection listView = new ListViewSelection(new Rect(1, 1, 95, 24), list);
            dialog.Add(listView);
            Application.Run(dialog);
            return (false);
        }

        public static void ShowDialogHelpSolution()
        {
            List<string> list = new List<string>();
            list.Add("h - help solution");
            list.Add("i - open solution");
            list.Add("o - open solution folder");
            list.Add("p - open code in folder");
            ShowDialogList("Help Solution", list);
        }

        public static void ShowDialogHelpRepository()
        {
            List<string> list = new List<string>();
            list.Add("h - help repository");
            list.Add("o - open repository folder");
            list.Add("p - open code in folder");
            list.Add("b - branchs");
            list.Add("z - not staged");
            list.Add("x - not commited");
            ShowDialogList("Help Repository", list);
        }

        public static void ShowLog()
        {
            _logs = new List<string>();
            if (_windowLog != null)
                return;
            var top = Application.Top;
            _windowLog = new Window(new Rect(3, 2, top.Frame.Width - 6, top.Frame.Height - 4), "Log");
            _listViewLog = new ListView(new Rect(1, 1, 95, 20), _logs);
            _windowLog.Add(_listViewLog);
            top.Add(_windowLog);
            top.SetFocus(_windowLog);
            Application.Refresh();
        }

        public static void AddLog(string message)
        {
            if (_windowLog == null)
                return;
            if (message.Length > 70)
                message = message.Substring(0, 70);
            _logs.Add(message);
            _listViewLog.SetSource(GetLogs());
            _listViewLog.SetNeedsDisplay();
            Application.Refresh();
        }

        private static List<string> GetLogs()
        {
            List<string> logs = new List<string>();
            for (int i = _logs.Count - 1; i >= 0 && logs.Count < 20; i--)
                logs.Add(_logs[i]);
            return (logs);
        }

        public static void AddLogStart(string message)
        {
            AddLog($"start: {message}");
        }

        public static void AddLogEnd(string message)
        {
            AddLog($"end  : {message}");
        }

        public static void CloseLog()
        {
            if (_windowLog == null)
                return;
            var top = Application.Top;
            top.Remove(_windowLog);
            _windowLog = null;
            top.SetFocus(top.Subviews[0]);
        }
    }
}
