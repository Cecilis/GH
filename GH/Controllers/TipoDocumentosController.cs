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
    [RoutePrefix("api/TipoDocumentos")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class TipoDocumentosController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<TipoDocumento> GetAllTipoDocumentos(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoDocumento
                     .OrderBy(td => td.nombre)
                     .Where(td => condicion ? td.activo == bStatus : td.activo == td.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllTipoDocumentosLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.TipoDocumento
                    .Where(td => condicion ? td.activo == bStatus : td.activo == td.activo)
                    .OrderBy(td => td.nombre)
                    .Select(td => new ListTextValue { Text = td.nombre, Value = td.id_tipo_documento, Activo = td.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllTipoDocumentosAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var tipodocumentos = await db.TipoDocumento
                                            .OrderBy(td => td.nombre)
                                            .Where(td => condicion ? td.activo == bStatus : td.activo == td.activo)
                                            .Select(td => new
                                            {
                                                td.id_tipo_documento,
                                                td.nombre,
                                                td.aplica_a,
                                                td.vencimiento,
                                                td.nro_dias_vencimiento,
                                                td.activo
                                            })
                                            .ToListAsync();

            if (tipodocumentos == null)
            {
                return NotFound();
            }

            return Ok(tipodocumentos);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(TipoDocumento))]
        public IHttpActionResult GetTipoDocumentoById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var tipodocumentos = db.TipoDocumento
                                    .Where(td => (td.id_tipo_documento == id) && (condicion ? td.activo == bStatus : td.activo == td.activo))
                                    .Select(td => new
                                    {
                                        td.id_tipo_documento,
                                        td.nombre,
                                        td.aplica_a,
                                        td.vencimiento,
                                        td.nro_dias_vencimiento,
                                        td.activo
                                    })
                                    .SingleOrDefault();

            if (tipodocumentos == null)
            {
                return NotFound();
            }
            return Ok(tipodocumentos);
        }

        [HttpGet]
        [Route("ApplyTo/{apply_to?}/{status?}")]
        [ResponseType(typeof(List<ListTextValue>))]        
        public IHttpActionResult GetTipoDocumentoApplyTo(string apply_to, string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            if (!((apply_to.Equals("P")) || (apply_to.Equals("U"))))
            {
                return BadRequest();
            }

            List<ListTextValue> tiposDocumento = db.TipoDocumento
                                .Where(td => td.aplica_a.Equals(apply_to) && (condicion ? td.activo == bStatus : td.activo == td.activo))
                                .OrderBy(td => td.nombre)
                                .Select(td => new ListTextValue { Text = td.nombre, Value = td.id_tipo_documento, Activo = td.activo })
                                .ToList();

            if (tiposDocumento == null)
            {
                return NotFound();
            }

            return Ok(tiposDocumento);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetTipoDocumentosByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.TipoDocumento
                            .Where(td => condicion ? td.activo == bStatus : td.activo == td.activo)
                            .OrderBy(td => td.nombre)
                            .Select(td => new
                            {
                                td.id_tipo_documento,
                                td.nombre,
                                td.aplica_a,
                                td.vencimiento,
                                td.nro_dias_vencimiento,                                
                                td.activo
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
        public IHttpActionResult PutTipoDocumento(int id, TipoDocumento tipodocumento)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipodocumento.id_tipo_documento)
            {
                return BadRequest();
            }

            db.Entry(tipodocumento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TipoDocumentoExists(id))
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
        [Route("LogicDeleteTipoDocumento/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteTipoDocumento(int id, TipoDocumento tipodocumento)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipodocumento.id_tipo_documento)
            {
                return BadRequest();
            }
            //eliminacion logica
            this.UpdateActiveTipoDocumento(tipodocumento);
            db.Entry(tipodocumento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TipoDocumentoExists(id))
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
        [ResponseType(typeof(TipoDocumento))]
        public IHttpActionResult PostTipoDocumento(TipoDocumento tipoDocumento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.TipoDocumento.Add(tipoDocumento);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "TipoDocumentos", id = tipoDocumento.id_tipo_documento }, tipoDocumento);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(TipoDocumento))]
        public IHttpActionResult DeleteTipoDocumento(int id)
        {
            TipoDocumento tipoDocumento = db.TipoDocumento.Find(id);
            if (tipoDocumento == null)
            {
                return NotFound();
            }
           
            try
            {
                db.TipoDocumento.Remove(tipoDocumento);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(tipoDocumento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoDocumentoExists(int id)
        {
            return db.TipoDocumento.Count(e => e.id_tipo_documento == id) > 0;
        }

        private void UpdateActiveTipoDocumento(TipoDocumento tipodocumento)
        {
            //coloco el estatus activo en falso
            tipodocumento.activo = false;
        }

    }
}