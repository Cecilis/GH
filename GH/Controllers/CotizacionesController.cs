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
    [RoutePrefix("api/Cotizaciones")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class CotizacionesController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        #region Solicitudes
        [HttpGet]
        [Route("GetAll")]
        public IQueryable<Cotizacion> GetCotizacion()
        {
            return db.Cotizacion
                    .OrderByDescending(c => c.anio)
                    .ThenBy(c => c.nro_cotizacion);
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetCotizacionAsync()
        {
            var cotizacion = await db.Cotizacion
                    .OrderByDescending(c => c.anio)
                    .ThenBy(c => c.nro_cotizacion)
                    .ToListAsync();

            if (cotizacion == null)
            {
                return NotFound();
            }

            return Ok(cotizacion);
        }

        [HttpGet]
        [Route("GetById/{id?}")]
        [ResponseType(typeof(Cotizacion))]
        public IHttpActionResult GetCotizacion(int id)
        {
           var cotizacion = db.Cotizacion
                            .Where(c => (c.id_cotizacion == id))
                            .Include(c => c.CotizacionDetalles)
                            .Select(c => new {
                                c.id_cotizacion,
                                c.id_cliente,
                                c.cliente_CUIT,
                                c.cliente_denominacion,
                                cliente_id_provincia = c.Localidad.Provincias.id_provincia,
                                cliente_provincia_nombre = c.Localidad.Provincias.provincia,
                                c.cliente_id_localidad,
                                cliente_localidad_nombre = c.Localidad.localidad,
                                c.cliente_direccion,
                                c.cliente_mail,
                                c.cliente_telefono,
                                c.fecha_alta,
                                c.id_usuario_alta,
                                c.observacion_alta,
                                c.estatus,
                                c.anio,
                                c.nro_cotizacion,
                                c.revision,
                                c.duracion_oferta,
                                c.vigencia,
                                c.condicion_pago,
                                id_provincia = c.Localidad.Provincias.id_provincia,
                                provincia_nombre = c.Localidad.Provincias.provincia,
                                c.id_localidad,
                                localidad_nombre = c.Localidad.localidad,
                                c.locacion,
                                c.contacto,
                                c.telefono,
                                id_servicio = c.TipoServicio.Servicios.id_servicio,
                                c.servicio_descripcion,
                                c.id_tipo_servicio,
                                c.tipo_servicio_descripcion,
                                c.estatus_evaluacion,
                                c.observacion_evaluacion,
                                c.fecha_evaluacion,
                                c.id_usuario_evaluacion,
                                c.estatus_aprobacion,
                                c.observacion_aprobacion,
                                c.fecha_aprobacion,
                                c.id_usuario_aprobacion,
                                c.total_impuesto,
                                c.subtotal,
                                c.total,
                                cotizaciondetalles1 = c.CotizacionDetalles.ToList(),
                                cotizaciondetalles = c.CotizacionDetalles
                                                      .OrderBy(cd => cd.ItemsServicio.descripcion)
                                                      .ThenBy(cd => cd.UnidadMedida.descripcion)
                                                      .Select(cd => new
                                                      {
                                                          cd.id_cotizacion,
                                                          cd.id_cotizacion_detalle,
                                                          cd.id_item,
                                                          cd.id_unidad_medida,
                                                          cd.impuesto,
                                                          cd.impuesto_item_servicio,
                                                          cd.fecha_alta,
                                                          cd.subtotal,
                                                          cd.total_impuesto,
                                                          cd.total,
                                                          descripcion = cd.ItemsServicio.descripcion,
                                                          unidad_medida = cd.ItemsServicio.UnidadMedida.abreviatura,
                                                          cd.cantidad,
                                                          cd.precio_item_servicio,
                                                          cd.precio
                                                      })
                            })
                            .SingleOrDefault();

            if (cotizacion == null)
            {
                return NotFound();
            }
            return Ok(cotizacion);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        public IEnumerable GetCotizacionByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Cotizacion
                            .OrderByDescending(c => c.anio)
                            .ThenBy(c => c.nro_cotizacion)
                            .Where (c => (c.estatus == "0" || c.estatus == "1")
                             && (filter ? c.cliente_denominacion.Contains(pagingParameterModel.filterWord) || c.cliente_CUIT.ToString().Contains(pagingParameterModel.filterWord) : c.cliente_denominacion == c.cliente_denominacion))

                            .Select(c => new {
                                c.id_cotizacion,
                                c.cliente_denominacion,
                                c.fecha_alta,
                                c.estatus,
                                c.nro_cotizacion,
                                c.anio,                                
                                c.revision,
                                c.duracion_oferta,
                                c.vigencia,
                                c.condicion_pago,
                                locacion_completa = c.Localidad.Provincias.provincia + " - " + c.Localidad.localidad + " - " + c.locacion,
                                c.contacto,                                
                                c.tipo_servicio_descripcion,
                                c.servicio_descripcion,
                                c.estatus_aprobacion,
                                c.total,
                                total_items = c.CotizacionDetalles.Select(cd => cd.id_cotizacion_detalle).Count()
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
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCotizacion(int id, Cotizacion cotizacion)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != cotizacion.id_cotizacion)
            {
                return BadRequest();
            }

            var cotizacionActual = db.Cotizacion
                .Where(c => c.id_cotizacion == cotizacion.id_cotizacion)
                .Include(c => c.CotizacionDetalles)
                .SingleOrDefault();

            if (cotizacionActual != null)
            {

                db.Entry(cotizacionActual).CurrentValues.SetValues(cotizacion);

                if ((cotizacion.CotizacionDetalles != null) && (cotizacion.CotizacionDetalles.Count > 0))
                {
                    foreach (var detalleActual in cotizacionActual.CotizacionDetalles.ToList())
                    {
                        if (!cotizacion.CotizacionDetalles.Any(c => c.id_cotizacion == detalleActual.id_cotizacion))
                            db.CotizacionDetalles.Remove(detalleActual);
                    }

                    foreach (var detalle in cotizacion.CotizacionDetalles)
                    {
                        var detalleActual = cotizacionActual.CotizacionDetalles
                            .Where(c => c.id_cotizacion_detalle == detalle.id_cotizacion_detalle)
                            .SingleOrDefault();

                        if (detalleActual != null)
                            db.Entry(detalleActual).CurrentValues.SetValues(detalle);
                        else
                        {
                            // Insert detalle
                            var item = new CotizacionDetalles
                            {
                                id_cotizacion_detalle = detalle.id_cotizacion_detalle,
                                id_cotizacion = detalle.id_cotizacion,
                                id_item = detalle.id_item,
                                id_unidad_medida = detalle.id_unidad_medida,
                                cantidad = detalle.cantidad,
                                precio_item_servicio = detalle.precio_item_servicio,
                                impuesto_item_servicio = detalle.impuesto_item_servicio,
                                precio = detalle.precio,
                                impuesto = detalle.impuesto,
                                total_impuesto = detalle.total_impuesto,
                                subtotal = detalle.subtotal,
                                total = detalle.total
                            };
                            cotizacionActual.CotizacionDetalles.Add(item);
                        }
                    }
                }
                else
                {
                    db.CotizacionDetalles.RemoveRange(cotizacionActual.CotizacionDetalles);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CotizacionExists(id))
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
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(Cotizacion))]
        public IHttpActionResult PostCotizacion(Cotizacion cotizacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            try
            {
                cotizacion.fecha_alta = DateTime.Now;
                db.Cotizacion.Add(cotizacion);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Cotizaciones", id = cotizacion.id_cotizacion }, cotizacion);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(Cotizacion))]
        public IHttpActionResult DeleteCotizacion(int id)
        {
            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            try
            {
                db.Cotizacion.Remove(cotizacion);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(cotizacion);
        }


        #endregion|

        #region Evaluaciones

        [HttpGet]
        [Route("Evaluacion/GetById/{id?}")]
        [Authorize(Roles = "Administrador, Evaluador")]
        [ResponseType(typeof(Cotizacion))]
        public IHttpActionResult GetCotizacionPorEvaluar(int id)
        {
            var cotizacion = db.Cotizacion
                            .Where(c => (c.id_cotizacion == id) && (c.estatus == "1" || c.estatus == "2"))
                            .Include(c => c.CotizacionDetalles)
                            .Select(c => new {
                                c.id_cotizacion,
                                c.id_cliente,
                                c.cliente_CUIT,
                                c.cliente_denominacion,
                                cliente_id_provincia = c.Localidad.Provincias.id_provincia,
                                cliente_provincia = c.Localidad.Provincias.provincia,
                                c.cliente_id_localidad,
                                cliente_localidad = c.Localidad.localidad,
                                c.cliente_direccion,
                                c.cliente_mail,
                                c.cliente_telefono,
                                c.fecha_alta,
                                c.id_usuario_alta,
                                c.observacion_alta,
                                c.estatus,
                                c.anio,
                                c.nro_cotizacion,
                                c.revision,
                                c.duracion_oferta,
                                c.vigencia,
                                c.condicion_pago,
                                id_provincia = c.Localidad.Provincias.id_provincia,
                                provincia = c.Localidad.Provincias.provincia,
                                c.id_localidad,
                                localidad = c.Localidad.localidad,
                                c.locacion,
                                c.contacto,
                                c.telefono,
                                id_servicio = c.TipoServicio.Servicios.id_servicio,
                                c.servicio_descripcion,
                                c.id_tipo_servicio,
                                c.tipo_servicio_descripcion,
                                c.estatus_evaluacion,
                                c.observacion_evaluacion,
                                c.fecha_evaluacion,
                                c.id_usuario_evaluacion,
                                c.estatus_aprobacion,
                                c.observacion_aprobacion,
                                c.fecha_aprobacion,
                                c.id_usuario_aprobacion,
                                c.total_impuesto,
                                c.subtotal,
                                c.total,
                                cotizaciondetalles = c.CotizacionDetalles
                                                      .OrderBy(cd => cd.ItemsServicio.descripcion)
                                                      .ThenBy(cd => cd.UnidadMedida.descripcion)
                                                      .Select(cd => new
                                                      {
                                                          descripcion = cd.ItemsServicio.descripcion,
                                                          unidad_medida = cd.ItemsServicio.UnidadMedida.abreviatura,
                                                          cd.cantidad,
                                                          cd.precio_item_servicio,
                                                          cd.precio
                                                      })
                            })
                            .SingleOrDefault();

            if (cotizacion == null)
            {
                return NotFound();
            }
            return Ok(cotizacion);
        }

        [HttpGet]
        [Route("Evaluacion/Count")]
        public int CountEvaluaciones()
        {
            return db.Cotizacion.Count(c => (c.estatus == "1"));
        }

        [HttpGet]
        [Route("Evaluacion/GetByPages/{pageNumber?}/{pageSize?}")]
        [Authorize(Roles = "Administrador, Evaluador")]
        public IEnumerable GetCotizacionPorEvaluarByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            //EnEdicion = '0',
            //Creada = '1',
            //EnEvaluacion = '2',
            //PendientePorAprobacion = '3',
            //ContratoGenerado = '4'

            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Cotizacion
                            .OrderByDescending(c => c.anio)
                            .ThenBy(c => c.nro_cotizacion)
                            .Where(c => (c.estatus == "1" || c.estatus == "2")
                               && (filter ? c.cliente_denominacion.Contains(pagingParameterModel.filterWord) || c.cliente_CUIT.ToString().Contains(pagingParameterModel.filterWord) : c.cliente_denominacion == c.cliente_denominacion))
                            .Select(c => new {
                                c.id_cotizacion,
                                c.cliente_denominacion,
                                c.fecha_alta,
                                c.estatus,
                                c.estatus_evaluacion,
                                estatus_evaluacion_descripcion = c.estatus == "1" ? "Pendiente" : c.estatus_evaluacion == true ? "Aprobada" : "Negada",
                                c.nro_cotizacion,
                                c.anio,
                                c.revision,
                                c.duracion_oferta,
                                c.vigencia,
                                c.condicion_pago,
                                locacion_completa = c.Localidad.Provincias.provincia + " - " + c.Localidad.Provincias.provincia + " - " + c.locacion,
                                c.contacto,
                                c.tipo_servicio_descripcion,
                                c.servicio_descripcion,
                                c.estatus_aprobacion,
                                c.total,
                                total_items = c.CotizacionDetalles.Select(cd => cd.id_cotizacion_detalle).Count()
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
        [Route("Evaluacion/Update/{id?}")]
        [Authorize(Roles = "Administrador, Evaluador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCotizacionEvaluacion(int id, CotizacionEvaluacionModel evaluacion)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != evaluacion.id_cotizacion)
            {
                return BadRequest();
            }


            var cotizacionActual = db.Cotizacion
                .Where(c => c.id_cotizacion == evaluacion.id_cotizacion)
                .Include(c => c.CotizacionDetalles)
                .SingleOrDefault();

            evaluacion.fecha_evaluacion = DateTime.Now;
            evaluacion.estatus = "2";

            db.Entry(cotizacionActual).CurrentValues.SetValues(evaluacion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CotizacionExists(id))
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
        [Route("Evaluacion/Bulk/Update/{id?}")]
        [Authorize(Roles = "Administrador, Evaluador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCotizacionEvaluacionBulk(List<CotizacionEvaluacionModel> lstEvaluacionesCotizacion)
        {
            if (lstEvaluacionesCotizacion.Count == 0)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            foreach(var evaluacion in lstEvaluacionesCotizacion)
            {
                var cotizacionActual = db.Cotizacion
                    .Where(c => c.id_cotizacion == evaluacion.id_cotizacion)
                    .SingleOrDefault();

                evaluacion.fecha_evaluacion = DateTime.Now;
                evaluacion.estatus = "2";
                db.Entry(cotizacionActual).CurrentValues.SetValues(evaluacion);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        #endregion

        #region Aprobaciones

        [HttpGet]
        [Route("Aprobacion/GetById/{id?}")]
        [ResponseType(typeof(Cotizacion))]
        public IHttpActionResult GetCotizacionPorAprobacion(int id)
        {
            var cotizacion = db.Cotizacion
                            .Where(c => (c.id_cotizacion == id) && (c.estatus == "2" || c.estatus == "3"))
                            .Include(c => c.CotizacionDetalles)
                            .Select(c => new {
                                c.id_cotizacion,
                                c.id_cliente,
                                c.cliente_CUIT,
                                c.cliente_denominacion,
                                cliente_id_provincia = c.Localidad.Provincias.id_provincia,
                                cliente_provincia = c.Localidad.Provincias.provincia,
                                c.cliente_id_localidad,
                                cliente_localidad = c.Localidad.localidad,
                                c.cliente_direccion,
                                c.cliente_mail,
                                c.cliente_telefono,
                                c.fecha_alta,
                                c.id_usuario_alta,
                                c.observacion_alta,
                                c.estatus,
                                c.anio,
                                c.nro_cotizacion,
                                c.revision,
                                c.duracion_oferta,
                                c.vigencia,
                                c.condicion_pago,
                                id_provincia = c.Localidad.Provincias.id_provincia,
                                provincia = c.Localidad.Provincias.provincia,
                                c.id_localidad,
                                localidad = c.Localidad.localidad,
                                c.locacion,
                                c.contacto,
                                c.telefono,
                                id_servicio = c.TipoServicio.Servicios.id_servicio,
                                c.servicio_descripcion,
                                c.id_tipo_servicio,
                                c.tipo_servicio_descripcion,
                                c.estatus_evaluacion,
                                c.observacion_evaluacion,
                                c.fecha_evaluacion,
                                c.id_usuario_evaluacion,
                                c.estatus_aprobacion,
                                c.observacion_aprobacion,
                                c.fecha_aprobacion,
                                c.id_usuario_aprobacion,
                                c.total_impuesto,
                                c.subtotal,
                                c.total,
                                cotizaciondetalles = c.CotizacionDetalles
                                                      .OrderBy(cd => cd.ItemsServicio.descripcion)
                                                      .ThenBy(cd => cd.UnidadMedida.descripcion)
                                                      .Select( cd => new
                                                      {
                                                          descripcion = cd.ItemsServicio.descripcion,
                                                          unidad_medida = cd.ItemsServicio.UnidadMedida.abreviatura,
                                                          cd.cantidad,
                                                          cd.precio_item_servicio,
                                                          cd.precio
                                                      })
                            })
                            .SingleOrDefault();

            if (cotizacion == null)
            {
                return NotFound();
            }
            return Ok(cotizacion);
        }

        [HttpGet]
        [Route("Aprobacion/GetByPages/{pageNumber?}/{pageSize?}")]
        [Authorize(Roles = "Administrador, Aprobador")]
        public IEnumerable GetCotizacionPorAprobarByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Cotizacion
                            .OrderByDescending(c => c.anio)
                            .ThenBy(c => c.nro_cotizacion)
                            .Where(c => (c.estatus == "2" || c.estatus == "3")
                            && (filter ? c.cliente_denominacion.Contains(pagingParameterModel.filterWord) || c.cliente_CUIT.ToString().Contains(pagingParameterModel.filterWord) : c.cliente_denominacion == c.cliente_denominacion))

                            .Select(c => new {
                                c.id_cotizacion,
                                c.cliente_denominacion,
                                c.fecha_alta,
                                c.estatus,
                                c.estatus_aprobacion,
                                estatus_aprobacion_descripcion = c.estatus == "2" ? "Pendiente" : c.estatus_aprobacion == true ? "Aprobada" : "Negada",
                                c.nro_cotizacion,
                                c.anio,
                                c.revision,
                                c.duracion_oferta,
                                c.vigencia,
                                c.condicion_pago,
                                locacion_completa = c.Localidad.Provincias.provincia + " - " + c.Localidad.Provincias.provincia + " - " + c.locacion,
                                c.contacto,
                                c.tipo_servicio_descripcion,
                                c.servicio_descripcion,
                                c.total,
                                total_items = c.CotizacionDetalles.Select(cd => cd.id_cotizacion_detalle).Count()
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

        [HttpGet]
        [Route("Aprobacion/Count")]
        public int CountAprobacion()
        {
            return db.Cotizacion.Count(c => (c.estatus == "2"));
        }

        [HttpPut]
        [Route("Aprobacion/Update/{id?}")]
        [Authorize(Roles = "Administrador, Aprobador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCotizacionPorAprobar(int id, CotizacionAprobacionModel aprobacion)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != aprobacion.id_cotizacion)
            {
                return BadRequest();
            }

            var cotizacionActual = db.Cotizacion
                .Where(c => c.id_cotizacion == aprobacion.id_cotizacion)
                .Include(c => c.CotizacionDetalles)
                .SingleOrDefault();

            aprobacion.fecha_aprobacion = DateTime.Now;
            aprobacion.estatus = "3";
            db.Entry(cotizacionActual).CurrentValues.SetValues(aprobacion);

            try
            {
                db.SaveChanges();

                var estatus = db.Cotizacion
                                .Where(c =>
                                           c.id_cotizacion == aprobacion.id_cotizacion &&
                                           c.estatus == "3" &&
                                           c.estatus_aprobacion == true
                                        )
                                .Select(c => new CotizacionEstatusModel
                                {
                                    id_cotizacion = c.id_cotizacion,
                                    estatus = c.estatus,
                                    estatus_aprobacion = c.estatus_aprobacion.Value ? true : false
                                })
                                .SingleOrDefault();

                if (estatus != null)
                {
                    return Ok(estatus);
                }                                       

            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CotizacionExists(id))
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
        [Route("Aprobacion/Bulk/Update/{id?}")]
        [Authorize(Roles = "Administrador, Aprobador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCotizacionPorAprobarBulk(List<CotizacionAprobacionModel> lstaprobacionesCotizacion)
        {
            if (lstaprobacionesCotizacion.Count == 0)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //using (GHDBContext db = new GHDBContext())
            //{
            //    try
            //    {
            //      //disable detection of changes to improve performance
            //      db.Configuration.AutoDetectChangesEnabled = false;

            //      //for all the entities to update...
            //      MyObjectEntity entityToUpdate = new MyObjectEntity() {Id=123, Quantity=100};
            //      db.MyObjectEntity.Attach(entityToUpdate);

            //      //then perform the update
            //      db.SaveChanges();
            //    }
            //    finally
            //    {
            //      //re-enable detection of changes
            //      db.Configuration.AutoDetectChangesEnabled = true;
            //    }
            //}

            foreach (var aprobacion in lstaprobacionesCotizacion)
            {
                var cotizacionActual = db.Cotizacion
                    .Where(c => c.id_cotizacion == aprobacion.id_cotizacion)
                    .SingleOrDefault();

                aprobacion.fecha_aprobacion = DateTime.Now;

                db.Entry(cotizacionActual).CurrentValues.SetValues(aprobacion);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        #endregion

        protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

        private bool CotizacionExists(int id)
            {
                return db.Cotizacion.Count(e => e.id_cotizacion == id) > 0;
            }

    }
}