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
using System.Collections;
using System.Web;
using Newtonsoft.Json;
using System.Web.Http.Results;
using GH.Utilities;
using GH.ViewModels;
using System.Threading.Tasks;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/TipoServicios")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class TipoServiciosController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<TipoServicio> GetAllTipoServicios(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoServicio
                     .OrderBy(ts => ts.nombre)
                     .Where(ts => condicion ? ts.activo == bStatus : ts.activo == ts.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllTipoServiciosLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoServicio
                    .Where(ts => condicion ? ts.activo == bStatus : ts.activo == ts.activo)
                    .OrderBy(ts => ts.nombre)
                    .Select(ts => new ListTextValue { Text = ts.nombre, Value = ts.id_tipo_servicio, Activo = ts.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllTipoServiciosAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var tipoServicios = await db.TipoServicio
                    .OrderBy(ts => ts.nombre)
                    .Where(ts => condicion ? ts.activo == bStatus : ts.activo == ts.activo)
                    .Select(ts => new
                    {
                        ts.id_tipo_servicio,
                        ts.id_servicio,
                        servicio_nombre = ts.Servicios.nombre,
                        ts.nombre,
                        ts.activo
                    })
                    .ToListAsync();

            if (tipoServicios == null)
            {
                return NotFound();
            }

            return Ok(tipoServicios);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(TipoServicio))]
        public IHttpActionResult GetTipoServicioById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;



            var tipoServicios = db.TipoServicio
                            .Where(ts => (ts.id_tipo_servicio == id) && (condicion ? ts.activo == bStatus : ts.activo == ts.activo))
                            .AsEnumerable()
                            .Select(ts => new
                            {                                
                                ts.id_tipo_servicio,
                                ts.id_servicio,                                
                                ts.nombre,
                                descripcion = HttpUtility.HtmlDecode(ts.descripcion),
                                ts.activo
                            })
                            .SingleOrDefault();

            if (tipoServicios == null)
            {
                return NotFound();
            }
            return Ok(tipoServicios);
        }

        [HttpGet]
        [Route("GetByServicio/{id}/{status?}")]
        [ResponseType(typeof(List<ListTextValue>))]
        public IHttpActionResult GetTipoServiciosByServicio(int id, string status = null)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            List<ListTextValue> tipoServicios = db.TipoServicio
                                                 .Where(ts => (ts.id_servicio.Equals(id)) && (condicion ? ts.activo == bStatus : ts.activo == ts.activo))
                                                 .OrderBy(ts => ts.nombre)
                                                 .Select(ts => new ListTextValue { Text = ts.nombre, Value = ts.id_tipo_servicio, Activo = ts.activo })
                                                 .ToList();

            if (tipoServicios == null)
            {
                return NotFound();
            }

            return Ok(tipoServicios);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        public IEnumerable GetTipoServiciosByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.TipoServicio
                            .Where(b => (filter ? b.nombre.Contains(pagingParameterModel.filterWord) : b.nombre == b.nombre) && (condicion ? b.activo == bStatus : b.activo == b.activo))
                            .OrderBy(ts => ts.Servicios.nombre)
                            .ThenBy(ts => ts.nombre)
                            .Select(ts => new
                            {
                                ts.id_tipo_servicio,
                                ts.id_servicio,
                                servicio_nombre = ts.Servicios.nombre,
                                ts.nombre,
                                ts.activo
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
        public IHttpActionResult PutTipoServicio(int id, TipoServicio tipoServicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoServicio.id_tipo_servicio)
            {
                return BadRequest();
            }

            tipoServicio.descripcion = WebUtility.HtmlEncode(tipoServicio.descripcion);

            db.Entry(tipoServicio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoServicioExists(id))
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
        [ResponseType(typeof(TipoServicio))]
        public IHttpActionResult PostTipoServicio(TipoServicio tipoServicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                tipoServicio.descripcion = WebUtility.HtmlEncode(tipoServicio.descripcion);
                db.TipoServicio.Add(tipoServicio);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "TipoServicios", id = tipoServicio.id_tipo_servicio }, tipoServicio);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(TipoServicio))]
        public IHttpActionResult DeleteTipoServicio(int id)
        {
            TipoServicio tipoServicio = db.TipoServicio.Find(id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            try
            {
                db.TipoServicio.Remove(tipoServicio);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(tipoServicio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoServicioExists(int id)
        {
            return db.TipoServicio.Count(e => e.id_tipo_servicio == id) > 0;
        }

    }
}