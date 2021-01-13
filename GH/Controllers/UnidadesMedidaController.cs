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
    [RoutePrefix("api/UnidadesMedida")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class UnidadesMedidaController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<UnidadMedida> GetAllUnidadesMedida(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.UnidadMedida
                     .OrderBy(um => um.descripcion)
                     .Where(um => condicion ? um.activo == bStatus : um.activo == um.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllUnidadesMedidaLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.UnidadMedida
                    .Where(um => condicion ? um.activo == bStatus : um.activo == um.activo)
                    .OrderBy(um => um.descripcion)
                    .Select(um => new ListTextValue { Text = um.descripcion, Value = um.id_unidad_medida, Activo = um.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllUnidadesMedidaAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var unidadesmedida = await db.UnidadMedida
                    .OrderBy(um => um.descripcion)
                    .Where(um => condicion ? um.activo == bStatus : um.activo == um.activo)
                    .Select(um => new
                    {
                        um.id_unidad_medida,
                        um.descripcion,
                        um.abreviatura,
                        um.activo
                    })
                    .ToListAsync();

            if (unidadesmedida == null)
            {
                return NotFound();
            }

            return Ok(unidadesmedida);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(UnidadMedida))]
        public IHttpActionResult GetUnidadMedidaById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var unidadesmedida = db.UnidadMedida
                            .Where(um => (um.id_unidad_medida == id) && (condicion ? um.activo == bStatus : um.activo == um.activo))
                            .Select(um => new
                            {
                                um.id_unidad_medida,
                                um.descripcion,
                                um.abreviatura,
                                um.activo
                            })
                            .SingleOrDefault();

            if (unidadesmedida == null)
            {
                return NotFound();
            }
            return Ok(unidadesmedida);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetUnidadesMedidaByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.UnidadMedida
                            .Where(um => condicion ? um.activo == bStatus : um.activo == um.activo)
                            .OrderBy(um => um.descripcion)
                            .Select(um => new
                            {
                                um.id_unidad_medida,
                                um.descripcion,
                                um.abreviatura,
                                um.activo
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
        public IHttpActionResult PutUnidadMedida(int id, UnidadMedida unidadMedida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != unidadMedida.id_unidad_medida)
            {
                return BadRequest();
            }

            db.Entry(unidadMedida).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UnidadMedidaExists(id))
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
        [ResponseType(typeof(UnidadMedida))]
        public IHttpActionResult PostUnidadMedida(UnidadMedida unidadMedida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.UnidadMedida.Add(unidadMedida);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "UnidadesMedida", id = unidadMedida.id_unidad_medida }, unidadMedida);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(UnidadMedida))]
        public IHttpActionResult DeleteUnidadMedida(int id)
        {
            UnidadMedida unidadMedida = db.UnidadMedida.Find(id);
            if (unidadMedida == null)
            {
                return NotFound();
            }

            try
            {
                db.UnidadMedida.Remove(unidadMedida);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(unidadMedida);
        }

        [HttpPut]
        [Route("LogicDeleteUnidadMedida/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteUnidadMedida(int id, UnidadMedida unidadMedida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != unidadMedida.id_unidad_medida)
            {
                return BadRequest();
            }

            this.UpdateActiveUnidadMedida(unidadMedida);
            db.Entry(unidadMedida).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UnidadMedidaExists(id))
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

        public void UpdateActiveUnidadMedida(UnidadMedida unidadMedida)
        {
            //coloco el estatus activo en falso
            unidadMedida.activo = false;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UnidadMedidaExists(int id)
        {
            return db.UnidadMedida.Count(e => e.id_unidad_medida == id) > 0;
        }
    }
}