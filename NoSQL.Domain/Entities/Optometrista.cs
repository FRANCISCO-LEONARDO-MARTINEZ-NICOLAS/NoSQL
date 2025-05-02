namespace NoSQL.Domain.Entities
{
    public class Optometrista
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Celular { get; set; }
        public string NumeroEmergencia { get; set; }
        public string Correo { get; set; }
        public string CedulaProfesional { get; set; }
    }
}