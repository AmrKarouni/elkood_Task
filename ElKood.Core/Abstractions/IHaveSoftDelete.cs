namespace ElKood.Core.Abstractions
{
    public interface IHaveSoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
