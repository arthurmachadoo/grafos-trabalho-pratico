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
        string[,] matriz = new string[nVertices + 1, nVertices + 1];

        for (int i = 0; i <= nVertices; i++)
        {
            for (int j = 0; j <= nVertices; j++)
            {
                if (i == 0 && j == 0)
                    matriz[i, j] = "V";
                else if (i == 0 && j != 0)
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
        List<int>[] mat = new List<int>[nVertices];

        for (int i = 0; i < nVertices; i++)
        {
            List<int> temp = new List<int>();
            temp.Add(i + 1);
            mat[i] = temp;
        }

        foreach (Aresta a in arestas)
        {
            mat[a.vOrigemId - 1].Add(a.vDestinoId);
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
        string rep = calcDensidade() <= 0.5 ? representacao.lista.ToString() : representacao.matriz.ToString();
        Console.WriteLine("------------------------------");
        Console.WriteLine(
            $"Por conta da densidade do grafo ser igual a {calcDensidade()}, \nutilizamos a seguinte estrutura para representá-lo:");

        if (rep == representacao.lista.ToString())
        {
            leituraListaAdj();
        }
        else if (rep == representacao.matriz.ToString())
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
        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices.");
            return;
        }

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

        int rsp = 0;
        int origem = 0;
        int vDestino = nVertices + 1;

        while (rsp != 1 && rsp != 2)
        {
            Console.WriteLine($"Deseja escolher o vértice de origem e destino? \n1) sim \n2) nao");
            bool ehNumerico = int.TryParse(Console.ReadLine(), out rsp);

            if (!ehNumerico)
            {
                Console.WriteLine("Opção inválida. Digite 1 para sim ou 2 para não.");
            }

            if (rsp == 1)
            {
                while (origem <= 0 || origem > nVertices)
                {
                    Console.WriteLine($"Digite o vértice de origem:");
                    origem = int.Parse(Console.ReadLine());
                }

                while (vDestino <= 0 || vDestino > nVertices)
                {
                    Console.WriteLine($"Digite o vértice de destino:");
                    vDestino = int.Parse(Console.ReadLine());
                }
            }
            else if (rsp == 2)
            {
                origem = 1;
                vDestino = nVertices;
            }
        }

        distancias[origem] = 0;
        var vertOrigem = vertices.First(v => v.id == origem);
        vertOrigem.distancia = 0;

        var adjacencia = new Dictionary<int, List<(int destino, int peso)>>();
        foreach (var v in vertices)
            adjacencia[v.id] = new List<(int, int)>();

        foreach (var a in arestas)
        {
            if (!adjacencia.ContainsKey(a.vOrigemId))
                adjacencia[a.vOrigemId] = new List<(int, int)>();

            adjacencia[a.vOrigemId].Add((a.vDestinoId, a.peso));
        }

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
                break;

            visitados.Add(atual);

            if (!adjacencia.ContainsKey(atual))
                continue;

            foreach (var arco in adjacencia[atual])
            {
                int idDestino = arco.destino;
                int peso = arco.peso;

                if (visitados.Contains(idDestino))
                    continue;

                if (distancias[atual] != int.MaxValue && distancias[atual] + peso < distancias[idDestino])
                {
                    distancias[idDestino] = distancias[atual] + peso;
                    anterior[idDestino] = atual;
                }
            }
        }

        foreach (Vertice v in vertices)
        {
            v.predecessores.Clear();
            v.distancia = distancias[v.id];
            if (anterior[v.id].HasValue)
                v.predecessores.Add(anterior[v.id].Value);
        }

        int custoMáximo = 0;
        Console.WriteLine($"Menores distâncias a partir do vértice {origem}:");

        foreach (Vertice v in vertices.OrderBy(x => x.id))
        {
            if (v.id <= vDestino)
            {
                if (v.distancia == int.MaxValue)
                    Console.WriteLine($"Vértice {v.id}: distância = ∞ (inacessível)");
                else
                {
                    Console.WriteLine($"Vértice {v.id}: distância = {v.distancia}");
                    custoMáximo += v.distancia;
                }
            }
        }

        Console.WriteLine($"Custo máximo: {custoMáximo}");
    }

    public void fluxoMaximoComEdmondsKarp()
    {

        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices.");
            return;
        }

        int origem = 1;
        int destino = nVertices;
        int opcao = 0;

        while (opcao != 1 && opcao != 2)
        {
            Console.WriteLine("\nDeseja escolher origem e destino para o fluxo máximo?");
            Console.WriteLine("1) Sim (escolher vértices)");
            Console.WriteLine("2) Não (usar 1º e último vértice)");

            bool ehNumerico = int.TryParse(Console.ReadLine(), out opcao);

            if (!ehNumerico || (opcao != 1 && opcao != 2))
            {
                Console.WriteLine("Opção inválida. Digite 1 ou 2.");
                opcao = 0;
                continue;
            }

            if (opcao == 1)
            {
                while (origem <= 0 || origem > nVertices)
                {
                    Console.WriteLine("Digite o vértice de origem:");
                    if (!int.TryParse(Console.ReadLine(), out origem))
                        origem = 0;
                }

                while (destino <= 0 || destino > nVertices)
                {
                    Console.WriteLine("Digite o vértice de destino:");
                    if (!int.TryParse(Console.ReadLine(), out destino))
                        destino = 0;
                }
            }
        }

        var capacidades = new Dictionary<(int, int), int>();
        var fluxoAresta = new Dictionary<(int, int), int>();

        foreach (var aresta in arestas)
        {
            capacidades[(aresta.vOrigemId, aresta.vDestinoId)] = aresta.fluxoMaximo;
            fluxoAresta[(aresta.vOrigemId, aresta.vDestinoId)] = 0;

            if (!capacidades.ContainsKey((aresta.vDestinoId, aresta.vOrigemId)))
            {
                capacidades[(aresta.vDestinoId, aresta.vOrigemId)] = 0;
                fluxoAresta[(aresta.vDestinoId, aresta.vOrigemId)] = 0;
            }
        }

        int fluxoMaximo = 0;
        var arestasCorte = new List<(int origem, int destino)>();

        while (true)
        {
            var antecessor = new Dictionary<int, int?>();
            var fila = new Queue<int>();

            foreach (var v in vertices)
                antecessor[v.id] = null;

            fila.Enqueue(origem);
            antecessor[origem] = -1;

            bool encontrouCaminho = false;

            while (fila.Count > 0 && !encontrouCaminho)
            {
                int atual = fila.Dequeue();

                foreach (var vizinho in vertices.Select(v => v.id))
                {
                    if (antecessor[vizinho] == null && capacidades.ContainsKey((atual, vizinho)) &&
                        capacidades[(atual, vizinho)] > 0)
                    {
                        antecessor[vizinho] = atual;
                        if (vizinho == destino)
                        {
                            encontrouCaminho = true;
                            break;
                        }

                        fila.Enqueue(vizinho);
                    }
                }
            }

            if (!encontrouCaminho)
                break;

            int gargalo = int.MaxValue;
            int vAtual = destino;
            while (vAtual != origem)
            {
                int antec = antecessor[vAtual].Value;
                gargalo = Math.Min(gargalo, capacidades[(antec, vAtual)]);
                vAtual = antec;
            }

            vAtual = destino;
            while (vAtual != origem)
            {
                int antec = antecessor[vAtual].Value;
                capacidades[(antec, vAtual)] -= gargalo;
                capacidades[(vAtual, antec)] += gargalo;
                fluxoAresta[(antec, vAtual)] += gargalo;
                fluxoAresta[(vAtual, antec)] -= gargalo;
                vAtual = antec;
            }

            fluxoMaximo += gargalo;
        }

        var visitadosCorte = new HashSet<int>();
        var filaCorte = new Queue<int>();
        filaCorte.Enqueue(origem);
        visitadosCorte.Add(origem);

        while (filaCorte.Count > 0)
        {
            int atual = filaCorte.Dequeue();
            foreach (var vizinho in vertices.Select(v => v.id))
            {
                if (!visitadosCorte.Contains(vizinho) &&
                    capacidades.ContainsKey((atual, vizinho)) &&
                    capacidades[(atual, vizinho)] > 0)
                {
                    visitadosCorte.Add(vizinho);
                    filaCorte.Enqueue(vizinho);
                }
            }
        }

        foreach (var aresta in arestas)
        {
            if (visitadosCorte.Contains(aresta.vOrigemId) &&
                !visitadosCorte.Contains(aresta.vDestinoId))
            {
                arestasCorte.Add((aresta.vOrigemId, aresta.vDestinoId));
            }
        }

        Console.WriteLine("\n========== RESULTADO EDMONDS-KARP ==========");
        Console.WriteLine($"Origem: {origem}, Destino: {destino}");
        Console.WriteLine($"Fluxo Máximo: {fluxoMaximo}");
        Console.WriteLine($"\nCorte Mínimo ({arestasCorte.Count} arestas):");

        int somaCapacidades = 0;
        foreach (var (orig, dest) in arestasCorte)
        {
            var arestaInfo = arestas.First(a => a.vOrigemId == orig && a.vDestinoId == dest);
            Console.WriteLine($"  Aresta: {orig} → {dest} (Capacidade: {arestaInfo.fluxoMaximo})");
            somaCapacidades += arestaInfo.fluxoMaximo;
        }

        Console.WriteLine($"\nCapacidade Total do Corte: {somaCapacidades}");
        Console.WriteLine("==========================================\n");
    }

    public void custoMínimo()
    {
        if (calcDensidade() <= 0.5)
        {
            Kruskal();
        }
        else
        {
            Prim();
        }

    }

    public void Kruskal()
    {
        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices.");
            return;
        }

        Console.WriteLine("\n========== ALGORITMO DE KRUSKAL ==========");

        var arestaOrdenada = arestas.OrderBy(a => a.peso).ToList();

        var conjuntos = new Dictionary<int, int>();
        var classificacao = new Dictionary<int, int>();

        foreach (var v in vertices)
        {
            conjuntos[v.id] = v.id;
            classificacao[v.id] = 0;
        }

        var agmArestas = new List<Aresta>();
        int custoTotalAGM = 0;

        foreach (var aresta in arestaOrdenada)
        {
            int raizOrigem = aresta.vOrigemId;
            while (conjuntos[raizOrigem] != raizOrigem)
                raizOrigem = conjuntos[raizOrigem];

            int raizDestino = aresta.vDestinoId;
            while (conjuntos[raizDestino] != raizDestino)
                raizDestino = conjuntos[raizDestino];

            if (raizOrigem != raizDestino)
            {
                agmArestas.Add(aresta);
                custoTotalAGM += aresta.peso;

                if (classificacao[raizOrigem] < classificacao[raizDestino])
                {
                    conjuntos[raizOrigem] = raizDestino;
                }
                else if (classificacao[raizOrigem] > classificacao[raizDestino])
                {
                    conjuntos[raizDestino] = raizOrigem;
                }
                else
                {
                    conjuntos[raizDestino] = raizOrigem;
                    classificacao[raizOrigem]++;
                }

                if (agmArestas.Count == vertices.Count - 1)
                    break;
            }
        }

        Console.WriteLine($"\nArvore Geradora Mínima (AGM) - {agmArestas.Count} arestas:");
        foreach (var aresta in agmArestas)
        {
            Console.WriteLine($"  {aresta.vOrigemId} → {aresta.vDestinoId} (Peso: {aresta.peso})");
        }

        Console.WriteLine($"\nCusto Total da AGM: {custoTotalAGM}");

        if (agmArestas.Count == vertices.Count - 1)
        {
            Console.WriteLine("✓ Árvore geradora mínima encontrada com sucesso!");
        }
        else
        {
            Console.WriteLine("✗ Grafo desconectado: não foi possível criar árvore geradora completa.");
        }

        Console.WriteLine("========================================\n");
    }


    public void Prim()
    {
        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices.");
            return;
        }

        Console.WriteLine("\n========== ALGORITMO DE PRIM ==========");

        var naArvore = new HashSet<int>();
        var agmArestas = new List<Aresta>();
        int custoTotalAGM = 0;

        naArvore.Add(vertices.First().id);

        while (naArvore.Count < vertices.Count)
        {
            Aresta arestaMinima = null;
            int pesoMinimo = int.MaxValue;

            foreach (var aresta in arestas)
            {
                bool origemNaArvore = naArvore.Contains(aresta.vOrigemId);
                bool destinoNaArvore = naArvore.Contains(aresta.vDestinoId);

                if ((origemNaArvore && !destinoNaArvore) || (!origemNaArvore && destinoNaArvore))
                {
                    if (aresta.peso < pesoMinimo)
                    {
                        pesoMinimo = aresta.peso;
                        arestaMinima = aresta;
                    }
                }
            }

            if (arestaMinima == null)
                break;

            agmArestas.Add(arestaMinima);
            custoTotalAGM += arestaMinima.peso;

            if (naArvore.Contains(arestaMinima.vOrigemId))
                naArvore.Add(arestaMinima.vDestinoId);
            else
                naArvore.Add(arestaMinima.vOrigemId);
        }

        Console.WriteLine($"\nArvore Geradora Mínima (AGM) - {agmArestas.Count} arestas:");
        foreach (var aresta in agmArestas)
        {
            Console.WriteLine($"  {aresta.vOrigemId} → {aresta.vDestinoId} (Peso: {aresta.peso})");
        }

        Console.WriteLine($"\nCusto Total da AGM: {custoTotalAGM}");

        if (agmArestas.Count == vertices.Count - 1)
        {
            Console.WriteLine("✓ Árvore geradora mínima encontrada com sucesso!");
        }
        else
        {
            Console.WriteLine("✗ Grafo desconectado: não foi possível criar árvore geradora completa.");
        }

        Console.WriteLine("=====================================\n");
    }

    public void agendamentoComWelshPowell()
    {
        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices (rotas).");
            return;
        }

        Console.WriteLine("\n========== AGENDAMENTO WELSH-POWELL ==========");

        var grauVertice = new Dictionary<int, int>();

        foreach (var v in vertices)
        {
            int grau = 0;
            foreach (var aresta in arestas)
            {
                if (aresta.vOrigemId == v.id || aresta.vDestinoId == v.id)
                    grau++;
            }

            grauVertice[v.id] = grau;
        }

        var verticesOrdenados = vertices.OrderByDescending(v => grauVertice[v.id]).ToList();

        var coloracao = new Dictionary<int, int>();
        foreach (var v in vertices)
            coloracao[v.id] = -1;

        int corAtual = 0;

        foreach (var vertice in verticesOrdenados)
        {
            if (coloracao[vertice.id] != -1)
                continue;

            coloracao[vertice.id] = corAtual;

            foreach (var outroVertice in verticesOrdenados)
            {
                if (coloracao[outroVertice.id] != -1)
                    continue;

                bool saoAdjacentes = false;
                foreach (var aresta in arestas)
                {
                    if ((aresta.vOrigemId == vertice.id && aresta.vDestinoId == outroVertice.id) ||
                        (aresta.vOrigemId == outroVertice.id && aresta.vDestinoId == vertice.id))
                    {
                        saoAdjacentes = true;
                        break;
                    }
                }

                if (!saoAdjacentes)
                {
                    bool podeColorir = true;
                    foreach (var jaColorido in coloracao.Where(kv => kv.Value == corAtual))
                    {
                        if (jaColorido.Key == outroVertice.id)
                            continue;

                        foreach (var aresta in arestas)
                        {
                            if ((aresta.vOrigemId == outroVertice.id && aresta.vDestinoId == jaColorido.Key) ||
                                (aresta.vOrigemId == jaColorido.Key && aresta.vDestinoId == outroVertice.id))
                            {
                                podeColorir = false;
                                break;
                            }
                        }

                        if (!podeColorir)
                            break;
                    }

                    if (podeColorir)
                        coloracao[outroVertice.id] = corAtual;
                }
            }

            corAtual++;
        }

        int numTurnos = coloracao.Values.Max() + 1;

        Console.WriteLine($"\n✓ Coloração de Vértices (Agendamento de Rotas):");
        Console.WriteLine($"Número Mínimo de Turnos: {numTurnos}");
        Console.WriteLine($"\nAlocação de Rotas por Turno:");

        for (int turno = 0; turno < numTurnos; turno++)
        {
            var rotasNoTurno = coloracao
                .Where(kv => kv.Value == turno)
                .Select(kv => kv.Key)
                .OrderBy(id => id)
                .ToList();

            Console.WriteLine(
                $"\n  Turno {turno + 1}: {string.Join(", ", rotasNoTurno)} (Total: {rotasNoTurno.Count} rotas)");
        }

        bool coloracaoValida = true;
        foreach (var aresta in arestas)
        {
            if (coloracao[aresta.vOrigemId] == coloracao[aresta.vDestinoId])
            {
                coloracaoValida = false;
                Console.WriteLine($"✗ ERRO: Aresta {aresta.vOrigemId} → {aresta.vDestinoId} tem vértices mesma cor!");
                break;
            }
        }

        if (coloracaoValida)
            Console.WriteLine($"\n✓ Coloração válida: Nenhum conflito de recursos detectado!");

        Console.WriteLine("\n============================================\n");
    }

    public void RotaUnicaDeInspecao()
    {
        if (vertices == null || vertices.Count == 0)
        {
            Console.WriteLine("Grafo vazio: sem vértices.");
            return;
        }

        Console.WriteLine("\n========== ROTA UNICA DE INSPECAO ==========");

        Console.WriteLine("\n----- CENÁRIO A: Percurso de Rotas (Caminho Euleriano) -----");
        VerificarEEncontrarCircuitoEuleriano();

        Console.WriteLine("\n----- CENÁRIO B: Percurso de Hubs (Caminho Hamiltoniano) -----");
        VerificarEEncontrarCircuitoHamiltoniano();

        Console.WriteLine("\n============================================\n");
    }

    private void VerificarEEncontrarCircuitoEuleriano()
    {
        var graus = new Dictionary<int, int>();

        foreach (var v in vertices)
            graus[v.id] = 0;

        foreach (var a in arestas)
        {
            graus[a.vOrigemId]++;
            graus[a.vDestinoId]++;
        }

        int verticesGrauImpar = 0;
        var verticesImpares = new List<int>();

        foreach (var kv in graus)
        {
            if (kv.Value % 2 != 0)
            {
                verticesGrauImpar++;
                verticesImpares.Add(kv.Key);
            }
        }

        if (verticesGrauImpar == 0)
        {
            Console.WriteLine("\n✓ CIRCUITO EULERIANO EXISTE!");
            var circuito = EncontrarCircuitoEulerianoOtimizado();
            if (circuito != null && circuito.Count > 0)
            {
                Console.WriteLine($"\n  Circuito Euleriano encontrado ({circuito.Count} rotas):");
                Console.Write("  Sequência de hubs: ");
                int limite = Math.Min(50, circuito.Count);
                for (int i = 0; i < limite; i++)
                {
                    Console.Write(circuito[i]);
                    if (i < limite - 1)
                        Console.Write(" → ");
                }
                if (circuito.Count > 50)
                    Console.Write(" → ... (circuito completo)");
                Console.WriteLine();
            }
        }
        else if (verticesGrauImpar == 2)
        {
            Console.WriteLine("\n⚠ CAMINHO EULERIANO EXISTE!");
            Console.WriteLine($"  Vértices com grau ímpar: {string.Join(", ", verticesImpares)}");
        }
        else
        {
            Console.WriteLine("\n✗ NÃO EXISTE CIRCUITO OU CAMINHO EULERIANO!");
            Console.WriteLine($"  {verticesGrauImpar} vértices com grau ímpar");
        }
    }

    private List<int> EncontrarCircuitoEulerianoOtimizado()
    {
        if (arestas.Count == 0)
            return new List<int>();

        var arestasCopia = new LinkedList<(int origem, int destino)>();
        foreach (var a in arestas)
            arestasCopia.AddLast((a.vOrigemId, a.vDestinoId));

        var pilha = new Stack<int>();
        var circuito = new List<int>();
        int verticeAtual = vertices.First().id;
        pilha.Push(verticeAtual);

        while (pilha.Count > 0)
        {
            verticeAtual = pilha.Peek();
            bool encontrouAresta = false;

            var node = arestasCopia.First;
            while (node != null)
            {
                var proximo = node.Next;
                if (node.Value.origem == verticeAtual)
                {
                    pilha.Push(node.Value.destino);
                    arestasCopia.Remove(node);
                    encontrouAresta = true;
                    break;
                }
                node = proximo;
            }

            if (!encontrouAresta)
                circuito.Add(pilha.Pop());
        }

        circuito.Reverse();
        return circuito;
    }

    private void VerificarEEncontrarCircuitoHamiltoniano()
    {
        Console.WriteLine("\nProcurando Circuito Hamiltoniano...");
        
        int verticeInicio = vertices.First().id;
        bool[] visitados = new bool[nVertices + 1];
        var caminho = new List<int>();

        if (BuscaHamiltoniana(verticeInicio, verticeInicio, visitados, caminho))
        {
            Console.WriteLine("\n✓ CIRCUITO HAMILTONIANO EXISTE!");
            Console.WriteLine($"  Circuito encontrado ({caminho.Count} vértices):");
            Console.Write("  Sequência de hubs: ");
            for (int i = 0; i < caminho.Count; i++)
            {
                Console.Write(caminho[i]);
                if (i < caminho.Count - 1)
                    Console.Write(" → ");
                else
                    Console.Write(" → " + verticeInicio);
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("\n✗ NÃO EXISTE CIRCUITO HAMILTONIANO!");
        }
    }

    private bool BuscaHamiltoniana(int verticeAtual, int verticeInicio, bool[] visitados, List<int> caminho)
    {
        visitados[verticeAtual] = true;
        caminho.Add(verticeAtual);

        if (caminho.Count == vertices.Count)
        {
            bool existeArestaRetorno = false;
            foreach (var aresta in arestas)
            {
                if (aresta.vOrigemId == verticeAtual && aresta.vDestinoId == verticeInicio)
                {
                    existeArestaRetorno = true;
                    break;
                }
            }

            if (existeArestaRetorno)
                return true;

            visitados[verticeAtual] = false;
            caminho.RemoveAt(caminho.Count - 1);
            return false;
        }

        foreach (var aresta in arestas)
        {
            int proximo = -1;
            
            if (aresta.vOrigemId == verticeAtual)
                proximo = aresta.vDestinoId;
            else if (aresta.vDestinoId == verticeAtual)
                proximo = aresta.vOrigemId;

            if (proximo > 0 && proximo <= nVertices && !visitados[proximo])
            {
                if (BuscaHamiltoniana(proximo, verticeInicio, visitados, caminho))
                    return true;
            }
        }

        visitados[verticeAtual] = false;
        caminho.RemoveAt(caminho.Count - 1);
        return false;
    }
}
