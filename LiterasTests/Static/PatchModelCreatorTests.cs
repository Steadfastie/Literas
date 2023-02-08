using LiterasBusiness;
using LiterasDataTransfer.Dto;
using LiterasModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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

    public static IEnumerable<object[]> GetData(int index, int count = 1)
    {
        var allSeeds = new List<object[]>()
        {
            // UserDtos with different values
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Different name" },
                1
            },

            // UserDtos with same values
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                0
            },

            // UserDtos with source null values
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" },
                1
            },

            // UserDtos with null values in changed
            new object[]
            {
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name"},
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
                0
            },

            // DocumentDtos with different values
            new object[]
            {
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Different content" },
                1
            },

            // DocumentDtos with different ids
            new object[]
            {
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new DocumentDto() { Id = Guid.NewGuid(), CreatorId = Guid.Empty, Title = "Different title", Content = "Different content" },
                2
            },

            // DocumentDtos with UserDtos
            new object[]
            {
                new DocumentDto() { Id = Guid.Empty, CreatorId = Guid.Empty, Title = "Title", Content = "Content" },
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password" },
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}
