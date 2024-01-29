using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntrevistaAvanade.Models
{
    public class Iphone : Smartphone
    {
        public bool iPhoneComJailBreak { get; set; } = false;
        public bool sistemaIntegro { get; set; } = true;


        public Iphone(string numero, string modelo, string imei, int memoria, List<string> aplicativosInstalados, List<string> blackListAnatel, List<Veiculo> veiculoEstacionado) : base(numero, modelo, imei, memoria, aplicativosInstalados, blackListAnatel, veiculoEstacionado)
        {
            iPhoneComJailBreak = false;
            CarregarAplicativosInstalados();
        }

        public override void TirarCelularDoBolso(string modelo)
        {

            Console.Clear();
            Console.WriteLine($"Você tirou seu {modelo} do bolso.");
            bool menuIphone = true;
            while (menuIphone)
            {
                if (sistemaIntegro)
                {
                    Console.WriteLine("╔══════════════════╗");
                    Console.WriteLine("║    Menu iPhone   ║");
                    Console.WriteLine("╠══════════════════╣");
                    Console.WriteLine("║ 1. Fazer Ligação ║");
                    Console.WriteLine("║ 2. Abrir Loja    ║");
                    Console.WriteLine("║ 3. Comunicar     ║");
                    Console.WriteLine("║    Perda/Roubo   ║");
                    Console.WriteLine("║ 4. Desligar      ║");
                    Console.WriteLine("║    Aparelho      ║");
                    Console.WriteLine("╚══════════════════╝");

                    string opcaoEscolhidaNoMenu = Console.ReadLine();
                    switch (opcaoEscolhidaNoMenu)
                    {
                        case "1":
                            Ligar();
                            break;
                        case "2":
                            bool menuAppStore = true;
                            while (menuAppStore)
                            {
                                Console.Clear();
                                Console.WriteLine("╔══════════════════╗");
                                Console.WriteLine("║     AppStore     ║");
                                Console.WriteLine("╠══════════════════╣");
                                Console.WriteLine("║1- Instalar App   ║");
                                Console.WriteLine("║2- Desinst. App   ║");
                                Console.WriteLine("║3- Abrir App      ║");
                                Console.WriteLine("║                  ║");
                                Console.WriteLine("║  Apps Instalados ║");
                                foreach (var app in AplicativosInstalados)
                                {
                                    Console.WriteLine($"║{app,-10}        ║");
                                }
                                Console.WriteLine("║                  ║");
                                Console.WriteLine($"║Memória Disp: {Memoria,3} ║");
                                Console.WriteLine("╚══════════════════╝");
                                Console.WriteLine("Escolha uma opção, ou qualquer tecla para voltar ao menu principal");
                                string opcaoLojaAppStore = Console.ReadLine();

                                if (opcaoLojaAppStore == "1")
                                {
                                    Console.WriteLine("Leia o QR Code do aplicativo que deseja instalar:");
                                    Thread.Sleep(2000);
                                    Console.WriteLine("*Lendo QRCode AvaParking...*");
                                    Thread.Sleep(2000);
                                    InstalarAplicativo("AvaParking", 32, true);
                                    Console.Clear();
                                }
                                else if (opcaoLojaAppStore == "2")
                                {
                                    Console.WriteLine("Digite o nome do aplicativo que deseja desinstalar:");
                                    string nomeApp = Console.ReadLine();
                                    DesinstalarAplicativo(nomeApp);
                                    Console.Clear();


                                }
                                else if (opcaoLojaAppStore == "3")
                                {
                                    Console.WriteLine("Digite o nome do aplicativo que deseja abrir:");
                                    string nomeAppParaAbrir = Console.ReadLine();
                                    AbrirAplicativo(nomeAppParaAbrir);
                                    menuAppStore = false;

                                }
                                else if (opcaoLojaAppStore == "-do jailbreak -thisiphone --force")
                                {
                                    RealizarJailBreakNoIphone();
                                    if (sistemaIntegro == false)
                                    {
                                        menuAppStore = false;
                                    }
                                }
                                else
                                {
                                    menuAppStore = false;
                                }
                            }
                            break;
                        case "3":
                            ComunicarPerdaRoubo(IMEI);
                            break;
                        case "4":
                            menuIphone = false;
                            break;
                        default:
                            Console.WriteLine("Opção inválida");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("12%%#*@@!AA2(&#&^@@$ ERROR - SYSTEM NOT FOUND 11723^^q%%@@*!&&^%$@)");
                    Console.ReadLine();
                    menuIphone = false;
                }
            }
        }

        public override void InstalarAplicativo(string nomeApp, int tamanhoApp, bool aplicativoCertificado)
        {
            if (aplicativoCertificado)
            {
                if (tamanhoApp <= Memoria)
                {
                    Console.WriteLine($"Instalando o aplicativo \"{nomeApp}\" no iPhone.");
                    AplicativosInstalados.Add(nomeApp);
                    Memoria -= tamanhoApp;
                    Console.WriteLine($"\"{nomeApp}\" instalado com sucesso!");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Não há espaço suficiente para instalar o aplicativo \"{nomeApp}\". Memória necessária: {tamanhoApp}. Memória disponível: {Memoria}.");
                    Console.ReadLine();
                }
            }
            else if (!aplicativoCertificado && iPhoneComJailBreak)
            {
                Console.WriteLine($"Instalando aplicativo \"/{nomeApp}/\" via JailBreak no iPhone.");
                AplicativosInstalados.Add(nomeApp);
                Memoria -= tamanhoApp;
                Console.WriteLine($"\"{nomeApp}\" instalado com sucesso!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"\"{nomeApp}\" não é um aplicativo certificado pela Apple e não pode ser instalado no iPhone (convencionalmente).");
                Console.ReadLine();
            }
        }

        private void DesinstalarAplicativo(string nomeApp)
        {
            if (AplicativosInstalados.Contains(nomeApp))
            {
                Console.WriteLine($"Desinstalando aplicativo \"{nomeApp}\" do iPhone.");
                Thread.Sleep(1000);
                AplicativosInstalados.Remove(nomeApp);
                Memoria += 16;
                Console.WriteLine($"\"{nomeApp}\" foi desinstalado com sucesso!");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"O aplicativo \"{nomeApp}\" não está instalado.");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private void CarregarAplicativosInstalados()
        {
            int tamanhoAplicativoPadrao = 16;
            int quantidadeMaximaAplicativos = Memoria / tamanhoAplicativoPadrao;

            for (int i = 1; i <= quantidadeMaximaAplicativos; i++)
            {
                AplicativosInstalados.Add($"App{i}");
                Memoria -= tamanhoAplicativoPadrao;

            }
        }

        private void RealizarJailBreakNoIphone()
        {
            Console.WriteLine("Carregando pacotes necessários...");
            Thread.Sleep(2000);
            Console.WriteLine("Instalando dependências...");
            Thread.Sleep(2000);
            Console.WriteLine("Iniciando software...");
            Thread.Sleep(2000);
            if (!iPhoneComJailBreak)
            {
                Console.WriteLine("O processo de JailBreak é arriscado e pode danificar seu iPhone. Deseja continuar? (S/N)");
                string desejarContinuarJailBreak = Console.ReadLine();

                if (desejarContinuarJailBreak == "S")
                {
                    Random random = new Random();
                    double chanceDeSucesso = random.NextDouble();
                    Console.WriteLine("Tentando realizar JailBreak...");
                    Thread.Sleep(2000);
                    if (chanceDeSucesso <= 0.1) // 10% de chance de sucesso
                    {
                        Console.WriteLine("JailBreak realizado com sucesso!");
                        iPhoneComJailBreak = true;
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("12%%#*@@!AA2(&#&^@@$ ERROR - SYSTEM NOT FOUND 11723^^q%%@@*!&&^%$@)");
                        sistemaIntegro = false;
                        Console.ReadLine();
                    }
                }
                else
                {
                }
            }
            else
            {
                Console.WriteLine("Seu iPhone já possui JailBreak e está desbloqueado");
            }
        }

    }
}
