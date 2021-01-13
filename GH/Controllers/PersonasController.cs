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
    [RoutePrefix("api/Personas")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class PersonasController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Personas> GetAllPersonas(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Personas
                     .OrderBy(p => p.Apellido)
                     .ThenBy(p => p.Nombre)
                     .Where(p => condicion ? p.Activo == bStatus : p.Activo == p.Activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllPersonasLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Personas
                    .Where(p => condicion ? p.Activo == bStatus : p.Activo == p.Activo)
                    .OrderBy(p => p.Apellido)
                    .ThenBy(p => p.Nombre)
                    .Select(p => new ListTextValue { Text = p.Apellido + " " + p.Nombre, Value = p.id_persona, Activo = p.Activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllPersonasAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var personas = await db.Personas
                        .Where(p => condicion ? p.Activo == bStatus : p.Activo == p.Activo)
                        .OrderBy(x => x.Nombre)
                        .ThenBy(x => x.Apellido)
                        .Select(p => new {
                            p.id_persona,
                            p.id_tipo_persona,
                            tipo_persona_descripcion = p.TipoPersona.descripcion,
                            p.id_base,
                            base_nombre = p.Base.nombre,
                            p.id_empleado,
                            empleado_nombre_apellido = p.Empleado.Nombre + " " + p.Empleado.Apellido,
                            p.Legajo,
                            p.DNI,
                            p.Apellido,
                            p.Nombre,
                            p.Mail,
                            p.id_tipo_empleado,
                            tipo_empleado_descripcion = p.Gremio.gremio,
                            p.id_categoria,
                            categoria_descripcion = p.Categoria.descripcion,
                            p.Activo,
                            p.FechaAlta,
                            p.FechaBaja
                        })
                    .ToListAsync();

            if (personas == null)
            {
                return NotFound();
            }

            return Ok(personas);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(Personas))]
        public IHttpActionResult GetPersonaById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var personas = db.Personas
                            .Where(p => (p.id_persona == id) && (condicion ? p.Activo == bStatus : p.Activo == p.Activo))
                            .Select(p => new {
                                p.id_persona,
                                p.id_tipo_persona,
                                tipo_persona_descripcion = p.TipoPersona.descripcion,
                                p.id_base,
                                base_nombre = p.Base.nombre,
                                p.id_empleado,
                                empleado_nombre_apellido = p.Empleado.Nombre + " " + p.Empleado.Apellido,
                                p.Legajo,
                                p.DNI,
                                p.Apellido,
                                p.Nombre,
                                p.Mail,
                                p.id_tipo_empleado,
                                tipo_empleado_descripcion = p.Gremio.gremio,
                                p.id_categoria,
                                categoria_descripcion = p.Categoria.descripcion,
                                p.Activo,
                                p.FechaAlta,
                                p.FechaBaja,
                                documentacion = p.Documentacion
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

            if (personas == null)
            {
                return NotFound();
            }
            return Ok(personas);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        public IEnumerable GetPersonasByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Personas
                        .Where(b => (filter ? b.Apellido.Contains(pagingParameterModel.filterWord) || b.Nombre.Contains(pagingParameterModel.filterWord) : b.Apellido == b.Apellido) && (condicion ? b.Activo == bStatus : b.Activo == b.Activo))
                        .OrderBy(x => x.Nombre)
                        .ThenBy(x => x.Apellido)
                        .Select(p => new {
                            p.id_persona,
                            p.id_tipo_persona,
                            tipo_persona_descripcion = p.TipoPersona.descripcion,
                            p.id_base,
                            base_nombre = p.Base.nombre,
                            p.id_empleado,
                            empleado_nombre_apellido = p.Empleado.Nombre + " " + p.Empleado.Apellido,
                            p.Legajo,
                            p.DNI,
                            p.Apellido,
                            p.Nombre,
                            p.Mail,
                            p.id_tipo_empleado,
                            tipo_empleado_descripcion = p.Gremio.gremio,
                            p.id_categoria,
                            categoria_descripcion = p.Categoria.descripcion,
                            p.Activo,
                            p.FechaAlta,
                            p.FechaBaja,
                            documentacion = p.Documentacion
                                              .Select(d => new
                                              {
                                                  d.id_documento,
                                                  d.documento,
                                                  d.id_tipo_documento,
                                                  tipo_documento_nombre = d.TipoDocumento.nombre,
                                                  d.fecha_alta
                                              })
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
        public IHttpActionResult PutPersonas(int id, Personas persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != persona.id_persona)
            {
                return BadRequest();
            }

            var personaActual = db.Personas.Include("Documentacion")
                                             .Where(s => s.id_persona == persona.id_persona)
                                             .FirstOrDefault<Personas>();

            db.Entry(personaActual).CurrentValues.SetValues(persona);

            var documentosEliminados = personaActual.Documentacion
                                            .Where(q => !persona.Documentacion.Select(doc => doc.id_documento)
                                            .Contains(q.id_documento))
                                            .ToList<Documentacion>();

            var documentosNuevos = persona.Documentacion
                                            .Where(q => !personaActual.Documentacion.Select(doc => doc.id_documento)
                                            .Contains(q.id_documento))
                                            .ToList<Documentacion>();

            documentosEliminados.ForEach(c => personaActual.Documentacion.Remove(c));

            foreach (Documentacion c in documentosNuevos)
            {
                if (db.Entry(c).State == EntityState.Detached)
                    db.Documentacion.Attach(c);

                personaActual.Documentacion.Add(c);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PersonasExists(id))
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
        [ResponseType(typeof(Personas))]
        public IHttpActionResult PostPersonas(Personas personas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Personas.Add(personas);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Personas", id = personas.id_persona }, personas);
        }

        [HttpPost]
        [Route("UploadDocumento")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        [ResponseType(typeof(Personas))]
        public IHttpActionResult UploadDocumentacion()
        {
            int i = 0;
            string result = string.Empty;

            int totalDocumentosIncluidos = 0;
            int idPersonaActual = 0;
            int idTipoDocumentoActual = 0;
            string nombreArchivoActual = string.Empty;

            Documentacion documento = new Documentacion();
            Personas persona = new Personas();

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

                        int.TryParse(nombreArchivoActual.Substring(0, 10), out idPersonaActual);
                        int.TryParse(nombreArchivoActual.Substring(10, 10), out idTipoDocumentoActual);

                        persona = db.Personas.Find(idPersonaActual);

                        documento.id_documento = 0;
                        documento.id_tipo_documento = idTipoDocumentoActual;
                        documento.documento = nombreArchivoActual;
                        documento.fecha_alta = DateTime.Now;

                        persona.Documentacion.Add(documento);
                        db.Entry(persona).State = EntityState.Modified;
                        db.SaveChanges();

                        totalDocumentosIncluidos++;

                    }

                    catch (Exception ex)
                    {
                        if (File.Exists(nombreRutaArchivo))
                        {
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
                var personas = db.Personas
                                      .Where(u => u.id_persona == idPersonaActual)
                                      .Select(p => new {
                                             p.id_persona,
                                             p.id_tipo_persona,
                                             tipo_persona_descripcion = p.TipoPersona.descripcion,
                                             p.id_base,
                                             base_nombre = p.Base.nombre,
                                             p.id_empleado,
                                             empleado_nombre_apellido = p.Empleado.Nombre + " " + p.Empleado.Apellido,
                                             p.Legajo,
                                             p.DNI,
                                             p.Apellido,
                                             p.Nombre,
                                             p.Mail,
                                             p.id_tipo_empleado,
                                             tipo_empleado_descripcion = p.Gremio.gremio,
                                             p.id_categoria,
                                             categoria_descripcion = p.Categoria.descripcion,
                                             p.Activo,
                                             p.FechaAlta,
                                             p.FechaBaja,
                                             documentacion = p.Documentacion
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

                if (personas == null)
                {
                    return NotFound();
                }

                return Ok(personas);
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
                                                    .ToLowerInvariant().Replace(" ", "") + '_'
                                                    + documento.fecha_alta.Year
                                                    + documento.fecha_alta.Month.ToString().PadLeft(2, '0')
                                                    + documento.fecha_alta.Day.ToString().PadLeft(2, '0') + ".pdf";

            var documentoBytes = File.ReadAllBytes(nombreRutaDocumento);
            var documentoStream = new MemoryStream(documentoBytes);
            return new DocumentoDownloadResult(documentoStream, Request, nombreDocumentoUsuario);
        }

        //TODO: ajustar el content type dependiendo del tipo de archivo, validar tipos de archivos permitidos por tipo de documenttacion
        [HttpGet]
        [Route("DownloadDocumento/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador, Presupuestador")]
        public HttpResponseMessage DowloadDocumentacion(int id)
        {
            Documentacion documento = db.Documentacion
                                        .Include("TipoDocumento")
                                        .Where(d => d.id_documento == id)
                                        .SingleOrDefault();

            var nombreRutaDocumento = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documento.documento);

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
        [ResponseType(typeof(Personas))]
        public IHttpActionResult DeletePersonas(int id)
        {


            Personas persona = db.Personas
                                  .Include(x => x.Documentacion)
                                  .Where(u => u.id_persona == id)
                                  .SingleOrDefault();
            if (persona == null)
            {
                return NotFound();
            }
            try
            {
                foreach (Documentacion documento in persona.Documentacion.ToList())
                {
                    var nombreRutaDocumento = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + documento.documento);
                    File.Delete(nombreRutaDocumento);
                    db.Documentacion.Remove(documento);
                }
                db.Personas.Remove(persona);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok(persona);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonasExists(int id)
        {
            return db.Personas.Count(e => e.id_persona == id) > 0;
        }

    }
}