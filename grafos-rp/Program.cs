using System.IO;
using grafos_rp;

internal class Program
{
    public static void Main(string[] args)
    {
        Grafo grafo = new Grafo();
        grafo.preencheVerticesEArestas();
        Console.WriteLine($"Vertices: {grafo.vertices.Count}");
        Console.WriteLine($"Arestas: {grafo.arestas.Count}");
        grafo.representacaoGrafo();
        
    }
}