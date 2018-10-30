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
            // MenuBar.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_Git", new MenuItem [] {
                    new MenuItem ("_Status", "", () => { _lazy.RefreshGitStatus(); }),
                    new MenuItem ("_Fetch", "", () => { _lazy.FetchGit(); }),
                    new MenuItem ("_Checkout Branch", "", () => { _lazy.CheckoutBranchGit(); }),
                    new MenuItem ("_Pull", "", () => { _lazy.PullGit(); }),
                    new MenuItem ("_Add", "", () => { _lazy.AddGit(); }),
                    new MenuItem ("_Commit", "", () => { _lazy.CommitGit(); }),
                    new MenuItem ("_Push", "", () => { _lazy.PushGit(); }),
                }),
                new MenuBarItem ("_VSTS", new MenuItem [] {
                    new MenuItem ("_No donuts for you", "", () => { if (Quit ()) top.Running = false; })
                }),
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                })
            });
            top.Add(menu);
            //Load Arg
            _lazy = new Lazy(win);
            if (!string.IsNullOrEmpty(Solution))
                _lazy.Solution = LoaderService.Load(Solution);
            if (_lazy.Solution == null)
            {
                Console.WriteLine($"Can`t find {Solution}");
            }
            else
            {
                this._lazy.RefreshUI();
                Application.Run();
            }
            return 0;
        }

        static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit", "Are you sure you want to quit?", "Yes", "No");
            return n == 0;
        }
    }
}
