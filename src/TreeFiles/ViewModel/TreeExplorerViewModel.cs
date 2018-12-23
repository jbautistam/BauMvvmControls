using System;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;

namespace Bau.Controls.BauMVVMControls.TreeFiles.ViewModel
{
	/// <summary>
	///		ViewModel para el árbol de soluciones
	/// </summary>
	public class TreeExplorerViewModel : TreeViewModel<FileNodeViewModel>
	{   
		// Eventos públicos
		public event EventHandler<EventArguments.FileEventArgs> OpenFile;
		public event EventHandler<EventArguments.FileEventArgs> ChangedFile;
		// Variables privadas
		private string _sourcePath, _selectedPath, _selectedFile;
		private FileNodeViewModel _nodeToCopy;
		private bool _showFiles, _cut;

		public TreeExplorerViewModel(string sourcePath)
		{
			// Inicializa las propiedades
			SourcePath = sourcePath;
			ChangeUpdated = false;
			// Inicializa los comandos
			PropertyChanged += (sender, evntArgs) =>
										{
											if (evntArgs.PropertyName == nameof(SelectedNode))
												ChangeSelectedItem();
										};
			PropertiesCommand = new BaseCommand(parameter => ExecuteAction(nameof(PropertiesCommand), parameter),
												parameter => CanExecuteAction(nameof(PropertiesCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
			CopyCommand = new BaseCommand("Copiar", parameter => ExecuteAction(nameof(CopyCommand), parameter),
										  parameter => CanExecuteAction(nameof(CopyCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
			CutCommand = new BaseCommand("Cortar", parameter => ExecuteAction(nameof(CutCommand), parameter),
										 parameter => CanExecuteAction(nameof(CutCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
			PasteCommand = new BaseCommand("Pegar", parameter => ExecuteAction(nameof(PasteCommand), parameter),
										   parameter => CanExecuteAction(nameof(PasteCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
			DeleteCommand = new BaseCommand(parameter => ExecuteAction(nameof(DeleteCommand), parameter),
											parameter => CanExecuteAction(nameof(DeleteCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
			RefreshCommand = new BaseCommand(parameter => ExecuteAction(nameof(RefreshCommand), parameter),
												parameter => CanExecuteAction(nameof(RefreshCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
			RenameCommand = new BaseCommand("Cambiar nombre", parameter => ExecuteAction(nameof(RenameCommand), parameter),
											parameter => CanExecuteAction(nameof(RenameCommand), parameter))
								.AddListener(this, nameof(SelectedNode));
		}

		/// <summary>
		///		Cambia el elemento seleccionado
		/// </summary>
		private void ChangeSelectedItem()
		{
			FileNodeViewModel file = GetSelectedFile();

				if (file != null)
				{ 
					// Obtiene el directorio y archivo
					if (!file.IsFolder)
					{
						SelectedPath = System.IO.Path.GetDirectoryName(file.File);
						SelectedFile = file.File;
					}
					else
					{
						SelectedPath = file.File;
						SelectedFile = null;
					}
					// Lanza el evento de cambio de archivo
					ChangedFile?.Invoke(this, new EventArguments.FileEventArgs(file.File));
				}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		private void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(CopyCommand):
				case nameof(CutCommand):
						AddFileToCopyBuffer(action == nameof(CutCommand));
					break;
				case nameof(PasteCommand):
						PasteFile();
					break;
				case nameof(DeleteCommand):
						// DeleteFile(SelectedItem as FileNodeViewModel);
					break;
				case nameof(PropertiesCommand):
						RaiseEventOpenFile(GetSelectedFile()?.File);
					break;
				case nameof(RefreshCommand):
						LoadNodes();
					break;
				case nameof(RenameCommand):
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		private bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(PropertiesCommand):
				case nameof(DeleteCommand):
				case nameof(CopyCommand):
				case nameof(CutCommand):
				case nameof(RenameCommand):
					return SelectedNode != null;
				case nameof(PasteCommand):
					return _nodeToCopy != null;
				case nameof(RefreshCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Lanza el evento de abrir archivo
		/// </summary>
		public void RaiseEventOpenFile(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName) && !System.IO.Directory.Exists(fileName))
				OpenFile?.Invoke(this, new EventArguments.FileEventArgs(fileName));
		}

		/// <summary>
		///		Añade un archivo al buffer de copia
		/// </summary>
		private void AddFileToCopyBuffer(bool cut)
		{
			_nodeToCopy = SelectedNode;
			_cut = cut;
		}

		/// <summary>
		///		Pega un archivo
		/// </summary>
		private void PasteFile()
		{
			if (_nodeToCopy != null)
			{ 
				// Copia el elemento que teníamos en memoria sobre el nodo seleccionado
				Copy(_nodeToCopy, SelectedNode, !_cut);
				// Indica que ya no hay ningún archivo que copiar
				_nodeToCopy = null;
			}
		}

		/// <summary>
		///		Copia un nodo sobre otro
		/// </summary>
		public void Copy(FileNodeViewModel nodeSource, FileNodeViewModel nodeTarget, bool blnCopy)
		{
			/*
						if (CanCopy(nodeSource, nodeTarget))
							{ // Dependiendo de cuál sea el destino, llama a una rutina de copia
									if (nodeTarget is SolutionFolderNodeViewModel)
										{ if (nodeSource is ProjectNodeViewModel)
												PasteProject(nodeSource as ProjectNodeViewModel, Solution,
																		 (nodeTarget as SolutionFolderNodeViewModel).Folder, blnCopy);
											else if (nodeSource is SolutionFolderNodeViewModel)
												PasteSolution((nodeSource as SolutionFolderNodeViewModel).Folder,
																			(nodeTarget as SolutionFolderNodeViewModel).Folder, blnCopy);
										}
									else
										PasteFile(GetCopyFile(nodeSource), GetCopyFile(nodeTarget), blnCopy);
								// Actualiza
									Refresh();
							}
			*/
		}

		/// <summary>
		///		Comprueba si puede copiar un archivo en otro
		/// </summary>
		private bool CanCopy(FileNodeViewModel nodeSource, FileNodeViewModel nodeTarget)
		{
			bool blnCanCopy = false; // ... supone que no se puede copiar

			/*
							// Comprueba si se puede copiar
								if (!nodeSource.NodeID.EqualsIgnoreCase(nodeTarget.NodeID))
									{ if (nodeTarget is SolutionFolderNodeViewModel && 
													(nodeSource is SolutionFolderNodeViewModel || nodeSource is ProjectNodeViewModel))
											blnCanCopy = true;
										else if (nodeTarget is ProjectNodeViewModel && nodeSource is FileNodeViewModel)
											{ ProjectModel projectTarget = (nodeTarget as ProjectNodeViewModel).Project;
												FileModel fileSource = (nodeSource as FileNodeViewModel).File;

													if (projectTarget.Definition.GlobalId.EqualsIgnoreCase(fileSource.SearchProject().Definition.GlobalId))
														blnCanCopy = true;
											}
										else if (nodeTarget is FileNodeViewModel && nodeSource is FileNodeViewModel)
											{ FileModel source = (nodeSource as FileNodeViewModel).File;
												FileModel target = (nodeTarget as FileNodeViewModel).File;

													if (source.SearchProject().Definition.GlobalId.EqualsIgnoreCase(target.SearchProject().Definition.GlobalId))
														{ if (target.IsFolder)
																blnCanCopy = true;
														}
											}
									}
			*/
			// Devuelve el valor que indica si se puede copiar
			return blnCanCopy;
		}

		/*
				/// <summary>
				///		Pega un archivo
				/// </summary>
				private void PasteFile(FileModel fileToCopy, FileModel fileTarget, bool blnCopy)
				{ if (fileToCopy != null)
						{ string pathTarget = fileTarget.FullFileName;
							bool blnIsCopied = false;

								// Obtiene el directorio destino
									if (!System.IO.Directory.Exists(pathTarget))
										pathTarget = System.IO.Path.GetDirectoryName(pathTarget);
								// Copia / mueve el archivo / carpeta
									if (fileToCopy.IsFolder || CheckIsPackage(fileToCopy))
										{ LibCommonHelper.Files.HelperFiles.CopyPath(fileToCopy.FullFileName, 
																													 LibCommonHelper.Files.HelperFiles.GetConsecutivePath(pathTarget,
																																																					System.IO.Path.GetFileName(fileToCopy.FullFileName)));
											blnIsCopied = true; // ... supone que se ha podido copiar
										}
									else
										blnIsCopied = LibCommonHelper.Files.HelperFiles.CopyFile(fileToCopy.FullFileName, 
																																			 LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathTarget, 
																																																													System.IO.Path.GetFileName(fileToCopy.FullFileName)));
								// Si la acción es para cortar, elimina el archivo inicial
									if (blnIsCopied && !blnCopy)
										{ if (fileToCopy.IsFolder || CheckIsPackage(fileToCopy))
												LibCommonHelper.Files.HelperFiles.KillPath(fileToCopy.FullFileName);
											else
												LibCommonHelper.Files.HelperFiles.KillFile(fileToCopy.FullFileName);
										}
						}
				}
		*/

		/// <summary>
		///		Abre el archivo en el explorador
		/// </summary>
		private void OpenFileExplorer()
		{
			FileNodeViewModel file = GetSelectedFile();

				if (file != null)
				{
					string path = file.File;

						// Obtiene el nombre del directorio si se le ha pasado un archivo
						if (!System.IO.Directory.Exists(path))
							path = System.IO.Path.GetDirectoryName(path);
						// Abre el explorador de archivos
						if (System.IO.Directory.Exists(path))
							Libraries.LibSystem.Files.WindowsFiles.OpenDocumentShell("explorer.exe", path);
				}
		}

		/// <summary>
		///		Abre el archivo con Windows
		/// </summary>
		private void OpenWithWindows()
		{
			FileNodeViewModel file = GetSelectedFile();

				if (file != null && !string.IsNullOrEmpty(file.File) && System.IO.File.Exists(file.File))
					Libraries.LibSystem.Files.WindowsFiles.OpenDocumentShell(file.File);
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override void LoadNodesData()
		{
			if (!string.IsNullOrEmpty(SourcePath) && System.IO.Directory.Exists(SourcePath))
			{
				FileNodeViewModel nodeRoot = new FileNodeViewModel(null, SourcePath, ShowFiles);

					// Asigna los datos al nodo
					nodeRoot.Text = SourcePath;
					nodeRoot.IsExpanded = true;
					// Añade el nodo raíz a la colección de nodos
					Children.Add(nodeRoot);
			}
		}

		/// <summary>
		///		Obtiene el archivo seleccionado
		/// </summary>
		public FileNodeViewModel GetSelectedFile()
		{
			if (SelectedNode != null && SelectedNode is FileNodeViewModel node)
				return node;
			else
				return null;
		}

		/// <summary>
		///		Directorio base del árbol
		/// </summary>
		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				if (CheckProperty(ref _sourcePath, value))
					LoadNodes();
			}
		}

		/// <summary>
		///		Directorio seleccionado
		/// </summary>
		public string SelectedPath
		{
			get { return _selectedPath; }
			set { CheckProperty(ref _selectedPath, value); }
		}

		/// <summary>
		///		Archivo seleccionado
		/// </summary>
		public string SelectedFile
		{
			get { return _selectedFile; }
			set { CheckProperty(ref _selectedFile, value); }
		}

		/// <summary>
		///		Indica si se deben mostrar los archivos en el árbol
		/// </summary>
		public bool ShowFiles
		{
			get { return _showFiles; }
			set
			{
				if (CheckProperty(ref _showFiles, value))
					LoadNodes();
			}
		}

		/// <summary>
		///		Comando para copiar archivos / proyectos
		/// </summary>
		public BaseCommand CopyCommand { get; }

		/// <summary>
		///		Comando para cortar archivos / proyectos
		/// </summary>
		public BaseCommand CutCommand { get; }

		/// <summary>
		///		Comando para pegar archivos / proyectos
		/// </summary>
		public BaseCommand PasteCommand { get; }

		/// <summary>
		///		Comando para cambiar nombre de archivo
		/// </summary>
		public BaseCommand RenameCommand { get; }

		/// <summary>
		///		Comando para ver las propiedades de un archivo
		/// </summary>
		public BaseCommand PropertiesCommand { get; }

		/// <summary>
		///		Comando para borrar un archivo
		/// </summary>
		public BaseCommand DeleteCommand { get; }

		/// <summary>
		///		Comando para actualizar el árbol
		/// </summary>
		public BaseCommand RefreshCommand { get; }
	}
}
