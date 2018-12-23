using System;
using System.Windows.Controls;

namespace Bau.Controls.BauMVVMControls.TreeFiles.Control
{
	/// <summary>
	///		Control para mostrar un árbol de directorios y una lista de archivos
	/// </summary>
	public partial class TreeListFilesView : UserControl
	{
		public TreeListFilesView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa los manejadores de eventos
			trvLocalFiles.SelectedPathChanged += (sender, evntArgs) => lswLocalFiles.SourcePath = trvLocalFiles.SelectedPath;
		}
	}
}
