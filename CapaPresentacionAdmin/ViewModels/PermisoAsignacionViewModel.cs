using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacionAdmin.ViewModels
{
	public class PermisoAsignacionViewModel
	{
        public int IdPermiso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Asignado { get; set; }
    }
}