using System;

namespace Bau.Controls.BauMVVMControls.TreeFiles.ViewModel
{
	/// <summary>
	///		ViewModel para una lista de archivos
	/// </summary>
	public class FileListViewModel : Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.ControlGenericListViewModel<FileListItemViewModel>
	{   
		// Eventos públicos
		public event EventHandler<EventArguments.FileEventArgs> OpenFile;
		// Variables privadas
		private string _sourcePath;

		public FileListViewModel(string sourcePath)
		{
			SourcePath = sourcePath;
		}

		/// <summary>
		///		Carga los elementos
		/// </summary>
		public void LoadItems()
		{ 
			// Limpia los elementos
			Items.Clear();
			// Añade los archivos
			if (!string.IsNullOrEmpty(SourcePath) && System.IO.Directory.Exists(SourcePath))
				try
				{
					foreach (string fileName in System.IO.Directory.GetFiles(SourcePath))
						Items.Add(new FileListItemViewModel(fileName));
				}
				catch { }
		}

		/// <summary>
		///		Directorio base de la lista
		/// </summary>
		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				if (CheckProperty(ref _sourcePath, value))
					LoadItems();
			}
		}
	}
}
