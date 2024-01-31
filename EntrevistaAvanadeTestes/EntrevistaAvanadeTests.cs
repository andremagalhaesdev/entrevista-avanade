using EntrevistaAvanade.Models;
using Xunit;

namespace EntrevistaAvanadeTestes
{
    public class EntrevistaAvanadeTests
    {
        private Estacionamento _estacionamento;

        public EntrevistaAvanadeTests()
        {
            _estacionamento = new Estacionamento();
        }

        // Testes para Estacionamento //

        [Fact]
        public void DeveRetornarCustoZeroSeOTempoDePermanenciaForMenorQue15Minutos()
        {
            // Arrange
            DateTime dataDeEntrada = DateTime.Now.AddHours(-0.2); //  Menos que 15 minutos (0.26 horas)
            int categoriaDoVeiculoEstacionado = 1;
            bool confirmacaoDeSeguro = true;

            // Act
            decimal resultado = _estacionamento.CalcularPrecoTotalDoServicoPorCategoria(dataDeEntrada, categoriaDoVeiculoEstacionado, confirmacaoDeSeguro);

            // Assert
            Assert.Equal(0, resultado);
        }

        [Theory]
        [InlineData(1, true, 6.00)] // Categoria 1, Com Seguro
        [InlineData(1, false, 5.00)] // Categoria 1, Sem Seguro
        [InlineData(2, true, 9.00)] // Categoria 2, Com Seguro
        [InlineData(2, false, 7.00)] // Categoria 2, Sem Seguro
        [InlineData(3, true, 13.00)] // Categoria 3, Com Seguro
        [InlineData(3, false, 10.00)] // Categoria 3, Sem Seguro
        public void DeveRetornarPrecoTotalParaAte1Hora(int categoria, bool comSeguro, decimal precoEsperado)
        {
            // Arrange
            DateTime dataDeEntrada = DateTime.Now.AddHours(-0.9); // Menos que 1 hora

            // Act
            decimal resultado = _estacionamento.CalcularPrecoTotalDoServicoPorCategoria(dataDeEntrada, categoria, comSeguro);

            // Assert
            Assert.Equal(precoEsperado, resultado);
        }

        [Theory]
        [InlineData(1, true, 8.00)] // Categoria 1, Com Seguro
        [InlineData(1, false, 7.00)] // Categoria 1, Sem Seguro
        [InlineData(2, true, 11.00)] // Categoria 2, Com Seguro
        [InlineData(2, false, 9.00)] // Categoria 2, Sem Seguro
        [InlineData(3, true, 15.00)] // Categoria 3, Com Seguro
        [InlineData(3, false, 12.00)] // Categoria 3, Sem Seguro
        public void DeveRetornarPrecoTotalPara1HoraMaisFracao(int categoria, bool comSeguro, decimal precoEsperado)
        {
            // Arrange
            DateTime dataDeEntrada = DateTime.Now.AddHours(-1.1); // 1 Hora + Fração de 1 hora

            // Act
            decimal resultado = _estacionamento.CalcularPrecoTotalDoServicoPorCategoria(dataDeEntrada, categoria, comSeguro);

            // Assert
            Assert.Equal(precoEsperado, resultado);
        }

        [Fact]
        public void DeveLancarUmaExcecaoSeDateTimeNowForMenorQueDateTimeDeEntrada()
        {
            // Arrange
            DateTime dataDeEntrada = DateTime.Now.AddHours(1.0); // Uma hora após a data de agora
            int categoriaDoVeiculoEstacionado = 2;
            bool confirmacaoDeSeguro = false;

            // Act
            Action act = () => _estacionamento.CalcularPrecoTotalDoServicoPorCategoria(dataDeEntrada, categoriaDoVeiculoEstacionado, confirmacaoDeSeguro);

            // Assert
            InvalidOperationException excecaoLancada = Assert.Throws<InvalidOperationException>(act);
            Assert.NotNull(excecaoLancada);
            Assert.Equal("Sistema desatualizado. Verifique sua data e hora e tente novamente.", excecaoLancada.Message);
        }
    }
}
