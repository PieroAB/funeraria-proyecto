using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Proyecto_Funeraria2020.Models;
using System.IO;
using WebGrease;
using System.Dynamic;
using System.Web.Mvc.Ajax;

namespace Proyecto_Funeraria2020.Controllers
{    
    public class VentaController : Controller
    {

        SqlConnection cn = new SqlConnection("server=.;database=DBFUNERARIA2020; integrated security=true");

        IEnumerable<Representante> comboRepresentantes() {
            List<Representante> temporal = new List<Representante>();
            using (SqlCommand cmd = new SqlCommand("sp_comboRepresentantes", cn)) {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) {
                    Representante reg = new Representante();
                    reg.codRepresentante = dr.GetString(0);
                    reg.combonombres = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<EstadoBoleta> listaEstadosBoleta()
        {
            List<EstadoBoleta> temporal = new List<EstadoBoleta>();
            using (SqlCommand cmd = new SqlCommand("sp_listaEstadoBoleta", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EstadoBoleta reg = new EstadoBoleta();
                    reg.codEstado = dr.GetInt32(0);
                    reg.desEstado = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }
        IEnumerable<DetallePlan> listaDetPlanBoleta(string id = "") {
            List<DetallePlan> temporal = new List<DetallePlan>();
            using (SqlCommand cmd= new SqlCommand("sp_lista_det_plan_boleta", cn)) {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read()){
                    DetallePlan reg = new DetallePlan();
                    reg.codItem = dr.GetString(0);
                    reg.nomItem = dr.GetString(1);
                    reg.desItem = dr.GetString(2);
                    reg.cantidad = dr.GetInt32(3);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        
       
        /*OBTENER AUTOGENERADO DE TB_TRANSACCCIONES*/
        string autogeneraBoleta()
        {
            string nbol = "";
            using (SqlCommand cmd = new SqlCommand("select dbo.generaBoleta()", cn))
            {
                cn.Open();
                nbol = (string)cmd.ExecuteScalar();
                cn.Close();
            }
            return nbol;
        }

        IEnumerable<Planes> Catalogo() {

            List<Planes> temporal = new List<Planes>();
            using (SqlCommand cmd = new SqlCommand("sp_listaPlan", cn)) {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) {
                    Planes reg = new Planes();
                    reg.codPlan = dr.GetString(0);
                    reg.nombreplan = dr.GetString(1);
                    reg.precioPlan = dr.GetDecimal(2);
                    reg.imgPlan = dr.GetString(3);
                    reg.estPlan = dr.GetInt32(4);
                    reg.cuentaplanes = dr.GetString(5);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<Transaccion> listaBoletas(){
            List<Transaccion> temporal = new List<Transaccion>();
                using (SqlCommand cmd = new SqlCommand("sp_listaBoletas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Transaccion reg = new Transaccion();
                        reg.codBoleta = dr.GetString(0);
                        reg.fechaEmisionBoleta = dr.GetDateTime(1);
                        reg.codRepresentante = dr.GetString(2);
                        reg.dirSepelio = dr.GetString(3);
                        reg.nomCementerio = dr.GetString(4);
                        reg.fechaSepelio = dr.GetDateTime(5);
                        reg.precioSinIGV = dr.GetDecimal(6);
                        reg.Total = dr.GetDecimal(7);
                        reg.estado = dr.GetInt32(8);
                        reg.nombreestado = dr.GetString(9);
                        temporal.Add(reg);
                    }
                    dr.Close(); cn.Close();
                }
                return temporal;
            }


        public ActionResult Boletas(DateTime? f1 = null, DateTime? f2 = null, int v=0, string boton="")
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


                List<Transaccion> temporal = new List<Transaccion>();
                if (f1 == null || f2 == null)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_listaBoletas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Transaccion reg = new Transaccion();
                            reg.codBoleta = dr.GetString(0);
                            reg.fechaEmisionBoleta = dr.GetDateTime(1);
                            reg.codRepresentante = dr.GetString(2);
                            reg.dirSepelio = dr.GetString(3);
                            reg.nomCementerio = dr.GetString(4);
                            reg.fechaSepelio = dr.GetDateTime(5);
                            reg.precioSinIGV = dr.GetDecimal(6);
                            reg.Total = dr.GetDecimal(7);
                            reg.estado = dr.GetInt32(8);
                            reg.nombreestado = dr.GetString(9);
                            temporal.Add(reg);
                        }
                        dr.Close(); cn.Close();
                    }
                    
                    int filas =5;
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
                    ViewBag.f1 = f1;
                    ViewBag.f2 = f2;
                    ViewBag.v = v;
                    ViewBag.npags = npags;

                    return View(temporal.Skip(v * filas).Take(filas));

                }
                else
                if ((DateTime)f1 > (DateTime)f2)
                {
                    ViewBag.mensaje = "la fecha 1 es mayor a la otra fecha";
                    using (SqlCommand cmd = new SqlCommand("sp_listaBoletas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Transaccion reg = new Transaccion();
                            reg.codBoleta = dr.GetString(0);
                            reg.fechaEmisionBoleta = dr.GetDateTime(1);
                            reg.codRepresentante = dr.GetString(2);
                            reg.dirSepelio = dr.GetString(3);
                            reg.nomCementerio = dr.GetString(4);
                            reg.fechaSepelio = dr.GetDateTime(5);
                            reg.precioSinIGV = dr.GetDecimal(6);
                            reg.Total = dr.GetDecimal(7);
                            reg.estado = dr.GetInt32(8);
                            reg.nombreestado = dr.GetString(9);
                            temporal.Add(reg);
                        }
                        dr.Close(); cn.Close();
                    }
                    int filas = 5;
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
                    ViewBag.f1 = f1;
                    ViewBag.f2 = f2;
                    ViewBag.v = v;
                    ViewBag.npags = npags;

                    return View(temporal.Skip(v * filas).Take(filas));

                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("sp_listafechaBoletas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@f1", (DateTime)f1);
                        cmd.Parameters.AddWithValue("@f2", (DateTime)f2);
                        cn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Transaccion reg = new Transaccion();
                            reg.codBoleta = dr.GetString(0);
                            reg.fechaEmisionBoleta = dr.GetDateTime(1);
                            reg.codRepresentante = dr.GetString(2);
                            reg.dirSepelio = dr.GetString(3);
                            reg.nomCementerio = dr.GetString(4);
                            reg.fechaSepelio = dr.GetDateTime(5);
                            reg.precioSinIGV = dr.GetDecimal(6);
                            reg.Total = dr.GetDecimal(7);
                            reg.estado = dr.GetInt32(8);
                            reg.nombreestado = dr.GetString(9);
                            temporal.Add(reg);
                        }
                        dr.Close(); cn.Close();
                    }
                    int filas = 5;
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
                    ViewBag.f1 = f1;
                    ViewBag.f2 = f2;
                    ViewBag.v = v;
                    ViewBag.npags = npags;

                    return View(temporal.Skip(v * filas).Take(filas));
                }
            }
        }

        /*FALTA DETALLAR*/
       public ActionResult DetalleBoleta(String id = "") {

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

                if (id == "")
                {

                    return RedirectToAction("Boletas");
                }
                else
                {

                    ViewBag.boldet = listaDetPlanBoleta(id);
                    Transaccion reg = listaBoletas().Where(x => x.codBoleta == id).FirstOrDefault();
                    return View(reg);
                }
            }
        }

