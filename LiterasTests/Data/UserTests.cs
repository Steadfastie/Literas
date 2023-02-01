using LiterasData.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TestsLiteras.Data;

public class UserTests
{
    [Theory]
    [InlineData("login@gmail.com", "password", "fullname")]
    public void Constructor_SetsFullname_WhenNameIsProvided(string login, string password, string name)
    {
        var user = new User(login, password, name);
        Assert.Equal(user.Fullname, name);
    }

    [Theory]
    [InlineData("login@gmail.com", "password")]
    public void Constructor_SetsFullname_WhenNameIsNotProvided(string login, string password)
    {
        var user = new User(login, password);
        Assert.Equal(user.Fullname, login[..login.IndexOf('@')]);
    }

    [Theory]
    [InlineData("login", "password")]
    public void Constructor_DoesNotSetFullname_WhenNameIsNotProvidedAndLoginIsWithoutAt(string login, string password)
    {
        var user = new User(login, password);
        Assert.Null(user.Fullname);
    }
}