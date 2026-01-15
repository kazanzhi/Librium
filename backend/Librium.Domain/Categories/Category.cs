using Librium.Domain.Common;

namespace Librium.Domain.Categories;

public class Category
{
    private Category() { }
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public static ValueOrResult<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValueOrResult<Category>.Failure("Category name is required.");

        if (name.Length > 100)
            return ValueOrResult<Category>.Failure("Category name cannot exceed 100 characters.");

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        };

        return ValueOrResult<Category>.Success(category);
    }

    public ValueOrResult Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValueOrResult.Failure("Category name is required.");

        if (name.Length > 100)
            return ValueOrResult<Category>.Failure("Category name cannot exceed 100 characters.");

        Name = name.Trim();

        return ValueOrResult.Success();
    }
}
