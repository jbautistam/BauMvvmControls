using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Controls.BauMVVMControls.TreeFiles.ViewModel;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;

namespace Bau.Controls.BauMVVMControls.TreeFiles.Control
{
	/// <summary>
	///		control de usuario para mostrar un árbol de archivos
	/// </summary>
	public partial class TreeFilesView : UserControl
	{ 
		// Propiedades
		public static readonly DependencyProperty SourcePathProperty =
							DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(TreeFilesView),
														new FrameworkPropertyMetadata("C:\\", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static readonly DependencyProperty SelectedPathProperty =
							DependencyProperty.Register(nameof(SelectedPath), typeof(string), typeof(TreeFilesView),
														new FrameworkPropertyMetadata("C:\\", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static readonly DependencyProperty ShowFilesProperty =
							DependencyProperty.Register(nameof(ShowFiles), typeof(bool), typeof(TreeFilesView),
														new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		// Eventos
		public static readonly RoutedEvent SelectedPathChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedPathChanged), RoutingStrategy.Bubble,
																									   typeof(RoutedPropertyChangedEventHandler<string>),
																									   typeof(TreeFilesView));
		// Eventos públicos
		public event EventHandler<EventArguments.FileEventArgs> OpenFile;
		// Variables privadas
		private TreeExplorerViewModel _treeViewModel;
		private Point _startDrag;
		private readonly DragDropTreeExplorerController _dragDropController = new DragDropTreeExplorerController();

		public TreeFilesView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el formulario
			InitForm();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			trvExplorer.DataContext = ViewModelData;
			trvExplorer.ItemsSource = ViewModelData.Children;
			ViewModelData.ChangedFile += (sender, evntArgs) => RaiseSelectedPathEvent();
		}

		/// <summary>
		///		Obtiene el nombre de archivo seleccionado
		/// </summary>
		public string GetSelectedFile()
		{
			return (trvExplorer.SelectedItem as FileNodeViewModel)?.File;
		}

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		public void Refresh()
		{
			ViewModelData.LoadNodes();
		}

		/// <summary>
		///		Lanza el evento <see cref="SelectedPathChangedEvent"/>
		/// </summary>
		private void RaiseSelectedPathEvent()
		{
			FileNodeViewModel file = ViewModelData.GetSelectedFile();

				if (file?.IsFolder ?? false)
				{
					SelectedPath = file.File;
					RaiseEvent(new RoutedEventArgs(SelectedPathChangedEvent));
				}
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public TreeExplorerViewModel ViewModelData
		{
			get
			{ 
				// Crea la colección de nodos si no estaba en memoria
				if (_treeViewModel == null)
				{ 
					// Asigna el dataContext
					_treeViewModel = new TreeExplorerViewModel(SourcePath);
					// Asigna los manejadores de eventos
					_treeViewModel.OpenFile += (sender, evntArgs) => OpenFile?.Invoke(this, evntArgs);
				}
				// Devuelve el ViewModel
				return _treeViewModel;
			}
		}

		/// <summary>
		///		Directorio origen
		/// </summary>
		public string SourcePath
		{
			get { return (string) GetValue(SourcePathProperty); }
			set
			{
				SetValue(SourcePathProperty, value);
				ViewModelData.SourcePath = value;
			}
		}

		/// <summary>
		///		Directorio seleccionado
		/// </summary>
		public string SelectedPath
		{
			get { return (string) GetValue(SelectedPathProperty); }
			set
			{
				SetValue(SelectedPathProperty, value);
				ViewModelData.SelectedPath = value;
			}
		}

		/// <summary>
		///		Indica si se deben mostrar los archivos
		/// </summary>
		public bool ShowFiles
		{
			get { return (bool) GetValue(ShowFilesProperty); }
			set
			{
				SetValue(ShowFilesProperty, value);
				ViewModelData.ShowFiles = value;
			}
		}

		/// <summary>
		///		Controlador del evento <see cref="SelectedPathChangedEvent"/>
		/// </summary>
		public event RoutedEventHandler SelectedPathChanged
		{
			add { AddHandler(SelectedPathChangedEvent, value); }
			remove { RemoveHandler(SelectedPathChangedEvent, value); }
		}

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvExplorer.DataContext is TreeExplorerViewModel && (sender as TreeView)?.SelectedItem is FileNodeViewModel node)
				(trvExplorer.DataContext as TreeExplorerViewModel).SelectedNode = node;
		}

		private void trvExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ViewModelData.PropertiesCommand.Execute(null);
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModelData.SelectedNode = null;
		}

		private void trvExplorer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_startDrag = e.GetPosition(null);
		}

		private void trvExplorer_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Point mouse = e.GetPosition(null);
				Vector difference = _startDrag - mouse;

					if (mouse.X < trvExplorer.ActualWidth - 50 &&
							(Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance ||
							 Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance))
						_dragDropController.InitDragOperation(trvExplorer, trvExplorer.SelectedItem as IHierarchicalViewModel);
			}
		}

		private void trvExplorer_DragEnter(object sender, DragEventArgs e)
		{
			_dragDropController.TreatDragEnter(e);
		}

		private void trvExplorer_Drop(object sender, DragEventArgs e)
		{
			FileNodeViewModel nodeSource = _dragDropController.GetDragDropFileNode(e.Data) as FileNodeViewModel;

				if (nodeSource != null)
				{
					TreeViewItem trvNode = new Libraries.BauMvvm.Views.Tools.ToolsWpf().FindAncestor<TreeViewItem>((DependencyObject) e.OriginalSource);

						if (trvNode != null)
						{
							FileNodeViewModel nodeTarget = trvNode.Header as FileNodeViewModel;

								if (nodeSource != null && nodeTarget != null)
									ViewModelData.Copy(nodeSource, nodeTarget,
													   (e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey);
						}
				}
		}
	}
}