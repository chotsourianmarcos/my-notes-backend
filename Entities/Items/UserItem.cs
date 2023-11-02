using Entities.Items;
using Logic.Exceptions;
using Resources.Strings.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class UserItem : GenericItem<UserItem>
    {
        public UserItem() 
        {
            this.IdWeb = Guid.NewGuid();
            this.HashedPassword = UserConstants.NotGeneratedValue;
            this.HashedRefreshToken = UserConstants.NotGeneratedValue;
            this.Notes = new List<NoteItem>();
        }
        [Range(0, int.MaxValue, ErrorMessage = ModelErrorMsg.ValueNotNegative)]
        public int Id { get; set; }
        public Guid IdWeb { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ModelErrorMsg.ValueNotNegative)]
        public int IdRol { get; set; }
        [StringLength(30, MinimumLength = 12)]
        public string Name { get; set; } = "";
        [StringLength(60, MinimumLength = 4)]
        [EmailAddress(ErrorMessage = ModelErrorMsg.InvalidEmailAdress)]
        public string Email { get; set; } = "";
        [StringLength(200, MinimumLength = 0)]
        public string HashedPassword { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public string HashedRefreshToken { get; set; }
        public int FailedLogins { get; set; }
        public DateTime InsertDate { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ModelErrorMsg.ValueNotNegative)]
        public int State { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<NoteItem>? Notes { get; set; }

        public bool ValidateModel(bool toBeInserted)
        {
            var validDataAnnotations = ValidateDataAnnotations(this);
            if (!validDataAnnotations.IsSuccess)
            {
                throw new InvalidModelException(InvalidModelExceptionType.InvalidDataModel, validDataAnnotations.ErrorsToString());
            }
            else
            {
                return true;
            }
            //validación por dataannotations
            //fluent validation para cosas más raras, verlo
            //o con atributos customizados para validar y eso
            //excluir caracteres y cosas raras
            //tmb validar por atributos relacionados, por ejemplo
            //el IdRol con el UserRolEnum (o que uno sea privado
            //y dependa del otro)
            //y validacion en distintos contextos tmb, por parámetros
            //según el momento o estado de la entidad
            //y acaso validar tras insertar en base y sino  un rollback o ver
            //si es que se puede.hacer
            //en fin, hay de todo. BBPP, SOLID, 4 ppios OOP, etc

            //tratar de tener en el mismo lado la config a base de datos
            //y la validación del modelo
            //limitar la base con generosidad igual.

            //podría por ejemplo encapsular todas las limitaciones de modelo ...
            //incluso que el front las "pida" al iniciar la página cada vez?
            //o usar el mismo archivo. ver.
            //y no tenerlo hardcodeado tampoco en los mensajes. verlo.
            //recordar typar los objetos de const en TypeScript. es un bardo sino.
        }
    }
    public static class UserConstants {
        public const string NotGeneratedValue = "NOT GENERATED";
    }
}