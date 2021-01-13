using GH.Models;
using GH.Utilities;
using GH.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [RoutePrefix("api/Localidad")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class LocalidadController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Localidad> GetAllLocalidad(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Localidad
                     .OrderBy(s => s.Provincias.provincia)
                     .ThenBy(s => s.localidad)
                     .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllLocalidadLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Localidad
                    .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                    .OrderBy(s => s.localidad)
                    .Select(s => new ListTextValue { Text = s.localidad, Value = s.id_localidad, Activo = s.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllLocalidadAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var bases = await db.Localidad
                    .OrderBy(s => s.Provincias.provincia)
                    .ThenBy(s => s.localidad)
                    .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                    .Select(s => new
                    {
                        s.id_localidad,
                        s.localidad,
                        provincia = s.Provincias.provincia,
                        id_provincia = s.Provincias.id_provincia,
                        s.activo,
                        s.fecha_alta,
                        s.fecha_baja
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
        [ResponseType(typeof(Localidad))]
        public IHttpActionResult GetProvinciaById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var localidad = db.Localidad
                            .Where(s => (s.id_localidad == id) && (condicion ? s.activo == bStatus : s.activo == s.activo))
                            .Select(s => new
                            {
                                s.id_localidad,
                                s.localidad,
                                provincia = s.Provincias.provincia,
                                id_provincia = s.Provincias.id_provincia,
                                s.activo,
                                s.fecha_alta,
                                s.fecha_baja
                            })
                            .SingleOrDefault();

            if (localidad == null)
            {
                return NotFound();
            }
            return Ok(localidad);
        }

        [ResponseType(typeof(List<ListTextValue>))]
        [Route("GetByProvincias/{id}/{status?}")]
        public IHttpActionResult GetLocalidadesByProvincia(int id, string status = null)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            List<ListTextValue> localidades = db.Localidad
                                .Where(l => (l.id_provincia.Equals(id)) && (condicion ? l.activo == bStatus : l.activo == l.activo))
                                .OrderBy(l => l.localidad)
                                .Select(l => new ListTextValue { Text = l.localidad, Value = l.id_localidad, Activo = l.activo })
                                .ToList();

            if (localidades == null)
            {
                return NotFound();
            }

            return Ok(localidades);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{filterWord?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetLocalidadByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Localidad
                            .Where(s => (filter ? s.localidad.Contains(pagingParameterModel.filterWord) : s.localidad == s.localidad) && (condicion ? s.activo == bStatus : s.activo == s.activo))
                            .OrderBy(s => s.Provincias.provincia)
                            .ThenBy(s => s.localidad)
                            .Select(s => new
                            {
                                s.id_localidad,
                                s.localidad,
                                provincia = s.Provincias.provincia,
                                id_provincia = s.Provincias.id_provincia,
                                s.activo,
                                s.fecha_alta,
                                s.fecha_baja
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
        public IHttpActionResult PutLocalidad(int id, Localidad localidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != localidad.id_localidad)
            {
                return BadRequest();
            }

            bool status_activo_anterior = db.Localidad.Where(b => b.id_localidad == localidad.id_localidad).Select(b => b.activo).SingleOrDefault();

            if (!localidad.activo == status_activo_anterior)
                if (!localidad.activo)
                    localidad.fecha_baja = DateTime.Now;
                else
                    localidad.fecha_baja = null; //Esto en caso de que se quiera limpiar al reactivar

            db.Entry(localidad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalidadExists(id))
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
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Localidad))]
        public IHttpActionResult PostLocalidad(Localidad localidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Localidad.Add(localidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Localidad", id = localidad.id_localidad }, localidad);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Localidad))]
        public IHttpActionResult DeleteLocalidad(int id)
        {
            Localidad localidad = db.Localidad.Find(id);
            if (localidad == null)
            {
                return NotFound();
            }

            try
            {
                db.Localidad.Remove(localidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(localidad);
        }

               


        [HttpPut]
        [Route("LogicDeleteLocalidad/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteProvincia(int id, Localidad localidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != localidad.id_localidad)
            {
                return BadRequest();
            }
            this.UpdateActiveDateLocalidad(localidad);
            db.Entry(localidad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalidadExists(id))
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

        public void UpdateActiveDateLocalidad(Localidad localidad)
        {
            //coloco el estatus activo en falso
            localidad.activo = false;
            //coloco la fecha de baja la fecha en la cual se elimino
            localidad.fecha_baja = DateTime.Now;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocalidadExists(int id)
        {
            return db.Localidad.Count(e => e.id_localidad == id) > 0;
        }

    }
}