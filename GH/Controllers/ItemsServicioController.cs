using GH.Utilities;
using GH.Models;
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
    [RoutePrefix("api/ItemsServicio")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class ItemsServicioController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<ItemsServicio> GetAllItemsServicio(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.ItemsServicio
                     .OrderBy(si => si.descripcion)
                     .Where(si => condicion ? si.activo == bStatus : si.activo == si.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllItemsServicioLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.ItemsServicio
                    .Where(si => condicion ? si.activo == bStatus : si.activo == si.activo)
                    .OrderBy(si => si.descripcion)
                    .Select(si => new ListTextValue { Text = si.descripcion, Value = si.id_item, Activo = si.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllItemsServicioAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var itemsServicios = await db.ItemsServicio
                    .OrderBy(si => si.descripcion)
                    .Where(si => condicion ? si.activo == bStatus : si.activo == si.activo)
                    .Select(si => new
                    {
                        si.id_item,
                        si.id_tipo_servicio,
                        tiposervicio_nombre = si.TipoServicio.nombre,
                        id_servicio = si.TipoServicio.Servicios.id_servicio,
                        servicio_nombre = si.TipoServicio.Servicios.nombre,
                        si.descripcion,
                        si.precio,
                        si.impuesto,
                        si.id_unidad_medida,
                        unidad_medida_descripcion = si.UnidadMedida.descripcion,
                        si.activo
                    })
                    .ToListAsync();

            if (itemsServicios == null)
            {
                return NotFound();
            }

            return Ok(itemsServicios);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(ItemsServicio))]
        public IHttpActionResult GetServicioItemById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var itemsServicios = db.ItemsServicio
                            .Where(si => (si.id_item == id) && (condicion ? si.activo == bStatus : si.activo == si.activo))
                            .Select(si => new
                            {
                                si.id_item,
                                si.id_tipo_servicio,
                                tipo_servicio_nombre = si.TipoServicio.nombre,
                                id_servicio = si.TipoServicio.Servicios.id_servicio,
                                servicio_nombre = si.TipoServicio.Servicios.nombre,
                                si.descripcion,
                                si.precio,
                                si.impuesto,
                                si.id_unidad_medida,
                                unidad_medida_descripcion = si.UnidadMedida.descripcion,
                                si.activo
                            })
                            .SingleOrDefault();

            if (itemsServicios == null)
            {
                return NotFound();
            }
            return Ok(itemsServicios);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        public IEnumerable GetItemsServicioByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.ItemsServicio
                            .Where(b => (filter ? b.descripcion.Contains(pagingParameterModel.filterWord) : b.descripcion == b.descripcion) && (condicion ? b.activo == bStatus : b.activo == b.activo))
                            .OrderBy(si => si.TipoServicio.Servicios.nombre)
                            .ThenBy(si => si.TipoServicio.nombre)
                            .ThenBy(si => si.descripcion)
                            .Select(si => new
                            {
                                si.id_item,
                                si.id_tipo_servicio,
                                tipo_servicio_nombre = si.TipoServicio.nombre,
                                id_servicio = si.TipoServicio.Servicios.id_servicio,
                                servicio_nombre = si.TipoServicio.Servicios.nombre,
                                si.descripcion,
                                si.precio,
                                si.impuesto,
                                si.id_unidad_medida,
                                unidad_medida_descripcion = si.UnidadMedida.descripcion,
                                unidad_medida_abreviatura = si.UnidadMedida.abreviatura,
                                si.activo
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

        [Route("GetByTipoServicio/{id}/{status?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        public IHttpActionResult GetItemsServicioByIdTipoServicio(int id, string status = null)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var itemsServicios = db.ItemsServicio
                                .Include(x => x.UnidadMedida)
                                .Where(si => (si.id_tipo_servicio.Equals(id)) && (condicion ? si.activo == bStatus : si.activo == si.activo))
                                .OrderBy(x => x.descripcion)
                                .Select(si => new
                                {
                                    si.id_item,
                                    si.id_tipo_servicio,
                                    tipo_servicio_nombre = si.TipoServicio.nombre,
                                    id_servicio = si.TipoServicio.Servicios.id_servicio,
                                    servicio_nombre = si.TipoServicio.Servicios.nombre,
                                    si.descripcion,
                                    si.precio,
                                    si.impuesto,
                                    si.id_unidad_medida,
                                    unidad_medida_descripcion = si.UnidadMedida.descripcion,
                                    unidad_medida_abreviatura = si.UnidadMedida.abreviatura,
                                    descripcion_um_abreviatura = si.descripcion + " " + si.UnidadMedida.abreviatura,
                                    si.activo
                                })
                                .ToList();
                                //.Select(si => new ListTextValue { Text = si.descripcion +  " " + si.UnidadMedida.abreviatura, Value = si.id_item, Activo = si.activo })


            if (itemsServicios == null)
            {
                return NotFound();
            }

            return Ok(itemsServicios);
        }

        [HttpPut]
        [Route("Update/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItemsServicio(int id, ItemsServicio servicioItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servicioItems.id_item)
            {
                return BadRequest();
            }

            db.Entry(servicioItems).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ItemsServicioExists(id))
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
        [ResponseType(typeof(ItemsServicio))]
        public IHttpActionResult PostItemsServicio(ItemsServicio servicioItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.ItemsServicio.Add(servicioItems);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "ItemsServicio", id = servicioItems.id_item }, servicioItems);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(ItemsServicio))]
        public IHttpActionResult DeleteItemsServicio(int id)
        {
            ItemsServicio servicioItems = db.ItemsServicio.Find(id);
            if (servicioItems == null)
            {
                return NotFound();
            }

            try
            {
                db.ItemsServicio.Remove(servicioItems);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(servicioItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemsServicioExists(int id)
        {
            return db.ItemsServicio.Count(e => e.id_item == id) > 0;
        }

    }
}
