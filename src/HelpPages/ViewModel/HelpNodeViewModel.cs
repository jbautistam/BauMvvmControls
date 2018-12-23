using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.ViewModels.Media;
using Bau.Controls.BauMVVMControls.HelpPages.Model;

namespace Bau.Controls.BauMVVMControls.HelpPages.ViewModel
{
	/// <summary>
	///		Nodo del árbol para un archivo o carpeta
	/// </summary>
	public class HelpNodeViewModel : ControlHierarchicalViewModel
	{
		public HelpNodeViewModel(HelpNodeViewModel parent, HelpItemModel helpItem)
										: base(parent, helpItem.Title, helpItem, helpItem.IsFolder)
		{
			HelpItem = helpItem;
			if (HelpItem.IsFolder)
			{
				ImageSource = "/BauMVVMControls;component/Themes/Images/Folder.png";
				Foreground = MvvmColor.Navy;
				IsBold = true;
			}
			else
				ImageSource = "/BauMVVMControls;component/Themes/Images/File.png";
		}


		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildrenData()
		{
			if (HelpItem.IsFolder)
				try
				{ 
					foreach (HelpItemModel item in HelpItem.Childs)
						Children.Add(new HelpNodeViewModel(this, item));
				}
				catch (Exception exception)
				{
					System.Diagnostics.Debug.WriteLine("Excepción: " + exception.Message);
				}
		}

		/// <summary>
		///		Datos de la ayuda
		/// </summary>
		public HelpItemModel HelpItem { get; }
	}
}
