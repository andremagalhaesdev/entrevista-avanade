using EntrevistaAvanade.Models;
Console.OutputEncoding = System.Text.Encoding.UTF8;
DotNetEnv.Env.Load();

Estacionamento AvaParking = new Estacionamento();
bool exibirMenu = true;

while (exibirMenu)
{
  Console.Clear();
  Console.WriteLine("╔════════════════════════════════════╗");
  Console.WriteLine("║    Seja Bem-vindo ao AvaParking    ║");
  Console.WriteLine("╠════════════════════════════════════╣");
  Console.WriteLine("║ 1. Estacionar Veículo              ║");
  Console.WriteLine("║ 2. Retirar Veículo Estacionado     ║");
  Console.WriteLine("║ 3. Listar Véiculos (Gerência)      ║");
  Console.WriteLine("║ 4. Sair do Sistema                 ║");
  Console.WriteLine("╚════════════════════════════════════╝");

  switch (Console.ReadLine())
  {
    case "1":
      AvaParking.AdicionarVeiculo();
      break;

    case "2":
      AvaParking.RetirarVeiculo();
      break;

    case "3":
      AvaParking.ListarVeiculos();
      break;

    case "4":
      exibirMenu = false;
      break;

    default:
      Console.WriteLine("Opção inválida");
      break;
  }

  Console.WriteLine("Pressione uma tecla para continuar");
  Console.ReadLine();
}
Console.Clear();
Console.WriteLine("Sistema Encerrado");