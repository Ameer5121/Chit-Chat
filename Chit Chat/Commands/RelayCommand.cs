using ChitChat.Helper.Language;
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
        private readonly Func<UserModel, Task> execute1;
        private readonly Action execute3;
        private readonly Action<UserModel> execute2;
        private readonly Action execute4;
        private readonly Action execute5;
        private readonly Action<string> execute6;
        private readonly Func<NameChangeModel, Task> execute7;
        private readonly Func<bool, Task> execute8;
        private readonly Action<ILanguage> execute9;
        private readonly Func<MessageModel, Task> execute10; 
        private readonly Func<bool> canExecute;
        public RelayCommand(Func<Task> execute) : this(execute, canExecute: null)
        {
        }
        public RelayCommand(Func<UserModel, Task> execute1) : this(execute1, canExecute: null)
        {
        }

        public RelayCommand(Action<UserModel> execute2, Action execute3, Action execute4, Action execute5) : this(execute2, execute3, execute4, execute5, canExecute: null)
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
        public RelayCommand(Func<bool, Task> execute8) : this(execute8, canExecute: null)
        {
        }

        public RelayCommand(Action<ILanguage> execute9) : this(execute9, canExecute: null)
        {
        }
        public RelayCommand(Func<MessageModel, Task> execute10) : this(execute10, canExecute: null)
        {
        }

        public RelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute is null");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Func<UserModel, Task> execute1, Func<bool> canExecute)
        {
            if (execute1 == null)
                throw new ArgumentNullException("execute1 is null");

            this.execute1 = execute1;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action<UserModel> execute2, Action execute3, Action execute4, Action execute5, Func<bool> canExecute)
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
        public RelayCommand(Func<bool, Task> execute8, Func<bool> canExecute)
        {
            if (execute8 == null)
                throw new ArgumentNullException("execute8 is null");

            this.execute8 = execute8;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action<ILanguage> execute9, Func<bool> canExecute)
        {
            if (execute9 == null)
                throw new ArgumentNullException("execute9 is null");

            this.execute9 = execute9;
            this.canExecute = canExecute;
        }

        public RelayCommand(Func<MessageModel, Task> execute10, Func<bool> canExecute)
        {
            if (execute10 == null)
                throw new ArgumentNullException("execute10 is null");

            this.execute10 = execute10;
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
                execute2(parameter as UserModel);
                execute3();
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
            else if (execute8 != null)
            {
                await execute8((bool)parameter);
            }
            else if (execute9 != null)
            {             
                execute9(parameter as ILanguage);
            }
            else if (execute10 != null)
            {
              await execute10(parameter as MessageModel);
            }
        }
    }
}
