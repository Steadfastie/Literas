using LiterasBusiness;
using LiterasDataTransfer.Dto;
using LiterasModels.Abstractions;

namespace TestsLiteras.Static;

public class PatchModelCreatorTests
{
    [Theory]
    [MemberData(nameof(GetData), 0, 6)]
    public void Generate_WithDifferentValues(IBaseDto source, IBaseDto changed, int modelAmount)
    {
        var modelList = PatchModelCreator<IBaseDto>.Generate(source, changed);

        Assert.NotNull(modelList);
        Assert.Equal(modelAmount, modelList.Count);
    }

    [Theory]
    [MemberData(nameof(GetData), 6, 1)]
    public void Generate_WithDifferentTypes(IBaseDto source, IBaseDto changed)
    {
        Assert.Throws<ArgumentException>(() => PatchModelCreator<IBaseDto>.Generate(source, changed));
    }

    /// <summary>
    /// This method is used to provide test method with data. Look through the data below to pick.
    /// </summary>
    /// <param name="index">Index of array of object</param>
    /// <param name="count">How many of them to take</param>
    public static IEnumerable<object[]> GetData(int index, int count = 1)
    {
        var allSeeds = new List<object[]>()
        {
            // 1. UserDtos with different values
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Different name" },
                1
            },

            // 2. UserDtos with same values
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                0
            },

            // 3. UserDtos with source null values
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                1
            },

            // 4. UserDtos with null values in changed
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name"},
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
                0
            },

            // 5. DocumentDtos with different values
            new object[]
            {
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Different content" },
                1
            },

            // 6. DocumentDtos with different ids
            new object[]
            {
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new DocumentDto() { Id = Guid.NewGuid(), CreatorId = Guid.Empty, Title = "Different title", Content = "Different content" },
                2
            },

            // 7. DocumentDtos with UserDtos
            new object[]
            {
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}