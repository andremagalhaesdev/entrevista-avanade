using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntrevistaAvanade.Models
{
    public abstract class Smartphone
    {
        protected string Numero { get; set; }
        public string Modelo { get; set; }
        protected string IMEI { get; set; }
        protected int Memoria { get; set; }
        protected List<Veiculo> VeiculosEstacionados = new List<Veiculo>();

        protected List<string> AplicativosInstalados { get; set; }
        protected static List<string> BlackListAnatel { get; set; }



        public Smartphone(string numeroTelefone, string modeloTelefone, string imeiTelefone, int memoriaTelefone, List<string> aplicativosInstalados, List<string> blackListAnatel, List<Veiculo> avaParking)
        {
            Numero = numeroTelefone;
            Modelo = modeloTelefone;
            IMEI = imeiTelefone;
            Memoria = memoriaTelefone;
            AplicativosInstalados = aplicativosInstalados;
            BlackListAnatel = blackListAnatel;
            VeiculosEstacionados = avaParking;
        }

        public void ChamadorEmergenciaBot(string numeroParaLigar)
        {
            Console.WriteLine($"*Ligando para {numeroParaLigar}*");

        }

        public void Ligar()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════╗");
            Console.WriteLine("║     Chamadas     ║");
            Console.WriteLine("╠══════════════════╣");
            Console.WriteLine("║  Digite o Número ║");
            Console.WriteLine("║                  ║");
            Console.WriteLine("║     1  2  3      ║");
            Console.WriteLine("║     4  5  6      ║");
            Console.WriteLine("║     7  8  9      ║");
            Console.WriteLine("║        0         ║");
            Console.WriteLine("╚══════════════════╝");

            Console.CursorLeft = 4; // Define a posição do cursor quanto a coluna
            Console.CursorTop = 4;  // Define a posição do cursor quanto à linha

            string numeroParaLigar = Console.ReadLine();

            Console.CursorLeft = 4; // Define a posição do cursor quanto a coluna
            Console.CursorTop = 10;  // Define a posição do cursor quanto à linha
            Console.WriteLine($"*Ligando para {numeroParaLigar}*");
            Thread.Sleep(2000);
            if (BlackListAnatel.Contains(IMEI))
            {
                Console.WriteLine("Celular bloqueado por perda/roubo. Procure a Polícia Civil de Pernambuco para mais informações.");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"*Você não tem saldo suficiente para completar essa ligação.*");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public void ReceberLigacao(string numeroQueEstaLigando)
        {
            Console.WriteLine($"*Recebendo ligação de {numeroQueEstaLigando}*");
        }

        public abstract void InstalarAplicativo(string nomeApp, int espacoNecessario, bool aplicativoCertificado);
        public abstract void DesinstalarAplicativo(string nomeApp);

        public abstract void TirarCelularDoBolso(string modelo);
        public abstract void CarregarAplicativosInstalados();


        protected static void ComunicarPerdaRoubo(string imei)
        {
            Console.WriteLine("Você tem certeza que deseja comunicar a perda/roubo deste aparelho? Digite Sim para confirmar.");
            string confirmacaoComunicarPerdaRoubo = Console.ReadLine();
            if (confirmacaoComunicarPerdaRoubo.ToUpper() == "SIM")
            {
                Console.WriteLine("Processando...");
                Thread.Sleep(2000);
                if (BlackListAnatel.Contains(imei))
                {
                    Console.WriteLine("Este aparelho já está bloqueado por perda/roubo. Procure a Polícia Civil de Pernambuco para mais informações.");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                else
                {
                    BlackListAnatel.Add(imei);
                    Console.WriteLine($"O IMEI {imei} foi adicionado à Black List da Anatel. Não será mais possível realizar chamadas.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            else
            {
                return;
            }

        }
        protected void AbrirAplicativo(string nomeApp)
        {
            if (AplicativosInstalados.Contains(nomeApp))
            {
                Console.WriteLine($"Abrindo aplicativo \"{nomeApp}\".");
                Thread.Sleep(2000);
                if (nomeApp == "AvaParking")
                {
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════╗");
                    Console.WriteLine("║              AvaParking App            ║");
                    Console.WriteLine("╠════════════════════════════════════════╣");
                    Console.WriteLine("║ Digite a placa do seu veículo para     ║");
                    Console.WriteLine("║ realizar o pagamento:                  ║");
                    Console.WriteLine("║                                        ║");
                    Console.WriteLine("║ PLACA:                                 ║");
                    Console.WriteLine("╚════════════════════════════════════════╝");

                    Console.CursorLeft = 9; // Define a posição do cursor quanto a coluna
                    Console.CursorTop = 6;  // Define a posição do cursor quanto à linha

                    string placaDigitadaApp = Console.ReadLine();


                    Console.CursorLeft = 4; // Define a posição do cursor quanto a coluna
                    Console.CursorTop = 8;  // Define a posição do cursor quanto à linha
                    Console.WriteLine("Processando...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    RetirarVeiculoViaApp(placaDigitadaApp);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("╔══════════════════╗");
                    Console.WriteLine($"║    {nomeApp,7}       ║");
                    Console.WriteLine("╠══════════════════╣");
                    Console.WriteLine("║                  ║");
                    Console.WriteLine("║                  ║");
                    Console.WriteLine($"║ Conteúdo do {nomeApp} ║");
                    Console.WriteLine("║                  ║");
                    Console.WriteLine("║                  ║");
                    Console.WriteLine("║                  ║");
                    Console.WriteLine("╚══════════════════╝");
                    Console.ReadLine();
                    Console.Clear();

                }
            }
            else
            {
                Console.WriteLine($"O aplicativo \"{nomeApp}\" não está instalado.");
                Console.ReadLine();
                Console.Clear();
            }
        }


        protected void RetirarVeiculoViaApp(string placaVeiculoEmRemocao)
        {
            Estacionamento AvaParking = new Estacionamento();
            Veiculo veiculoEmRemocao = VeiculosEstacionados.FirstOrDefault(x => x.Placa == placaVeiculoEmRemocao);

            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║             AvaParking App             ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            if (veiculoEmRemocao != null)
            {
                decimal valorDevido = AvaParking.CalcularPrecoTotalDoServicoPorCategoria(veiculoEmRemocao.DataEntrada, veiculoEmRemocao.Categoria, veiculoEmRemocao.DesejaSeguro);
                if (valorDevido == 0)
                {
                    VeiculosEstacionados.Remove(veiculoEmRemocao);
                    Console.WriteLine("║    Obrigado por utilizar o AvaParking  ║");
                    Console.WriteLine("║             Volte Sempre!              ║");
                    Console.WriteLine("╚════════════════════════════════════════╝");
                }
                else
                {
                    Console.WriteLine($"║Valor Total para {veiculoEmRemocao.Placa}:               ║");
                    Console.WriteLine($"║             R${valorDevido}                    ║");
                    Console.WriteLine("║ 1- Pagar com Wallet (Apple/SamsungPay) ║");
                    Console.WriteLine("║ 2- Cancelar                            ║");
                    Console.WriteLine("╚════════════════════════════════════════╝");
                    string opcaoPagamentoApp = Console.ReadLine();
                    if (opcaoPagamentoApp == "1")
                    {
                        Console.WriteLine("Processando Pagamento...");
                        Thread.Sleep(2000);
                        Console.WriteLine("Pagamento realizado com sucesso!");
                        VeiculosEstacionados.Remove(veiculoEmRemocao);
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.WriteLine("╔════════════════════════════════════════╗");
                        Console.WriteLine("║             AvaParking App             ║");
                        Console.WriteLine("╠════════════════════════════════════════╣");
                        Console.WriteLine("║    Obrigado por utilizar o AvaParking  ║");
                        Console.WriteLine("║             Volte Sempre!              ║");
                        Console.WriteLine("╚════════════════════════════════════════╝");
                        Console.ReadLine();
                    }
                    else if (opcaoPagamentoApp == "2")
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Tente novamente.");
                    }
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("║        Veículo não encontrado!         ║");
                Console.WriteLine("║      Verifique a placa digitada.       ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}