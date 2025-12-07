using System.Collections.Generic;

namespace grafos_rp;

public class Vertice
{
    public int id;
    public List<int> sucessores = new List<int>();
    public List<int> predecessores = new List<int>();
    
    public Vertice(int id)
    {
        this.id = id;
    }

    public void addSucessor(int sucessor)
    {
        sucessores.Add(sucessor);
    }

    public void addPredecessor(int predecessor)
    {
        predecessores.Add(predecessor);
    }

    public string suc()
    {
        string str = "";

        foreach (var s in sucessores)
        {
            str += $"{s} ";
        }
        
        return str;
    }

    public string pred()
    {
        string str = "";

        foreach (var p in predecessores)
        {
            str += $"{p} ";
        }
        return str;
    }

    public override string ToString()
    {
        string str = $"id: {id}, predecessores: {pred()}, sucessores: {suc()} ";
        return str;
    }
}