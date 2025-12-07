using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace grafos_rp;

public class Grafo
{
    public List<Aresta> arestas = new List<Aresta>();
    public List<Vertice> vertices = new List<Vertice>();
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
        atualizaQtdDeVerticesEArestas();
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
                    v.addSucessor(aresta.vDestinoId);
                }

                if (v.id == aresta.vDestinoId)
                {
                    v.addPredecessor(aresta.vOrigemId);
                }
            }
        }
    }
    private void adicionaTodosOsVertices(List<int> verticesComRepeticao)
    {
        List<int> tmp = new List<int>();
        foreach (int i in verticesComRepeticao)
        {
            if (!tmp.Contains(i))
            {
                tmp.Add(i);
            }
        }
        tmp.Sort();
        
        foreach (int i in tmp)
        {
            vertices.Add(new Vertice(i));
        }
    }
    public void atualizaQtdDeVerticesEArestas()
    {
        nVertices = vertices.Count;
        nArestas = arestas.Count;
    }
    public void leituraMatrizAdj()
    {
        string [,] matriz = new string[nVertices+1, nVertices+1];
        
        for (int i = 0; i <= nVertices; i++)
        {
            for (int j = 0; j<= nVertices; j++)
            {
                if (i == 0 && j == 0)
                    matriz[i, j] = "V";
                else if (i == 0 && j!=0)
                    matriz[i, j] = $"{j}";
                else if (j == 0 && i != 0)
                {
                    matriz[i, j] = $"{i}";
                }
                else
                    matriz[i, j] = $"{0}";
            }
        }
        
        foreach (Aresta a in arestas)
        {
            matriz[a.vOrigemId, a.vDestinoId] = $"{a.peso}";
        }

        Console.WriteLine("---- MATRIZ DE ADJACÊNCIA ----");
        for (int i = 0; i <= nVertices; i++)
        {
            for (int j = 0; j <= nVertices; j++)
            {
                Console.Write($"{matriz[i, j]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("------------------------------");
    }
    public void leituraListaAdj()
    {
        List<int> [] mat = new List<int>[500];
        
        for (int i = 0; i < nVertices; i++)
        {
            List<int> temp = new List<int>();
            temp.Add(i+1);
            mat[i] = temp;
        }
        
        foreach (Aresta a in arestas)
        {
            mat[a.vOrigemId-1].Add(a.vDestinoId);
        }

        Console.WriteLine("---- LISTA DE ADJACÊNCIA ----");
        for (int i = 0; i < nVertices; i++)
        {
            List<int> lista = mat[i];
            foreach (int v in lista)
            {
                Console.Write($"{v} ");
            }

            Console.WriteLine();
        }
        Console.WriteLine("-----------------------------");
    }
    public double calcDensidade()
    {
        return (double)nArestas / (nVertices * (nVertices - 1));
    }
    public void representacaoGrafo()
    {
        string rep = calcDensidade() <= 0.5 ?  representacao.lista.ToString() :  representacao.matriz.ToString();
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Por conta da densidade do grafo ser igual a {calcDensidade()}, \nutilizamos a seguinte estrutura para representá-lo:");
        
        if (rep == representacao.lista.ToString())
        {
            leituraListaAdj();
        } else if (rep == representacao.matriz.ToString())
        {
            leituraMatrizAdj();
        }
    }
    public enum representacao
    {
        matriz,
        lista
    }
}
