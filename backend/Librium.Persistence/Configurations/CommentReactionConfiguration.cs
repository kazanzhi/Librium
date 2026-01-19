    using Librium.Domain.Comments;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace Librium.Persistence.Configurations;
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ReactionType)
                    .IsRequired();

            builder.Property(x => x.CommentId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasIndex(x => new { x.CommentId, x.UserId })
                .IsUnique();
        }
    }
