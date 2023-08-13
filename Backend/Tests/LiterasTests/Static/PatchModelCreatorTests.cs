using System.Reflection;
using LiterasCore;
using LiterasData;
using LiterasData.DTO;

namespace TestsLiteras.Static;

public class PatchModelCreatorTests
{
    [Theory]
    [MemberData(nameof(GetData), 0, 5)]
    public void Generate_WithDifferentValues(IBaseDto source, IBaseDto changed, int modelAmount)
    {
        var modelList = PatchModelCreatorDto<IBaseDto>.Generate(source, changed);

        Assert.NotNull(modelList);
        Assert.Equal(modelAmount, modelList.Count);
    }

    [Theory]
    [MemberData(nameof(GetData), 5, 1)]
    public void Generate_WithDifferentTypes(IBaseDto source, IBaseDto changed)
    {
        Assert.Throws<ArgumentException>(() => PatchModelCreatorDto<IBaseDto>.Generate(source, changed));
    }

    [Theory]
    [MemberData(nameof(GetData), 6, 2)]
    public void Generate_WithIgnoredProperties(
        IBaseDto source, IBaseDto changed, PropertyInfo[] ignoredProperties, int modelAmount)
    {
        var modelList = PatchModelCreatorDto<IBaseDto>.Generate(source, changed, ignoredProperties);

        Assert.NotNull(modelList);
        Assert.Equal(modelAmount, modelList.Count);
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
            //// 1. UserDtos with different values
            //new object[]
            //{
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Different name" },
            //    1
            //},

            //// 2. UserDtos with same values
            //new object[]
            //{
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
            //    0
            //},

            //// 3. UserDtos with source null values
            //new object[]
            //{
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
            //    1
            //},

            //// 4. UserDtos with null values in changed
            //new object[]
            //{
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name"},
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
            //    0
            //},

            // 5. DocDtos with different values
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new DocDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Different content" },
                1
            },

            //// 6. DocDtos with UserDtos
            //new object[]
            //{
            //    new DocDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
            //    new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
            //},

            // 7. DocDtos with list of ignored properties
            new object[]
            {
                new DocDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new DocDto() { Id = Guid.NewGuid(), CreatorId = Guid.NewGuid(), Title = "New title", Content = "New content" },
                new PropertyInfo[]
                {
                    typeof(DocDto).GetProperty("Id")!,
                    typeof(DocDto).GetProperty("CreatorId")!
                },
                2
            },

            //// 8. DocDtos with spoiled list of ignored properties
            //new object[]
            //{
            //    new DocDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
            //    new DocDto() { Id = Guid.NewGuid(), CreatorId = Guid.NewGuid(), Title = "New title", Content = "New content" },
            //    new PropertyInfo[]
            //    {
            //        typeof(DocDto).GetProperty("Id")!,
            //        typeof(DocDto).GetProperty("CreatorId")!,
            //        typeof(UserDto).GetProperty("Fullname")!
            //    },
            //    2
            //},
        };

        return allSeeds.Skip(index).Take(count);
    }
}
