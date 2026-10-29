namespace Conde.Waerden;

/// <summary>
/// As progressões aritméticas dentro de <c>1..n</c>: os conjuntos que a
/// coloração precisa evitar deixar de uma cor só.
///
/// Uma progressão de comprimento <c>k</c> é <c>a, a+d, a+2d, ...</c> com passo
/// <c>d</c> positivo. Elas são poucas e são sempre as mesmas, então são
/// montadas uma vez e reusadas em toda a varredura — montar de novo a cada
/// coloração seria o grosso do tempo.
/// </summary>
public static class Progressoes
{
    /// <summary>Todas as progressões de comprimento <paramref name="k"/> em 1..n.</summary>
    public static List<int[]> Todas(int n, int k)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfLessThan(k, 1);

        var todas = new List<int[]>();

        // Comprimento 1 e o caso degenerado: a progressao e um numero sozinho e
        // o passo nao quer dizer nada. Sem esta saida, a condicao do laco de
        // baixo vira "a <= n", que nunca deixa de valer, e o gerador nao
        // termina. Foi assim que a bateria de testes travou.
        if (k == 1)
        {
            for (var a = 1; a <= n; a++)
            {
                todas.Add([a]);
            }

            return todas;
        }

        for (var a = 1; a <= n; a++)
        {
            for (var d = 1; a + ((k - 1) * d) <= n; d++)
            {
                var progressao = new int[k];
                for (var i = 0; i < k; i++)
                {
                    progressao[i] = a + (i * d);
                }

                todas.Add(progressao);
            }
        }

        return todas;
    }

    /// <summary>
    /// Quantas progressões de comprimento <paramref name="k"/> existem em 1..n,
    /// pela conta fechada. É o juiz da enumeração: se o gerador esquecer uma ou
    /// repetir outra, os dois números deixam de bater.
    ///
    /// Para cada passo <c>d</c> cabem <c>n − (k−1)d</c> começos, e a soma disso
    /// sobre os passos possíveis é o total.
    /// </summary>
    public static long Quantidade(int n, int k)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfLessThan(k, 1);

        // O mesmo caso degenerado da Todas: com k = 1 o passo nao restringe
        // nada e a soma nao para.
        if (k == 1)
        {
            return n;
        }

        long total = 0;
        for (var d = 1; (k - 1) * d < n; d++)
        {
            total += n - ((k - 1) * d);
        }

        return total;
    }

    /// <summary>
    /// As progressões que TERMINAM em cada posição. É o que a busca com
    /// retrocesso precisa: ao pintar a posição <c>p</c>, só as progressões que
    /// acabam ali podem ter ficado monocromáticas agora.
    /// </summary>
    public static List<int[]>[] PorFim(int n, int k)
    {
        var porFim = new List<int[]>[n + 1];
        for (var i = 0; i <= n; i++)
        {
            porFim[i] = [];
        }

        foreach (var progressao in Todas(n, k))
        {
            porFim[progressao[^1]].Add(progressao);
        }

        return porFim;
    }
}
