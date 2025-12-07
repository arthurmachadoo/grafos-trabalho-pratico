using System.IO;
using System.Reflection;

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
        string caminhoAbsoluto = Path.Combine(AppContext.BaseDirectory, caminho).Replace("/bin/Debug/net9.0", "");
        if (File.Exists(caminhoAbsoluto))
        {
            return File.ReadAllLines(caminhoAbsoluto);
        }
        return [];
    }

    public string[] lerPrimeiraLinha()
    {
        string[] linhas = lerLinhas();
        var linhaAtual = linhas[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return linhaAtual;
    }
}