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
using Newtonsoft.Json;
using System.Web;
using System.Web.Http.Results;
using GH.Utilities;
using GH.ViewModels;
using System.Threading.Tasks;

namespace GH.Controllers
{
    [Authorize]
    [RoutePrefix("api/Empleados")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class EmpleadosController : ApiController
    {
        private GHDBContext db = new GHDBContext();

        [HttpGet]
        [Route("GetAll/{status?}")]
        public IQueryable<Empleados> GetAllEmpleados(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Empleados
                     .OrderBy(em => em.Apellido)
                     .ThenBy(em => em.Nombre)
                     .Where(em => condicion ? em.Activo == bStatus : em.Activo == em.Activo)
                     .AsNoTracking();
        }

        [HttpGet]
        [Route("GetAllLstTxtVal/{status?}")]
        public IQueryable<ListTextValue> GetAllEmpleadosLstTxtVal(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Empleados
                    .Where(em => condicion ? em.Activo == bStatus : em.Activo == em.Activo)
                    .OrderBy(em => em.Apellido)
                    .ThenBy(em => em.Nombre)
                    .Select(em => new ListTextValue { Text = em.Apellido + " " + em.Nombre, Value = em.id_empleado, Activo = em.Activo })
                    .AsQueryable();
        }

        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IHttpActionResult> GetAllGremiosAsync(string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var empleados = await db.Empleados
                    .Where(em => condicion ? em.Activo == bStatus : em.Activo == em.Activo)
                    .OrderBy(em => em.Apellido)
                    .ThenBy(em => em.Nombre)
                    .Select(em => new
                    {
                        em.id_empleado,
                        em.Nombre,
                        em.Apellido,
                        em.DNI,
                        em.Legajo,
                        em.Mail,
                        em.id_base,
                        base_nombre = em.Bases.nombre,
                        em.id_categoria,
                        categoria_descripcion = em.Categorias.descripcion,
                        gremio_nombre = em.Gremios.gremio,
                        em.FechaAlta,
                        em.FechaBaja,
                        em.Activo,
                        em.Tipo_Empleado,
                        tipo_empleado_descripcion = em.Gremios.gremio
                    })
                    .ToListAsync();

            if (empleados == null)
            {
                return NotFound();
            }

            return Ok(empleados);
        }

        [HttpGet]
        [Route("GetById/{id?}/{status?}")]
        [ResponseType(typeof(Empleados))]
        public IHttpActionResult GetEmpleadoById(int id, string status = null)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            var empleados = db.Empleados
                            .Where(em => (em.id_empleado == id) && (condicion ? em.Activo == bStatus : em.Activo == em.Activo))
                            .OrderBy(em => em.Apellido)
                            .ThenBy(em => em.Nombre)
                            .Select(em => new
                            {
                                em.id_empleado,
                                em.Nombre,
                                em.Apellido,
                                em.DNI,
                                em.Legajo,
                                em.Mail,
                                em.id_base,
                                base_nombre = em.Bases.nombre,
                                em.id_categoria,
                                categoria_descripcion = em.Categorias.descripcion,
                                gremio_nombre = em.Gremios.gremio,
                                em.FechaAlta,
                                em.FechaBaja,
                                em.Activo,                                
                                em.Tipo_Empleado,
                                tipo_empleado_descripcion = em.Gremios.gremio
                                
                            })
                            .SingleOrDefault();

            if (empleados == null)
            {
                return NotFound();
            }
            return Ok(empleados);
        }
   
        [HttpGet]
        [Route("CountByLegajo/{legajo}/{id?}/{status?}")]
        [ResponseType(typeof(Int16))]
        public int CountEmpleadosByLegajoId([FromUri]long legajo, [FromUri]int id = 0, string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Empleados.Count(e => (e.Legajo == legajo) && (id > 0 ? e.id_empleado != id : e.id_empleado == e.id_empleado) && (condicion ? e.Activo == bStatus : e.Activo == e.Activo));
        }

        [HttpGet]
        [Route("CountByDNI/{dni}/{id?}/{status?}")]
        public int CountEmpleadosByDNIId([FromUri]long dni, [FromUri]int id = 0, string status = null)
        {
            bool bStatus = false;
            bool condicion = Boolean.TryParse(status, out bStatus) ? true : false;

            return db.Empleados.Count(e => (e.DNI == dni) && (id > 0 ? e.id_empleado != id : e.id_empleado == e.id_empleado) && (e.id_empleado != id) && (condicion ? e.Activo == bStatus : e.Activo == e.Activo));
        }

        [HttpGet]
        [Route("GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, RRHH, Supervisor")]
        public IEnumerable GetEmpleadosByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            bool bStatus = false;
            bool condicion = Boolean.TryParse(pagingParameterModel.status, out bStatus) ? true : false;
            bool filter = (!string.IsNullOrEmpty(pagingParameterModel.filterWord)) ? true : false;

            var source = db.Empleados
                            .Where(b => (filter ? b.Nombre.Contains(pagingParameterModel.filterWord) || b.Apellido.Contains(pagingParameterModel.filterWord) : b.Apellido == b.Apellido) && (condicion ? b.Activo == bStatus : b.Activo == b.Activo))
                            .OrderBy(em => em.Apellido)
                            .ThenBy(em => em.Nombre)
                            .Select(em => new
                            {
                                em.id_empleado,
                                em.Nombre,
                                em.Apellido,
                                em.DNI,
                                em.Legajo,
                                em.Mail,
                                em.id_base,
                                base_nombre = em.Bases.nombre,
                                em.id_categoria,
                                categoria_descripcion = em.Categorias.descripcion,
                                gremio_nombre = em.Gremios.gremio,
                                em.FechaAlta,
                                em.FechaBaja,
                                em.Activo,
                                em.Tipo_Empleado,
                                tipo_empleado_descripcion = em.Gremios.gremio
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
        [Authorize(Roles = "Administrador, RRHH, Supervisor")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmpleados(int id, Empleados empleados)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empleados.id_empleado)
            {
                return BadRequest();
            }

            db.Entry(empleados).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmpleadosExists(id))
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
        [Route("LogicDeleteEmpleados/{id?}")]
        [Authorize(Roles = "Administrador, RRHH, Supervisor")]
        [ResponseType(typeof(void))]
        public IHttpActionResult LogicDeleteEmpleados(int id, Empleados empleados)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empleados.id_empleado)
            {
                return BadRequest();
            }

            //eliminacion logica
            this.UpdateActiveDateEmpleado(empleados);
            db.Entry(empleados).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmpleadosExists(id))
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
        [Authorize(Roles = "Administrador, RRHH, Supervisor")]
        [ResponseType(typeof(Empleados))]
        public IHttpActionResult PostEmpleados(Empleados empleados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Empleados.Add(empleados);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return CreatedAtRoute("DefaultApi", new { controller = "Empleados", id = empleados.id_empleado }, empleados);
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        [Authorize(Roles = "Administrador, RRHH, Supervisor")]
        [ResponseType(typeof(Empleados))]
        public IHttpActionResult DeleteEmpleados(int id)
        {
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return NotFound();
            }

            try
            {
                db.Empleados.Remove(empleados);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(empleados);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpleadosExists(int id)
        {
            return db.Empleados.Count(e => e.id_empleado == id) > 0;
        }

        private void UpdateActiveDateEmpleado(Empleados empleados)
        {
            //coloco el estatus activo en falso
            empleados.Activo = false;
            //coloco la fecha de baja la fecha en la cual se elimino
            empleados.FechaBaja = DateTime.Now;
        }

    }
}