using Conde.Waerden;

if (args.Length == 0)
{
    Console.WriteLine("uso: medidor <duas|custo|tres|numero|melhor>");
    return 1;
}

switch (args[0])
{
    case "duas":
        Duas();
        break;
    case "custo":
        Custo();
        break;
    case "tres":
        Tres();
        break;
    case "numero":
        Numero();
        break;
    case "melhor":
        Melhor();
        break;
    default:
        Console.WriteLine($"comando desconhecido: {args[0]}");
        return 1;
}

return 0;

static void Duas()
{
    Console.WriteLine("Duas cores, progressoes de tres termos");
    Console.WriteLine("n   coloracoes  progressoes  sem monocromatica  minimo  nos da busca");
    foreach (var l in Medidas.DuasCores([3, 4, 5, 6, 7, 8, 9, 10, 11, 12]))
    {
        Console.WriteLine($"{l.N,-4}{l.Coloracoes,-12}{l.Progressoes,-13}{l.SemMonocromatica,-19}{l.MenosMonocromaticas,-8}{l.NosDaBusca}");
    }
}

static void Custo()
{
    Console.WriteLine("O que desistir cedo economiza");
    Console.WriteLine("n   coloracoes  nos da busca  razao");
    foreach (var l in Medidas.Custo([5, 10, 15, 20, 25, 30]))
    {
        Console.WriteLine($"{l.N,-4}{l.Coloracoes,-12}{l.NosDaBusca,-14}{l.Razao}");
    }
}

static void Tres()
{
    Console.WriteLine("Tres cores, progressoes de tres termos");
    Console.WriteLine("n   coloracoes      nos da busca  existe coloracao sem progressao");
    foreach (var l in Medidas.TresCores([10, 15, 20, 24, 25, 26, 27]))
    {
        Console.WriteLine($"{l.N,-4}{l.Coloracoes,-16}{l.NosDaBusca,-14}{(l.Existe ? "sim" : "NAO")}");
    }
}

static void Numero()
{
    var dois = Medidas.Numero(2, 3, 20);
    Console.WriteLine($"W(2,3) = {dois}");
    var quatro = Medidas.Numero(2, 4, 40);
    Console.WriteLine($"W(2,4) = {quatro}");
    var tresCores = Medidas.Numero(3, 3, 30);
    Console.WriteLine($"W(3,3) = {tresCores}");
}

static void Melhor()
{
    Console.WriteLine("Depois de W(2,3), quantas progressoes sobram na melhor coloracao");
    Console.WriteLine("n    minimo  coloracao");
    for (var n = 8; n <= 16; n++)
    {
        var (menos, coloracao) = Medidas.Melhor(n);
        Console.WriteLine($"{n,-5}{menos,-8}{coloracao}");
    }
}
