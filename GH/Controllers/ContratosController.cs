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
    [RoutePrefix("api/Contratos")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class ContratosController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll")]
        public IQueryable<Contrato> GetContratos()
        {
            return db.Contrato
                        .OrderByDescending(c => c.Cotizacion.anio)
                        .ThenBy(c => c.Cotizacion.nro_cotizacion);
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetContratosAsync()
        {
            var contrato = await db.Contrato
                    .OrderByDescending(c => c.Cotizacion.anio)
                    .ThenBy(c => c.Cotizacion.nro_cotizacion)
                    .ToListAsync();

            if (contrato == null)
            {
                return NotFound();
            }

            return Ok(contrato);
        }

        [HttpGet]
        [Route("GetById/{id?}")]
        [ResponseType(typeof(Contrato))]
        public IHttpActionResult GetById(int id)
        {
            var contrato = db.Cotizacion
                  .Include(c => c.Contrato)
                  .Where(c =>
                                c.id_cotizacion == id &&
                                c.estatus == "3" &&
                                c.estatus_aprobacion == true
                         )
                  .Select(s => new CotizacionContratoModel
                  {
                      codigo = "F003",
                      revision = s.revision.ToString(),
                      fecha = s.fecha_aprobacion.ToString(),
                      nro_cotizacion = s.nro_cotizacion.ToString(),
                      contacto = s.contacto,
                      localidad = s.Localidad.localidad + " / " + s.locacion,
                      telefono = s.telefono,
                      tipo_servicio = s.TipoServicio.Servicios.nombre + " - " + s.TipoServicio.nombre,
                      duracion_semanal = "No Aplicable",
                      plazo_servicio = "No Aplicable",
                      descripcion_carga = "No Aplicable",
                      volumen = "No Aplicable",
                      densidad = "No Aplicable",
                      peso = "No Aplicable",
                      lugar_prestacion = "No Aplicable",
                      lugar_descarga = "No Aplicable",
                      distancia_estimada = "No Aplicable",
                      volumen_mensual_estimado = "No Aplicable",
                      tipo_servicio_observacion = "No Aplicable",
                      descripcion = "No Aplicable",
                      nota_items = "",
                      duracion_oferta = "No Aplicable",
                      condicion_pago = s.condicion_pago.ToString() + "días",
                      plazo_entrega = "Una vez adjudicado el servicio o recibida la Orden de Compra, el plazo de entrega a coordinar con el Cliente.",
                      cotizacion_observacion = s.observacion_aprobacion,
                      nota = "Confirmación del Servicio: Mediante Orden de Compra y/o Aceptación  del Servicios, vía fax o e-mail",
                      cotizacion_decision = s.estatus_aprobacion.Value ? true : false,
                      nombre_firma = "Javier Correa",
                      cargo_firma = "Socio Gerente",
                      compañia_firma = "Grupo Horizonte S.R.L"
                  })
                  .SingleOrDefault();

            if (contrato == null)
            {
                return NotFound();
            }
            return Ok(contrato);
        }

        [HttpGet]
        [Route("getPDFInfoByCotizacionId/{id?}")]
        [ResponseType(typeof(Contrato))]
        public IHttpActionResult getPDFInfoByCotizacionId(int id)
        {
            var cotizacion = db.Cotizacion
                            .Where(c => (c.id_cotizacion == id) &&
                                        (c.estatus == "3" || c.estatus_aprobacion == true))
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
        [Route("GetByCotizacionId/{id?}")]
        [ResponseType(typeof(Contrato))]
        public IHttpActionResult GetByCotizacionId(int id)
        {
            var cotizacion = db.Cotizacion
                            .Where(c => (c.id_cotizacion == id) &&
                                        (c.estatus == "3" || c.estatus_aprobacion == true))
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
                                                      }),
                               personasAsignadasIds = c.Contrato.Personas
                                                .Select(p => p.id_persona),
                               unidadesAsignadasIds = c.Contrato.Unidades
                                                .Select(u => u.id_unidad)
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
        [Authorize(Roles = "Administrador, Aprobador")]
        public IEnumerable GetContratosByPages([FromUri]PagingParameterModel pagingParameterModel)
        {
            //EnEdicion = '0',
            //Creada = '1',
            //Evaluada = '2',
            //ConDecisionTomada = '3',
            //ConContratoGenerado = '4'
            //Aprobadas con o sin
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Cotizacion
                            .OrderByDescending(c => c.anio)
                            .ThenBy(c => c.nro_cotizacion)
                            .Where(c => (c.estatus_aprobacion == true) && (c.estatus == "3" || c.estatus == "4") 
                                     && (filter ? c.cliente_denominacion.Contains(pagingParameterModel.filterWord) || c.cliente_CUIT.ToString().Contains(pagingParameterModel.filterWord) : c.cliente_denominacion == c.cliente_denominacion))
                            .Select(c => new
                            {
                                c.id_cotizacion,
                                c.cliente_denominacion,
                                c.fecha_alta,
                                c.estatus,
                                c.estatus_evaluacion,
                                estatus_evaluacion_descripcion = c.estatus == "3" ? "Pendiente" : c.estatus_aprobacion == true ? "Aprobada" : "Negada",
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
        [Route("Update/{id?}")]
        [Authorize(Roles = "Administrador, Aprobador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContratos(int id, Contrato contrato)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contrato.id_contrato)
            {
                return BadRequest();
            }

            var contratoActual = db.Contrato
                .Where(c => c.id_cotizacion == contrato.id_cotizacion)
                .SingleOrDefault();

            if (contratoActual != null)
            {
                db.Entry(contratoActual).CurrentValues.SetValues(contrato);
            }
            else
                return BadRequest();

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ContratoExists(id))
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
        [Authorize(Roles = "Administrador, Aprobador")]
        [ResponseType(typeof(Contrato))]
        public IHttpActionResult PostContratos(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Contrato.Add(contrato);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Contratos", id = contrato.id_cotizacion }, contrato);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Aprobador")]
        [ResponseType(typeof(Contrato))]
        public IHttpActionResult DeleteContratos(int id)
        {
            Contrato contrato = db.Contrato.Find(id);
            if (contrato == null)
            {
                return NotFound();
            }

            try
            {
                db.Contrato.Remove(contrato);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(contrato);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContratoExists(int id)
        {
            return db.Contrato.Count(e => e.id_contrato == id) > 0;
        }

    }
}