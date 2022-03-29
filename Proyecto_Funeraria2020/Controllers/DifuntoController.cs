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
    public class DifuntoController : Controller
    {

        SqlConnection cn = new SqlConnection("server=.;database=DBFUNERARIA2020; integrated security=true");
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


        IEnumerable<Parentesco> listaParentescos()
        {
            List<Parentesco> temporal = new List<Parentesco>();
            using (SqlCommand cmd = new SqlCommand("sp_listaParentesco", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Parentesco reg = new Parentesco();
                    reg.codParentesco = dr.GetInt32(0);
                    reg.desParentesco = dr.GetString(1);
                    temporal.Add(reg);

                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        IEnumerable<EstadoCivil> listaEstadoCivil()
        {
            List<EstadoCivil> temporal = new List<EstadoCivil>();
            using (SqlCommand cmd = new SqlCommand("sp_listaEstadoCivil", cn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EstadoCivil reg = new EstadoCivil();
                    reg.codEstadoCivil = dr.GetString(0);
                    reg.desEstadoCivil = dr.GetString(1);
                    temporal.Add(reg);

                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<TipoDocumento> listaTipoDocumentoDifunto()
        {
            List<TipoDocumento> temporal = new List<TipoDocumento>();
            using (SqlCommand cmd = new SqlCommand("sp_listaTipoDocD", cn))
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

        IEnumerable<Representante> ComboRepresentantes()
        {
            List<Representante> temporal = new List<Representante>();
            using (SqlCommand cmd = new SqlCommand("sp_listaRepresentante_Difunto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    Representante reg = new Representante();
                    reg.codRepresentante = dr.GetString(0);
                    reg.nomRepresentante = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<Difunto> listaDifunto(String difu = "")
        {

            List<Difunto> temporal = new List<Difunto>();
            using (SqlCommand cmd = new SqlCommand("sp_listaDifunto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@lik", difu);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Difunto reg = new Difunto();
                    reg.codDifunto = dr.GetString(0);
                    reg.codTipoDocumento = dr.GetInt32(1);
                    reg.nomDifunto = dr.GetString(2);
                    reg.apeDifunto = dr.GetString(3);
                    reg.codGenero = dr.GetString(4);
                    reg.codEstadoCivil = dr.GetString(5);
                    reg.fecNacDifunto = dr.GetDateTime(6);
                    reg.fecFallDifunto = dr.GetDateTime(7);
                    reg.lugFallDifunto = dr.GetString(8);
                    reg.imgActaDifunto = dr.GetString(9);
                    reg.asignaDifunto = dr.GetString(10);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();

            }
            return temporal;
        }



        /*LISTAR - CREAR - ACTUALIZAR - ELIMINAR - RELACIONAR : DIFUNTO */

        string CRUDDifunto(string procedure, List<SqlParameter> arreglo)
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
                    if (procedure.Equals("sp_insertDifunto"))
                    {
                        mensaje = "Difunto Registrado";
                    }
                    if (procedure.Equals("sp_updateDifunto"))
                    {
                        mensaje = "Difunto Actualizado";
                    }
                    if (procedure.Equals("sp_deleteDifunto"))
                    {
                        TempData["difu"] = "Difunto Eliminado Exitosamente";
                    }
                    if (procedure.Equals("sp_asignarRepresentante"))
                    {

                        mensaje = "Representante asignado Correctamente";
                    }
                }
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.Close(); ViewBag.mensaje = mensaje; }
            return mensaje;
        }



        public ActionResult Difuntos(int v=0, string para = "", string boton="")
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
                IEnumerable<Difunto> temporal = listaDifunto(para);
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

        /*INSERTAR DIFUNTO*/

        public ActionResult CreateDifunto()
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

                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero");
                ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento");
                ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil");
                return View(new Difunto());
            }
        }

        [HttpPost]
        public ActionResult CreateDifunto(Difunto reg, HttpPostedFileBase archivo)
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


            if (archivo == null)
            {
                ViewBag.mensaje = "No ah seleccionado el archivo";
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
                ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);
                return View(reg);
            }
            if (Path.GetExtension(archivo.FileName) != ".png"
              && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
                ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            List<SqlParameter> arreglo = new List<SqlParameter>() {

                new SqlParameter(){ ParameterName="@doc", SqlDbType= SqlDbType.VarChar, Value=reg.codDifunto},
                new SqlParameter(){ ParameterName="@tipdoc", SqlDbType=SqlDbType.Int, Value=reg.codTipoDocumento },
                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomDifunto},
                new SqlParameter(){ ParameterName="@ape", SqlDbType=SqlDbType.VarChar, Value=reg.apeDifunto},
                new SqlParameter(){ ParameterName="@gen",SqlDbType=SqlDbType.Char, Value=reg.codGenero},
                new SqlParameter(){ ParameterName="@civ",SqlDbType=SqlDbType.VarChar, Value=reg.codEstadoCivil},
                new SqlParameter(){ ParameterName="@nac",SqlDbType=SqlDbType.Date, Value=reg.fecNacDifunto},
                new SqlParameter(){ ParameterName="@fal",SqlDbType=SqlDbType.Date, Value=reg.fecFallDifunto},
                new SqlParameter(){ ParameterName="@lug",SqlDbType=SqlDbType.VarChar, Value=reg.lugFallDifunto},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar,
                    Value="~/Imagenes/Actas/"+Path.GetFileName(archivo.FileName)}
            };
            ViewBag.mensaje = CRUDDifunto("sp_insertDifunto", arreglo);

            archivo.SaveAs(Path.Combine(Server.MapPath("~/Imagenes/Actas/") + Path.GetFileName(archivo.FileName)));

            ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
            ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
            ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);
            return View(reg);
        }


        /*ACTUALIZAR DIFUNTO*/
        public ActionResult EditDifunto(String id = null)
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


                Difunto reg = listaDifunto().Where(x => x.codDifunto == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("Difuntos");
                else
                {
                    ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                    ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
                    ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);
                    return View(reg);
                }
            }
        }

        [HttpPost]
        public ActionResult EditDifunto(Difunto reg, HttpPostedFileBase archivo)
        {
            string ruta;


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

            if (archivo == null)
            {
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
                ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);

                ruta = reg.imgActaDifunto;
            }
            else
            if (Path.GetExtension(archivo.FileName) != ".png"
               && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
                ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
                ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);

                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            else

                ruta = "~/Imagenes/Actas/" + Path.GetFileName(archivo.FileName);

            List<SqlParameter> arreglo = new List<SqlParameter>(){
               new SqlParameter(){ ParameterName="@doc", SqlDbType= SqlDbType.VarChar, Value=reg.codDifunto},
                new SqlParameter(){ ParameterName="@tipdoc", SqlDbType=SqlDbType.Int, Value=reg.codTipoDocumento},
                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomDifunto},
                new SqlParameter(){ ParameterName="@ape", SqlDbType=SqlDbType.VarChar, Value=reg.apeDifunto},
                new SqlParameter(){ ParameterName="@gen",SqlDbType=SqlDbType.Char, Value=reg.codGenero},
                new SqlParameter(){ ParameterName="@civ",SqlDbType=SqlDbType.VarChar, Value=reg.codEstadoCivil},
                new SqlParameter(){ ParameterName="@nac",SqlDbType=SqlDbType.Date, Value=reg.fecNacDifunto},
                new SqlParameter(){ ParameterName="@fal",SqlDbType=SqlDbType.Date, Value=reg.fecFallDifunto},
                new SqlParameter(){ ParameterName="@lug",SqlDbType=SqlDbType.VarChar, Value=reg.lugFallDifunto},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar, Value=ruta}
                };
            ViewBag.mensaje = CRUDDifunto("sp_updateDifunto", arreglo);

            if (archivo != null)
                archivo.SaveAs(Server.MapPath(ruta));
            ViewBag.generos = new SelectList(listaGeneros(), "codGenero", "desgenero", reg.codGenero);
            ViewBag.tipdoc = new SelectList(listaTipoDocumentoDifunto(), "codTipoDocumento", "desTipoDocumento", reg.codTipoDocumento);
            ViewBag.estciviles = new SelectList(listaEstadoCivil(), "codEstadoCivil", "desEstadoCivil", reg.codEstadoCivil);


            return View(reg);
        }

        /*ELIMINAR DIFUNTO*/
        public ActionResult DeleteDifunto(string id = "")
        {

            List<SqlParameter> arreglo = new List<SqlParameter>() {
             new SqlParameter(){ ParameterName="@doc", SqlDbType= SqlDbType.VarChar,Value=id }
            };
            ViewBag.mensaje = CRUDDifunto("sp_deleteDifunto", arreglo);
            TempData["msj"] = "Difunto " + id + " Eliminado";
            return RedirectToAction("Difuntos");
        }


        /*ASIGNAR PARENTESCO ENTRE DIFUNTO Y REPRESENTANTE*/
        public ActionResult AsignarRelacion(String id = "")
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

                Difunto reg = listaDifunto().Where(x => x.codDifunto == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("Difuntos");
                else
                {
                    RepresentanteDifunto reg2 = new RepresentanteDifunto();
                    reg2.codDifunto = reg.codDifunto;
                    reg2.nombreDifunto = reg.nomDifunto;
                    reg2.apellidoDifunto = reg.apeDifunto;
                    ViewBag.representantes = new SelectList(ComboRepresentantes(), "codRepresentante", "nomRepresentante");
                    ViewBag.parentescos = new SelectList(listaParentescos(), "codParentesco", "desParentesco");
                    return View(reg2);
                }
            }
        }


        [HttpPost]
        public ActionResult AsignarRelacion(RepresentanteDifunto reg2)
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


            ViewBag.representantes = new SelectList(ComboRepresentantes(), "codRepresentante", "nomRepresentante", reg2.codRepresentante);
            ViewBag.parentescos = new SelectList(listaParentescos(), "codParentesco", "desParentesco", reg2.codParentesco);

            List<SqlParameter> arreglo = new List<SqlParameter>(){
                new SqlParameter(){ ParameterName="@rep", SqlDbType= SqlDbType.VarChar, Value=reg2.codRepresentante},
                 new SqlParameter(){ ParameterName="@dif", SqlDbType= SqlDbType.VarChar, Value=reg2.codDifunto},
                  new SqlParameter(){ ParameterName="@par", SqlDbType= SqlDbType.Int, Value=reg2.codParentesco}
                };
            ViewBag.mensaje = CRUDDifunto("sp_asignarRepresentante", arreglo);

            ViewBag.representantes = new SelectList(ComboRepresentantes(), "codRepresentante", "nomRepresentante", reg2.codRepresentante);
            ViewBag.parentescos = new SelectList(listaParentescos(), "codParentesco", "desParentesco", reg2.codParentesco);

            return View(reg2);

        }
    }
}