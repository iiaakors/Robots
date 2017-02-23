using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RobotsInterview
{
	public class DelegateCommand : ICommand
	{
		public DelegateCommand(Action executedelegate, Func<bool> canexecutedelegate)
		{
			ExecuteDelegate = executedelegate;
			CanExecuteDelegate = canexecutedelegate;
		}

		private Action ExecuteDelegate { get; set; }

		private Func<bool> CanExecuteDelegate { get; set; }

		public bool CanExecute(object parameter)
		{
			return CanExecuteDelegate();
		}

		public void Execute(object parameter)
		{
			ExecuteDelegate();
		}

		public event EventHandler CanExecuteChanged;
		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, EventArgs.Empty);
		}
		
	}
}
