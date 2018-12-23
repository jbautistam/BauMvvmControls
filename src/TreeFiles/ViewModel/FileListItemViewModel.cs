using System;

namespace Bau.Controls.BauMVVMControls.TreeFiles.ViewModel
{
	/// <summary>
	///		ViewModel para un elemento de una lista de archivos
	/// </summary>
	public class FileListItemViewModel : Libraries.BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel
	{   
		// Variables privadas
		private string _fullFileName, _fileName;
		private DateTime _createdAt;
		private long _size;

		public FileListItemViewModel(string fileName) : base(fileName, fileName)
		{
			FullFileName = fileName;
		}

		/// <summary>
		///		Actualiza los datos del archivo
		/// </summary>
		private void Refresh()
		{
			bool initialized = false;

				// Inicializa los datos del archivo
				if (System.IO.File.Exists(FullFileName))
					try
					{
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(FullFileName);

							// Asigna los datos
							FileName = System.IO.Path.GetFileName(FullFileName);
							DateCreate = fileInfo.CreationTime;
							Size = fileInfo.Length;
							// Indica que se ha inicializado correctamente
							initialized = true;
					}
					catch { }
				// Si no se ha inicializado, se muestran los datos de error
				if (!initialized)
				{
					FileName = "Error";
					DateCreate = DateTime.Now;
					Size = 0;
				}
		}

		/// <summary>
		///		Nombre completo del archivo
		/// </summary>
		public string FullFileName
		{
			get { return _fullFileName; }
			set
			{
				if (CheckProperty(ref _fullFileName, value))
					Refresh();
			}
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Fecha de creación
		/// </summary>
		public DateTime DateCreate
		{
			get { return _createdAt; }
			set { CheckProperty(ref _createdAt, value); }
		}

		/// <summary>
		///		Tamaño del archivo
		/// </summary>
		public long Size
		{
			get { return _size; }
			set { CheckProperty(ref _size, value); }
		}
	}
}
