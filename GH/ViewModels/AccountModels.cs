using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace GH.ViewModels
{

    #region ViewModels

    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "Contraseña y confirmación no coinciden.")]
    public class ChangePasswordModel
    {

        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Contraseña actual")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "Contraseña nueva")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirmación nueva contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }

    public class RoleModel
    {
        [Required]
        [Display(Name = "Rol")]
        public string RoleName { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Contraseña y confirmación no coinciden.")]
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirmación nueva contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Pregunta seguridad")]
        public string PasswordQuestion { get; set; } = "NA";

        [DataType(DataType.Text)]
        [Display(Name = "Respuesta pregunta seguridad")]
        public string PasswordAnswer { get; set; } = "NA";

        [Display(Name = "Roles")]
        public List<RoleModel> Roles { get; set; } = null;
        
    }

    public class MemberShipUserModel
    {
        [Display(Name = "Obseración")]
        public string Comment { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Correo")]
        public string Email { get; set; }
        [Display(Name = "Aprobado")]
        public bool IsApproved { get; set; }
        [Display(Name = "Bloqueado")]
        public bool IsLockedOut { get; set; }
        [Display(Name = "En Línea")]
        public bool IsOnline { get; set; }
        [Display(Name = "Última actividad")]
        public DateTime LastActivityDate { get; set; }
        [Display(Name = "Último bloqueo")]
        public DateTime LastLockoutDate { get; set; }
        [Display(Name = "Último inicio de sessión")]
        public DateTime LastLoginDate { get; set; }
        [Display(Name = "Último cambio de contraseña")]
        public DateTime LastPasswordChangedDate { get; set; }
        [Display(Name = "Pregunta seguridad")]
        public string PasswordQuestion { get; set; }
        public string ProviderName { get; set; }
        public object ProviderUserKey { get; set; }
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        [Display(Name = "Roles")]
        public List<RoleModel> Roles { get; set; } = null;
    }

    public class AccountModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirmación nueva contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required]
        public string LoggedOn { get; set; }
    }
    #endregion

    #region Services

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        MembershipCreateStatus CreateUser(RegisterModel registerModel);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El campo es obligatorio.", "Usuario");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("El campo es obligatorio.", "Contraseña");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El campo es obligatorio.", "Usuario");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("El campo es obligatorio.", "Contraseña");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("El campo es obligatorio.", "e-mail");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public MembershipCreateStatus CreateUser(RegisterModel registerModel)
        {

            if (String.IsNullOrEmpty(registerModel.UserName)) throw new ArgumentException("El campo es obligatorio.", "Usuario");
            if (String.IsNullOrEmpty(registerModel.Password)) throw new ArgumentException("El campo es obligatorio.", "Contraseña");
            if (String.IsNullOrEmpty(registerModel.ConfirmPassword)) throw new ArgumentException("El campo es obligatorio.", "Contraseña confirmación");
            if (String.IsNullOrEmpty(registerModel.Email)) throw new ArgumentException("El campo es obligatorio.", "e-mail");

            MembershipCreateStatus status;
            _provider.CreateUser(registerModel.UserName, registerModel.Password, registerModel.Email, registerModel.PasswordQuestion, registerModel.PasswordAnswer, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El campo es obligatorio.", "Usuario");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("El campo es obligatorio.", "Contraseña anterior");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("El campo es obligatorio.", "Contraseña nueva");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                var cpr = currentUser.ChangePassword(oldPassword, newPassword);
                return cpr;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Usuario ya registrado. Por favor ingrese un nombre de usuario diferente e intente nueva, .";

                case MembershipCreateStatus.DuplicateEmail:
                    return "El usuario ya fue asignado a un correo. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

}
