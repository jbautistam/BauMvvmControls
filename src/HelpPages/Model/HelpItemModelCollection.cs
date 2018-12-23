using System;
using System.Collections.Generic;

namespace Bau.Controls.BauMVVMControls.HelpPages.Model
{
	/// <summary>
	///		Colección de <see cref="HelpItemModel"/>
	/// </summary>
	public class HelpItemModelCollection : List<HelpItemModel>
	{
		/// <summary>
		///		Obtiene el código de una colección de elementos
		/// </summary>
		public string GetCode(int tabs)
		{
			string code = "";

				// Añade el código
				foreach (HelpItemModel item in this)
				{
					string child = item.GetCode(tabs);

						if (!string.IsNullOrEmpty(child))
						{
							if (!string.IsNullOrEmpty(code))
								code += Environment.NewLine;
							code += child;
						}
				}
				// Devuelve el código
				return code;
		}
	}
}
