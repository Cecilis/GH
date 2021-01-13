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
using GH.ViewModels;
using System.Collections;
using System.Web;
using Newtonsoft.Json;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Categorias")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class CategoriasController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Categorias> GetAllCategorias(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Categorias
                     .OrderBy(stu => stu.descripcion)
                     .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllCategoriasLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Categorias
                    .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                    .OrderBy(stu => stu.descripcion)
                    .Select(stu => new ListTextValue { Text = stu.descripcion, Value = stu.id_categoria, Activo = stu.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllCategoriasAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var categorias = await db.Categorias
                    .OrderBy(stu => stu.descripcion)
                    .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                    .Select(stu => new
                    {
                        stu.id_categoria,
                        stu.descripcion,
                        stu.activo
                    })
                    .ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(Categorias))]
        public IHttpActionResult GetCategoriasById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var categorias = db.Categorias
                            .Where(stu => (stu.id_categoria == id) && (condicion ? stu.activo == bStatus : stu.activo == stu.activo))
                            .Select(stu => new
                            {
                                stu.id_categoria,
                                stu.descripcion,
                                stu.activo
                            })
                            .SingleOrDefault();

            if (categorias == null)
            {
                return NotFound();
            }
            return Ok(categorias);
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        public IEnumerable GetCategoriasByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.Categorias
                            .Where(stu => condicion ? stu.activo == bStatus : stu.activo == stu.activo)
                            .OrderBy(stu => stu.descripcion)
                            .Select(stu => new
                            {
                                stu.id_categoria,
                                stu.descripcion,
                                stu.activo
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
        public IHttpActionResult PutCategorias(int id, Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorias.id_categoria)
            {
                return BadRequest();
            }

            db.Entry(categorias).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CategoriasExists(id))
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
        [ResponseType(typeof(Categorias))]
        public IHttpActionResult PostCategorias(Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                db.Categorias.Add(categorias);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Categorias", id = categorias.id_categoria }, categorias);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Presupuestador")]
        [ResponseType(typeof(Categorias))]
        public IHttpActionResult DeleteCategorias(int id)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return NotFound();
            }
            try
            {
                db.Categorias.Remove(categorias);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok(categorias);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriasExists(int id)
        {
            return db.Categorias.Count(e => e.id_categoria == id) > 0;
        }
    }
}