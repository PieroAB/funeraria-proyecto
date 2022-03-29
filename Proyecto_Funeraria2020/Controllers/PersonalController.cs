using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Proyecto_Funeraria2020.Models;
using System.IO;

namespace Proyecto_Funeraria2020.Controllers
{
    public class PersonalController : Controller
    {
        SqlConnection cn = new SqlConnection("server=.;database=DBFUNERARIA2020; integrated security=true");

        IEnumerable<Cargo> listaCargos()
        {
            List<Cargo> temporal = new List<Cargo>();
            using (SqlCommand cmd = new SqlCommand("sp_listaCargo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Cargo reg = new Cargo();
                    reg.codCargo = dr.GetInt32(0);
                    reg.desCargo = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        IEnumerable<Genero> listaGeneros()
        {
            List<Genero> temporal = new List<Genero>();
            using (SqlCommand cmd = new SqlCommand("sp_listaGenero", cn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Genero reg = new Genero();
                    reg.codGenero = dr.GetString(0);
                    reg.desgenero = dr.GetString(1);
                    temporal.Add(reg);

                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        IEnumerable<EstadoPersonal> listaEstados()
        {
            List<EstadoPersonal> temporal = new List<EstadoPersonal>();
            using (SqlCommand cmd = new SqlCommand("sp_listaEstadoPersonal", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EstadoPersonal reg = new EstadoPersonal();
                    reg.codEstado = dr.GetInt32(0);
                    reg.desEstado = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        IEnumerable<Distrito> listaDistritos()
        {
            List<Distrito> temporal = new List<Distrito>();
            using (SqlCommand cmd = new SqlCommand("sp_listaDistrito", cn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Distrito reg = new Distrito();
                    reg.codDistrito = dr.GetInt32(0);
                    reg.nomDistrito = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        IEnumerable<Personal> ListaPersonales(String para = "")
        {
            List<Personal> temporal = new List<Personal>();
            using (SqlCommand cmd = new SqlCommand("sp_listaPersonal", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@lik", para);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Personal reg = new Personal();
                    reg.dniPersonal = dr.GetString(0);
                    reg.nomPersonal = dr.GetString(1);
                    reg.apePersonal = dr.GetString(2);
                    reg.codGenero = dr.GetString(3);
                    reg.dirPersonal = dr.GetString(4);
                    reg.codDistrito = dr.GetInt32(5);
                    reg.fecNacPersonal = dr.GetDateTime(6);
                    reg.telPersonal = dr.GetString(7);
                    reg.emailPersonal = dr.GetString(8);
                    reg.codCargo = dr.GetInt32(9);
                    reg.sueldoPersonal = dr.GetDecimal(10);
                    reg.usuPersonal = dr.GetString(11);
                    reg.conPersonal = dr.GetString(12);
                    reg.codEstadoPersonal = dr.GetInt32(13);
                    reg.imgPersonal = dr.GetString(14);
                    reg.asigPersonal = dr.GetString(15);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        /*ACCESO AL SISTEMA*/
        public ActionResult login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult login(string x = "", string y = "") {

            Personal per = null;
            if ((x == "" || y == "") || (x == null || y == null  )) {
                ViewBag.login = "Contraseña o Usuario Incorrecto";
                return View();
            }
            else {
                using (SqlCommand cmd = new SqlCommand("sp_accesoPersonal", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usu", x);
                    cmd.Parameters.AddWithValue("@con", y);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        per = new Personal();
                        per.dniPersonal = dr.GetString(0);
                        per.nomPersonal = dr.GetString(1);
                        per.apePersonal = dr.GetString(2);
                        per.codGenero = dr.GetString(3);
                        per.dirPersonal = dr.GetString(4);
                        per.codDistrito = dr.GetInt32(5);
                        per.fecNacPersonal = dr.GetDateTime(6);
                        per.telPersonal = dr.GetString(7);
                        per.emailPersonal = dr.GetString(8);
                        per.codCargo = dr.GetInt32(9);
                        per.sueldoPersonal = dr.GetDecimal(10);
                        per.usuPersonal = dr.GetString(11);
                        per.conPersonal = dr.GetString(12);
                        per.codEstadoPersonal = dr.GetInt32(13);
                        per.imgPersonal = dr.GetString(14);
                    }
                    if (per == null)
                    {
                        ViewBag.login = "Contraseña o Usuario Incorrecto";
                        return View();
                    }
                    else if (per!=null && per.codCargo == 3) {
                        ViewBag.login = "No cuenta con permisos necesarios";
                        return View();
                    }
                    else
                    {
                        string pagina = "";
                        string controller = "";
                        ViewBag.personal = per.nomPersonal;
                        ViewBag.imagen = per.imgPersonal;
                        TempData["login"] = "Bienvenido " + per.nomPersonal;
                        Session["usuario"] = per;
                        if (per.codCargo == 1) {
                            pagina = "Personales";
                            controller = "Personal";
                        }
                        if (per.codCargo == 2) {
                            pagina = "Representantes";
                            controller = "Representante";
                        }
                        if (per.codCargo == 4) {
                            pagina = "Boletas";
                            controller = "Venta";
                        }
                        if (per.codCargo == 5) {
                            pagina = "CreateProducto";
                            controller = "Item";
                        }
                        return RedirectToAction(pagina, controller);
                    }
                }
            }
        }

        /*CERRAR SESION*/
        public ActionResult CerrarSesion() {

            if (Session["usuario"] == null)
            {

                return RedirectToAction("login");
            }
            Personal p = (Personal)Session["usuario"];
            TempData["login"] = "Lo esperamos pronto " + p.nomPersonal;


            Session.Clear();
            return RedirectToAction("login");
        }


        /* CRUD PERSONAL*/


        // LISTADO
        public ActionResult Personales(int v = 0, string para = "", string boton = "")
        {
            if (Session["usuario"] == null) {

                return RedirectToAction("login");
            }

            /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
            Personal p = (Personal)Session["usuario"];
            ViewBag.personal = p.nomPersonal;
            ViewBag.imagen = p.imgPersonal;
            ViewBag.cargo = p.codCargo;

            int filas = 4;
                IEnumerable<Personal> temporal = ListaPersonales(para);
                int n = temporal.Count();
                int npags = n % filas > 0 ? n / filas + 1 : n / filas;
                if (boton == "r")
                {
                    v -= 1;
                    if (v < 0) v = 0;
                }
                else if (boton == "a")
                {
                    v += 1;
                    if (v > npags - 1) v = npags - 1;
                }
                else if (boton == "au")
                {
                    v = npags - 1;
                }
                else if (boton == "rp")
                {

                    v = 0;
                }
                ViewBag.para = para;
                ViewBag.v = v;
                ViewBag.npags = npags;
             //   ViewBag.listado = temporal;

                return View(temporal.Skip(v * filas).Take(filas)); }

            //ListaPersonales(para)
   
        string CRUDPersonal(string procedure, List<SqlParameter> arreglo)
        {
            string mensaje = "";
            try
            {
                using (SqlCommand cmd = new SqlCommand(procedure, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arreglo.ToArray());
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (procedure.Equals("sp_insertPersonal"))
                    {
                        mensaje = "Personal Registrado ";
                    }
                    if (procedure.Equals("sp_updatePersonal"))
                    {
                        mensaje = "Personal Actualizado ";
                    }
                    if (procedure.Equals("sp_deletePersonal"))
                    {
                        TempData["pers"] = "Personal Eliminado Exitosamente";
                    }
                    if (procedure.Equals("sp_addUsuario"))
                    {

                        mensaje = "Usuario asignado Correctamente";
                    }
                }
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.Close(); ViewBag.mensaje = mensaje; }
            return mensaje;
        }

        /*
        public Personal BuscarPersonal(string id)
        {
            return ListaPersonales().Where(x => x.dniPersonal == id).FirstOrDefault();
        }
        */

        /*CREAR PERSONAL*/
        public ActionResult CreatePersonal()
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("login");
            }

            else
            {
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                Personal p = (Personal)Session["usuario"];
                ViewBag.personal = p.nomPersonal;
                ViewBag.imagen = p.imgPersonal;
                ViewBag.cargo = p.codCargo;
                /*-----------------------------*/

                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero");
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito");
                ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo");
                return View(new Personal());
            }
        }

        [HttpPost]
        public ActionResult CreatePersonal(Personal reg, HttpPostedFileBase archivo)
        {
            Personal a = (Personal)Session["usuario"];

            /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
            ViewBag.personal = a.nomPersonal;
            ViewBag.imagen = a.imgPersonal;
            ViewBag.cargo = a.codCargo;
            /*-----------------------------*/


            if (ModelState.IsValid == false) {

                return View(reg); }

            if (archivo == null)
            {

                ViewBag.mensaje = "Seleccione una imagen";
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
                ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);
                return View(reg);
            }
            if (Path.GetExtension(archivo.FileName) != ".png"
              && Path.GetExtension(archivo.FileName) != ".jpg")
            {

                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
                ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            List<SqlParameter> arreglo = new List<SqlParameter>() {

                new SqlParameter(){ ParameterName="@dni", SqlDbType= SqlDbType.Char, Value=reg.dniPersonal},
                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomPersonal },
                new SqlParameter(){ ParameterName="@ape", SqlDbType=SqlDbType.VarChar, Value=reg.apePersonal},
                new SqlParameter(){ ParameterName="@gen", SqlDbType=SqlDbType.Char, Value=reg.codGenero },
                new SqlParameter(){ ParameterName="@dir",SqlDbType=SqlDbType.VarChar, Value=reg.dirPersonal},
                new SqlParameter(){ ParameterName="@dis",SqlDbType=SqlDbType.Int, Value=reg.codDistrito},
                new SqlParameter(){ ParameterName="@nac",SqlDbType=SqlDbType.DateTime, Value=reg.fecNacPersonal},
                new SqlParameter(){ ParameterName="@tel",SqlDbType=SqlDbType.VarChar, Value=reg.telPersonal},
                new SqlParameter(){ ParameterName="@email",SqlDbType=SqlDbType.VarChar, Value=reg.emailPersonal},
                new SqlParameter(){ ParameterName="@car",SqlDbType=SqlDbType.Int, Value=reg.codCargo},
                new SqlParameter(){ ParameterName="@sue",SqlDbType=SqlDbType.Decimal, Value=reg.sueldoPersonal},

                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar,
                    Value="~/Imagenes/Personal/"+Path.GetFileName(archivo.FileName)}
            };
            ViewBag.mensaje = CRUDPersonal("sp_insertPersonal", arreglo);

            archivo.SaveAs(Path.Combine(Server.MapPath("~/Imagenes/Personal/") + Path.GetFileName(archivo.FileName)));

            ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
            ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
            ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);


            return View(reg);
        }


        /*ACTUALIZAR PERSONAL*/
        public ActionResult EditPersonal(String id = null)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("login");
            }
            else {

                Personal a = (Personal)Session["usuario"];

            /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
                ViewBag.imagen = a.imgPersonal;
                ViewBag.cargo = a.codCargo;
                /*-----------------------------*/


                Personal reg = ListaPersonales().Where(x => x.dniPersonal == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("Personales");
                else
                {
                    ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                    ViewBag.estadosPer = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstadoPersonal);
                    ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
                    ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);
                    return View(reg);
                } 
            }
        }

        [HttpPost]
        public ActionResult EditPersonal(Personal reg, HttpPostedFileBase archivo)
        {
            string ruta;
            Personal a = (Personal)Session["usuario"];

            /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
            ViewBag.personal = a.nomPersonal;
            ViewBag.imagen =a.imgPersonal;
            ViewBag.cargo = a.codCargo;
            /*-----------------------------*/

            if (archivo == null)
            {
                ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.estadosper = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstadoPersonal);
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
 
                ruta = reg.imgPersonal;
            }
            else
            if (Path.GetExtension(archivo.FileName) != ".jpg"
             && Path.GetExtension(archivo.FileName) != ".png")
            {
                ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.estadosper = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstadoPersonal);
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            else
            
                ruta = "~/Imagenes/Personal/" + Path.GetFileName(archivo.FileName);
            
            
            List<SqlParameter> arreglo = new List<SqlParameter>(){
                new SqlParameter(){ ParameterName="@dni", SqlDbType= SqlDbType.Char, Value=reg.dniPersonal},
                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomPersonal },
                new SqlParameter(){ ParameterName="@ape", SqlDbType=SqlDbType.VarChar, Value=reg.apePersonal},
                new SqlParameter(){ ParameterName="@gen", SqlDbType=SqlDbType.Char, Value=reg.codGenero },
                new SqlParameter(){ ParameterName="@dir",SqlDbType=SqlDbType.VarChar, Value=reg.dirPersonal},
                new SqlParameter(){ ParameterName="@dis",SqlDbType=SqlDbType.Int, Value=reg.codDistrito},
                new SqlParameter(){ ParameterName="@nac",SqlDbType=SqlDbType.DateTime, Value=reg.fecNacPersonal},
                new SqlParameter(){ ParameterName="@tel",SqlDbType=SqlDbType.VarChar, Value=reg.telPersonal},
                new SqlParameter(){ ParameterName="@email",SqlDbType=SqlDbType.VarChar, Value=reg.emailPersonal},
                new SqlParameter(){ ParameterName="@car",SqlDbType=SqlDbType.Int, Value=reg.codCargo},
                new SqlParameter(){ ParameterName="@sue",SqlDbType=SqlDbType.Decimal, Value=reg.sueldoPersonal},
                new SqlParameter(){ ParameterName="@est",SqlDbType=SqlDbType.Int, Value=reg.codEstadoPersonal},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar, Value=ruta}
                };
            ViewBag.mensaje = CRUDPersonal("sp_updatePersonal", arreglo);

            if (archivo != null)
                archivo.SaveAs(Server.MapPath(ruta));
            /*
                        reg = BuscarPersonal(reg.dniPersonal);*/
            ViewBag.cargos = new SelectList(listaCargos(), "codCargo", "desCargo", reg.codCargo);
            ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
            ViewBag.estadosper = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstadoPersonal);
            ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);

            return View(reg);
        }


        /*ELIMINAR EL PERSONAL*/
        public ActionResult DeletePersonal(string id = "")
        {
            List<SqlParameter> arreglo = new List<SqlParameter>() {
             new SqlParameter(){ ParameterName="@dni", SqlDbType= SqlDbType.Char,Value=id }
            };
            ViewBag.mensaje = CRUDPersonal("sp_deletePersonal", arreglo);
            TempData["msj"] = "Personal " + id + " Eliminado";
            return RedirectToAction("Personales");
        }


        /*ASIGNAR USUARIO*/
        public ActionResult AsignarUsuario(String id = "")
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("login");
            }

            else
            {
                Personal a = (Personal)Session["usuario"];
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
                ViewBag.imagen = a.imgPersonal;
                ViewBag.cargo = a.codCargo;
                /*-----------------------------*/

                Personal reg = ListaPersonales().Where(x => x.dniPersonal == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("Personales");
                else
                {
                    return View(reg);
                }
            }
        }

        [HttpPost]
        public ActionResult AsignarUsuario(Personal reg)
        {
            Personal a = (Personal)Session["usuario"];
            /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
            ViewBag.personal = a.nomPersonal;
            ViewBag.imagen = a.imgPersonal;
            ViewBag.cargo = a.codCargo;
            /*-----------------------------*/

            List<SqlParameter> arreglo = new List<SqlParameter>(){
                new SqlParameter(){ ParameterName="@dni", SqlDbType= SqlDbType.Char, Value=reg.dniPersonal},
                 new SqlParameter(){ ParameterName="@usu", SqlDbType= SqlDbType.VarChar, Value=reg.usuPersonal},
                  new SqlParameter(){ ParameterName="@con", SqlDbType= SqlDbType.VarChar, Value=reg.conPersonal}
                };
            ViewBag.mensaje = CRUDPersonal("sp_addUsuario", arreglo);

            return View(reg);

        }

    }
}