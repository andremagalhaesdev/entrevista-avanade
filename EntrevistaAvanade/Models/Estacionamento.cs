using System.Text.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Text.RegularExpressions;



namespace EntrevistaAvanade.Models
{
    public class Estacionamento
    {
        private decimal precoInicial;
        private decimal precoPorHora = 2.00M;
        private int capacidadeMaxima = 1;

        private List<Veiculo> veiculosEstacionados = new List<Veiculo>();

        private Conversation conversacaoComIA;

        public Estacionamento()
        {
            Veiculo veiculoInicial = new Veiculo("RBV6J88", new DateTime(2024, 01, 30, 08, 00, 0), 3, true);
            veiculosEstacionados.Add(veiculoInicial);
        }


        public void AdicionarVeiculo()
        {
            if (veiculosEstacionados.Count < capacidadeMaxima)
            {
                bool menuConfirmacaoEstacionamento = true;

                while (menuConfirmacaoEstacionamento)
                {

                    Console.Clear();
                    Console.WriteLine("ESTACIONAR VEÍCULO");
                    Console.WriteLine("Identificando placa...");
                    Thread.Sleep(2000);
                    string placaIdentificada = SensorDePlacas();
                    Console.WriteLine("Identificamos sua placa como " + placaIdentificada + ". Está correto?");
                    Console.WriteLine("1 - Sim, 2 - Não");

                    string confirmacaoDePlaca = Console.ReadLine();

                    Console.WriteLine("Carregando...");
                    Thread.Sleep(2000);

                    if (confirmacaoDePlaca.ToUpper() == "1")
                    {
                        Console.Clear();
                        int categoriaDoVeiculoEstacionado = InformarCategoriaManualmente();
                        Console.WriteLine("Carregando...");
                        Thread.Sleep(1000);
                        Console.Clear();
                        bool confirmacaoDeSeguro = DesejaAdicionarSeguro(categoriaDoVeiculoEstacionado);

                        CadastrarVeiculo(placaIdentificada, categoriaDoVeiculoEstacionado, confirmacaoDeSeguro);
                        menuConfirmacaoEstacionamento = false;
                    }
                    else if (confirmacaoDePlaca.ToUpper() == "2")
                    {
                        Console.Clear();

                        bool menuValidaPlaca = true;
                        while (menuValidaPlaca)
                        {
                            Console.Clear();
                            Console.Write("Digite sua placa, por favor. (Sem hífen, ex: ABC9G12)");
                            Console.WriteLine("");
                            placaIdentificada = Console.ReadLine();
                            Thread.Sleep(1000);
                            Console.WriteLine("Aguarde, estamos consultando sua placa em nossos sistemas.");
                            Thread.Sleep(1000);
                            if (!ValidarPlaca(placaIdentificada))
                            {
                                Console.Clear();
                                Console.WriteLine("Placa inválida. Certifique-se de digitar no formato correto.");
                                Thread.Sleep(2000);
                            }
                            else
                            {
                                menuValidaPlaca = false;
                            }

                        }

                        string resultadoConsultaPlacaViaApi = ConsultarPlacaViaAPI(placaIdentificada);
                        string resultadoConsultaPlacaViaIA = ConsultarPlacaViaIA(resultadoConsultaPlacaViaApi);

                        if (resultadoConsultaPlacaViaIA.ToUpper() == "VEICULO ROUBADO")
                        {
                            Console.Clear();
                            Console.WriteLine("Aguarde um momento. Um de nossos colaboradores está a caminho.");
                            Console.WriteLine("...");
                            Thread.Sleep(1000);
                            Console.WriteLine("*Acionando Seguranças...*");
                            Console.WriteLine("...");
                            Thread.Sleep(1000);
                            Console.WriteLine("...");
                            Thread.Sleep(1000);
                            AcionarPolicia();
                            menuConfirmacaoEstacionamento = false;
                            continue;
                        }
                        else if (resultadoConsultaPlacaViaIA.ToUpper() != "ERROR")
                        {
                            Console.WriteLine($"Verificação realizada com sucesso!");
                            bool confirmacaoDeSeguro = DesejaAdicionarSeguro(int.Parse(resultadoConsultaPlacaViaIA));
                            CadastrarVeiculo(placaIdentificada, int.Parse(resultadoConsultaPlacaViaIA), confirmacaoDeSeguro);
                            menuConfirmacaoEstacionamento = false;
                            continue;

                        }
                        else
                        {
                            Console.WriteLine($"Não conseguimos recuperar os dados do seu veículo.");
                            int categoriaDoVeiculoEstacionado = InformarCategoriaManualmente();
                            bool confirmacaoDeSeguro = DesejaAdicionarSeguro(int.Parse(resultadoConsultaPlacaViaIA));
                            CadastrarVeiculo(placaIdentificada, categoriaDoVeiculoEstacionado, confirmacaoDeSeguro);
                        }
                        menuConfirmacaoEstacionamento = false;
                    }
                    else
                    {
                        Console.WriteLine($"Opção inválida! Pressione qualquer tecla para reiniciar ou 1 para sair.");
                        string teclaPressionada = Console.ReadLine();
                        if (teclaPressionada == "1")
                        {
                            menuConfirmacaoEstacionamento = false;
                        }
                        else
                        {
                            Console.Clear();
                            continue;
                        }
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Desculpe, estamos lotados no momento.");
            }
        }

        private void CadastrarVeiculo(string placa, int categoria, bool desejaSeguro)
        {
            Console.Clear();
            Veiculo novoVeiculo = new Veiculo(placa, DateTime.Now, categoria, desejaSeguro);
            veiculosEstacionados.Add(novoVeiculo);
            Console.WriteLine("Finalizado!");
            Console.WriteLine($"Seu veículo, de categoria {ObterCategoriaPorCodigo(categoria)} e placa {placa} já pode ser estacionado!");
            Console.WriteLine($"Você entrou às {DateTime.Now.ToString("HH:mm, dd-MM-yyyy")}. Tolerância de 15 minutos.");
        }

        public void RetirarVeiculo()
        {
            Console.Clear();
            Console.WriteLine("RETIRAR VEÍCULO");
            Console.WriteLine("Digite a placa do veículo a ser removido:");
            string placaDoVeiculo = Console.ReadLine();
            Console.WriteLine("Procurando seu veículo...");
            Veiculo veiculoEmRemocao = veiculosEstacionados.FirstOrDefault(x => x.Placa == placaDoVeiculo);
            Thread.Sleep(2000);

            if (veiculoEmRemocao != null)
            {
                decimal valorDevido = CalcularPrecoTotalDoServicoPorCategoria(veiculoEmRemocao.DataEntrada, veiculoEmRemocao.Categoria, veiculoEmRemocao.DesejaSeguro);
                Console.Clear();
                Console.WriteLine("Obrigado por utilizar o AvaParking! Volte sempre.");
                if (valorDevido == 0)
                {
                    veiculosEstacionados.Remove(veiculoEmRemocao);
                    Console.WriteLine("Digite qualquer tecla para sair.");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.WriteLine($"O preço total do estacionamento foi de de: R${valorDevido}");
                    Console.WriteLine("Para pagamento via App Ava Parking, digite 1. Ou siga para o guichê de pagamento à vista.");
                    Console.WriteLine("");
                    string opcaoDePagamento = Console.ReadLine();
                    if (int.TryParse(opcaoDePagamento, out int opcaoPagamentoApp))
                    {
                        if (opcaoPagamentoApp == 1)
                        {
                            bool menuCelular = true;
                            while (menuCelular)
                            {
                                Console.Clear();
                                Console.WriteLine("Escolha sua próxima ação:");
                                Console.WriteLine("1- *Pegar celular iPhone*");
                                Console.WriteLine("2- *Pegar celular Android*");
                                Console.WriteLine("3- *Desistir e pagar à vista*");
                                Console.WriteLine("");
                                string opcaoCelular = Console.ReadLine();

                                switch (opcaoCelular)
                                {
                                    case "1":
                                        Smartphone iPhone = new Iphone("1234-5678", "iPhone 11", "75626348", 64, new List<string>(), new List<string>(), veiculosEstacionados);
                                        iPhone.TirarCelularDoBolso(iPhone.Modelo);
                                        break;
                                    case "2":
                                        Smartphone Galaxy = new Android("9876-5432", "Galaxy S22", "75776311", 128, new List<string>(), new List<string>(), veiculosEstacionados);
                                        Galaxy.TirarCelularDoBolso(Galaxy.Modelo);
                                        break;
                                    case "3":
                                        Console.Clear();
                                        Console.WriteLine("Obrigado por utilizar o AvaParking! Até a próxima");
                                        veiculosEstacionados.Remove(veiculoEmRemocao);
                                        menuCelular = false;
                                        break;
                                    default:
                                        menuCelular = false;
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Obrigado por utilizar o AvaParking! Até a próxima");
                        Console.WriteLine("");
                        veiculosEstacionados.Remove(veiculoEmRemocao);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente.");
            }
        }

        public void ListarVeiculos()
        {
            Console.Clear();
            Console.WriteLine("Digite a senha de gerenciador para listar os veículos estacionados:");
            string senhaDigitada = Console.ReadLine();
            string senhaAdm = "04042000"; // Data de Fundação da Avanade;

            if (senhaDigitada == senhaAdm)
            {
                if (veiculosEstacionados.Any())
                {
                    Console.Clear();
                    Console.WriteLine("Os veículos estacionados são:");
                    foreach (var veiculo in veiculosEstacionados)
                    {
                        Console.WriteLine("Placa: " + veiculo.Placa + "   Entrada: " + veiculo.DataEntrada.ToString() + "  Categoria: " + ObterCategoriaPorCodigo(veiculo.Categoria) + "  Seguro: " + (veiculo.DesejaSeguro ? "Contratado" : "Sem seguro"));
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Não há veículos estacionados.");
                }

            }
            else
            {
                Console.WriteLine("Senha incorreta.");
            }

        }

        private string SensorDePlacas()
        {
            Random random = new Random();

            char letra1 = (char)random.Next('A', 'Z' + 1);
            char letra2 = (char)random.Next('A', 'Z' + 1);

            int numero1 = random.Next(0, 10);
            int numero2 = random.Next(0, 10);
            int numero3 = random.Next(0, 10);

            string placa = $"{letra1}{letra2}{numero1}{numero2}{letra2}{numero3}{numero3}";

            return placa;
        }

        private string ConsultarPlacaViaAPI(string placa)
        {
            return "Trata-se de uma moto";
            try
            {
                DotNetEnv.Env.Load();
                string apiUrl = Environment.GetEnvironmentVariable("API_PLACAS_URL");
                string apiKey = Environment.GetEnvironmentVariable("API_PLACAS_KEY");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync($"{apiUrl}{placa}/{apiKey}").Result;

                    string respostaApi = response.Content.ReadAsStringAsync().Result;

                    JsonDocument jsonDocument = JsonDocument.Parse(respostaApi);

                    string marca = jsonDocument.RootElement.GetProperty("MARCA").GetString();
                    string modelo = jsonDocument.RootElement.GetProperty("MODELO").GetString();
                    string restricao = jsonDocument.RootElement.GetProperty("situacao").GetString();

                    string respostaApiTratada = $"Marca: {marca}, Modelo: {modelo}, Restrição: {restricao}";

                    return respostaApiTratada;
                }
            }
            catch
            {
                return "ERROR";
            }
        }


        private string ConsultarPlacaViaIA(string respostaApiTratada)
        {
            try
            {
                DotNetEnv.Env.Load();
                Console.WriteLine("Cheguei aqui.");
                string apiKey = Environment.GetEnvironmentVariable("API_IA_KEY");
                Console.WriteLine("Cheguei aqui.");
                string queryApi = "Você receberá uma mensagem do tipo Marca: {marca}, Modelo: {modelo}, Restrição: {restricao}. Você deverá analisar e responder com a categoria do veículo. Ex: 1, 2, 3 ou VEICULO ROUBADO. [1 - Moto, 2 - Carro, 3 - Utilitário ou VEICULO ROUBADO - Se o campo restrição apresentar informação de roubo. Caso a resposta não faça sentido, responda ERROR. Não responda nada mais que essas opções.";
                Console.WriteLine("Cheguei aqui.");
                OpenAIAPI api = new OpenAIAPI(apiKey);
                Console.WriteLine("Cheguei aqui.");
                conversacaoComIA = api.Chat.CreateConversation();
                conversacaoComIA.Model = Model.ChatGPTTurbo;
                conversacaoComIA.RequestParameters.Temperature = 0;
                conversacaoComIA.AppendSystemMessage(queryApi);

                conversacaoComIA.AppendUserInput(respostaApiTratada);
                Console.WriteLine("Cheguei aqui.");
                string result = conversacaoComIA.GetResponseFromChatbotAsync().Result;
                return result.ToString();
            }
            catch
            {
                return "ERROR";
            }
        }

        private int InformarCategoriaManualmente()
        {
            while (true)
            {
                Console.WriteLine("Informe a categoria de seu veículo: \n" +
                                  "1 - Moto\n" +
                                  "2 - Carro\n" +
                                  "3 - Utilitário\n");

                int categoriaDoVeiculoEstacionado = int.Parse(Console.ReadLine());

                switch (categoriaDoVeiculoEstacionado)
                {
                    case 1:
                    case 2:
                    case 3:
                        return categoriaDoVeiculoEstacionado;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                }
            }
        }

        private decimal CalcularPrecoSeguroPorCategoria(int categoriaDoVeiculoEstacionado)
        {
            switch (categoriaDoVeiculoEstacionado)
            {
                case 1:
                    return 1.00M;
                case 2:
                    return 2.00M;
                case 3:
                    return 3.00M;
                default:
                    return 0;
            }
        }

        public decimal CalcularPrecoTotalDoServicoPorCategoria(DateTime dataDeEntrada, int categoriaDoVeiculoEstacionado, bool confirmacaoDeSeguro)
        {
            decimal precoTotal;

            double minutosDecorridos = (DateTime.Now - dataDeEntrada).TotalMinutes;

            if (minutosDecorridos <= 15)
            {
                return 0;
            }
            else
            {
                switch (categoriaDoVeiculoEstacionado)
                {
                    case 1:
                        precoInicial = 3.00M;
                        break;
                    case 2:
                        precoInicial = 5.00M;
                        break;
                    case 3:
                        precoInicial = 8.00M;
                        break;
                    default:
                        precoInicial = 5.00M;
                        break;
                }

                precoTotal = precoInicial + precoPorHora * (decimal)Math.Ceiling(minutosDecorridos / 60.0);

                if (confirmacaoDeSeguro)
                {
                    precoTotal += CalcularPrecoSeguroPorCategoria(categoriaDoVeiculoEstacionado);
                }

                return precoTotal;

            }

        }

        private string ObterCategoriaPorCodigo(int categoria)
        {
            switch (categoria)
            {
                case 1:
                    return "Moto";
                case 2:
                    return "Carro";
                case 3:
                    return "Utilitário";
                default:
                    return "Não identificada";
            }
        }

        private bool DesejaAdicionarSeguro(int categoria)
        {
            Console.Clear();
            Console.WriteLine($"Deseja contratar seguro de estacionamento para {ObterCategoriaPorCodigo(categoria)} por apenas R${CalcularPrecoSeguroPorCategoria(categoria)}? \n Digite 1 para SIM, ou qualquer tecla para ignorar.");
            bool confirmacaoDeSeguro = Console.ReadLine() == "1" ? true : false;
            if (confirmacaoDeSeguro)
            {
                Console.WriteLine("Contratando seguro...");
                Thread.Sleep(2000);
                Console.WriteLine("Apólice gerada com sucesso!");
                Thread.Sleep(2000);
                Console.Clear();
            }
            return confirmacaoDeSeguro;
        }

        private void AcionarPolicia()
        {
            Smartphone BotPhone = new Android("1234-5678", "BotPhone 3000", "75626348", 64, new List<string>(), new List<string>(), veiculosEstacionados);
            BotPhone.ChamadorEmergenciaBot("190");
            Console.ReadLine();
        }

        private bool ValidarPlaca(string placa)
        {
            string padrao = @"^[A-Z]{3}\d{4}[A-Z]?$";
            return Regex.IsMatch(placa, padrao);
        }
    }
}

