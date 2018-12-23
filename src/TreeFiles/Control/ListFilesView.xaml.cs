using System;
using System.Windows;
using System.Windows.Controls;

namespace Bau.Controls.BauMVVMControls.TreeFiles.Control
{
	/// <summary>
	///		Control de usuario para mostrar una lista de archivos
	/// </summary>
	public partial class ListFilesView : UserControl
	{ 
		// Propiedades
		public static readonly DependencyProperty SourcePathProperty =
							DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(ListFilesView),
														new FrameworkPropertyMetadata("C:\\", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		// Eventos públicos
		public event EventHandler<EventArguments.FileEventArgs> OpenFile;
		// Variables privadas
		private ViewModel.FileListViewModel _listViewModel;

		public ListFilesView()
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
			lswFiles.DataContext = ViewModelData;
			lswFiles.ItemsSource = ViewModelData.Items;
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public ViewModel.FileListViewModel ViewModelData
		{
			get
			{ 
				// Crea la colección de nodos si no estaba en memoria
				if (_listViewModel == null)
				{ 
					// Asigna el dataContext
					_listViewModel = new ViewModel.FileListViewModel(SourcePath);
					// Asigna los manejadores de eventos
					_listViewModel.OpenFile += (sender, evntArgs) => OpenFile?.Invoke(this, evntArgs);
				}
				// Devuelve el ViewModel
				return _listViewModel;
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
	}
}
