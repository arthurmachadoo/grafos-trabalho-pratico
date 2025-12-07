using System.IO;

namespace grafos_rp;

public class Leitor
{
    public string caminho;

    public Leitor(string caminho)
    {
        this.caminho = caminho;
    }

    public string[] lerLinhas()
    {
        if (File.Exists(caminho))
        {
            return File.ReadAllLines(caminho);
        }
        return [];
    }

    private string[] lerPrimeiraLinha()
    {
        var primeiraLinha =  lerLinhas()[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return primeiraLinha;
    }

    public int qtdVertices()
    {
        return int.Parse(lerPrimeiraLinha()[0]);
    }

    public int qtdArestas()
    {
        return int.Parse(lerPrimeiraLinha()[1]);
    }

    public double densidade()
    {
        return (double)qtdArestas() / (qtdVertices() * (qtdVertices() - 1));
    }
    
    // public void Leitura()
    // {
    //     if (File.Exists(caminho))
    //     {
    //         string[] linhas = File.ReadAllLines(caminho);
    //         var primeiraLinha = linhas[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //         nVertices = int.Parse(primeiraLinha[0]);
    //         nArestas = int.Parse(primeiraLinha[1]);
    //             
    //         foreach (string linha in linhas)
    //         {
    //             if (linha != linhas[0])
    //             {
    //                 var linhaAtual = linha.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //                 int id = int.Parse(linhaAtual[0]); 
    //                 int sucessor = int.Parse(linhaAtual[1]);
    //                 var pesoAresta = linhaAtual[2];
    //                 var fluxoMaximo = linhaAtual[3];
    //
    //
    //
    //             }
    //                 
    //             //linha pos 0 = vertice
    //             //linha pos 1 = vertice sucessor
    //             //linha pos 2 = peso aresta
    //             //linha pos 3 = fluxo maximo
    //                 
    //                 
    //             Console.WriteLine(calcDensidade() > 0.5 ? leituraMatrizAdj() : leituraListaAdj());
    //
    //         }
    //     }
    // }
}