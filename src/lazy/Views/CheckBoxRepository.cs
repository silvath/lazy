using NStack;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy.Views
{
    public class CheckBoxRepository : CheckBox
    {
        private Lazy _lazy = null;
        public CheckBoxRepository(Lazy lazy, int x, int y, ustring s, bool is_checked) : base(x, y, s, is_checked)
        {
            _lazy = lazy;
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            if (kb.Key == Key.ControlH)
            {
                WindowManager.ShowDialogHelpRepository();
            }
            else if (kb.Key == Key.ControlB)
            {
                this._lazy.ShowBranchs(this.Text.ToString());
                return (true);
            }
            else if (kb.Key == Key.ControlZ)
            {
                this._lazy.ShowFilesNotStaged(this.Text.ToString());
                return (true);
            }
            else if (kb.Key == Key.ControlX)
            {
                this._lazy.ShowFilesNotCommited(this.Text.ToString());
                return (true);
            }
            else if (kb.Key == Key.ControlO)
            {
                this._lazy.OpenCommandRepository(this.Text.ToString());
                return (true);
            }
            else if (kb.Key == Key.ControlP)
            {
                this._lazy.OpenCodeRepository(this.Text.ToString());
                return (true);
            }
            return base.ProcessKey(kb);
        }
    }
}
