namespace SharedKernel.Audit
{
    public interface IAuiditEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
