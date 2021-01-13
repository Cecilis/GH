using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GH.Models;
using System.Web;
using Newtonsoft.Json;
using System.Collections;
using System.Web.Http.Results;
using GH.Utilities;
using System.Threading.Tasks;
using GH.ViewModels;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Clientes")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class ClientesController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Clientes> GetAllClientes(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Clientes
                     .OrderBy(c => c.Denominacion)
                     .Where(c => condicion ? c.activo == bStatus : c.activo == c.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllClientesLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Clientes
                    .Where(c => condicion ? c.activo == bStatus : c.activo == c.activo)
                    .OrderBy(c => c.Denominacion)
                    .Select(c => new ListTextValue { Text = c.Denominacion, Value = c.id_cliente, Activo = c.activo == true ? true : false })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetByTerm/{term}/{status?}")]
        [ResponseType(typeof(List<string>))]
        public IHttpActionResult GetAllClientesByTerm(string term, string status = null)
        {
            if (term == null)
            {
                return BadRequest();
            }

            //.Where(s => s.CUIT.ToString().StartsWith(term) || s.Denominacion.ToString().Contains(term))
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var clientes = db.Clientes
                    .Where(c => (c.CUIT.ToString().StartsWith(term) || 
                                 c.Denominacion.ToString().Contains(term)) &&
                                 (condicion ? c.activo == bStatus : c.activo == c.activo))
                    .OrderBy(c => c.Denominacion)
                    .Select(c => new
                    {
                        c.id_cliente,
                        c.CUIT,
                        c.Denominacion,
                        c.id_localidad,
                        localidad = c.Localidades.localidad,
                        id_provincia = c.Localidades.Provincias.id_provincia,
                        provincia = c.Localidades.Provincias.provincia,
                        c.Direccion,                        
                        c.Telefono,
                        c.Mail,
                        c.activo
                    })
                    .ToList();

            if (clientes == null)
            {
                return NotFound();
            }

            return Ok(clientes);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(SubTipoUnidad))]
        public IHttpActionResult GetClienteById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var clientes = db.Clientes
                            .Where(c => (c.id_cliente == id) && (condicion ? c.activo == bStatus : c.activo == c.activo))
                            .Select(c => new
                            {
                                c.id_cliente,
                                c.Denominacion,
                                c.CUIT,
                                c.Direccion,
                                c.fecha_alta,
                                c.fecha_baja,
                                c.Mail,
                                c.Telefono,
                                c.id_localidad,
                                localidad = c.Localidades.localidad,
                                id_provincia = c.Localidades.Provincias.id_provincia,
                                provincia = c.Localidades.Provincias.provincia,
                                c.activo     
                            })
                            .SingleOrDefault();

            if (clientes == null)
            {
                return NotFound();
            }
            return Ok(clientes);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetClienteByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Clientes
                            .Where(b => (filter ? b.Denominacion.Contains(pagingParameterModel.filterWord) : b.Denominacion == b.Denominacion) && (condicion ? b.activo == bStatus : b.activo == b.activo))
                            .OrderBy(c => c.Denominacion)
                            .Select(c => new
                            {
                                c.id_cliente,
                                c.Denominacion,
                                c.CUIT,
                                c.Direccion,
                                c.fecha_alta,
                                c.fecha_baja,
                                c.Mail,
                                c.Telefono,
                                id_provincia = c.Localidades.Provincias.id_provincia,
                                provincia = c.Localidades.Provincias.provincia,
                                c.id_localidad,
                                localidad = c.Localidades.localidad,
                                c.activo
                            })
                            .AsQueryable();
                            

            int count = source.Count();
            int CurrentPage = pagingParameterModel.pageNumber;
            int PageSize = pagingParameterModel.pageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            var havePreviousPage = CurrentPage > 1 ? "true" : "false";
            var haveNextPage = CurrentPage < TotalPages ? "true" : "false";

            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                havePreviousPage,
                haveNextPage
            };

            // Se agrega los valores para el control del paginado en el encabezado del mensaje a retornar
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            return items;
        }

        [HttpGet]
        [Route("GetByCUIT/{cuit}/{status?}")]
        [ResponseType(typeof(SubTipoUnidad))]
        public IHttpActionResult GetClienteByCUIT(long cuit, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var clientes = db.Clientes
                            .Where(c => (c.CUIT == cuit) && (condicion ? c.activo == bStatus : c.activo == c.activo))
                            .Select(c => new
                            {
                                c.id_cliente,
                                c.Denominacion,
                                c.CUIT,
                                c.Direccion,
                                c.fecha_alta,
                                c.fecha_baja,
                                c.Mail,
                                c.Telefono,
                                c.Localidad,
                                c.activo
                            })
                            .SingleOrDefault();

            if (clientes == null)
            {
                return NotFound();
            }
            return Ok(clientes);
        }


        [HttpGet]
        [Route("CountByCUIT/{cuit}/{id?}/{status?}")]
        public int CountClientesByCUITId([FromUri]long cuit, int id = 0, string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Clientes.Count(c => (c.CUIT == cuit) && (id > 0 ? c.id_cliente != id : c.id_cliente == c.id_cliente) && (condicion ? c.activo == bStatus : c.activo == c.activo));
        }

        [HttpPut]
        [Route("Update/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientes(int id, Clientes clientes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientes.id_cliente)
            {
                return BadRequest();
            }

            db.Entry(clientes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ClientesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Clientes))]
        public IHttpActionResult PostClientes(Clientes clientes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            try
            {
                db.Clientes.Add(clientes);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Clientes", id = clientes.id_cliente }, clientes);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Clientes))]
        public IHttpActionResult DeleteClientes(int id)
        {
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return NotFound();
            }

            try
            {
                db.Clientes.Remove(clientes);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(clientes);
        }


        [HttpPut]
        [Route("LogicDeleteCliente/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteCliente(int id, Clientes clientes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientes.id_cliente)
            {
                return BadRequest();
            }

            this.UpdateActiveDateCliente(clientes);
            db.Entry(clientes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ClientesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public void UpdateActiveDateCliente(Clientes clientes)
        {
            //coloco el estatus activo en falso
            clientes.activo = false;
            //coloco la fecha de baja la fecha en la cual se elimino
            clientes.fecha_baja = DateTime.Now;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientesExists(int id)
        {
            return db.Clientes.Count(e => e.id_cliente == id) > 0;
        }

    }
}