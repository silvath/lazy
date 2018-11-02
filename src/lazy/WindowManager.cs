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
        public static string ShowDialogText(string title, string text)
        {
            string response = "";
            var dialog = new Dialog(title, 50, 10, new Button("Ok") { Clicked = () => { response = _textField.Text.ToString(); Application.RequestStop(); } }, new Button("Cancel") { Clicked = () => { Application.RequestStop(); } });
            Label label = new Label(1, 1, text);
            dialog.Add(label);
            _textField = new TextField(1, 1, 40, "");
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

        public static void ShowDialogHelpSolution()
        {
            List<string> list = new List<string>();
            list.Add("h - help repository");
            ShowDialogList("Help Solution", list);
        }

        public static void ShowDialogHelpRepository()
        {
            List<string> list = new List<string>();
            list.Add("h - help repository");
            list.Add("b - branchs");
            ShowDialogList("Help Repository", list);
        }
    }
}
