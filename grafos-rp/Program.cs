using System.IO;
using grafos_rp;

internal class Program
{
    public static void Main(string[] args)
    {
        Logger.Initialize();
        try
        {
            Menu();
        }
        finally
        {
            Logger.Close();
        }
    }

    public static void Menu()
    {
        string [] grafos = 
        [
            "grafo01.dimacs",
            "grafo02.dimacs",
            "grafo03.dimacs",
            "grafo04.dimacs",
            "grafo05.dimacs",
            "grafo06.dimacs",
            "grafo07.dimacs"
        ];

        Grafo grafo = null;
        
        Logger.WriteLine("Entrega Máxima Logística S.A.");
        Logger.WriteLine("CEO's: Arthur Machado Ferreira, Alan Torres de Sá e Vitor Ribeiro");
        Logger.WriteLine("=================================================================");
        Logger.WriteLine("Rotas Logísticas Disponíveis para Otimização:");
        int nRotas = 1;
        foreach (string s in grafos)
        {
            Logger.WriteLine($"Rota {nRotas}: {s}");
            nRotas++;
        }
        Logger.WriteLine("=================================================================");

        Logger.WriteLine("Qual Rota Logistíca você gostaria de otimizar?");
        
        int rsp = 0;
        bool ehNumerico = false;
        while (!ehNumerico) {
            ehNumerico = int.TryParse(Console.ReadLine(), out rsp);

            if (!ehNumerico)
            {
                Logger.WriteLine("Opção inválida. Digite um número de 1 a 7.");
            }
        }

        switch (rsp)
        {
            case 1:
                grafo = new Grafo(grafos[rsp-1]);
                grafo.preencheVerticesEArestas();
                break;
            case 2:
                grafo = new Grafo(grafos[rsp-1]);
                grafo.preencheVerticesEArestas();
                break;
            case 3:
                grafo = new Grafo(grafos[rsp-1]);
                grafo.preencheVerticesEArestas();
                break;
            case 4:
                grafo = new Grafo(grafos[rsp-1]);
                grafo.preencheVerticesEArestas();
                break;
            case 5:
                grafo = new Grafo(grafos[rsp-1]);
                grafo.preencheVerticesEArestas();
                break;
            case 6:
                grafo = new Grafo(grafos[rsp - 1]);
                grafo.preencheVerticesEArestas();
                break;
            case 7:
                grafo = new Grafo(grafos[rsp - 1]);
                grafo.preencheVerticesEArestas();
                break;
            default:
                Logger.WriteLine("Opção inválida. Encerrando o programa.");
                return;
        }

        Logger.WriteLine($"\nRota {rsp} carregada com sucesso!");
        Logger.WriteLine($"Vértices: {grafo.nVertices} | Arestas: {grafo.nArestas}");

        int operacoesRsp = 0;
        bool execucaoPrograma = true;
        
        while (execucaoPrograma)
        {
            Logger.WriteLine("\nDada a Rota Logística selecionada, segue as opções disponíveis para serem realizadas:");
            Logger.WriteLine("1 - Exibir Rota Logística como um Grafo");
            Logger.WriteLine("2 - Roteamento com menor custo usando Dijkstra");
            Logger.WriteLine("3 - Capacidade máxima de escoamento com Edmonds-Karp");
            Logger.WriteLine("4 - Expansao da Rede de comunicação com Custo Mínimo");
            Logger.WriteLine("5 - Agendamento de manutenções sem conflito com Welsh-Powell");
            Logger.WriteLine("6 - Rota Única de Inspeção");
            Logger.WriteLine("Digite qualquer outro número para encerrar o programa.");
            Logger.WriteLine("Escolha uma opção: ");

            bool entradaValida = false;
            while (!entradaValida)
            {
                entradaValida = int.TryParse(Console.ReadLine(), out operacoesRsp);

                if (!entradaValida)
                {
                    Logger.WriteLine("Opção inválida. Digite um número válido.");
                }
            }

            switch (operacoesRsp)
            {
                case 1:
                    grafo.representacaoGrafo();
                    break;
                case 2:
                    grafo.caminhoMinimoComDijkstra();
                    break;
                case 3:
                    grafo.fluxoMaximoComEdmondsKarp();
                    break;
                case 4:
                    grafo.custoMínimo();
                    break;
                case 5:
                    grafo.agendamentoComWelshPowell();
                    break;
                case 6:
                    grafo.RotaUnicaDeInspecao();
                    break;
                default:
                    Logger.WriteLine("Opção inválida. Encerrando o programa.");
                    execucaoPrograma = false;
                    break;
            }
        }

        Logger.WriteLine("\nPrograma encerrado com sucesso!");
    }
}

