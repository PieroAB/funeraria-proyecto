using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Proyecto_Funeraria2020.Models;
using System.IO;
using Proyecto_Funeraria2020.Controllers;

namespace Proyecto_Funeraria2020.Controllers
{
    public class ItemController : Controller
    {
      

        SqlConnection cn = new SqlConnection("server=.;database=DBFUNERARIA2020; integrated security=true");
        IEnumerable<Material> listaMateriales()
        {
            List<Material> temporal = new List<Material>();
            using (SqlCommand cmd = new SqlCommand("sp_listaMaterial", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Material reg = new Material();
                    reg.codMaterial = dr.GetString(0);
                    reg.nomMaterial = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        IEnumerable<Color> listaColores()
        {
            List<Color> temporal = new List<Color>();
            using (SqlCommand cmd = new SqlCommand("sp_listaColor", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Color reg = new Color();
                    reg.codColor = dr.GetString(0);
                    reg.nomColor = dr.GetString(1);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        public IEnumerable<Categoria> listaCategorias()
        {
            List<Categoria> temporal = new List<Categoria>();
            using (SqlCommand cmd = new SqlCommand("sp_listaCategoriaItem", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Categoria reg = new Categoria();

                    reg.codCategoria = dr.GetInt32(0);
                    reg.nomCategoria = dr.GetString(1);
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


        IEnumerable<ProductoServicio> listaProductos()
        {
            List<ProductoServicio> temporal = new List<ProductoServicio>();
            using (SqlCommand cmd = new SqlCommand("sp_listaProducto", cn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
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
                    reg.existEnPlan = dr.GetString(10);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }

        IEnumerable<ProductoServicio> listaServicios()
        {
            List<ProductoServicio> temporal = new List<ProductoServicio>();
            using (SqlCommand cmd = new SqlCommand("sp_listaServicio", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
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
                    reg.existEnPlan = dr.GetString(10);
                    temporal.Add(reg);
                }
                dr.Close(); cn.Close();
            }
            return temporal;
        }


        string CRUDProductoServicio(string procedure, List<SqlParameter> arreglo)
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
                    if (procedure.Equals("sp_insertProducto"))
                    {
                        mensaje = "Producto Registrado";
                    }
                    if (procedure.Equals("sp_updateProducto"))
                    {
                        mensaje = "Producto Actualizado";
                    }
                    if (procedure.Equals("sp_deleteProducto"))
                    {
                        TempData["prod"] = "Producto Eliminado Exitosamente";
                    }
                    if (procedure.Equals("sp_insertServicio"))
                    {
                        mensaje = "Servicio Registrado";
                    }
                    if (procedure.Equals("sp_updateServicio"))
                    {
                        mensaje = "Servicio Actualizado";
                    }
                    if (procedure.Equals("sp_deleteServicio"))
                    {
                        TempData["prod"] = "Servicio Eliminado Correctamente";
                    }
                }
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.Close(); ViewBag.mensaje = mensaje; }
            return mensaje;
        }

        /*Buscar Codigo ITEM*/

        public ProductoServicio BuscarCodItemP(string id)
        {
            return listaProductos().Where(x => x.codItem == id).FirstOrDefault();
        }


        /*PRODUCTOS*/
        public ActionResult CreateProducto()
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

                ViewBag.productos = listaProductos();
                ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor");
                ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial");
                return View(new ProductoServicio());
            }
        }

        [HttpPost]
        public ActionResult CreateProducto(ProductoServicio reg, HttpPostedFileBase archivo)
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
                ViewBag.productos = listaProductos();
                ViewBag.mensaje = "Seleccione una imagen";
                ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
                ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);
                return View(reg);
            }
            if (Path.GetExtension(archivo.FileName) != ".png"
              && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.productos = listaProductos();
                ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
                ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            List<SqlParameter> arreglo = new List<SqlParameter>() {

                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomItem},
                new SqlParameter(){ ParameterName="@des", SqlDbType=SqlDbType.VarChar, Value=reg.desItem},
                new SqlParameter(){ ParameterName="@col",SqlDbType=SqlDbType.Char, Value=reg.codColor},
                new SqlParameter(){ ParameterName="@mat",SqlDbType=SqlDbType.Char, Value=reg.codMaterial},
                new SqlParameter(){ ParameterName="@sto",SqlDbType=SqlDbType.Int, Value=reg.stockItem},
                new SqlParameter(){ ParameterName="@pre",SqlDbType=SqlDbType.Decimal, Value=reg.precioItem},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar,
                    Value="~/Imagenes/Productos/"+Path.GetFileName(archivo.FileName)}
            };
            ViewBag.mensaje = CRUDProductoServicio("sp_insertProducto", arreglo);

            archivo.SaveAs(Path.Combine(Server.MapPath("~/Imagenes/Productos/") + Path.GetFileName(archivo.FileName)));

            ViewBag.productos = listaProductos();
            ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
            ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);
            return View(reg);
        }

        public ActionResult EditProducto(String id = null)
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


                ProductoServicio reg = listaProductos().Where(x => x.codItem == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("CreateProducto");
                else
                {
                    ViewBag.productos = listaProductos();

                    ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                    ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
                    ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);
                    return View(reg);
                }
            }
        }

