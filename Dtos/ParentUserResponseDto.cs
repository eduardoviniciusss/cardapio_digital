namespace cardapio_digital.Dtos
{
    // Usuário com papel Responsável, para o Administrador escolher
    // em nome de quem vai atuar.
    public class ParentUserResponseDto
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public bool HasRegistration { get; set; }
    }
}