        /*NUEVA BOLETA*/
        public ActionResult Tienda()
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


                if (Session["carrito"] == null)
                {
                    Session["carrito"] = new List<DetalleTransaccion>();
                }
                ViewBag.cantidad = 1;
                return View(Catalogo());
            }
        }


        public ActionResult Agregar(String id, int cantidad = 0)
        {
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<DetalleTransaccion>();
                return RedirectToAction("Tienda");
            }

            string mensaje = "";
            Planes reg = Catalogo().Where(x => x.codPlan == id).FirstOrDefault();

            //definir el Item y agregar valores al objeto(reg)
            DetalleTransaccion dt = new DetalleTransaccion();
            dt.codItemPlan = reg.codPlan;
            dt.nombrePlan = reg.nombreplan;
            dt.cantidad = cantidad;
            dt.importe = reg.precioPlan;
            List<DetalleTransaccion> temporal = (List<DetalleTransaccion>)Session["carrito"];


            DetalleTransaccion existe = temporal.Where(x => x.codItemPlan == id).FirstOrDefault();
            if (existe == null)
            {
                temporal.Add(dt);
            }
            else
            {
                temporal.Remove(existe);
                existe.cantidad = existe.cantidad + cantidad;
                temporal.Add(existe);

                //Devuelve el arreglo ordenado
                List<DetalleTransaccion> ordenado = temporal.OrderBy(o => o.codItemPlan).ToList();

                //Mandarlo a Session
                Session["nuevoPlan"] = ordenado;
            }

            mensaje = "Plan "+id +" agregado!!";
            TempData["msj"] = mensaje;
            return RedirectToAction("Tienda");
        }


        public ActionResult Canasta()
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

                //si el session esta vacio
                if (Session["carrito"] == null)
                    return RedirectToAction("Tienda");
                else
                {
                    //visualizar lo almacenado en el Session
                    return View((List<DetalleTransaccion>)Session["carrito"]);
                }
            }
        }

        public ActionResult DeleteDetBol(String id = null)
        {
            List<DetalleTransaccion> temporal = (List<DetalleTransaccion>)Session["carrito"];

            temporal.Remove(temporal.Where(x => x.codItemPlan == id).FirstOrDefault());
            
            return RedirectToAction("Canasta");
        }


        public ActionResult RealizarBoleta()
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


                if (Session["carrito"] == null)
                    return RedirectToAction("Tienda");
                else
                {
                    ViewBag.repre = new SelectList(comboRepresentantes(), "codRepresentante", "combonombres");
                    return View(new Transaccion());
                }
            }
        }

        [HttpPost]
        public ActionResult RealizarBoleta(Transaccion reg)
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


            string mensaje = "";
            decimal subtotal = 0;
            decimal total;
            string nbol = autogeneraBoleta();
            int numero = 0;

            //abrir la conexion y definir un  transaccion
            cn.Open();
            SqlTransaction t = cn.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                foreach (DetalleTransaccion x in (List<DetalleTransaccion>)Session["carrito"])
                {
                    subtotal += x.subtotal;
                }

                total = subtotal+(subtotal * 18/100);

                    //insertar registros a tb_pedidos
                    SqlCommand cmd = new SqlCommand("sp_createTransaccion", cn, t);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nb", nbol);
                cmd.Parameters.AddWithValue("@rep", reg.codRepresentante);
                cmd.Parameters.AddWithValue("@dir", reg.dirSepelio);
                cmd.Parameters.AddWithValue("@cem", reg.nomCementerio);
                cmd.Parameters.AddWithValue("@fs", reg.fechaSepelio);
                cmd.Parameters.AddWithValue("@sub", subtotal);
                cmd.Parameters.AddWithValue("@total", total);
                numero=cmd.ExecuteNonQuery(); //EJECUTAR EL INSERT INTO


                //insertar registros a tb_pedidosdeta
                foreach (DetalleTransaccion x in (List<DetalleTransaccion>)Session["carrito"])
                {
                    cmd = new SqlCommand("sp_insertDetalleTransaccion", cn, t);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bol", nbol);
                    cmd.Parameters.AddWithValue("@plan", x.codItemPlan);
                    cmd.Parameters.AddWithValue("@can", x.cantidad);
                    cmd.Parameters.AddWithValue("@imp", x.importe);
                    cmd.Parameters.AddWithValue("@sub", x.subtotal);
                    cmd.ExecuteNonQuery();
                }
                t.Commit();
                mensaje = string.Format("Solicitud Registrada {0}", nbol);
            }
            catch (Exception ex)
            {
                t.Rollback();
                mensaje = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            if (numero == 0)
            {
                ViewBag.mensaje = mensaje;
                ViewBag.repre = new SelectList(comboRepresentantes(), "codRepresentante", "combonombres", reg.codRepresentante);
                return View(reg);
            }
            else
            {
                return RedirectToAction("FinTransaccion", new { msg = mensaje });
            }
        }


        public ActionResult FinTransaccion(string msg)
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

            //AL FINALIZAR CERRAMOS EL SESSSION CON EL METODO ABANDOM
            Session.Remove("carrito");

            //ESTE ACTION IMPRIME LOS RESULTADOS DEL MENSAJE
            ViewBag.mensaje = msg;
            return View();
        }

        public ActionResult ActualizarBoleta(String id = null)
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


                Transaccion reg = listaBoletas().Where(x => x.codBoleta == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("Boletas");
                else
                {
                    ViewBag.estados = new SelectList(listaEstadosBoleta(), "codEstado", "desEstado", reg.estado);

                    return View(reg);
                }
            }
        }


        [HttpPost]
        public ActionResult ActualizarBoleta(Transaccion reg) {
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


            String mensaje = "";
            using (SqlCommand cmd = new SqlCommand("sp_editaBoleta", cn)) {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", reg.codBoleta);
                    cmd.Parameters.AddWithValue("@dir", reg.dirSepelio);
                    cmd.Parameters.AddWithValue("@cem", reg.nomCementerio);
                    cmd.Parameters.AddWithValue("@fsep", reg.fechaSepelio);
                    cmd.Parameters.AddWithValue("@est", reg.estado);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Boleta " + reg.codBoleta + " Actualizada";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;

                }
                finally {
                    cn.Close();
                    ViewBag.estados = new SelectList(listaEstadosBoleta(), "codEstado", "desEstado", reg.estado);
                    ViewBag.mensaje = mensaje;
                }
            }
            return View(reg);
        }




        public ActionResult AnularBoleta(string id= null) {
            String mensaje;
            if (id == null)
            {

                return RedirectToAction("Boletas");
            }
            else {
                using (SqlCommand cmd = new SqlCommand("sp_anulaBoleta", cn)) {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod", id);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        mensaje = "Boleta " + id + " Anulada";
                        TempData["msj"] = mensaje;
                    }
                    catch (Exception ex)
                    {
                        mensaje = ex.Message;
                    }
                    finally {
                        cn.Close();
                    }
                    
                }
                return RedirectToAction("Boletas");
            }
        }


    }
}