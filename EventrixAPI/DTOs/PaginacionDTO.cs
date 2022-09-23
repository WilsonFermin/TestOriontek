namespace EventrixAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;

        private int recordsPorPagina = 10;

        private readonly int CantidadMaximaPorPagina = 30;

        public int RecordsPorPagina
        {
            get { return recordsPorPagina; }

            set 
            { 
                recordsPorPagina = (value > CantidadMaximaPorPagina) ? CantidadMaximaPorPagina : value; 
            }
        }
    }
}
