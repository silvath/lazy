using System;
using System.ComponentModel.DataAnnotations;
using lazy.Service;
using McMaster.Extensions.CommandLineUtils;
using Terminal.Gui;

namespace lazy
{
    [Command(Description = "lazy")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private Lazy _lazy = new Lazy();

        [Option("-s")]
        public string Solution { get; } = string.Empty;

        private int OnExecute()
        {
            //Load Arg
            if (!string.IsNullOrEmpty(Solution))
                _lazy.Solution = LoaderService.Load(Solution);
            //Init
            Application.Init();
            var top = Application.Top;
            // Creates the top-level window to show
            var win = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), "lazy");
            top.Add(win);
            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    //new MenuItem ("_New", "Creates new file", NewFile),
                    //new MenuItem ("_Close", "", () => Close ()),
                    new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                }),
                new MenuBarItem ("_Edit", new MenuItem [] {
                    new MenuItem ("_Copy", "", null),
                    new MenuItem ("C_ut", "", null),
                    new MenuItem ("_Paste", "", null)
                })
            });
            top.Add(menu);
            this._lazy.RefreshSolution(win);
            //// Add some controls
            //win.Add(
            //        new Label(3, 2, "Login: "),
            //        new TextField(14, 2, 40, ""),
            //        new Label(3, 4, "Password: "),
            //        new TextField(14, 4, 40, "") { Secret = true },
            //        new CheckBox(3, 6, "Remember me"),
            //        new RadioGroup(3, 8, new[] { "_Personal", "_Company" }),
            //        new Button(3, 14, "Ok"),
            //        new Button(10, 14, "Cancel"),
            //        new Label(3, 18, "Press ESC and 9 to activate the menubar"));
            Application.Run();
            return 0;
        }

        static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");
            return n == 0;
        }
    }
}
