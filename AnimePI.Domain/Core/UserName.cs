namespace AnimePI.Domain.Core;

public class UserName
{

    public string FirstName { get; set; }
    public string Surname { get; set; }

    public string FullName => FirstName + ' ' + Surname;

    public UserName(string firstName, string surname)
    {
        FirstName = firstName;
        Surname = surname;
    }
}