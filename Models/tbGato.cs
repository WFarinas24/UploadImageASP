using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_Gatos.Models
{
	public class tbGato
	{
		[Key]
		public int id { get; set; }
		public string nombre { get; set; }
		public byte[] imagen { get; set; }

	}
}
