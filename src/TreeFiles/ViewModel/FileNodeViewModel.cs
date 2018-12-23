using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Controls.BauMVVMControls.TreeFiles.ViewModel
{
	/// <summary>
	///		Nodo del árbol para un archivo o carpeta
	/// </summary>
	public class FileNodeViewModel : ControlHierarchicalViewModel
	{
		public FileNodeViewModel(FileNodeViewModel parent, string fileName, bool showFiles)
										: base(parent, System.IO.Path.GetFileName(fileName), fileName, showFiles)
		{
			File = fileName;
			ShowFiles = showFiles;
			if (IsFolder)
				ImageSource = "/BauMVVMControls;component/Themes/Images/Folder.png";
			else
				ImageSource = "/BauMVVMControls;component/Themes/Images/File.png";
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildrenData()
		{
			if (IsFolder)
				try
				{ 
					// Carga los directorios
					foreach (string path in System.IO.Directory.GetDirectories(File))
						Children.Add(new FileNodeViewModel(this, path, ShowFiles));
					// Carga los archivos
					if (ShowFiles)
						foreach (string file in System.IO.Directory.GetFiles(File))
							Children.Add(new FileNodeViewModel(this, file, ShowFiles));
				}
				catch (Exception exception)
				{
					System.Diagnostics.Debug.WriteLine("Excepción: " + exception.Message);
				}
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public string File { get; }

		/// <summary>
		///		Indica si se deben mostrar los archivos de un directorio
		/// </summary>
		public bool ShowFiles { get; }

		/// <summary>
		///		Indica si es una carpeta
		/// </summary>
		public bool IsFolder
		{
			get { return System.IO.Directory.Exists(File); }
		}
	}
}
