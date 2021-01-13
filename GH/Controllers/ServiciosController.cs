using GH.Models;
using GH.Utilities;
using GH.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Servicios")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class ServiciosController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Servicios> GetAllServicios(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Servicios
                     .OrderBy(s => s.nombre)
                     .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllServiciosLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Servicios
                    .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                    .OrderBy(s => s.nombre)
                    .Select(s => new ListTextValue { Text = s.nombre, Value = s.id_servicio, Activo = s.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllServiciosAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var servicios = await db.Servicios
                    .OrderBy(s => s.nombre)
                    .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                    .Select(s => new
                    {
                        s.id_servicio,
                        s.nombre,
                        s.observaciones,
                        s.fecha_alta,
                        s.fecha_baja,
                        s.activo
                    })
                    .ToListAsync();

            if (servicios == null)
            {
                return NotFound();
            }

            return Ok(servicios);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(Servicios))]
        public IHttpActionResult GetServicioById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var servicios = db.Servicios
                            .Where(s => (s.id_servicio == id) && (condicion ? s.activo == bStatus : s.activo == s.activo))
                            .Select(s => new
                            {
                                s.id_servicio,
                                s.nombre,
                                s.observaciones,
                                s.fecha_alta,
                                s.fecha_baja,
                                s.activo
                            })
                            .SingleOrDefault();

            if (servicios == null)
            {
                return NotFound();
            }
            return Ok(servicios);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        public IEnumerable GetServiciosByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Servicios
                            .Where(b => (filter ? b.nombre.Contains(pagingParameterModel.filterWord)  : b.nombre == b.nombre) && (condicion ? b.activo == bStatus : b.activo == b.activo))
                            .OrderBy(s => s.nombre)
                            .Select(s => new
                            {
                                s.id_servicio,
                                s.nombre,
                                s.observaciones,
                                s.fecha_alta,
                                s.fecha_baja,
                                s.activo
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
 
        [HttpPut]
        [Route("Update/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServicios(int id, Servicios servicios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servicios.id_servicio)
            {
                return BadRequest();
            }

            db.Entry(servicios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiciosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(Servicios))]
        public IHttpActionResult PostServicios(Servicios servicios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Servicios.Add(servicios);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Servicios", id = servicios.id_servicio }, servicios);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(Servicios))]
        public IHttpActionResult DeleteServicios(int id)
        {
            Servicios servicios = db.Servicios.Find(id);
            if (servicios == null)
            {
                return NotFound();
            }

            try
            {
                db.Servicios.Remove(servicios);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(servicios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiciosExists(int id)
        {
            return db.Servicios.Count(e => e.id_servicio == id) > 0;
        }

    }
}