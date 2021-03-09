using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChitChat.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> Taskexecute;
        private readonly Action execute;

        private readonly Func<bool> canExecute;
        public RelayCommand(Func<Task> execute) : this(execute, canExecute: null)
        {
        }
        public RelayCommand(Action execute) : this(execute, canExecute: null)
        {
        }

        public RelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.Taskexecute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }


        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        public async void Execute(object parameter)
        {
            if (execute == null)
            {
               await Taskexecute();
                return;
            }
            execute();
        }
    }
}
