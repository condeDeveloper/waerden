namespace Conde.Waerden;

/// <summary>
/// O juiz: dada uma coloração de <c>1..n</c>, diz se existe progressão
/// aritmética monocromática, e quantas.
///
/// Ele olha a lista inteira de progressões, uma por uma, sem poda e sem
/// esperteza. É lento de propósito: é a resposta de referência contra a qual a
/// busca com retrocesso é conferida.
/// </summary>
public static class Juiz
{
    /// <summary>Existe progressão de uma cor só nesta coloração.</summary>
    public static bool TemMonocromatica(IReadOnlyList<int> cores, IReadOnlyList<int[]> progressoes)
    {
        ArgumentNullException.ThrowIfNull(cores);
        ArgumentNullException.ThrowIfNull(progressoes);

        foreach (var progressao in progressoes)
        {
            var cor = cores[progressao[0]];
            var igual = true;
            for (var i = 1; i < progressao.Length && igual; i++)
            {
                if (cores[progressao[i]] != cor)
                {
                    igual = false;
                }
            }

            if (igual)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Quantas progressões de uma cor só esta coloração deixa.</summary>
    public static int Quantas(IReadOnlyList<int> cores, IReadOnlyList<int[]> progressoes)
    {
        ArgumentNullException.ThrowIfNull(cores);
        ArgumentNullException.ThrowIfNull(progressoes);

        var quantas = 0;
        foreach (var progressao in progressoes)
        {
            var cor = cores[progressao[0]];
            var igual = true;
            for (var i = 1; i < progressao.Length && igual; i++)
            {
                if (cores[progressao[i]] != cor)
                {
                    igual = false;
                }
            }

            if (igual)
            {
                quantas++;
            }
        }

        return quantas;
    }

    /// <summary>A coloração escrita como texto, para caber no README.</summary>
    public static string Texto(IReadOnlyList<int> cores)
    {
        ArgumentNullException.ThrowIfNull(cores);
        var letras = new char[cores.Count - 1];
        for (var i = 1; i < cores.Count; i++)
        {
            letras[i - 1] = (char)('A' + cores[i]);
        }

        return new string(letras);
    }
}
