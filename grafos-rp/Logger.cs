using System;
using System.IO;

namespace grafos_rp;

public static class Logger
{
    private static StreamWriter? _logWriter;
    private static readonly object _lock = new object();
    private static string _logFilePath = "logs.txt";

    /// <summary>
    /// Inicializa o logger, criando ou limpando o arquivo de log
    /// </summary>
    public static void Initialize()
    {
        lock (_lock)
        {
            try
            {
                // Se o arquivo já existe, limpa o conteúdo
                _logWriter = new StreamWriter(_logFilePath, false);
                _logWriter.AutoFlush = true; // Garante que cada linha seja gravada imediatamente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar o logger: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Escreve uma linha no console e no arquivo de log
    /// </summary>
    public static void WriteLine(string message = "")
    {
        lock (_lock)
        {
            Console.WriteLine(message);
            _logWriter?.WriteLine(message);
        }
    }

    /// <summary>
    /// Escreve no console e no arquivo de log sem quebra de linha
    /// </summary>
    public static void Write(string message)
    {
        lock (_lock)
        {
            Console.Write(message);
            _logWriter?.Write(message);
        }
    }

    /// <summary>
    /// Finaliza o logger, fechando o arquivo de log
    /// </summary>
    public static void Close()
    {
        lock (_lock)
        {
            _logWriter?.Flush();
            _logWriter?.Close();
            _logWriter?.Dispose();
            _logWriter = null;
        }
    }
}
