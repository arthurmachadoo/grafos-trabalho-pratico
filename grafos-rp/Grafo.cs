using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace grafos_rp;

public class Grafo
{
    public List<Aresta> arestas = new List<Aresta>();
    public List<Vertice> vertices = new List<Vertice>();
    public double densidade;
    public int nVertices;
    public int nArestas;
    public string[] grafos = ["/Users/arthur/RiderProjects/grafos-rp/grafos-rp/dados/grafo01.dimacs"];
    public Leitor leitor;
    

    public Grafo(/*List<Aresta>? arestas, List<Vertice> vertice, double densidade*/)
    {
        leitor = new Leitor(grafos[0]);
        // grafos =
        // [
        //     // "grafos-rp/dados/grafo01.dimacs", "grafos-rp/dados/grafo02.dimacs",
        //     // "grafos-rp/dados/grafo03.dimacs", "grafos-rp/dados/grafo04.dimacs", "grafos-rp/dados/grafo05.dimacs",
        //     // "grafos-rp/dados/grafo06.dimacs", "grafos-rp/dados/grafo07.dimacs"
        // ];
    }

    public void preencheVerticesEArestas()
    {
        var linhas = leitor.lerLinhas();
        List<int> verticesComRepeticao = new List<int>();
        foreach (string linha in linhas)
        {
            if (linha != linhas[0])
            {
                var linhaAtual = linha.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int idVertice = int.Parse(linhaAtual[0]); 
                int idSucessor = int.Parse(linhaAtual[1]);
                var pesoAresta = int.Parse(linhaAtual[2]);
                var fluxoMaximo = int.Parse(linhaAtual[3]);
                
                arestas.Add(new Aresta(pesoAresta, idVertice, idSucessor, fluxoMaximo));
                verticesComRepeticao.Add(idVertice);
                verticesComRepeticao.Add(idSucessor);

            }
        }
        adicionaTodosOsVertices(verticesComRepeticao);
        adicionaSucessoresEPredecessores();

        foreach (Vertice v in vertices)
        {
            Console.WriteLine(v.ToString());
        }

        foreach (Aresta a in arestas)
        {
            Console.WriteLine(a.ToString());
        }
        
        
    }
    private void adicionaSucessoresEPredecessores()
    {
        foreach (Aresta aresta in arestas)
        {
            foreach (Vertice v in vertices)
            {
                if (v.id == aresta.vOrigemId)
                {
                    v.sucessores.Add(aresta.vDestinoId);
                }

                if (v.id == aresta.vDestinoId)
                {
                    v.predecessores.Add(aresta.vOrigemId);
                }
            }
        }
        
    }
    private void adicionaTodosOsVertices(List<int> verticesComRepeticao)
    {
        List<int> temp = verticesComRepeticao.Distinct().ToList();
        foreach (int i in temp)
        {
            vertices.Add(new Vertice(i));
        }
    }
    public List<string> leituraMatrizAdj()
    {
        return ["sra"];
    }
    public List<string> leituraListaAdj()
    {
        string[,] mat = new string[nVertices, nArestas];
        return ["sra"];
    }
    public double calcDensidade()
    {
        return (double)nArestas / (nVertices * (nVertices - 1));
    }
    public string representacaoGrafo()
    {
        string rep = calcDensidade() <= 0.5 ?  representacao.matriz.ToString() :  representacao.lista.ToString();
        return rep;
    }
    public enum representacao
    {
        matriz,
        lista
    }
}
