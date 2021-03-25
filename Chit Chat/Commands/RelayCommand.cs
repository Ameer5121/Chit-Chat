using ChitChat.Models;
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
        private readonly Func<Task> execute1;
        private readonly Func<object, Task> execute2;
        private readonly Action<UserModel> execute3;
        private readonly Action execute4;
        private readonly Action execute5;
        private readonly Func<bool> canExecute;
        public RelayCommand(Func<Task> execute1) : this(execute1, canExecute: null)
        {
        }
        public RelayCommand(Func<object, Task> execute2) : this(execute2, canExecute: null)
        {
        }

        public RelayCommand(Action<UserModel> execute3, Action execute4, Action execute5) : this(execute3, execute4, execute5, canExecute: null)
        {
        }

        public RelayCommand(Action execute3) : this(execute3, canExecute: null)
        {
        }

        public RelayCommand(Func<Task> execute1, Func<bool> canExecute)
        {
            if (execute1 == null)
                throw new ArgumentNullException("execute1 is null");

            this.execute1 = execute1;
            this.canExecute = canExecute;
        }

        public RelayCommand(Func<object, Task> execute2, Func<bool> canExecute)
        {
            if (execute2 == null)
                throw new ArgumentNullException("execute2 is null");

            this.execute2 = execute2;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action<UserModel> execute3, Action execute4, Action execute5, Func<bool> canExecute)
        {
            if (execute3 == null || execute4 == null || execute5 == null)
                throw new ArgumentNullException("execute3, 4 or 5 are null");

            this.execute3 = execute3;
            this.execute4 = execute4;
            this.execute5 = execute5;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action execute4, Func<bool> canExecute)
        {
            if (execute4 == null)
                throw new ArgumentNullException("execute4 is null");

            this.execute4 = execute4;
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
            if (execute1 != null)
            {
                await execute1();
            }else if(execute2 != null)
            {
                await execute2(parameter as UserModel);
            }else if(execute3 != null && execute4 != null && execute5 != null)
            {
                execute3(parameter as UserModel);
                execute4();
                execute5();
            }
            else if (execute4 != null)
            {
                execute4();
            }
        }
    }
}
