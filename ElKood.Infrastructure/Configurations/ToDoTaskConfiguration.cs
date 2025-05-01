using ElKood.Core.Entities;
using ElKood.Infrastructure.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElKood.Infrastructure.Configurations
{
    public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
    {
        public void Configure(EntityTypeBuilder<ToDoTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(ToDoTaskConsts.MaxNameLength);
            builder.Property(td => td.CreatetionDate).IsRequired().HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            builder.Property(td => td.CompletionDate).ValueGeneratedOnUpdate();
            builder.HasOne(t => t.Category).WithMany(td => td.Tasks).HasForeignKey(td => td.CategoryId);
        }
    }
}
