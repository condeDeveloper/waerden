namespace Conde.Waerden;

/// <summary>
/// A busca com retrocesso: pinta de 1 para n e abandona o ramo assim que a
/// última cor fecha uma progressão monocromática.
///
/// A poda é a mais simples possível e é a única: ao pintar a posição <c>p</c>,
/// só as progressões que TERMINAM em <c>p</c> podem ter acabado de ficar de uma
/// cor só, então só elas são conferidas. Não há heurística, ordem esperta nem
/// quebra de simetria — o que esta classe mede é exatamente quanto vale
/// desistir cedo, e nada mais.
/// </summary>
public sealed class Busca
{
    private readonly int _n;
    private readonly int _cores;
    private readonly List<int[]>[] _porFim;
    private readonly int[] _atual;

    public long Nos { get; private set; }

    public Busca(int n, int cores, int comprimento)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfLessThan(cores, 1);
        _n = n;
        _cores = cores;
        _porFim = Progressoes.PorFim(n, comprimento);
        _atual = new int[n + 1];
    }

    /// <summary>
    /// A primeira coloração sem progressão monocromática, ou nulo se não houver
    /// nenhuma — que é o que prova que o número de Van der Waerden foi passado.
    /// </summary>
    public int[]? Primeira()
    {
        Nos = 0;
        return Passo(1) ? (int[])_atual.Clone() : null;
    }

    /// <summary>Quantas colorações sem progressão monocromática existem.</summary>
    public long Contar()
    {
        Nos = 0;
        return Todas(1);
    }

    private bool Passo(int posicao)
    {
        if (posicao > _n)
        {
            return true;
        }

        for (var cor = 0; cor < _cores; cor++)
        {
            Nos++;
            _atual[posicao] = cor;
            if (Fechou(posicao))
            {
                continue;
            }

            if (Passo(posicao + 1))
            {
                return true;
            }
        }

        return false;
    }

    private long Todas(int posicao)
    {
        if (posicao > _n)
        {
            return 1;
        }

        long achadas = 0;
        for (var cor = 0; cor < _cores; cor++)
        {
            Nos++;
            _atual[posicao] = cor;
            if (!Fechou(posicao))
            {
                achadas += Todas(posicao + 1);
            }
        }

        return achadas;
    }

    private bool Fechou(int posicao)
    {
        foreach (var progressao in _porFim[posicao])
        {
            var cor = _atual[progressao[0]];
            var igual = true;
            for (var i = 1; i < progressao.Length && igual; i++)
            {
                if (_atual[progressao[i]] != cor)
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

    /// <summary>
    /// O menor <c>n</c> em que toda coloração tem progressão monocromática, que
    /// é o número de Van der Waerden. Devolve zero se não achar até o limite.
    /// </summary>
    public static int Numero(int cores, int comprimento, int limite)
    {
        for (var n = 1; n <= limite; n++)
        {
            if (new Busca(n, cores, comprimento).Primeira() is null)
            {
                return n;
            }
        }

        return 0;
    }
}
