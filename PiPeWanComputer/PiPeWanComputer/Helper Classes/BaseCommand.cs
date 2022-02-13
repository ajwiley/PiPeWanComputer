using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PiPeWanComputer.Helper_Classes
{
    public class BaseCommand : ICommand
    {
        private readonly Action<object> _Execute;
        private readonly Predicate<object> _CanExecute;

        public event EventHandler CanExecuteChanged;


        public BaseCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute == null || _CanExecute(parameter);
        }

        public virtual void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }
}
