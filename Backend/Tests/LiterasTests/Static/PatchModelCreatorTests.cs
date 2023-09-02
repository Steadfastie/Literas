using System.Reflection;
using LiterasData;
using LiterasData.DTO;

namespace TestsLiteras.Static;

public class PatchModelCreatorTests
{
    [Theory]
    [MemberData(nameof(GetData), 0, 4)]
    public void Generate_WithDifferentValues(IBaseDto source, IBaseDto changed, int modelAmount)
    {
        var modelList = PatchModelCreatorDto<IBaseDto>.Generate(source, changed);

        Assert.NotNull(modelList);
        Assert.Equal(modelAmount, modelList.Count);
    }

    [Theory]
    [MemberData(nameof(GetData), 4, 1)]
    public void Generate_WithDifferentTypes(IBaseDto source, IBaseDto changed)
    {
        Assert.Throws<ArgumentException>(() => PatchModelCreatorDto<IBaseDto>.Generate(source, changed));
    }

    [Theory]
    [MemberData(nameof(GetData), 5, 2)]
    public void Generate_WithIgnoredProperties(
        IBaseDto source, IBaseDto changed, PropertyInfo[] ignoredProperties, int modelAmount)
    {
        var modelList = PatchModelCreatorDto<IBaseDto>.Generate(source, changed, ignoredProperties);

        Assert.NotNull(modelList);
        Assert.Equal(modelAmount, modelList.Count);
    }

    /// <summary>
    ///     This method is used to provide test method with data. Look through the data below to pick.
    /// </summary>
    /// <param name="index">Index of array of object</param>
    /// <param name="count">How many of them to take</param>
    public static IEnumerable<object[]> GetData(int index, int count = 1)
    {
        var allSeeds = new List<object[]>
        {
            // 1. Dtos with different values
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test", Content = "Test" },
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test", Content = "Different test" },
                1
            },

            // 2. UserDtos with same values
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test", Content = "Test" },
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test", Content = "Test" },
                0
            },

            // 3. UserDtos with source null values
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test" },
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test", Content = "Test" },
                1
            },

            // 4. UserDtos with null values in changed
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test", Content = "Test" },
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test" },
                0
            },

            // 5. DocDtos with UserDtos
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatedAt = DateTime.Today, Title = "Test" },
                new EditorDto() { Id = Guid.Empty, DocId = Guid.Empty, UserId = Guid.Empty },
            },

            // 6. DocDtos with list of ignored properties
            new object[]
            {
                new DocDto { Id = Guid.Empty, CreatedAt = DateTime.UtcNow, Title = "Title", Content = "Content" },
                new DocDto { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Title = "New title", Content = "New content" },
                new[] { typeof(DocDto).GetProperty("Id")!, typeof(DocDto).GetProperty("CreatedAt")! }, 2
            },

            // 7. DocDtos with spoiled list of ignored properties
            new object[]
            {
                new DocDto { Id = Guid.Empty, CreatedAt = DateTime.UtcNow, Title = "Title", Content = "Content" },
                new DocDto { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Title = "New title", Content = "New content" },
                new PropertyInfo[]
                {
                    typeof(DocDto).GetProperty("Id")!,
                    typeof(DocDto).GetProperty("CreatedAt")!,
                    typeof(EditorDto).GetProperty("LastContributed")!
                },
                2
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}
