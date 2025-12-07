using System.IO;
using grafos_rp;

internal class Program
{
    public static void Main(string[] args)
    {
        Grafo grafo = new Grafo();
        var leitor = grafo.leitor;

        grafo.preencheVerticesEArestas();
        Console.WriteLine($"v: {leitor.qtdVertices()} a: {leitor.qtdArestas()}");
        Console.WriteLine($"densidade: {leitor.densidade()}");
        Console.WriteLine(grafo.representacaoGrafo());
    }
}