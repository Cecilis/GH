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
    [RoutePrefix("api/Provincias")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class ProvinciasController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Provincias> GetAllProvincias(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Provincias
                     .OrderBy(s => s.provincia)
                     .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllProvinciasLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Provincias
                    .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                    .OrderBy(s => s.provincia)
                    .Select(s => new ListTextValue { Text = s.provincia, Value = s.id_provincia, Activo = s.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllProvinciasAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var bases = await db.Provincias
                    .OrderBy(s => s.provincia)
                    .Where(s => condicion ? s.activo == bStatus : s.activo == s.activo)
                    .Select(s => new
                    {
                        s.id_provincia,
                        s.provincia,
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
        [ResponseType(typeof(Provincias))]
        public IHttpActionResult GetProvinciaById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var bases = db.Provincias
                            .Where(s => (s.id_provincia == id) && (condicion ? s.activo == bStatus : s.activo == s.activo))
                            .Select(s => new
                            {
                                s.id_provincia,
                                s.provincia,
                                s.activo,
                                s.fecha_alta,
                                s.fecha_baja
                            })
                            .SingleOrDefault();

            if (bases == null)
            {
                return NotFound();
            }
            return Ok(bases);
        }


        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{filterWord?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetProvinciasByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Provincias
                            .Where(s => (filter ? s.provincia.Contains(pagingParameterModel.filterWord) : s.provincia == s.provincia) && (condicion ? s.activo == bStatus : s.activo == s.activo))
                            .OrderBy(s => s.provincia)
                            .Select(s => new
                            {
                                s.id_provincia,
                                s.provincia,
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
        public IHttpActionResult PutProvincias(int id, Provincias provincias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != provincias.id_provincia)
            {
                return BadRequest();
            }

            bool status_activo_anterior = db.Provincias.Where(b => b.id_provincia == provincias.id_provincia).Select(b => b.activo).SingleOrDefault();

            if (!provincias.activo == status_activo_anterior)
                if (!provincias.activo)
                    provincias.fecha_baja = DateTime.Now;
                else
                    provincias.fecha_baja = null; //Esto en caso de que se quiera limpiar al reactivar


            db.Entry(provincias).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciasExists(id))
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
        [ResponseType(typeof(Provincias))]
        public IHttpActionResult PostProvincias(Provincias provincias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Provincias.Add(provincias);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Provincias", id = provincias.id_provincia }, provincias);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Provincias))]
        public IHttpActionResult DeleteProvincias(int id)
        {
            Provincias provincias = db.Provincias.Find(id);
            if (provincias == null)
            {
                return NotFound();
            }

            try
            {
                db.Provincias.Remove(provincias);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(provincias);
        }

        [HttpPut]
        [Route("LogicDeleteProvincia/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteProvincia(int id, Provincias provincias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != provincias.id_provincia)
            {
                return BadRequest();
            }


            this.UpdateActiveDateProvincia(provincias);
            db.Entry(provincias).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciasExists(id))
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

        public void UpdateActiveDateProvincia(Provincias provincias)
        {
            //coloco el estatus activo en falso
            provincias.activo = false;
            //coloco la fecha de baja la fecha en la cual se elimino
            provincias.fecha_baja = DateTime.Now;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProvinciasExists(int id)
        {
            return db.Provincias.Count(e => e.id_provincia == id) > 0;
        }

    }
}