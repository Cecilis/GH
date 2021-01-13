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
using GH.Utilities;
using GH.ViewModels;
using System.Threading.Tasks;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Unidades")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class UnidadesController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Unidades> GetAllUnidades(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Unidades
                     .OrderBy(stu => stu.Marca)
                     .ThenBy(stu => stu.Modelo)
                     .Where(stu => condicion ? stu.Activo == bStatus : stu.Activo == stu.Activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllUnidadesLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Unidades
                    .Where(stu => condicion ? stu.Activo == bStatus : stu.Activo == stu.Activo)
                    .OrderBy(stu => stu.Marca)
                    .ThenBy(stu => stu.Modelo)
                    .Select(stu => new ListTextValue { Text = stu.Marca + " " + stu.Modelo, Value = stu.id_unidad, Activo = stu.Activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllUnidadesAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var unidades = await db.Unidades
                        .Where(stu => condicion ? stu.Activo == bStatus : stu.Activo == stu.Activo)
                        .OrderBy(u => u.Marca)
                        .ThenBy(u => u.Modelo)
                        .Select(u => new {
                            u.id_unidad,
                            u.subtipo_unidad,
                            subtipo_unidad_descripcion = u.SubTipoUnidad.descripcion,
                            u.tipo_unidad,
                            tipo_unidad_descripcion = u.SubTipoUnidad.TipoUnidad.descripcion,
                            u.Marca,
                            u.Modelo,
                            u.fecha_alta,
                            u.fecha_baja,
                            u.Activo
                        })
                    .ToListAsync();

            if (unidades == null)
            {
                return NotFound();
            }

            return Ok(unidades);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(SubTipoUnidad))]
        public IHttpActionResult GetSubTipoUnidadById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var unidades = db.Unidades
                        .Where(stu => (stu.id_unidad == id) && (condicion ? stu.Activo == bStatus : stu.Activo == stu.Activo))
                        .OrderBy(u => u.Marca)
                        .ThenBy(u => u.Modelo)
                        .Select(u => new {
                            u.id_unidad,
                            u.tipo_unidad,
                            u.subtipo_unidad,
                            u.id_base,
                            u.anio,
                            u.Chasis,
                            u.Observaciones,
                            u.Patente,
                            u.ultimo_service,
                            u.Marca,
                            u.Modelo,
                            u.Fecha_Compra,
                            u.fecha_alta,
                            u.fecha_baja,
                            u.Activo,
                            documentacion = u.Documentacion
                                              .Select(d => new
                                              {
                                                  d.id_documento,
                                                  d.documento,
                                                  d.id_tipo_documento,
                                                  tipo_documento_nombre = d.TipoDocumento.nombre,
                                                  d.fecha_alta
                                              })
                        })
                        .SingleOrDefault();

            if (unidades == null)
            {
                return NotFound();
            }
            return Ok(unidades);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        public IEnumerable GetUnidadesByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Unidades
                        .Where(b => (filter ? b.Marca.Contains(pagingParameterModel.filterWord) || b.Modelo.Contains(pagingParameterModel.filterWord) : b.Marca == b.Marca) && (condicion ? b.Activo == bStatus : b.Activo == b.Activo))
                        .OrderBy(u => u.Marca)
                        .ThenBy(u => u.Modelo)
                        .Select(u => new {
                                            u.id_unidad,
                                            u.subtipo_unidad,
                                            subtipo_unidad_descripcion = u.SubTipoUnidad.descripcion,
                                            u.tipo_unidad,
                                            tipo_unidad_descripcion = u.SubTipoUnidad.TipoUnidad.descripcion,
                                            u.Marca,
                                            u.Modelo,
                                            u.fecha_alta,
                                            u.fecha_baja,
                                            u.Activo
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
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUnidades(int id, Unidades unidades)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != unidades.id_unidad)
            {
                return BadRequest();
            }


            Unidades unidadActual = db.Unidades
                                      .Include("Documentacion")
                                      .AsQueryable()
                                      .Where(s => s.id_unidad == unidades.id_unidad)
                                      .FirstOrDefault<Unidades>();

            db.Entry(unidadActual).CurrentValues.SetValues(unidades);


            var documentosEliminados   = unidadActual
                                            .Documentacion
                                            .Where(q => !unidades.Documentacion.Select(doc => doc.id_documento)
                                            .Contains(q.id_documento))
                                            .ToList<Documentacion>();

            var documentosNuevos = unidades.Documentacion
                                            .Where(q => !unidadActual.Documentacion.Select(doc => doc.id_documento)
                                            .Contains(q.id_documento))
                                            .ToList<Documentacion>();

            documentosEliminados.ForEach(c => unidadActual.Documentacion.Remove(c));

            foreach (Documentacion c in documentosNuevos)
            {
                if (db.Entry(c).State == EntityState.Detached)
                    db.Documentacion.Attach(c);

                unidadActual.Documentacion.Add(c);
            }          

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UnidadesExists(id))
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
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        [ResponseType(typeof(Unidades))]
        public IHttpActionResult PostUnidades(Unidades unidades)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Unidades.Add(unidades);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Unidades", id = unidades.id_unidad }, unidades);
        }

        [HttpPost]
        [Route("UploadDocumento")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        public IHttpActionResult UploadDocumentacion()
        {
            int i = 0;            
            string result = string.Empty;

            int totalDocumentosIncluidos = 0;
            int idUnidadActual = 0;
            int idTipoDocumentoActual = 0;
            string nombreArchivoActual = string.Empty;

            Documentacion documento = new Documentacion();
            Unidades unidad = new Unidades();

            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;


            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var documentoRecibido = httpRequest.Files[i];

                    var nombreRutaArchivo = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documentoRecibido.FileName);

                    try
                    {
                        documentoRecibido.SaveAs(nombreRutaArchivo);
                        nombreArchivoActual = httpRequest.Files[i].FileName;

                        int.TryParse(nombreArchivoActual.Substring(0, 10), out idUnidadActual);
                        int.TryParse(nombreArchivoActual.Substring(10, 10), out idTipoDocumentoActual);

                        unidad = db.Unidades.Find(idUnidadActual);

                        documento.id_documento = 0;
                        documento.id_tipo_documento = idTipoDocumentoActual; 
                        documento.documento = nombreArchivoActual;
                        documento.fecha_alta = DateTime.Now;

                        unidad.Documentacion.Add(documento);
                        db.Entry(unidad).State = EntityState.Modified;
                        db.SaveChanges();

                        totalDocumentosIncluidos++;

                    }

                    catch (Exception ex)
                    {
                        if (File.Exists(nombreRutaArchivo)){
                            File.Delete(nombreRutaArchivo);
                        }
                        return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
                    }
                    i++;
                }
            }

            if (totalDocumentosIncluidos <= 0)
            {
                return NotFound();
            }
            else
            {
                var unidades = db.Unidades
                                 .Where(u => u.id_unidad == idUnidadActual)
                                 .Select(u => new
                                 {
                                     u.id_unidad,
                                     u.tipo_unidad,
                                     u.subtipo_unidad,
                                     u.id_base,
                                     u.anio,
                                     u.Chasis,
                                     u.Observaciones,
                                     u.Patente,
                                     u.ultimo_service,
                                     u.Marca,
                                     u.Modelo,
                                     u.Fecha_Compra,
                                     u.fecha_alta,
                                     u.fecha_baja,
                                     u.Activo,
                                     documentacion = u.Documentacion
                                                      .Select(d => new
                                                      {
                                                          d.id_documento,
                                                          d.documento,
                                                          d.id_tipo_documento,
                                                          tipo_documento_nombre = d.TipoDocumento.nombre,
                                                          d.fecha_alta
                                                      })
                                 })
                                 .SingleOrDefault();

                if (unidades == null)
                {
                    return NotFound();
                }

                return Ok(unidades);
            }
            
        }

        [HttpGet]
        [Route("DowloadDocumentoStream/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        public IHttpActionResult DowloadDocumentacionStream(int id)
        {

            Documentacion documento = db.Documentacion
                                        .Include("TipoDocumento")
                                        .Where(d => d.id_documento == id)
                                        .SingleOrDefault();

            var nombreRutaDocumento = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documento.documento + ".pdf");
            var nombreDocumentoUsuario = documento.TipoDocumento.nombre
                                                    .ToLowerInvariant().Replace(" ","")  + '_' 
                                                    + documento.fecha_alta.Year
                                                    + documento.fecha_alta.Month.ToString().PadLeft(2,'0')
                                                    + documento.fecha_alta.Day.ToString().PadLeft(2, '0') + ".pdf";

            var documentoBytes = File.ReadAllBytes(nombreRutaDocumento);
            var documentoStream = new MemoryStream(documentoBytes);
            return new DocumentoDownloadResult(documentoStream, Request, nombreDocumentoUsuario);
        }

        //TODO: ajustar el content type dependiendo del tipo de archivo, validar tipos de archivos permitidos por tipo de documenttacion
        [HttpGet]
        [Route("DownloadDocumento/{id}")]
        public HttpResponseMessage DowloadDocumentacion(int id)
        {

            Documentacion documento = db.Documentacion
                                        .Include("TipoDocumento")
                                        .Where(d => d.id_documento == id)
                                        .SingleOrDefault();

            var nombreRutaDocumento = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documento.documento );

            var nombreDocumentoUsuario = documento.TipoDocumento.nombre
                                                    .ToLowerInvariant().Replace(" ", "") + '_'
                                                    + documento.fecha_alta.Year
                                                    + documento.fecha_alta.Month.ToString().PadLeft(2, '0')
                                                    + documento.fecha_alta.Day.ToString().PadLeft(2, '0');

            var documentoBytes = File.ReadAllBytes(nombreRutaDocumento);
            var documentoStream = new MemoryStream(documentoBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(documentoStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = nombreDocumentoUsuario;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        [ResponseType(typeof(Unidades))]
        public IHttpActionResult DeleteUnidades(int id)
        {
            Unidades unidad= db.Unidades
                                  .Include(x => x.Documentacion)
                                  .Where(u => u.id_unidad == id)
                                  .SingleOrDefault();
            if (unidad == null)
            {
                return NotFound();
            }
            try
            {
                foreach (Documentacion documento in unidad.Documentacion.ToList())
                {
                    var nombreRutaDocumento = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documento.documento);
                    File.Delete(nombreRutaDocumento);
                    db.Documentacion.Remove(documento);
                }
                db.Unidades.Remove(unidad);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok(unidad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UnidadesExists(int id)
        {
            return db.Unidades.Count(e => e.id_unidad == id) > 0;
        }

    }
}


//public void Update(UpdateParentModel model)
//{
//    var existingParent = _dbContext.Parents
//        .Where(p => p.Id == model.Id)
//        .Include(p => p.Children)
//        .SingleOrDefault();

//    if (existingParent != null)
//    {
//        // Update parent
//        _dbContext.Entry(existingParent).CurrentValues.SetValues(model);

//        // Delete children
//        foreach (var existingChild in existingParent.Children.ToList())
//        {
//            if (!model.Children.Any(c => c.Id == existingChild.Id))
//                _dbContext.Children.Remove(existingChild);
//        }

//        // Update and Insert children
//        foreach (var childModel in model.Children)
//        {
//            var existingChild = existingParent.Children
//                .Where(c => c.Id == childModel.Id)
//                .SingleOrDefault();

//            if (existingChild != null)
//                // Update child
//                _dbContext.Entry(existingChild).CurrentValues.SetValues(childModel);
//            else
//            {
//                // Insert child
//                var newChild = new Child
//                {
//                    Data = childModel.Data,
//                    //...
//                };
//                existingParent.Children.Add(newChild);
//            }
//        }

//        _dbContext.SaveChanges();
//    }
//}