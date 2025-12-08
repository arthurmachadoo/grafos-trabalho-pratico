using System.IO;
using grafos_rp;

internal class Program
{
    public static void Main(string[] args)
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

        Grafo grafo = new Grafo(grafos[3]);
        grafo.preencheVerticesEArestas();
        grafo.representacaoGrafo();
        grafo.caminhoMinimoComDijkstra();
        
        // Roteamento menor custo
        // Capacidade maxima de escoamento
        // Expansao da rede de comunicacao
        // Agendamento de manutencoes sem conflito
        // Rota unica de especao
    }
}
