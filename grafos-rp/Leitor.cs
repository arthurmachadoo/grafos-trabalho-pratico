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
}