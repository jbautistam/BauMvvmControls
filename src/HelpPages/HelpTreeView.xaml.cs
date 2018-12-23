using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Controls.BauMVVMControls.HelpPages.ViewModel;

namespace Bau.Controls.BauMVVMControls.HelpPages
{
	/// <summary>
	///		Control de usuario para mostrar un árbol de ayuda
	/// </summary>
	public partial class HelpTreeView : UserControl
	{   
		// Propiedades
		public static readonly DependencyProperty HelpFileNameProperty =
							DependencyProperty.Register(nameof(HelpFileName), typeof(string), typeof(HelpTreeView),
													    new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		// Eventos públicos
		public event EventHandler<EventArguments.HelpEventArgs> OpenHelp;
		// Variables privadas
		private HelpTreeExplorerViewModel treeViewModel;

		public HelpTreeView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el formulario
			grdData.DataContext = ViewModelData;
		}

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		public void Refresh()
		{
			ViewModelData.LoadNodes();
		}

		/// <summary>
		///		Lanza el evento para abrir una ayuda
		/// </summary>
		private void RaiseEventOpenHelp(HelpNodeViewModel selectedNode)
		{
			OpenHelp?.Invoke(this, new EventArguments.HelpEventArgs(selectedNode.HelpItem));
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public HelpTreeExplorerViewModel ViewModelData
		{
			get
			{ 
				// Crea la colección de nodos si no estaba en memoria
				if (treeViewModel == null)
				{ 
					// Asigna la página de ayuda
					if (string.IsNullOrWhiteSpace(HelpFileName))
						HelpFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\Help.xml");
					// Asigna el dataContext
					treeViewModel = new HelpTreeExplorerViewModel(HelpFileName);
				}
				// Devuelve el ViewModel
				return treeViewModel;
			}
		}

		/// <summary>
		///		Nombre del archivo de ayuda
		/// </summary>
		public string HelpFileName
		{
			get { return (string) GetValue(HelpFileNameProperty); }
			set
			{
				SetValue(HelpFileNameProperty, value);
				ViewModelData.HelpFileName = value;
			}
		}

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (trvExplorer.DataContext is HelpTreeExplorerViewModel && (sender as TreeView).SelectedItem is HelpNodeViewModel node)
				(trvExplorer.DataContext as HelpTreeExplorerViewModel).SelectedNode = node;
		}

		private void trvExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (ViewModelData.SelectedNode != null)
				RaiseEventOpenHelp(ViewModelData.SelectedNode);
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModelData.SelectedNode = null;
		}
	}
}