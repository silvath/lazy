using NStack;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy.Views
{
    public class CheckBoxSolution : CheckBox
    {
        private Lazy _lazy = null;
        public CheckBoxSolution(Lazy lazy, int x, int y, ustring s, bool is_checked) : base(x, y, s, is_checked)
        {
            _lazy = lazy;
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            if (kb.Key == Key.ControlH)
            {
                WindowManager.ShowDialogHelpSolution();
            }
            else if (kb.Key == Key.ControlO)
            {
                this._lazy.OpenCommandSolution(this.Text.ToString());
                return (true);
            }
            else if (kb.Key == Key.ControlP)
            {
                this._lazy.OpenCodeSolution(this.Text.ToString());
                return (true);
            }
            else if (kb.Key == Key.ControlI)
            {
                this._lazy.OpenSolution(this.Text.ToString());
                return (true);
            }

            return base.ProcessKey(kb);
        }
    }
}
