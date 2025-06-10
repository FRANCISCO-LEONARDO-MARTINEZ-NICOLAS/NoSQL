using System;

class Program
{
    static void Main()
    {
        Console.Write("Escribe la contraseña a hashear: ");
        string password = Console.ReadLine();
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine("Hash generado:");
        Console.WriteLine(hash);
    }
}
