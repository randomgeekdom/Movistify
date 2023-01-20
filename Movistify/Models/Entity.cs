using System.ComponentModel.DataAnnotations;

namespace Movistify.Models
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
