using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bakery.Extra
{
    public class Command : ICommand
    {
        private Action<object> _action;
        private Func<object, bool> _canexecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> action, Func<object, bool> canexecute = null)
        {
            _action = action;
            _canexecute = canexecute;
        }

        public bool CanExecute(object parameter) => _canexecute == null || _canexecute(parameter);

        public void Execute(object parameter) => _action(parameter);
    }
}
