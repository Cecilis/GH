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
    [RoutePrefix("api/TipoPersonas")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class TipoPersonasController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<TipoPersona> GetAllTipoPersonas(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoPersonas
                     .OrderBy(tp => tp.descripcion)
                     .Where(tp => condicion ? tp.activo == bStatus : tp.activo == tp.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllTipoPersonasLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoPersonas
                    .Where(tp => condicion ? tp.activo == bStatus : tp.activo == tp.activo)
                    .OrderBy(tp => tp.descripcion)
                    .Select(tp => new ListTextValue { Text = tp.descripcion, Value = tp.id_tipo_persona, Activo = tp.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllTipoPersonasAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var bases = await db.TipoPersonas
                    .OrderBy(tp => tp.descripcion)
                    .Where(tp => condicion ? tp.activo == bStatus : tp.activo == tp.activo)
                    .Select(tp => new
                    {
                        tp.id_tipo_persona,
                        tp.descripcion,
                        tp.activo
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
        [ResponseType(typeof(TipoPersona))]
        public IHttpActionResult GetTipoPersonaById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var tipopersonas = db.TipoPersonas
                                    .Where(tp => (tp.id_tipo_persona == id) && (condicion ? tp.activo == bStatus : tp.activo == tp.activo))
                                    .Select(tp => new
                                    {
                                        tp.id_tipo_persona,
                                        tp.descripcion,
                                        tp.activo
                                    })
                                    .SingleOrDefault();

            if (tipopersonas == null)
            {
                return NotFound();
            }
            return Ok(tipopersonas);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetTipoPersonasByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.TipoPersonas
                            .Where(tp => condicion ? tp.activo == bStatus : tp.activo == tp.activo)
                            .OrderBy(tp => tp.descripcion   )
                            .Select(tp => new
                            {
                                tp.id_tipo_persona,
                                tp.descripcion,
                                tp.activo
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
        public IHttpActionResult PutTipoPersona(int id, TipoPersona tipoPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoPersona.id_tipo_persona)
            {
                return BadRequest();
            }

            db.Entry(tipoPersona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TipoPersonaExists(id))
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

        [HttpPut]
        [Route("LogicDeleteTipoPersona/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteTipoPersona(int id, TipoPersona tipoPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoPersona.id_tipo_persona)
            {
                return BadRequest();
            }

            //eliminacion logica
            this.UpdateActiveTipoPersona(tipoPersona);
            db.Entry(tipoPersona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TipoPersonaExists(id))
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
        [ResponseType(typeof(TipoPersona))]
        public IHttpActionResult PostTipoPersona(TipoPersona tipoPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                db.TipoPersonas.Add(tipoPersona);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "TipoPersonas", id = tipoPersona.id_tipo_persona }, tipoPersona);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(TipoPersona))]
        public IHttpActionResult DeleteTipoPersona(int id)
        {
            TipoPersona tipoPersona = db.TipoPersonas.Find(id);
            if (tipoPersona == null)
            {
                return NotFound();
            }

            db.TipoPersonas.Remove(tipoPersona);
            db.SaveChanges();

            return Ok(tipoPersona);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoPersonaExists(int id)
        {
            return db.TipoPersonas.Count(e => e.id_tipo_persona == id) > 0;
        }

        private void UpdateActiveTipoPersona(TipoPersona tipoPersona)
        {
            //coloco el estatus activo en falso
            tipoPersona.activo = false;

        }

    }
}