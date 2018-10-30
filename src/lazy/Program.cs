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

        private Lazy _lazy = null;

        [Option("-s")]
        public string Solution { get; } = string.Empty;

        private int OnExecute()
        {
            //Init
            Application.Init();
            var top = Application.Top;
            // Creates the top-level window to show
            var win = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), "lazy");
            top.Add(win);
            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                }),
                new MenuBarItem ("_Git", new MenuItem [] {
                    new MenuItem ("_Status", "", () => { _lazy.RefreshGitStatus(); }),
                    new MenuItem ("_Checkout", "", () => { _lazy.CheckoutGit(); }),
                    new MenuItem ("_Pull", "", () => { _lazy.PullGit(); }),
                    new MenuItem ("_Add", "", () => { _lazy.AddGit(); }),
                    new MenuItem ("_Push", "", null)
                })
            });
            top.Add(menu);
            //Load Arg
            _lazy = new Lazy(win);
            if (!string.IsNullOrEmpty(Solution))
                _lazy.Solution = LoaderService.Load(Solution);
            this._lazy.RefreshUI();
            Application.Run();
            return 0;
        }

        static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit", "Are you sure you want to quit?", "Yes", "No");
            return n == 0;
        }
    }
}
