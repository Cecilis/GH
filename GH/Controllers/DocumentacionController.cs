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
using System.IO;
using GH.ViewModels;
using System.Threading.Tasks;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Documentacion")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class DocumentacionController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        //[HttpGet]
        //[Route("GetAll")]
        //public IQueryable<Documentacion> GetDocumentacion()
        //{
        //    return db.Documentacion
        //            .Include(x => x.TipoDocumento);
        //}

        //[HttpGet]
        //[Route("GetAllAsync")]
        //public async Task<IHttpActionResult> GetDocumentacionAsync()
        //{
        //    var documentacion = await db.Documentacion
        //            .OrderBy(c => c.documento)
        //            .ToListAsync();

        //    if (documentacion == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(documentacion);
        //}

        //[HttpGet]
        //[Route("GetById/{id?}")]
        //[ResponseType(typeof(List<Documentacion>))]
        //public IHttpActionResult GetDocumentacion(int id)
        //{
        //    var documentos = db.Documentacion
        //                                    .Where(x => x.id_documento == id)
        //                                    .Include(x => x.TipoDocumento)
        //                                    .FirstOrDefault();                                                                                     

        //    if (documentos == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(documentos);
        //}

        //[HttpGet]
        //[Route("GetByPages/{pageNumber?}/{pageSize?}")]
        //public IEnumerable GetDocumentacion([FromUri]PagingParameterModel pagingParameterModel)
        //{

        //    var source = db.Documentacion
        //            .Include(x => x.TipoDocumento)
        //            .OrderBy(x => x.documento).AsQueryable();

        //    int count = source.Count();
        //    int CurrentPage = pagingParameterModel.pageNumber;
        //    int PageSize = pagingParameterModel.pageSize;
        //    int TotalCount = count;
        //    int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
        //    var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        //    var havePreviousPage = CurrentPage > 1 ? "true" : "false";
        //    var haveNextPage = CurrentPage < TotalPages ? "true" : "false";

        //    var paginationMetadata = new
        //    {
        //        totalCount = TotalCount,
        //        pageSize = PageSize,
        //        currentPage = CurrentPage,
        //        totalPages = TotalPages,
        //        havePreviousPage,
        //        haveNextPage
        //    };

        //    // Se agrega los valores para el control del paginado en el encabezado del mensaje a retornar
        //    HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
        //    return items;
        //}

        //[HttpPut]
        //[Route("Update/{id?}")]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutDocumentacion(int id, Documentacion Documentacion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != Documentacion.id_documento)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(Documentacion).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        if (!DocumentacionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //[HttpPost]
        //[Route("Add")]
        //[ResponseType(typeof(Documentacion))]
        //public IHttpActionResult PostDocumentacion(Documentacion Documentacion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
            
        //    try
        //    {
        //        db.Documentacion.Add(Documentacion);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
        //    }

        //    return CreatedAtRoute("DefaultApi", new { controller = "Documentacion", id = Documentacion.id_documento }, Documentacion);
        //}

        [HttpDelete]
        [Route("Delete/{id?}")]
        [ResponseType(typeof(Documentacion))]
        public IHttpActionResult DeleteDocumentacion(int id)
        {
            Documentacion documento = db.Documentacion.Find(id);
            if (documento == null)
            {
                return NotFound();
            }

            try
            {
                var nombreRutaDocumento = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documento.documento);
                File.Delete(nombreRutaDocumento);
                db.Documentacion.Remove(documento);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(documento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentacionExists(int id)
        {
            return db.Documentacion.Count(e => e.id_documento == id) > 0;
        }

    }
}