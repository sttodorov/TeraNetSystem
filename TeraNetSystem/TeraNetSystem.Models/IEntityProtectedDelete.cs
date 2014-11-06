namespace TeraNetSystem.Models
{
    using System;

    public interface IEntityProtectedDelete
    {
        bool IsDeleted { get; set; }

        DateTime? DateDeleted { get; set; }
    }
}
