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
using System.Web.Http.Results;
using GH.Utilities;
using System.Threading.Tasks;
using System.Collections;
using GH.ViewModels;
using System.Web;
using Newtonsoft.Json;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Gremios")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class GremiosController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Gremios> GetAllGremios(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Gremios
                     .OrderBy(g => g.gremio)
                     .Where(g => condicion ? g.activo == bStatus : g.activo == g.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllGremiosLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Gremios
                    .Where(g => condicion ? g.activo == bStatus : g.activo == g.activo)
                    .OrderBy(g => g.gremio)
                    .Select(g => new ListTextValue { Text = g.id_gremio + " " + g.gremio, Value = g.id_gremio, Activo = g.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllGremiosAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var gremios = await db.Gremios
                    .Where(g => condicion ? g.activo == bStatus : g.activo == g.activo)
                    .OrderBy(g => g.gremio)
                    .Select(g => new
                    {
                        g.id_gremio,
                        g.gremio,
                        g.activo
                    })
                    .ToListAsync();

            if (gremios == null)
            {
                return NotFound();
            }

            return Ok(gremios);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(Gremios))]
        public IHttpActionResult GetGremioById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var gremios = db.Gremios
                            .Where(g => (g.id_gremio == id) && (condicion ? g.activo == bStatus : g.activo == g.activo))
                            .OrderBy(g => g.gremio)
                            .Select(g => new
                            {
                                g.id_gremio,
                                g.gremio,
                                g.activo
                            })
                            .SingleOrDefault();

            if (gremios == null)
            {
                return NotFound();
            }
            return Ok(gremios);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        public IEnumerable GetGremiosByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.Gremios
                            .Where(g => condicion ? g.activo == bStatus : g.activo == g.activo)
                            .OrderBy(g => g.gremio)
                            .Select(g => new
                            {
                                g.id_gremio,
                                g.gremio,
                                g.activo
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
        public IHttpActionResult PutGremios(int id, Gremios gremios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gremios.id_gremio)
            {
                return BadRequest();
            }

            db.Entry(gremios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!GremiosExists(id))
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
        [ResponseType(typeof(Gremios))]
        public IHttpActionResult PostGremios(Gremios gremios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                db.Gremios.Add(gremios);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Gremios", id = gremios.id_gremio }, gremios);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Gremios))]
        public IHttpActionResult DeleteGremios(int id)
        {
            Gremios gremios = db.Gremios.Find(id);
            if (gremios == null)
            {
                return NotFound();
            }
            try
            {
                db.Gremios.Remove(gremios);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(gremios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GremiosExists(int id)
        {
            return db.Gremios.Count(e => e.id_gremio == id) > 0;
        }

    }
}