using Entities.Items;

namespace Entities.Entities
{
    public class UserItem
    {
        public int Id { get; set; }
        public Guid IdWeb { get; set; }
        public int RolId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string HashedToken { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public int FailedLogins { get; set; }
        public DateTime InsertDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<NoteItem> Notes { get; set; }

        public bool ValidateValues(bool isNew, bool isActive)
        {
            return true;
        }
    }
}
