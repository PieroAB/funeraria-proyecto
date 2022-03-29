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
    public class PlanesController : Controller
    {
        SqlConnection cn = new SqlConnection("server=.;database=DBFUNERARIA2020; integrated security=true");
        IEnumerable<ProductoServicio> listaItems(String para = "")
        {
            List<ProductoServicio> temporal = new List<ProductoServicio>();
            using (SqlCommand cmd = new SqlCommand("sp_listaItems", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nom", para);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProductoServicio reg = new ProductoServicio();
                    reg.codItem = dr.GetString(0);
                    reg.nomItem = dr.GetString(1);
                    reg.desItem = dr.GetString(2);
                    reg.codColor = dr.GetString(3);
                    reg.codMaterial = dr.GetString(4);
                    reg.stockItem = dr.GetInt32(5);
                    reg.precioItem = dr.GetDecimal(6);
                    reg.imgItem = dr.GetString(7);
                    reg.codCategoria = dr.GetInt32(8);
                    reg.codEstado = dr.GetInt32(9);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }

            return temporal;
        }


        IEnumerable<DetallePlan> listaDetallePlanes(String id = "")
        {
            List<DetallePlan> temporal = new List<DetallePlan>();
            using (SqlCommand cmd = new SqlCommand("sp_detalleplanes", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@plan", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DetallePlan reg = new DetallePlan();
                    reg.codPlan = dr.GetString(0);
                    reg.codItem = dr.GetString(1);
                    reg.nomItem = dr.GetString(2);
                    reg.cantPlan = dr.GetInt32(3);
                    reg.subtotalrecibido = dr.GetDecimal(4);
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


        IEnumerable<Planes> listaPlanes()
        {
            List<Planes> temporal = new List<Planes>();
            using (SqlCommand cmd = new SqlCommand("sp_listaPlan", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Planes reg = new Planes();
                    reg.codPlan = dr.GetString(0);
                    reg.nombreplan = dr.GetString(1);
                    reg.precioPlan = dr.GetDecimal(2);
                    reg.imgPlan = dr.GetString(3);
                    reg.estPlan = dr.GetInt32(4);
                    reg.cuentaplanes = dr.GetString(5);
                    reg.existplanes = dr.GetString(6);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        string CRUDPlan(string procedure, List<SqlParameter> arreglo)
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
                    if (procedure.Equals("sp_deletePlan"))
                    {
                        mensaje = "Plan Eliminado";
                    }
                    if (procedure.Equals("sp_deleteDetallePlan"))
                    {
                        mensaje = "Detalle Plan Eliminado";
                    }
                }
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.Close(); ViewBag.mensaje = mensaje; }
            return mensaje;
        }


        /*OBTENER AUTOGENERADO DE TB_PLANES*/
        string autogeneraPlan()
        {
            string nplan = "";
            using (SqlCommand cmd = new SqlCommand("select dbo.generaCodigoPlan()", cn))
            {
                cn.Open();
                nplan = (string)cmd.ExecuteScalar();
                cn.Close();
            }
            return nplan;
        }


        /*Ver Planes*/
        public ActionResult Planes()
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

                Session.Remove("editarPlan");
                Session.Remove("editarDetPlan");





                return View(listaPlanes());
            }
        }


        /*VISTA PRINCIPAL DE LOS PLANES*/
        public ActionResult ItemsPlan(string para = "")
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



                if (Session["nuevoPlan"] == null)
                {
                    Session["nuevoPlan"] = new List<DetallePlan>();
                }
                ViewBag.cantidad = 1;
                //envio a la vista de productos
                return View(listaItems(para));
            }
        }

   
        /*METODO QUE AÑADE AL ARREGLO UN NUEVO ITEM*/
        [HttpPost]
        public ActionResult AnadirItems(String id = "", int cantidad = 0)
        {
  
            ProductoServicio reg = listaItems().Where(x => x.codItem == id).FirstOrDefault();


            DetallePlan dp = new DetallePlan();
            dp.codItem = reg.codItem;
            dp.nomItem = reg.nomItem;
            dp.cantPlan = cantidad;
            dp.precio = reg.precioItem;


            List<DetallePlan> temporal = (List<DetallePlan>)Session["nuevoPlan"];

            DetallePlan existe = temporal.Where(x => x.codItem == id).FirstOrDefault();
            if (existe == null){
                temporal.Add(dp);
            } else{
                temporal.Remove(existe);
                existe.cantPlan = existe.cantPlan + cantidad;
                temporal.Add(existe);

                //Devuelve el arreglo ordenado
                List<DetallePlan> ordenado = temporal.OrderBy(o => o.codItem).ToList();

                //Mandarlo a Session
                Session["nuevoPlan"] = ordenado; 
            }

            TempData["msj"] = dp.codItem + " correctamente agregado!!";
            return RedirectToAction("ItemsPlan");
        }

        /*VISTA DONDE SE ENCUENTRAN TODOS LOS ITEMS AGREGADOS*/
        public ActionResult VerItemsAnadidosPlanes()
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



                if (Session["nuevoPlan"] == null)
                    return RedirectToAction("ItemsPlan");
                else
                {

                    return View((List<DetallePlan>)Session["nuevoPlan"]);
                }
            }
        }

        /*METODO PARA QUITAR DEL ARREGLO UN ITEM*/
        public ActionResult DeleteItemPlan(String id = null)
        {
            List<DetallePlan> temporal = (List<DetallePlan>)Session["nuevoPlan"];

            temporal.Remove(temporal.Where(x => x.codItem == id).FirstOrDefault());
         
            return RedirectToAction("VerItemsAnadidosPlanes");
        }

       /*METODO PARA CREAR LA CABECERA DEL NUEVO PLAN*/
        public ActionResult NuevoPlan()
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


                if (Session["nuevoPlan"] == null)
                    return RedirectToAction("ItemsPlan");
                else
                {
                    ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado");
                    return View(new Planes());
                }
            }
        }

        [HttpPost]
        public ActionResult NuevoPlan(Planes reg, HttpPostedFileBase archivo)
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
            int numero = 0;
            string nplan = autogeneraPlan();

            if (archivo == null)
            {
                ViewBag.imag = "Seleccione una imagen";
                ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado", reg.estPlan);
                return View(reg);
            }
            if (Path.GetExtension(archivo.FileName) != ".png"
              && Path.GetExtension(archivo.FileName) != ".jpg")
            { 
                ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado", reg.estPlan);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }

            cn.Open();
            SqlTransaction t = cn.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                
                SqlCommand cmd = new SqlCommand(

                "iNSERT into tb_planes Values(@cp,@n,@p,@img,@est)", cn, t);
                cmd.Parameters.AddWithValue("@cp", nplan);
                cmd.Parameters.AddWithValue("@n", reg.nombreplan);
                cmd.Parameters.AddWithValue("@p", reg.precioPlan);
                cmd.Parameters.AddWithValue("@img", "~/Imagenes/Planes/" + Path.GetFileName(archivo.FileName));
                cmd.Parameters.AddWithValue("@est", reg.estPlan);
                numero=cmd.ExecuteNonQuery();
                if (numero == 0) {
                    t.Rollback();
                    return View(reg);
                }
                

               
                foreach (DetallePlan x in (List<DetallePlan>)Session["nuevoPlan"])
                {
                    cmd = new SqlCommand("Insert into tb_det_planes Values (@cp,@ci,@c)", cn, t);
                    cmd.Parameters.AddWithValue("@cp", nplan);
                    cmd.Parameters.AddWithValue("@ci", x.codItem);
                    cmd.Parameters.AddWithValue("@c", x.cantPlan);
                    cmd.ExecuteNonQuery();
                }
                t.Commit();
                mensaje = string.Format("{0} Registrado con éxito", nplan);
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
            archivo.SaveAs(Path.Combine(Server.MapPath("~/Imagenes/Planes/") + Path.GetFileName(archivo.FileName)));
            return RedirectToAction("FinPlan", new { msg = mensaje });
        }

        public ActionResult FinPlan(string msg)
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

                Session.Remove("nuevoPlan");

                Session.Remove("editarPlan");
                Session.Remove("editarDetPlan");
                ViewBag.mensaje = msg;
                return View();
            }
        }
        
        public ActionResult DetallePlan(string id = null)
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


                if (id == null)
                    return RedirectToAction("Planes");
                else
                {
                    ViewBag.detalle = listaDetallePlanes(id);

                    return View(listaPlanes().Where(x => x.codPlan == id).FirstOrDefault());
                }
            }
        }


        public ActionResult DeletePlan(String id="") {
            String mensaje = "";
            using (SqlCommand cmd = new SqlCommand("sp_del_planes", cn)) {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cod", id);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Plan " + id + " eliminado exitosamente";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;

                }
                finally {
                    cn.Close();
                    TempData["msj"] = mensaje;
                    
                }
            }
            return RedirectToAction("Planes");
        }


        /*ACTUALIZAR UN PLAN O SU DETALLE*/

        public ActionResult AnaItemsEditPlan(string para = "")
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


                if (Session["EditarPlan"] == null)
                {
                    return RedirectToAction("Planes");
                }
                ViewBag.cantidad = 1;
                
                return View(listaItems(para));
            }
        }


        public ActionResult EditarPlan(String id = "") {
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

                Planes reg = listaPlanes().Where(x => x.codPlan == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("Planes");
                else
                {
                    if (Session["editarPlan"] == null)
                    {
                        Session["editarPlan"] = new Planes();
                        Session["editarPlan"] = reg;
                    }
                    if (Session["editarDetPlan"] == null)
                    {
                        Session["editarDetPlan"] = new List<DetallePlan>();
                        Session["editarDetPlan"] = listaDetallePlanes(reg.codPlan);
                    }

                    ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado", reg.estPlan);
                    return View(reg);
                }
            }
        }
        [HttpPost]
        public ActionResult EditarPlan(Planes reg, HttpPostedFileBase archivo) {
            string mensaje = "",ruta;

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
                ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado", reg.estPlan);
                ruta = reg.imgPlan;
            }
            else
            if (Path.GetExtension(archivo.FileName) != ".png"
              && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado", reg.estPlan);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            else
            {
                ruta = "~/Imagenes/Planes/" + Path.GetFileName(archivo.FileName);
            }

            try
            {
                SqlCommand cmd = new SqlCommand("sp_actualizaPlan", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", reg.codPlan);
                cmd.Parameters.AddWithValue("@nom", reg.nombreplan);
                cmd.Parameters.AddWithValue("@pre", reg.precioPlan);
                cmd.Parameters.AddWithValue("@img", ruta);
                cmd.Parameters.AddWithValue("@est", reg.estPlan);
                cn.Open();
                cmd.ExecuteNonQuery();

                mensaje = string.Format("{0} Actualizado con éxito", reg.codPlan);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            if (archivo != null)
                archivo.SaveAs(Server.MapPath(ruta));
            ViewBag.estados = new SelectList(listaEstados(), "codEstado", "desEstado", reg.estPlan);
            ViewBag.mensaje = mensaje;
            return View(reg);
        }

        public ActionResult RemoverEditItem(string id = "")
        {
            List<DetallePlan> temporal = (List<DetallePlan>)Session["editarDetPlan"];
            Planes reg = (Planes)Session["editarPlan"];
            if (reg.codPlan == null) {
                return RedirectToAction("Planes");
            
            }
            temporal.Remove(temporal.Where(x => x.codItem == id).FirstOrDefault());
            return RedirectToAction("itemsEditPlanes");
        }


        [HttpPost]
        public ActionResult AnadirEditItems(String id = "", int cantidad = 0)
        {

            ProductoServicio reg = listaItems().Where(x => x.codItem == id).FirstOrDefault();

            DetallePlan dp = new DetallePlan();
            dp.codItem = reg.codItem;
            dp.nomItem = reg.nomItem;
            dp.cantPlan = cantidad;
            dp.subtotalrecibido = cantidad * reg.precioItem;


            List<DetallePlan> temporal = (List<DetallePlan>)Session["editarDetPlan"];

            DetallePlan existe = temporal.Where(x => x.codItem == id).FirstOrDefault();
            if (existe == null)
            {
                temporal.Add(dp);
            }
            else
            {
                temporal.Remove(existe);
                existe.cantPlan = existe.cantPlan + cantidad;
                temporal.Add(existe);

                //Devuelve el arreglo ordenado
                List<DetallePlan> ordenado = temporal.OrderBy(o => o.codItem).ToList();

                //Mandarlo a Session
                Session["editarDetPlan"] = ordenado;
            }

            TempData["msj"] = dp.codItem + " correctamente agregado!!";
            return RedirectToAction("AnaItemsEditPlan");
        }


        public ActionResult itemsEditPlanes()
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


                if (Session["editarPlan"] == null)
                    return RedirectToAction("Planes");
                else
                {

                    return View((List<DetallePlan>)Session["editarDetPlan"]);
                }
            }
        }
        [HttpPost]
        public ActionResult ActualizaDetPlan() {
            string mensaje = "";

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

            Planes reg = (Planes)Session["editarPlan"];
            string plan = reg.codPlan;

            cn.Open();
            SqlTransaction t = cn.BeginTransaction(IsolationLevel.Serializable);
            try
            {

                SqlCommand cmd = new SqlCommand("sp_deleteDetallePlan", cn, t);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", plan);
                
                cmd.ExecuteNonQuery();

                foreach (DetallePlan x in (List<DetallePlan>)Session["editarDetPlan"])
                {
                    cmd = new SqlCommand("sp_insDetallePlan", cn, t);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@plan", plan);
                    cmd.Parameters.AddWithValue("@item", x.codItem);
                    cmd.Parameters.AddWithValue("@cant", x.cantPlan);
                    cmd.ExecuteNonQuery();
                }
                t.Commit();
                mensaje = string.Format("Detalle del plan {0} Actualizado con éxito", plan);
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
            return RedirectToAction("FinPlan", new { msg = mensaje });
        }



    }
}