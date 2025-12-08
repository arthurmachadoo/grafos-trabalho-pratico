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
    public string[] grafos;
    public Leitor leitor;
    

    public Grafo(string caminho)
    {
        leitor = new Leitor(caminho);
    }
    public void preencheVerticesEArestas()
    {
        var linhas = leitor.lerLinhas();
        nVertices = int.Parse(leitor.lerPrimeiraLinha()[0]);
        nArestas = int.Parse(leitor.lerPrimeiraLinha()[1]);
        
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
            }
        }
        adicionaTodosOsVertices();
        adicionaSucessoresEPredecessores();
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
    private void adicionaTodosOsVertices()
    {
        for (int i = 1; i <= nVertices; i++)
        {
            vertices.Add(new Vertice(i));
        }
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
        List<int> [] mat = new List<int>[nVertices];
        
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

    public void caminhoMinimoComDijkstra()
    {
        // Implementação correta do algoritmo de Dijkstra que calcula a menor
        // distância da raiz (primeiro vértice em `vertices`) para todos os outros
        // vértices. Atualiza o campo `distancia` de cada `Vertice` e preenche
        // a lista `predecessores` com o predecessor imediato no caminho mínimo.
        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices.");
            return;
        }

        // Inicialização
        var distancias = new Dictionary<int, int>();
        var anterior = new Dictionary<int, int?>();
        var visitados = new HashSet<int>();

        foreach (var v in vertices)
        {
            distancias[v.id] = int.MaxValue;
            anterior[v.id] = null;
            v.predecessores.Clear();
            v.distancia = int.MaxValue;
        }

        int origem = vertices.First().id;
        distancias[origem] = 0;
        vertices.First().distancia = 0;

        // Construir lista de adjacência para relaxamento eficiente
        var adjacencia = new Dictionary<int, List<(int destino, int peso)>>();
        foreach (var v in vertices)
            adjacencia[v.id] = new List<(int, int)>();

        foreach (var a in arestas)
        {
            if (!adjacencia.ContainsKey(a.vOrigemId))
                adjacencia[a.vOrigemId] = new List<(int, int)>();

            adjacencia[a.vOrigemId].Add((a.vDestinoId, a.peso));
        }

        // Laço principal de Dijkstra (versão com busca linear do vértice não visitado
        // com menor distância). Para grafos pequenos/educacionais isto é suficiente.
        while (visitados.Count < vertices.Count)
        {
            int atual = -1;
            int melhor = int.MaxValue;

            foreach (var par in distancias)
            {
                if (visitados.Contains(par.Key))
                    continue;

                if (par.Value < melhor)
                {
                    melhor = par.Value;
                    atual = par.Key;
                }
            }

            if (atual == -1)
                break; // todos os vértices restantes são inacessíveis

            visitados.Add(atual);

            if (!adjacencia.ContainsKey(atual))
                continue;

            foreach (var arco in adjacencia[atual])
            {
                int idDestino = arco.destino;
                int peso = arco.peso;

                if (visitados.Contains(idDestino))
                    continue;

                // relaxamento
                if (distancias[atual] != int.MaxValue && distancias[atual] + peso < distancias[idDestino])
                {
                    distancias[idDestino] = distancias[atual] + peso;
                    anterior[idDestino] = atual;
                }
            }
        }

        // Atualiza os objetos Vertice com as distâncias e predecessores encontrados
        foreach (var v in vertices)
        {
            v.predecessores.Clear();
            v.distancia = distancias[v.id];
            if (anterior[v.id].HasValue)
                v.predecessores.Add(anterior[v.id].Value);
        }

        // Exibe resultados
        Console.WriteLine($"Menores distâncias a partir do vértice {origem}:");
        foreach (var v in vertices.OrderBy(x => x.id))
        {
            if (v.distancia == int.MaxValue)
                Console.WriteLine($"Vértice {v.id}: distância = ∞ (inacessível)");
            else
                Console.WriteLine($"Vértice {v.id}: distância = {v.distancia}");
        }
    }
}
