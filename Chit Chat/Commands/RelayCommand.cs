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
        private readonly Func<Task> execute;
        private readonly Func<object, Task> execute1;
        private readonly Action execute2;
        private readonly Action<UserModel> execute3;
        private readonly Action execute4;
        private readonly Action execute5;
        private readonly Action<string> execute6;
        private readonly Func<NameChangeModel, Task> execute7;
        private readonly Func<bool> canExecute;
        public RelayCommand(Func<Task> execute) : this(execute, canExecute: null)
        {
        }
        public RelayCommand(Func<object, Task> execute1) : this(execute1, canExecute: null)
        {
        }

        public RelayCommand(Action execute2, Action<UserModel> execute3, Action execute4, Action execute5) : this(execute2, execute3, execute4, execute5, canExecute: null)
        {
        }

        public RelayCommand(Action execute4) : this(execute4, canExecute: null)
        {
        }

        public RelayCommand(Action<string> execute6) : this(execute6, canExecute: null)
        {
        }
        public RelayCommand(Func<NameChangeModel, Task> execute7) : this(execute7, canExecute: null)
        {
        }

        public RelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute is null");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Func<object, Task> execute1, Func<bool> canExecute)
        {
            if (execute1 == null)
                throw new ArgumentNullException("execute1 is null");

            this.execute1 = execute1;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action execute2, Action<UserModel> execute3, Action execute4, Action execute5, Func<bool> canExecute)
        {
            if (execute2 == null || execute3 == null || execute4 == null || execute5 == null)
                throw new ArgumentNullException("execute2, 3,4 or 5 are null");

            this.execute2 = execute2;
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
        public RelayCommand(Action<string> execute6, Func<bool> canExecute)
        {
            if (execute6 == null)
                throw new ArgumentNullException("execute6 is null");

            this.execute6 = execute6;
            this.canExecute = canExecute;
        }
        public RelayCommand(Func<NameChangeModel, Task> execute7, Func<bool> canExecute)
        {
            if (execute7 == null)
                throw new ArgumentNullException("execute7 is null");

            this.execute7 = execute7;
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
            if (execute != null)
            {
                await execute();
            }
            else if (execute1 != null)
            {
                await execute1(parameter as UserModel);
            }
            else if (execute2 != null && execute3 != null && execute4 != null && execute5 != null)
            {
                execute2();
                execute3(parameter as UserModel);
                execute4();
                execute5();
            }
            else if (execute4 != null)
            {
                execute4();
            }
            else if (execute6 != null)
            {
                execute6(parameter as string);
            }
            else if (execute7 != null)
            {
                await execute7(parameter as NameChangeModel);
            }
        }
    }
}
