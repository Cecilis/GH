using GH.Models;
using GH.Utilities;
using GH.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
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
    [RoutePrefix("api/Bases")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class BasesController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Bases> GetAllBases(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Bases
                     .OrderBy(x => x.nombre)
                     .Where(b => condicion ? b.activo == bStatus : b.activo == b.activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllBasesLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Bases
                    .Where(b => condicion ? b.activo == bStatus : b.activo == b.activo)
                    .OrderBy(b => b.nombre)
                    .Select(b => new ListTextValue { Text = b.nombre, Value = b.id_base, Activo = b.activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllBasesAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var bases = await db.Bases
                    .OrderBy(b => b.nombre)
                    .Where(b => condicion ? b.activo == bStatus : b.activo == b.activo)
                    .Select(b => new
                    {
                        b.id_base,
                        b.nombre,
                        b.fecha_alta,
                        b.fecha_baja,
                        b.observaciones,
                        b.activo
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
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Bases))]
        public IHttpActionResult GetBaseById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;
            
            var bases = db.Bases
                            .Where(b => (b.id_base == id) && (condicion ? b.activo == bStatus : b.activo == b.activo))
                            .Select(b => new 
                            {
                                b.id_base,
                                b.nombre,
                                b.fecha_alta,
                                b.fecha_baja,
                                b.observaciones,
                                b.activo
                            })
                            .SingleOrDefault();

            if (bases == null)
            {
                return NotFound();
            }
            return Ok(bases);
        }

        [HttpGet]
        [Route("GetByPagesOld/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetBasesByPagesOld([FromUri]PagingParameterModel pagingParameterModel)
        {
                        
            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;

            var source = db.Bases
                            .Where(b => condicion ? b.activo == bStatus : b.activo == b.activo)
                            .OrderBy(b => b.nombre)
                            .ToList()
                            .Select(b => new
                            {
                                b.id_base,
                                b.nombre,
                                b.fecha_alta,
                                b.fecha_baja,
                                b.observaciones,
                                b.activo
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
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{filterWord?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetBasesByPages([FromUri]PagingParameterModel pagingParameterModel)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Bases
                            .Where(b => (filter ? b.nombre.Contains(pagingParameterModel.filterWord) : b.nombre == b.nombre) && (condicion ? b.activo == bStatus : b.activo == b.activo))
                            .OrderBy(b => b.nombre)
                            .ToList()
                            .Select(b => new
                            {
                                b.id_base,
                                b.nombre,
                                b.fecha_alta,
                                b.fecha_baja,
                                b.observaciones,
                                b.activo
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
        public IHttpActionResult PutBases(int id, Bases bases)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bases.id_base)
            {
                return BadRequest();
            }

            bool status_activo_anterior = db.Bases.Where(b => b.id_base == bases.id_base).Select(b => b.activo).SingleOrDefault();

            if (!bases.activo == status_activo_anterior)
                if (!bases.activo)
                    bases.fecha_baja = DateTime.Now;
                else
                    bases.fecha_baja = null; 

            db.Entry(bases).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BasesExists(id))
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
        [ResponseType(typeof(Bases))]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IHttpActionResult PostBases(Bases bases)
        {
            bases.fecha_alta = DateTime.Now;
            bases.fecha_baja = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Bases.Add(bases);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { controller = "Bases", id = bases.id_base }, bases);

            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
        }

        [HttpPut]
        [Route("LogicDeleteBases/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteBases(int id, Bases bases)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bases.id_base)
            {
                return BadRequest();
            }
            this.UpdateActiveDateBase(bases);
            db.Entry(bases).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BasesExists(id))
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



        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(Bases))]
        public IHttpActionResult DeleteBases(int id)
        {
            Bases bases = db.Bases.Find(id);
            if (bases == null)
            {
                return NotFound();
            }

            try
            {
                db.Bases.Remove(bases);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(bases);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BasesExists(int id)
        {
            return db.Bases.Count(e => e.id_base == id) > 0;
        }

        private void UpdateActiveDateBase(Bases bases)
        {
            //coloco el estatus activo en falso
            bases.activo = false;
            //coloco la fecha de baja la fecha en la cual se elimino
            bases.fecha_baja = DateTime.Now;
        }

    }
}