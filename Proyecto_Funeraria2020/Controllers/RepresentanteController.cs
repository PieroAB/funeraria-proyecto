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
    public class RepresentanteController : Controller
    {
        SqlConnection cn = new SqlConnection("server=.;database=DBFUNERARIA2020; integrated security=true");


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

        IEnumerable<Representante> ListaRepresentantes(String rep = "")
        {
            List<Representante> temporal = new List<Representante>();
            using (SqlCommand cmd = new SqlCommand("sp_listaRepresentantes", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@lik", rep);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Representante reg = new Representante();
                    reg.codRepresentante = dr.GetString(0);
                    reg.codTipoDocumento = dr.GetInt32(1);
                    reg.codTipoPersona = dr.GetInt32(2);
                    reg.nomRepresentante = dr.GetString(3);
                    reg.apeRepresentante = dr.GetString(4);
                    reg.codGenero = dr.GetString(5);
                    reg.dirRepresentante = dr.GetString(6);
                    reg.codDistrito = dr.GetInt32(7);
                    reg.telRepresentante = dr.GetString(8);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<TipoDocumento> listaTipoDocumentoRepresentante()
        {
            List<TipoDocumento> temporal = new List<TipoDocumento>();
            using (SqlCommand cmd = new SqlCommand("sp_listaTipoDocR", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TipoDocumento reg = new TipoDocumento();
                    reg.codTipoDocumento = dr.GetInt32(0);
                    reg.desTipoDocumento = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<TipoPersona> listaTipoPersona()
        {
            List<TipoPersona> temporal = new List<TipoPersona>();
            using (SqlCommand cmd = new SqlCommand("sp_listaTipoPersona", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TipoPersona reg = new TipoPersona();
                    reg.codTipoPersona = dr.GetInt32(0);
                    reg.desTipoPersona = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        public ActionResult Representantes(int v=0, string para = "", string boton="")
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("login","Personal");
            }
            else
            {
                Personal a = (Personal)Session["usuario"];
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
                ViewBag.imagen = a.imgPersonal;
                ViewBag.cargo = a.codCargo;
                /*-----------------------------*/

                int filas = 4;
                IEnumerable<Representante> temporal = ListaRepresentantes(para);
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

                //ViewBag.listado = listaDifunto(para);
                return View(temporal.Skip(v * filas).Take(filas));
            }
        }

        string CRUDRepresentante(string procedure, List<SqlParameter> arreglo)
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
                    if (procedure.Equals("sp_insertRepresentante"))
                    {
                        mensaje = "Representante Registrado";
                    }
                    if (procedure.Equals("sp_updateRepresentante"))
                    {
                        mensaje = "Representante Actualizado";
                    }
                    if (procedure.Equals("sp_deleteRepresentante"))
                    {
                        TempData["repr"] = "Representante Eliminado Exitosamente";
                    }
                }
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.Close(); ViewBag.mensaje = mensaje; }
            return mensaje;
        }

        /*CREAR REPRESENTANTE*/

        public ActionResult CreateRepresentante()
        {

            if (Session["usuario"] == null)
            {

                return RedirectToAction("login","Personal");
            }
            else
            {
                Personal a = (Personal)Session["usuario"];
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
                ViewBag.imagen = a.imgPersonal;
                ViewBag.cargo = a.codCargo;
                /*-----------------------------*/

                ViewBag.tipdoc = new SelectList(listaTipoDocumentoRepresentante(), "codTipoDocumento", "desTipoDocumento");
                ViewBag.tipper = new SelectList(listaTipoPersona(), "codTipoPersona", "desTipoPersona");
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero");
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito");
                return View(new Representante());
            }
        }

        [HttpPost]
        public ActionResult CreateRepresentante(Representante reg)
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("login","Personal");
            }

                Personal a = (Personal)Session["usuario"];
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
            ViewBag.imagen = a.imgPersonal;
            ViewBag.cargo = a.codCargo;
            /*-----------------------------*/

            List<SqlParameter> arreglo = new List<SqlParameter>() {

                new SqlParameter(){ ParameterName="@ndoc", SqlDbType= SqlDbType.VarChar, Value=reg.codRepresentante},
                new SqlParameter(){ ParameterName="@tipdoc", SqlDbType=SqlDbType.Int, Value=reg.codTipoDocumento},
                new SqlParameter(){ ParameterName="@tipper", SqlDbType=SqlDbType.Int, Value=reg.codTipoPersona},
                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomRepresentante},
                new SqlParameter(){ ParameterName="@ape",SqlDbType=SqlDbType.VarChar, Value=reg.apeRepresentante},
                new SqlParameter(){ ParameterName="@gen",SqlDbType= SqlDbType.Char, Value=reg.codGenero},
                new SqlParameter(){ ParameterName="@dir",SqlDbType= SqlDbType.VarChar, Value=reg.dirRepresentante},
                new SqlParameter(){ ParameterName="@dis",SqlDbType= SqlDbType.Int, Value=reg.codDistrito},
                new SqlParameter(){ ParameterName="@tel",SqlDbType= SqlDbType.VarChar, Value=reg.telRepresentante}
            };

            ViewBag.mensaje = CRUDRepresentante("sp_insertRepresentante", arreglo);

            ViewBag.tipdoc = new SelectList(listaTipoDocumentoRepresentante(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
            ViewBag.tipper = new SelectList(listaTipoPersona(), "codTipoPersona", "desTipoPersona", reg.codTipoPersona);
            ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
            ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
            return View(reg);

        }

        //ACTUALIZAR REPRESENTANTE
        public ActionResult EditRepresentante(String id = "")
        {

            if (Session["usuario"] == null)
            {

                return RedirectToAction("login","Personal");
            }

                Personal a = (Personal)Session["usuario"];
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
            ViewBag.imagen = a.imgPersonal;
            ViewBag.cargo = a.codCargo;
            /*-----------------------------*/


            Representante reg = ListaRepresentantes().Where(x => x.codRepresentante == id).FirstOrDefault();
            if (reg == null)
                return RedirectToAction("Representantes");
            else
            {
                ViewBag.tipdoc = new SelectList(listaTipoDocumentoRepresentante(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
                ViewBag.tipper = new SelectList(listaTipoPersona(), "codTipoPersona", "desTipoPersona", reg.codTipoPersona);
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);
                return View(reg);
            }
        }

        [HttpPost]
        public ActionResult EditRepresentante(Representante reg)
        {
            if (Session["usuario"] == null)
            {

                return RedirectToAction("login","Personal");
            }

                Personal a = (Personal)Session["usuario"];
                /*PARA ENVIAR EL NOMBRE DEL PERSONAL*/
                ViewBag.personal = a.nomPersonal;
            ViewBag.imagen = a.imgPersonal;
            ViewBag.cargo = a.codCargo;
            /*-----------------------------*/

            List<SqlParameter> arreglo = new List<SqlParameter>(){

                new SqlParameter(){ ParameterName="@ndoc", SqlDbType= SqlDbType.VarChar, Value=reg.codRepresentante},
                new SqlParameter(){ ParameterName="@tipdoc", SqlDbType=SqlDbType.Int, Value=reg.codTipoDocumento},
                new SqlParameter(){ ParameterName="@tipper", SqlDbType=SqlDbType.Int, Value=reg.codTipoPersona},
                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomRepresentante},
                new SqlParameter(){ ParameterName="@ape",SqlDbType=SqlDbType.VarChar, Value=reg.apeRepresentante},
                new SqlParameter(){ParameterName="@gen",SqlDbType= SqlDbType.Char, Value=reg.codGenero},
                new SqlParameter(){ParameterName="@dir",SqlDbType= SqlDbType.VarChar, Value=reg.dirRepresentante},
                new SqlParameter(){ParameterName="@dis",SqlDbType= SqlDbType.Int, Value=reg.codDistrito},
                new SqlParameter(){ParameterName="@tel",SqlDbType= SqlDbType.VarChar, Value=reg.telRepresentante}


            };

            ViewBag.mensaje = CRUDRepresentante("sp_updateRepresentante", arreglo);

            ViewBag.tipdoc = new SelectList(listaTipoDocumentoRepresentante(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
            ViewBag.tipper = new SelectList(listaTipoPersona(), "codTipoPersona", "desTipoPersona", reg.codTipoPersona);
            ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
            ViewBag.distritos = new SelectList(listaDistritos(), "codDistrito", "nomDistrito", reg.codDistrito);

            return View(reg);
        }

        /*ELIMINAR REPRESENTANTE*/
        public ActionResult DeleteRepresentante(string id = "")
        {

            List<SqlParameter> arreglo = new List<SqlParameter>() {
             new SqlParameter(){ ParameterName="@ndoc", SqlDbType= SqlDbType.VarChar,Value=id }
            };
            ViewBag.mensaje = CRUDRepresentante("sp_deleteRepresentante", arreglo);
            TempData["msj"] = "Representante " + id + " Eliminado";
            return RedirectToAction("Representantes");
        }
    }
}