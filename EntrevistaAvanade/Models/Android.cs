using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntrevistaAvanade.Models
{
    public class Android : Smartphone
    {
        public Android(string numeroTelefone, string modeloTelefone, string imeiTelefone, int memoriaTelefone, List<string> aplicativosInstalados, List<string> blackListAnatel, List<Veiculo> VeiculosEstacionados) : base(numeroTelefone, modeloTelefone, imeiTelefone, memoriaTelefone, aplicativosInstalados, blackListAnatel, VeiculosEstacionados)
        {
            CarregarAplicativosInstalados();
        }
        public override void TirarCelularDoBolso(string modelo)
        {
            Console.Clear();
            Console.WriteLine($"Você tirou seu {modelo} do bolso.");
            bool menuAndroid = true;
            while (menuAndroid)
            {
                Console.WriteLine("╔══════════════════╗");
                Console.WriteLine("║   Menu Android   ║");
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
                        bool menuPlayStore = true;
                        while (menuPlayStore)
                        {
                            Console.Clear();
                            Console.WriteLine("╔══════════════════╗");
                            Console.WriteLine($"║     PlayStore    ║");
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
                            string opcaoLojaPlaystore = Console.ReadLine();

                            if (opcaoLojaPlaystore == "1")
                            {
                                Console.WriteLine("Leia o QR Code do aplicativo que deseja instalar:");
                                Thread.Sleep(2000);
                                Console.WriteLine("*Lendo QRCode AvaParking...*");
                                Thread.Sleep(2000);
                                InstalarAplicativo("AvaParking", 32, true);
                            }
                            else if (opcaoLojaPlaystore == "2")
                            {
                                Console.WriteLine("Digite o nome do aplicativo que deseja desinstalar:");
                                string nomeApp = Console.ReadLine();
                                DesinstalarAplicativo(nomeApp);

                            }
                            else if (opcaoLojaPlaystore == "3")
                            {
                                Console.WriteLine("Digite o nome do aplicativo que deseja abrir:");
                                string nomeAppParaAbrir = Console.ReadLine();
                                AbrirAplicativo(nomeAppParaAbrir);
                            }
                            else
                            {
                                menuPlayStore = false;
                            }
                        }
                        break;
                    case "3":
                        ComunicarPerdaRoubo(IMEI);
                        break;
                    case "4":
                        menuAndroid = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }

        }

        public override void CarregarAplicativosInstalados()
        {
            int tamanhoAplicativoPadrao = 32;
            int quantidadeMaximaAplicativos = Memoria / tamanhoAplicativoPadrao;

            for (int i = 1; i <= quantidadeMaximaAplicativos; i++)
            {
                AplicativosInstalados.Add($"App{i}");
                Memoria -= tamanhoAplicativoPadrao;
            }
        }


        public override void InstalarAplicativo(string nomeApp, int tamanhoApp, bool aplicativoCertificado)
        {
            Console.WriteLine($"Instalando aplicativo \"{nomeApp}\" no Android.");
            Thread.Sleep(1000);
            if (tamanhoApp > Memoria)
            {
                Console.WriteLine($"Não há espaço suficiente para instalar o aplicativo \"{nomeApp}\". Por favor, desinstale aplicativos para liberar espaço.");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                AplicativosInstalados.Add(nomeApp);
                Memoria -= tamanhoApp;
                Console.WriteLine($"\"{nomeApp}\" instalado com sucesso!");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public override void DesinstalarAplicativo(string nomeApp)
        {
            if (AplicativosInstalados.Contains(nomeApp))
            {
                Console.WriteLine($"Desinstalando aplicativo \"{nomeApp}\" do Android.");
                Thread.Sleep(1000);
                AplicativosInstalados.Remove(nomeApp);
                Memoria += 32;
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
    }
}