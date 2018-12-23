using System;

namespace Bau.Controls.BauMVVMControls.HelpPages.Model
{
	/// <summary>
	///		Elemento de ayuda
	/// </summary>
	public class HelpItemModel
	{ 
		// Variables privadas
		private string _description, _code;

		/// <summary>
		///		Obtiene el código de la ayuda
		/// </summary>
		public string GetCode(int tabs)
		{
			if (!string.IsNullOrEmpty(Code))
				return Normalize(Code, tabs).Replace("@@Content@@", Childs.GetCode(tabs + 1));
			else
				return "";
		}

		/// <summary>
		///		Normaliza una cadena quitándole los tabuladores iniciales
		/// </summary>
		private string Normalize(string content, int previousTabs)
		{
			string result = "";

				// Normaliza el número de tabuladores de una cadena
				if (!string.IsNullOrEmpty(content))
				{
					string[] contentParts = content.Split('\n');

						if (contentParts.Length > 0)
						{
							int startTabs = CountTabs(contentParts[GetFirstLine(contentParts)]);

								foreach (string value in contentParts)
								{ 
									// Añade un salto de línea
									if (!string.IsNullOrEmpty(result))
										result += Environment.NewLine;
									// Añade la cadena con los tabuladores
									if (!string.IsNullOrEmpty(value))
									{
										int tabs = CountTabs(value) - startTabs + previousTabs;

											// Añade los tabuladores necesarios
											if (tabs > 0)
												result += new string('\t', tabs);
											// Añade la cadena
											result += value.Trim();
									}
								}
						}
				}
				// Devuelve la cadena resultante
				return result;
		}

		/// <summary>
		///		Obtiene la primera línea no vacía de un array
		/// </summary>
		private int GetFirstLine(string[] contentParts)
		{ 
			// Recorre las líneas
			for (int index = 0; index < contentParts.Length; index++)
				if (!string.IsNullOrEmpty(contentParts[index]))
					return index;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return 0;
		}

		/// <summary>
		///		Cuenta los tabuladores de una cadena
		/// </summary>
		private int CountTabs(string content)
		{
			int tabs = 0;
			bool found = false;

				// Cuenta los tabuladores				
				foreach (char character in content)
					if (!found)
					{
						if (character == '\t')
							tabs++;
						else
							found = true;
					}
				// Devuelve el número de tabuladores
				return tabs;
		}

		/// <summary>
		///		Título del nodo
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Descripción
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = Normalize(value, 0); }
		}

		/// <summary>
		///		Código 
		/// </summary>
		public string Code
		{
			get { return _code; }
			set { _code = Normalize(value, 0); }
		}

		/// <summary>
		///		Indica si es una carpeta (tiene elementos hijo)
		/// </summary>
		public bool IsFolder
		{
			get { return Childs.Count != 0; }
		}

		/// <summary>
		///		Elementos hijo
		/// </summary>
		public HelpItemModelCollection Childs { get; } = new HelpItemModelCollection();
	}
}
