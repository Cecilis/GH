using GH.Utilities;
using GH.Models;
using GH.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Security;

namespace GH.Controllers
{
    
    [RoutePrefix("api/Account")]
    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class AccountController : ApiController
    {
        private GHDBContext db = new GHDBContext();
        public IMembershipService MembershipService { get; set; }

        //string actionName = ControllerContext.RouteData.Values["action"].ToString();
        //string controllerName = ControllerContext.RouteData.Values["controller"].ToString();

        public AccountController()
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
        }

        // POST api/Account/Logout
        [Route("Logout")]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult Logout()
        {
            //TODO: Asegurar el signout del usuario actual 
            //Authentication.SignOut();
            return Ok();
        }

        // POST api/Account/ChangePassword
        [HttpPut]
        [Route("User/ChangePassword/{username}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult ChangePassword(string username, ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!(MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword)))
            {
                return Content(HttpStatusCode.Accepted, "Contraseña actual es incorrecta o la nueva contraseña no es válida");
            }

            return Ok();

        }

        [HttpGet]
        [Authorize]
        [Route("GetUserClaims")]
        [ResponseType(typeof(AccountModel))]
        public IHttpActionResult GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            AccountModel model = new AccountModel()
            {
                UserName = identityClaims.FindFirst("Username").Value,
                Email = identityClaims.FindFirst("Email").Value,
                FirstName = identityClaims.FindFirst("FirstName").Value,
                LastName = identityClaims.FindFirst("LastName").Value,
                LoggedOn = identityClaims.FindFirst("LoggedOn").Value
            };

            return Ok(model);
        }

