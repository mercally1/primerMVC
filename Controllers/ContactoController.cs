using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoCrud.Controllers;
using ProyectoCrud.Models;
using System.Data;

namespace ProyectoCrud.Controllers
{
    public class ContactoController : Controller
    {

        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private static List<Contacto> olista = new List<Contacto>();

        //in this view it do select the table 

        // GET: Contacto
        public ActionResult Inicio()
        {
            olista = new List<Contacto>();

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACTO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();

                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombre = dr["Nombre"].ToString();
                        nuevoContacto.Apellido = dr["Apellido"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        olista.Add(nuevoContacto);
                    }
                }
            }
            return View(olista);
        }

        public ActionResult Registrar()
        {
            return View();
        }

        //the view of the form edit
        public ActionResult Editar(int? idcontacto)
        {

            if (idcontacto == null)
                return RedirectToAction("Inicio", "Contacto");

            Contacto ocontacto = olista.Where(c => c.IdContacto == idcontacto).FirstOrDefault();


            return View(ocontacto);

        }


        public ActionResult Eliminar(int? idcontacto)
        {

            if (idcontacto == null)
                return RedirectToAction("Inicio", "Contacto");

            Contacto ocontacto = olista.Where(c => c.IdContacto == idcontacto).FirstOrDefault();


            return View(ocontacto);

        }


        //the view of the form register
        [HttpPost]
        public ActionResult Registrar(Contacto ocontacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar", oconexion);

                cmd.Parameters.AddWithValue("Nombre", ocontacto.Nombre);
                cmd.Parameters.AddWithValue("Apellido", ocontacto.Apellido);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();  
            }

            return RedirectToAction("Inicio", "Contacto");
        }

        [HttpPost]
        public ActionResult Editar(Contacto ocontacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", ocontacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombre", ocontacto.Nombre);
                cmd.Parameters.AddWithValue("Apellido", ocontacto.Apellido);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Inicio", "Contacto");
        }

        [HttpPost]
        public ActionResult Eliminar(string IdContacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", IdContacto);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Inicio", "Contacto");
        }
    }

}