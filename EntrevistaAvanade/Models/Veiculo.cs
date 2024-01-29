namespace EntrevistaAvanade.Models
{

    public class Veiculo
    {

        public string Placa { get; set; }
        public DateTime DataEntrada { get; set; }
        public int Categoria { get; set; }
        public bool DesejaSeguro { get; set; }

        public Veiculo(string placa, DateTime dataEntrada, int categoria, bool desejaSeguro)
        {
            Placa = placa;
            DataEntrada = dataEntrada;
            Categoria = categoria;
            DesejaSeguro = desejaSeguro;
        }

    }
}
