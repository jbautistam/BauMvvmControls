using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Controls.BauMVVMControls.HelpPages.Model;

namespace Bau.Controls.BauMVVMControls.HelpPages.Repository
{
	/// <summary>
	///		Clase para cargar los elementos de ayuda de un archivo XML
	/// </summary>
	public class HelpItemRepository
	{   
		// Constantes privadas
		private const string TagRoot = "HelpPages";
		private const string TagPage = "Page";
		private const string TagTitle = "Title";
		private const string TagDescription = "Description";
		private const string TagCode = "Code";

		/// <summary>
		///		Carga una serie de elementos de ayuda
		/// </summary>
		public HelpItemModelCollection Load(string fileName)
		{
			HelpItemModelCollection items = new HelpItemModelCollection();

				// Carga los datos
				if (System.IO.File.Exists(fileName))
				{
					MLFile fileML = LoadFile(fileName);

						// Obtiene los datos
						if (fileML != null)
							foreach (MLNode objMLRoot in fileML.Nodes)
								if (objMLRoot.Name == TagRoot)
									items.AddRange(LoadPages(objMLRoot));
				}
				// Devuelve la colección de elementos
				return items;
		}

		/// <summary>
		///		Carga las páginas de un nodo
		/// </summary>
		private HelpItemModelCollection LoadPages(MLNode nodeML)
		{
			HelpItemModelCollection pages = new HelpItemModelCollection();

				// Carga las páginas
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagPage)
					{
						HelpItemModel page = new HelpItemModel();

							// Asigna las propiedades de la página
							page.Title = childML.Attributes[TagTitle].Value;
							page.Description = childML.Nodes[TagDescription].Value;
							page.Code = childML.Nodes[TagCode].Value;
							// Añade las páginas hija
							page.Childs.AddRange(LoadPages(childML));
							// Añade la página a la colección
							pages.Add(page);
					}
				// Devuelve la colección de páginas
				return pages;
		}

		/// <summary>
		///		Carga un archivo XML sin tener en cuenta las excepciones
		/// </summary>
		private MLFile LoadFile(string fileName)
		{
			return new Libraries.LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);
		}
	}
}
