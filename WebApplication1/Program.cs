using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace WebApplication1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}

[JsonDerivedType(typeof(LegalPerson), nameof(LegalPerson))]
[JsonDerivedType(typeof(PrivatePerson), nameof(PrivatePerson))]
public abstract class Person
{
    public int Id { get; set; }
}

public class PrivatePerson : Person
{
    public string? FirstName { get; set; }
}

public class LegalPerson : Person
{
    public string? ContractName { get; set; }
}

[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("[action]")]
    public ActionResult<Person> Fail()
    {
        Person person = GetPerson();
        return Ok(person);
    }

    [HttpPost("[action]")]
    public ActionResult<Person> FailPost()
    {
        Person person = GetPerson();
        return Created(string.Empty, person);
    }

    [HttpGet("[action]")]
    public ActionResult<Person> Ok1()
    {
        Person person = GetPerson();
        return new OkObjectResult(person) { DeclaredType = typeof(Person) };
    }

    [HttpGet("[action]")]
    public ActionResult<Person> Ok2()
    {
        Person person = GetPerson();
        return person;
    }

    [HttpGet("[action]")]
    public ActionResult<IEnumerable<Person>> Ok3()
    {
        Person[] person = [GetPerson()];
        return Ok(person);
    }

    [HttpGet("[action]")]
    public IResult Ok4()
    {
        Person person = GetPerson();
        return Results.Ok(person);
    }

    private static Person GetPerson()
    {
        if (Random.Shared.Next(1) < 1000) // always true
            return new PrivatePerson { Id = 1, FirstName = "Test" };
        return new LegalPerson { Id = 1, ContractName = "Test" };
    }
}