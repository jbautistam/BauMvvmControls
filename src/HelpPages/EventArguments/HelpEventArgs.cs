using System;

namespace Bau.Controls.BauMVVMControls.HelpPages.EventArguments
{
	/// <summary>
	///		Argumento de los eventos de tratamiento de una página de ayud
	/// </summary>
	public class HelpEventArgs : EventArgs
	{
		public HelpEventArgs(Model.HelpItemModel helpItem)
		{
			HelpItem = helpItem;
		}

		/// <summary>
		///		Elemento de ayuda
		/// </summary>
		public Model.HelpItemModel HelpItem { get; }
	}
}
