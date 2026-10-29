namespace Conde.Waerden;

/// <summary>
/// As tabelas do README. Cada número sai de um método daqui e está preso num
/// teste; só entra aqui o que eu vi o programa imprimir.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaDuasCores(
        int N,
        long Coloracoes,
        long Progressoes,
        long SemMonocromatica,
        int MenosMonocromaticas,
        long NosDaBusca);

    public readonly record struct LinhaCusto(int N, long Coloracoes, long NosDaBusca, double Razao);

    public readonly record struct LinhaTresCores(int N, long Coloracoes, long NosDaBusca, bool Existe);

    /// <summary>
    /// Duas cores, progressões de três termos. A força bruta e a busca com
    /// retrocesso respondem a mesma pergunta por caminhos diferentes.
    /// </summary>
    public static List<LinhaDuasCores> DuasCores(IEnumerable<int> tamanhos)
    {
        var linhas = new List<LinhaDuasCores>();

        foreach (var n in tamanhos)
        {
            var bruta = Varredura.Contar(n, 2, 3);
            var busca = new Busca(n, 2, 3);
            var contadas = busca.Contar();

            linhas.Add(new LinhaDuasCores(
                n,
                bruta.Coloracoes,
                Progressoes.Quantidade(n, 3),
                contadas,
                bruta.MenosMonocromaticas,
                busca.Nos));

            if (contadas != bruta.SemMonocromatica)
            {
                throw new InvalidOperationException(
                    $"a busca e a forca bruta discordaram em n={n}: {contadas} contra {bruta.SemMonocromatica}");
            }
        }

        return linhas;
    }

    /// <summary>O que a poda economiza, tamanho por tamanho.</summary>
    public static List<LinhaCusto> Custo(IEnumerable<int> tamanhos)
    {
        var linhas = new List<LinhaCusto>();

        foreach (var n in tamanhos)
        {
            var busca = new Busca(n, 2, 3);
            busca.Contar();
            var coloracoes = Varredura.Quantidade(n, 2);
            linhas.Add(new LinhaCusto(n, coloracoes, busca.Nos, Math.Round((double)coloracoes / busca.Nos, 2)));
        }

        return linhas;
    }

    /// <summary>
    /// Três cores, progressões de três termos. Aqui a força bruta não serve:
    /// com 26 posições seriam 2,5 trilhões de colorações. A busca com retrocesso
    /// responde em segundos, e é isso que a tabela mostra.
    /// </summary>
    public static List<LinhaTresCores> TresCores(IEnumerable<int> tamanhos)
    {
        var linhas = new List<LinhaTresCores>();

        foreach (var n in tamanhos)
        {
            var busca = new Busca(n, 3, 3);
            var achou = busca.Primeira() is not null;
            linhas.Add(new LinhaTresCores(n, Varredura.Quantidade(n, 3), busca.Nos, achou));
        }

        return linhas;
    }

    /// <summary>O número de Van der Waerden, achado por varredura crescente.</summary>
    public static int Numero(int cores, int comprimento, int limite)
        => Busca.Numero(cores, comprimento, limite);

    /// <summary>
    /// A melhor coloração de um tamanho: a que deixa menos progressões de uma
    /// cor só. Depois do número de Van der Waerden, zero deixa de ser possível e
    /// a pergunta passa a ser quanto dá para chegar perto.
    /// </summary>
    public static (int Menos, string Coloracao) Melhor(int n)
    {
        var bruta = Varredura.Contar(n, 2, 3);
        return (bruta.MenosMonocromaticas, bruta.Melhor is null ? string.Empty : Juiz.Texto(bruta.Melhor));
    }
}