        [HttpPost]
        public ActionResult EditProducto(ProductoServicio reg, HttpPostedFileBase archivo)
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
                ViewBag.productos = listaProductos();
                ViewBag.mensaje = "Seleccione una imagen";
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
                ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);
                ruta = reg.imgItem;
            }
            else
            if (Path.GetExtension(archivo.FileName) != ".png"
               && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.productos = listaProductos();
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
                ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);

                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            else

                ruta = "~/Imagenes/Productos/" + Path.GetFileName(archivo.FileName);

            List<SqlParameter> arreglo = new List<SqlParameter>(){
               new SqlParameter(){ ParameterName="@cod", SqlDbType= SqlDbType.Char, Value=reg.codItem},
               new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomItem},
                new SqlParameter(){ ParameterName="@des", SqlDbType=SqlDbType.VarChar, Value=reg.desItem},
                new SqlParameter(){ ParameterName="@col",SqlDbType=SqlDbType.Char, Value=reg.codColor},
                new SqlParameter(){ ParameterName="@mat",SqlDbType=SqlDbType.Char, Value=reg.codMaterial},
                new SqlParameter(){ ParameterName="@sto",SqlDbType=SqlDbType.Int, Value=reg.stockItem},
                new SqlParameter(){ ParameterName="@pre",SqlDbType=SqlDbType.Decimal, Value=reg.precioItem},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar, Value=ruta},
                new SqlParameter(){ParameterName="@est",SqlDbType= SqlDbType.Int, Value=reg.codEstado}
                };
            ViewBag.mensaje = CRUDProductoServicio("sp_updateProducto", arreglo);

            if (archivo != null)
                archivo.SaveAs(Server.MapPath(ruta));

            ViewBag.productos = listaProductos();
            ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
            ViewBag.colores = new SelectList(listaColores(), "codColor", "nomColor", reg.codColor);
            ViewBag.materiales = new SelectList(listaMateriales(), "codMaterial", "nomMaterial", reg.codMaterial);

            reg = BuscarCodItemP(reg.codItem);
            return View(reg);
        }

        public ActionResult DeleteProducto(string id = "")
        {

            List<SqlParameter> arreglo = new List<SqlParameter>() {
             new SqlParameter(){ ParameterName="@cod", SqlDbType= SqlDbType.Char,Value=id }
            };
            ViewBag.mensaje = CRUDProductoServicio("sp_deleteProducto", arreglo);
            TempData["msj"] = "Producto " + id + " Eliminado";
            return RedirectToAction("CreateProducto");
        }

        /*SERVICIOS*/
        public ActionResult CreateServicio()
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

                ViewBag.servicios = listaServicios();
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado");
                return View(new ProductoServicio());
            }
        }

        [HttpPost]
        public ActionResult CreateServicio(ProductoServicio reg, HttpPostedFileBase archivo)
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
                ViewBag.servicios = listaServicios();
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                ViewBag.mensaje = "Seleccione una imagen";
                return View(reg);
            }
            if (Path.GetExtension(archivo.FileName) != ".png"
              && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.servicios = listaServicios();
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                ViewBag.imag = "Solo extensión .jpg y .png";
                return View(reg);
            }
            List<SqlParameter> arreglo = new List<SqlParameter>() {

                new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomItem},
                new SqlParameter(){ ParameterName="@des", SqlDbType=SqlDbType.VarChar, Value=reg.desItem},
                new SqlParameter(){ ParameterName="@pre",SqlDbType=SqlDbType.Decimal, Value=reg.precioItem},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar,
                    Value="~/Imagenes/Servicios/"+Path.GetFileName(archivo.FileName)}
            };
            ViewBag.mensaje = CRUDProductoServicio("sp_insertServicio", arreglo);

            archivo.SaveAs(Path.Combine(Server.MapPath("~/Imagenes/Servicios/") + Path.GetFileName(archivo.FileName)));
            ViewBag.servicios = listaServicios();
            return View(reg);
        }

        public ActionResult EditServicio(String id = null)
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



                ProductoServicio reg = listaServicios().Where(x => x.codItem == id).FirstOrDefault();
                if (reg == null)
                    return RedirectToAction("CreateServicio");
                else
                {
                    ViewBag.servicios = listaServicios();
                    ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                    return View(reg);
                }
            }
        }

        [HttpPost]
        public ActionResult EditServicio(ProductoServicio reg, HttpPostedFileBase archivo)
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
                ViewBag.servicios = listaServicios();
                ViewBag.mensaje = "Seleccione una Imagen";
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);
                ruta = reg.imgItem;
            }
            else
            if (Path.GetExtension(archivo.FileName) != ".png"
               && Path.GetExtension(archivo.FileName) != ".jpg")
            {
                ViewBag.servicios = listaServicios();
                ViewBag.imag = "Solo extensión .jpg y .png";
                ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);

                return View(reg);
            }
            else

                ruta = "~/Imagenes/Servicios/" + Path.GetFileName(archivo.FileName);

            List<SqlParameter> arreglo = new List<SqlParameter>(){
               new SqlParameter(){ ParameterName="@cod", SqlDbType= SqlDbType.Char, Value=reg.codItem},
               new SqlParameter(){ ParameterName="@nom", SqlDbType=SqlDbType.VarChar, Value=reg.nomItem},
                new SqlParameter(){ ParameterName="@des", SqlDbType=SqlDbType.VarChar, Value=reg.desItem},
                new SqlParameter(){ ParameterName="@pre",SqlDbType=SqlDbType.Decimal, Value=reg.precioItem},
                new SqlParameter(){ParameterName="@img",SqlDbType= SqlDbType.VarChar, Value=ruta},
                new SqlParameter(){ParameterName="@est",SqlDbType= SqlDbType.Int, Value=reg.codEstado}
                };
            ViewBag.mensaje = CRUDProductoServicio("sp_updateServicio", arreglo);

            if (archivo != null)
                archivo.SaveAs(Server.MapPath(ruta));

            ViewBag.servicios = listaServicios();
            ViewBag.estitems = new SelectList(listaEstados(), "codEstado", "desEstado", reg.codEstado);


            return View(reg);
        }

        public ActionResult DeleteServicio(string id = "")
        {

            List<SqlParameter> arreglo = new List<SqlParameter>() {
             new SqlParameter(){ ParameterName="@cod", SqlDbType= SqlDbType.Char,Value=id }
            };
            ViewBag.mensaje = CRUDProductoServicio("sp_deleteServicio", arreglo);
            TempData["msj"] = "Servicio " + id + " Eliminado";
            return RedirectToAction("CreateServicio");
        }
     

    }
}