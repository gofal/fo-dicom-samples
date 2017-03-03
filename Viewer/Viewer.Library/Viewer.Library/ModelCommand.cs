using System;
using System.Windows.Input;

namespace Viewer.Library
{
    public class ModelCommand<T> : ICommand
    {

        #region Private Fields

        private Predicate<T> _canExecute;
        private Action<T> _execute;

        #endregion

        #region public Properties

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        public ModelCommand(Action<T> executeMethod)
        {
            _execute = executeMethod;
            _canExecute = (T t) => true;
        }

        public ModelCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod)
        {
            _execute = executeMethod;
            _canExecute = canExecuteMethod;
        }

        #endregion

        #region public Methods

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion

        #region private Methods

        #endregion

    }
}
