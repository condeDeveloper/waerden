namespace Conde.Waerden;

/// <summary>
/// A força bruta: gera as <c>cores^n</c> colorações de <c>1..n</c>, uma por
/// uma, e pergunta ao juiz. Nenhuma poda, nenhuma simetria aproveitada.
///
/// É o caminho que não pode estar certo por acidente, e é o que dá autoridade
/// à busca com retrocesso do arquivo ao lado.
/// </summary>
public static class Varredura
{
    public readonly record struct Resultado(long Coloracoes, long SemMonocromatica, int MenosMonocromaticas, int[]? Melhor);

    public static long Quantidade(int n, int cores)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfLessThan(cores, 1);

        long total = 1;
        for (var i = 0; i < n; i++)
        {
            total *= cores;
        }

        return total;
    }

    public static Resultado Contar(int n, int cores, int comprimento)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var progressoes = Progressoes.Todas(n, comprimento);
        var atual = new int[n + 1];
        long coloracoes = 0;
        long sem = 0;
        var menos = int.MaxValue;
        int[]? melhor = null;

        Passo(1);
        return new Resultado(coloracoes, sem, menos == int.MaxValue ? 0 : menos, melhor);

        void Passo(int posicao)
        {
            if (posicao > n)
            {
                coloracoes++;
                var quantas = Juiz.Quantas(atual, progressoes);
                if (quantas == 0)
                {
                    sem++;
                }

                if (quantas < menos)
                {
                    menos = quantas;
                    melhor = (int[])atual.Clone();
                }

                return;
            }

            for (var cor = 0; cor < cores; cor++)
            {
                atual[posicao] = cor;
                Passo(posicao + 1);
            }
        }
    }
}
