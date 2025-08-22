using System.Text.RegularExpressions;

namespace AnimePI.Domain.Core;

public class Email
{
    public const int MaxLenght = 254;
    public const int MinLenght = 5;

    public string Mail { get; set; }

    protected Email() { }
    public Email(string mail)
    {
        Mail = mail;
        Validate();
    }

    private void Validate()
    {
        var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        if (!regexEmail.IsMatch(Mail)) throw new Exception("Invalid E-mail");
    }
}
