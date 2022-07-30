namespace Maicom.Customers.WebApi.Models;

public class Customer
{
    private Customer(ulong code, string person, string channel, string concept)
    {
        Code = code;
        Person = person;
        Channel = channel;
        Concept = concept;
    }

    public ulong Code { get; }
    public string Person { get; }
    public string Channel { get; }
    public string Concept { get; }

    public static Customer Create(ulong code, string person, string channel, string concept) =>
        new(code, person, channel, concept);
}