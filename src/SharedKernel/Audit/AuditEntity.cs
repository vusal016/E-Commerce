namespace SharedKernel.Audit
{
    public abstract class AuditEntity:BaseEntity, IAuditEntity
    {
        protected AuditEntity() : base()
        {

        }
        protected AuditEntity(Guid id) : base(id)
        {

        }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
