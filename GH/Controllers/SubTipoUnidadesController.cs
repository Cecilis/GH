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
using Newtonsoft.Json;
using System.Web;
using System.Web.Http.Results;
using GH.Utilities;
using GH.ViewModels;
using System.Threading.Tasks;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/SubTipoUnidades")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class SubTipoUnidadesController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<SubTipoUnidad> GetAllSubTipoUnidades(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.SubTipoUnidad
                     .OrderBy(stu => stu.descripcion)
                     .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllSubTipoUnidadesLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.SubTipoUnidad
                    .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                    .OrderBy(stu => stu.descripcion)
                    .Select(stu => new ListTextValue { Text = stu.descripcion, Value = stu.id_subtipo, Activo = stu.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllSubTipoUnidadesAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var subtiposunidad = await db.SubTipoUnidad
                    .OrderBy(stu => stu.descripcion)
                    .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                    .Select(stu => new
                    {
                        stu.id_tipo,
                        stu.id_subtipo,
                        tipo_unidad_descripcion = stu.TipoUnidad.descripcion,
                        stu.descripcion,
                        stu.fecha_alta,
                        stu.fecha_baja,                        
                        stu.activo
                    })
                    .ToListAsync();

            if (subtiposunidad == null)
            {
                return NotFound();
            }

            return Ok(subtiposunidad);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(SubTipoUnidad))]
        public IHttpActionResult GetSubTipoUnidadById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var subtiposunidad = db.SubTipoUnidad
                                    .Where(stu => (stu.id_subtipo == id) && (condicion ? stu.activo == bStatus : stu.activo == stu.activo))
                                    .Select(stu => new
                                    {
                                        stu.id_subtipo,
                                        stu.id_tipo,                                        
                                        tipo_unidad_descripcion = stu.TipoUnidad.descripcion,
                                        stu.descripcion,
                                        stu.fecha_alta,
                                        stu.fecha_baja,
                                        stu.activo
                                    })
                                    .SingleOrDefault();

            if (subtiposunidad == null)
            {
                return NotFound();
            }
            return Ok(subtiposunidad);
        }

        [ResponseType(typeof(List<ListTextValue>))]
        [Route("GetByTipoUnidad/{id}/{status?}")]
        public IHttpActionResult GetSubTipoUnidadesByTipoUnidad(int id, string status = null)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            List<ListTextValue> subtipoUnidades = db.SubTipoUnidad
                                .Where(stu => (stu.id_tipo.Equals(id)) && (condicion ? stu.activo == bStatus : stu.activo == stu.activo))
                                .OrderBy(stu => stu.descripcion)
                                .Select(stu => new ListTextValue { Text = stu.descripcion, Value = stu.id_subtipo, Activo = stu.activo })
                                .ToList();
            if (subtipoUnidades == null)
            {
                return NotFound();
            }

            return Ok(subtipoUnidades);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetSubTipoUnidadesByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.SubTipoUnidad
                            .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                            .OrderBy(stu => stu.descripcion)
                            .Select(stu => new
                            {
                                stu.id_subtipo,
                                stu.id_tipo,                                
                                tipo_unidad_descripcion = stu.TipoUnidad.descripcion,
                                stu.descripcion,
                                stu.fecha_alta,
                                stu.fecha_baja,
                                stu.activo
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
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubTipoUnidad(int id, SubTipoUnidad subTipoUnidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subTipoUnidad.id_subtipo)
            {
                return BadRequest();
            }

            db.Entry(subTipoUnidad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SubTipoUnidadExists(id))
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
        [ResponseType(typeof(SubTipoUnidad))]
        public IHttpActionResult PostSubTipoUnidad(SubTipoUnidad subTipoUnidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            try
            {
                db.SubTipoUnidad.Add(subTipoUnidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "SubTipoUnidades", id = subTipoUnidad.id_subtipo }, subTipoUnidad);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(SubTipoUnidad))]
        public IHttpActionResult DeleteSubTipoUnidad(int id)
        {
            SubTipoUnidad subTipoUnidad = db.SubTipoUnidad.Find(id);
            if (subTipoUnidad == null)
            {
                return NotFound();
            }

            try
            {
                db.SubTipoUnidad.Remove(subTipoUnidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(subTipoUnidad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubTipoUnidadExists(int id)
        {
            return db.SubTipoUnidad.Count(e => e.id_subtipo == id) > 0;
        }

    }
}