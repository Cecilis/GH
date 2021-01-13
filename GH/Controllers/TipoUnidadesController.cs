using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
    [RoutePrefix("api/TipoUnidades")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class TipoUnidadesController : ApiController
    {
        private GHDBContext db = new GHDBContext();


        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<TipoUnidad> GetAllTipoUnidades(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoUnidad
                     .OrderBy(x => x.descripcion)
                     .Where(tu => condicion ? tu.activo == bStatus : tu.activo == tu.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllTipoUnidadesLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoUnidad
                    .Where(tu => condicion ? tu.activo == bStatus : tu.activo == tu.activo)
                    .OrderBy( tu=> tu.descripcion)
                    .Select(tu => new ListTextValue { Text = tu.descripcion, Value = tu.id_tipo, Activo = tu.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllTipoUnidadesAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var bases = await db.TipoUnidad
                    .OrderBy(tu => tu.descripcion)
                    .Where(tu => condicion ? tu.activo == bStatus : tu.activo == tu.activo)
                    .Select(tu => new
                    {
                        tu.id_tipo,
                        tu.descripcion,
                        tu.fecha_alta,
                        tu.fecha_baja,
                        tu.autopropulsada,
                        tu.activo
                    })
                    .ToListAsync();

            if (bases == null)
            {
                return NotFound();
            }

            return Ok(bases);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(TipoUnidad))]
        public IHttpActionResult GetTipoUnidadById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var tipounidades = db.TipoUnidad
                            .Where(tu => (tu.id_tipo == id) && (condicion ? tu.activo == bStatus : tu.activo == tu.activo))
                            .Select(tu => new
                            {
                                tu.id_tipo,
                                tu.descripcion,
                                tu.fecha_alta,
                                tu.fecha_baja,
                                tu.autopropulsada,
                                tu.activo
                            })
                            .SingleOrDefault();

            if (tipounidades == null)
            {
                return NotFound();
            }
            return Ok(tipounidades);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetTipoUnidadesByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.TipoUnidad
                            .Where(tu => condicion ? tu.activo == bStatus : tu.activo == tu.activo)
                            .OrderBy(tu => tu.descripcion)
                            .Select(tu => new
                            {
                                tu.id_tipo,
                                tu.descripcion,
                                tu.fecha_alta,
                                tu.fecha_baja,
                                tu.autopropulsada,
                                tu.activo
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
        public IHttpActionResult PutTipoUnidad(int id, TipoUnidad tipoUnidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoUnidad.id_tipo)
            {
                return BadRequest();
            }

            db.Entry(tipoUnidad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TipoUnidadExists(id))
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
        [ResponseType(typeof(TipoUnidad))]
        public IHttpActionResult PostTipoUnidad(TipoUnidad tipoUnidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.TipoUnidad.Add(tipoUnidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "TipoUnidades", id = tipoUnidad.id_tipo }, tipoUnidad);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(TipoUnidad))]
        public IHttpActionResult DeleteTipoUnidad(int id)
        {
            TipoUnidad tipoUnidad = db.TipoUnidad.Find(id);
            if (tipoUnidad == null)
            {
                return NotFound();
            }
            
            try
            {
                db.TipoUnidad.Remove(tipoUnidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(tipoUnidad);
        }

        [HttpPut]
        [Route("LogicDeleteTipoUnidad/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteTipoUnidad(int id, TipoUnidad tipoUnidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoUnidad.id_tipo)
            {
                return BadRequest();
            }

            this.UpdateActiveDateTipoUnidad(tipoUnidad);
            db.Entry(tipoUnidad).State = EntityState.Modified;

            try
            {

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TipoUnidadExists(id))
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


        public void UpdateActiveDateTipoUnidad(TipoUnidad tipoUnidad)
        {
            //coloco el estatus activo en falso
            tipoUnidad.activo = false;
            //coloco la fecha de baja la fecha en la cual se elimino
            tipoUnidad.fecha_baja = DateTime.Now;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoUnidadExists(int id)
        {
            return db.TipoUnidad.Count(e => e.id_tipo == id) > 0;
        }

    }
}