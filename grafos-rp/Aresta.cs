namespace grafos_rp;

public class Aresta
{
    public int peso;
    public int vOrigemId;
    public int vDestinoId;
    public int fluxoMaximo;

    public Aresta(int peso, int vOrigemId, int vDestinoId, int fluxoMaximo)
    {
        this.peso = peso;
        this.vOrigemId = vOrigemId;
        this.vDestinoId = vDestinoId;
        this.fluxoMaximo = fluxoMaximo;
    }

    public override string ToString()
    {
        string str = $"(aresta: {vOrigemId},{vDestinoId}, peso: {peso}, fluxo maximo: {fluxoMaximo})";
        
        return str;
    }
}