using System;
using System.Windows.Input;

namespace CommonMVVM.Common
{
    public class DelegateCommand : ICommand
    {
        public event Action RequestCommand;

        private readonly Action<object> _action;
        private readonly Predicate<object> _predicate;

        public DelegateCommand(Action<object> action, Predicate<object> predicate)
        {
            _action = action;
            _predicate = predicate;
        }

        public DelegateCommand(Action<object> action)
        {
            _action = action;
            _predicate = p => true;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate?.Invoke(parameter) ?? false;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
            RequestCommand?.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
