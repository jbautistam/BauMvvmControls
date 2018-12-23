using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;
using Bau.Controls.BauMVVMControls.HelpPages.Model;

namespace Bau.Controls.BauMVVMControls.HelpPages.ViewModel
{
	/// <summary>
	///		ViewModel para el árbol de soluciones
	/// </summary>
	public class HelpTreeExplorerViewModel : TreeViewModel<HelpNodeViewModel>
	{   
		// Variables privadas
		private string _helpFileName;
		private HelpItemModel _helpSelected;

		public HelpTreeExplorerViewModel(string fileName)
		{
			HelpFileName = fileName;
			ChangeUpdated = false;
			InitCommands();
		}

		/// <summary>
		///		Inicializa los comandos
		/// </summary>
		private void InitCommands()
		{
			PropertyChanged += (sender, evntArgs) =>
									{
										if (evntArgs.PropertyName == nameof(SelectedNode))
										{
											SelectedNode = GetSelectedItem();
											HelpPageSelected = SelectedNode?.HelpItem;
										}
									};
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override void LoadNodesData()
		{
			if (!string.IsNullOrEmpty(HelpFileName))
				foreach (HelpItemModel helpItem in new Repository.HelpItemRepository().Load(HelpFileName))
					Children.Add(new HelpNodeViewModel(null, helpItem));
		}

		/// <summary>
		///		Obtiene el nodo seleccionado
		/// </summary>
		public HelpNodeViewModel GetSelectedItem()
		{
			if (SelectedNode != null && SelectedNode is HelpNodeViewModel helpNode)
				return helpNode;
			else
				return new HelpNodeViewModel(null, new HelpItemModel());
		}

		/// <summary>
		///		Nombre del archivo de ayuda
		/// </summary>
		public string HelpFileName
		{
			get { return _helpFileName; }
			set
			{
				if (CheckProperty(ref _helpFileName, value))
					LoadNodes();
			}
		}

		/// <summary>
		///		Elemento de ayuda seleccionado
		/// </summary>
		public HelpItemModel HelpPageSelected
		{
			get { return _helpSelected; }
			set { CheckObject(ref _helpSelected, value); }
		}
	}
}