        //Roles
        [HttpGet]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [Route("Role/GetAll")]
        [ResponseType(typeof(List<RoleModel>))]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult GetAllRoles()
        {
            List<RoleModel> roles = new List<RoleModel>();
            try
            {
                foreach (string role in Roles.GetAllRoles())
                    roles.Add(new RoleModel() { RoleName = role });

                if (roles == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok(roles);

        }

        [HttpGet]        
        [Route("Role/GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IEnumerable GetAllRolesByPages([FromUri]PagingParameterModel pagingParameterModel)
        {

            List<RoleModel> roles = new List<RoleModel>();
            foreach (string role in Roles.GetAllRoles())
                roles.Add(new RoleModel() { RoleName = role });

            var source = roles.AsQueryable().OrderBy(x => x.RoleName);

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
        [Route("Role/GetAllByUser/{username}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(List<string>))]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult GetAllByUser(string username)
        {
            List<string> roles = new List<string>();

            if (string.IsNullOrEmpty(username.Trim()))
            {
                return BadRequest();
            }
            try
            {
                foreach (string role in Roles.GetAllRoles())
                    roles.Add(role);

                if (roles == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok(roles);

        }

        [HttpPost]
        [Route("Role/Create")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult CreateRole(RoleModel role)
        {
            if (string.IsNullOrEmpty(role.RoleName.Trim()))
            {
                return BadRequest();
            }

            if (Roles.RoleExists(role.RoleName))
            {
                return Content(HttpStatusCode.Conflict, "Rol ha sido creado anteriormente");
            }

            try
            {
                Roles.CreateRole(role.RoleName);
                if (Array.IndexOf(Roles.GetAllRoles(), role.RoleName) == 0)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok();

        }

        [HttpDelete]
        [Route("Role/Delete/{rolename}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult RemoveRole(string rolename)
        {
            if (string.IsNullOrEmpty(rolename.Trim()))
            {
                return BadRequest();
            }

            if (!Roles.RoleExists(rolename))
            {
                return NotFound();
            }

            try
            {
                Roles.DeleteRole(rolename);
                if (Array.IndexOf(Roles.GetAllRoles(), rolename) == 0)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }
            return Ok();

        }

        //Users

        [HttpGet]
        [Route("User/GetAll")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(List<MemberShipUserModel>))]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult GetAllUsers()
        {
            List<MemberShipUserModel> msUserList = new List<MemberShipUserModel>();
            MemberShipUserModel userModel = new MemberShipUserModel();
            try
            {

                foreach (MembershipUser msUser in Membership.GetAllUsers())
                {
                    userModel.UserName = msUser.UserName;
                    userModel.Comment = msUser.Comment;
                    userModel.Email = msUser.Email;
                    userModel.CreationDate = msUser.CreationDate;
                    userModel.LastActivityDate = msUser.LastActivityDate;
                    userModel.LastLockoutDate = msUser.LastLockoutDate;
                    userModel.LastLoginDate = msUser.LastLoginDate;
                    userModel.LastPasswordChangedDate = msUser.LastPasswordChangedDate;
                    userModel.PasswordQuestion = msUser.PasswordQuestion;
                    userModel.ProviderName = msUser.ProviderName;
                    userModel.ProviderUserKey = msUser.ProviderUserKey;
                    userModel.IsApproved = msUser.IsApproved;
                    userModel.IsLockedOut = msUser.IsLockedOut;
                    userModel.IsOnline = msUser.IsOnline;
                    msUserList.Add(userModel);
                }

                if (msUserList.Count == 0)
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(msUserList);

        }


        [HttpGet]
        [Route("User/GetByPages/{pageNumber?}/{pageSize?}/{status?}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        public IEnumerable GetAllUsersByPages([FromUri]PagingParameterModel pagingParameterModel)
        {
            List<MemberShipUserModel> msUserList = new List<MemberShipUserModel>();
            MemberShipUserModel userModel = new MemberShipUserModel();

            var iii = User.Identity.GetUserName();

            var iiiii = Roles.GetRolesForUser("amber");

            var uu = Membership.GetAllUsers();

            foreach (MembershipUser msUser in Membership.GetAllUsers())
            {
                userModel = new MemberShipUserModel();
                userModel.UserName = msUser.UserName;
                userModel.Comment = msUser.Comment;
                userModel.Email = msUser.Email;
                userModel.CreationDate = msUser.CreationDate;
                userModel.LastActivityDate = msUser.LastActivityDate;
                userModel.LastLockoutDate = msUser.LastLockoutDate;
                userModel.LastLoginDate = msUser.LastLoginDate;
                userModel.LastPasswordChangedDate = msUser.LastPasswordChangedDate;
                userModel.PasswordQuestion = msUser.PasswordQuestion;
                userModel.ProviderName = msUser.ProviderName;
                userModel.ProviderUserKey = msUser.ProviderUserKey;
                userModel.IsApproved = msUser.IsApproved;
                userModel.IsLockedOut = msUser.IsLockedOut;
                userModel.IsOnline = msUser.IsOnline;
                msUserList.Add(userModel);
            }

            var source = msUserList.AsQueryable().OrderBy(x => x.UserName);

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
        [Route("User/GetByName/{username}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(MemberShipUserModel))]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult GetByName(string username)
        {
            MemberShipUserModel userModel = new MemberShipUserModel();
            try
            {
                MembershipUser msUser = Membership.GetUser(username);

                if (msUser == null)
                {
                    return NotFound();
                }

                userModel.UserName = msUser.UserName;
                userModel.Comment = msUser.Comment;
                userModel.Email = msUser.Email;
                userModel.CreationDate = msUser.CreationDate;
                userModel.LastActivityDate = msUser.LastActivityDate;
                userModel.LastLockoutDate = msUser.LastLockoutDate;
                userModel.LastLoginDate = msUser.LastLoginDate;
                userModel.LastPasswordChangedDate = msUser.LastPasswordChangedDate;
                userModel.PasswordQuestion = msUser.PasswordQuestion;
                userModel.IsApproved = msUser.IsApproved;
                userModel.IsLockedOut = msUser.IsLockedOut;
                userModel.IsOnline = msUser.IsOnline;
                userModel.Roles = new List<RoleModel>();

                foreach (string role in Roles.GetRolesForUser(msUser.UserName))
                    userModel.Roles.Add(new RoleModel() { RoleName = role });

            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(userModel);

        }

        [HttpPost]
        [Route("User/Register")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult UserRegister(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model);
                if (!(createStatus == MembershipCreateStatus.Success))
                {
                    return Content(HttpStatusCode.BadRequest, AccountValidation.ErrorCodeToString(createStatus));
                }
                if (!UserUpdatedRoles(model.UserName, model.Roles))
                {
                    return Content(HttpStatusCode.BadRequest, "Error al asignar roles, inetente nuevamente");
                }
                
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok();

        }


        [HttpPut]
        [Route("User/Update/{username}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string username, MemberShipUserModel userToUpdate)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (username != userToUpdate.UserName)
            {
                return NotFound();
            }

            MembershipUser msUser = Membership.GetUser(username);
            try
            {
                msUser.Email = userToUpdate.Email;
                msUser.Comment = userToUpdate.Comment;
                Membership.UpdateUser(msUser);
                UserUpdateRoles(username, userToUpdate.Roles);
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("User/Delete/{username}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult UserDelete(string username)
        {

            if (string.IsNullOrEmpty(username.Trim()))
            {
                return BadRequest();
            }

            var user = Membership.GetUser(username);

            if (user.UserName == null)
            {
                return NotFound();
            }

            try
            {
                Membership.DeleteUser(username);
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return Ok(user);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("User/UpdateRoles/{username}")]
        [Authorize(Roles = "Administrador, Parametrizador")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult UserUpdateRoles(string username, List<RoleModel> roles)
        {
            if (string.IsNullOrEmpty(username.Trim()) || roles.Count == 0)
            {
                return BadRequest();
            }

            var user = Membership.GetUser(username);

            if (user.UserName == null)
            {
                return NotFound();
            }
            try
            {
                var rolesPorUsuario = UserUpdatedRoles(username, roles) ? Roles.GetRolesForUser(username).ToList() : new List<string>();
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(ExceptionsHandlers.getHttpResponseMessage(ex));
            }

            return StatusCode(HttpStatusCode.NoContent);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No hay disponibles errores ModelState para enviar, por lo que simplemente devuelva un BadRequest vacío.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


        public bool UserUpdatedRoles(string username, List<RoleModel> roles)
        {
            bool userRoleUpdated = false;
            try
            {
                var rolesTodos = Roles.GetAllRoles();
                var rolesPorAsignar = roles.Select(x => x.RoleName).ToArray();
                var rolesPorAsignarValidos = rolesTodos.Intersect(rolesPorAsignar);
                var rolesAnteriores = Roles.GetRolesForUser(username);
                var rolesPorAgregar = rolesPorAsignarValidos.Except(rolesAnteriores);
                var rolesPorRemover = rolesAnteriores.Except(rolesPorAsignarValidos);
                var rolesPorUsuario = new List<string>();

                if (rolesPorAgregar.Count() > 0)
                {
                    Roles.AddUsersToRoles(new[] { username }, rolesPorAgregar.ToArray());
                }
                if (rolesPorRemover.Count() > 0)
                {
                    Roles.RemoveUsersFromRoles(new[] { username }, rolesPorRemover.ToArray());
                }
                userRoleUpdated = true;                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return userRoleUpdated;
        }


        //private IEnumerable<string> GetRoles(this HttpRequestContext context)
        //{
        //    var idt = context.Principal.Identity as ClaimsIdentity;
        //    if (idt == null)
        //        return null;
        //    var roles = idt.Claims.Where(x => x.Type == "role")
        //                           .Select(x => x.Value).FirstOrDefault();
        //    return roles;
        //}

        //private string GetUserName(this HttpRequestContext context)
        //{
        //    var idt = context.Principal.Identity as ClaimsIdentity;
        //    if (idt == null)
        //        return string.Empty;
        //    var userName = idt.Claims.Where(x => x.Type == "UserName")
        //                          .Select(x => x.Value).FirstOrDefault();
        //    return userName;
        //}

        #endregion
    }
}

































//namespace GH.Controllers
//{
//    public class AccountController : ApiController
//    {
//        // GET: api/Account
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET: api/Account/5
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST: api/Account
//        public void Post([FromBody]string value)
//        {
//        }

//        // PUT: api/Account/5
//        public void Put(int id, [FromBody]string value)
//        {
//        }

//        // DELETE: api/Account/5
//        public void Delete(int id)
//        {
//        }
//    }
//}
