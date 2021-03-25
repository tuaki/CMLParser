# Uživatelská dokumentace #

## Parametry ##

Základním prvkem jsou třídy `Default<T>` a `Optional<T>`, které reprezentují parametry programu. Obě mají vlastnost `T Value`, představující vlastní naparsovanou hodnotu parametru. Zároveň je pro ně definováno implicitní přetypování na `T`.

První jmenovaná třída má význam parametru, který má vždy nějakou hodnotu - buď je určena její defaultní hodnota, nebo je parametr povinný. Oproti tomu Trída `Optional<T>` představuje volitelný parametr, u kterého nás zajímá nejen hodnota, ale i to, zda jej vůbec uživatel zadal. K tomu slouží vlastnost `bool IsSet`.

### Podporované typy ###

Parametry jsou generické podle následujících typů:

- `bool`
- `int`
- `double`
- `string`
- `enum`

U všech kromě `bool` lze použít i odvozené pole, tedy např. `Array<int>`.

### Konfigurace ###

Pro vytvoření instance parametru je nutné nejdříve instanciovat továrnu
`ParameterFactory<T>`, na které se následně volají metody:

- **Identifier** - Definuje minimálně dlouhý a volitelně i krátký identifikátor.
  V případě, že metoda nebude zavolána, výsledný parametr bude tzv. *Plain*.
- **Description** - Popis, zobrazený uživateli po zavolání nápovědy.
- **DefaultValue** - Specifikuje defaultní hodnotu parametru. V případě, že se
  jedná o `Default<T>`, nebude tento parametr *Required*. Naopak pro
  `Optional<T>` bude ignorována, protože tam nedává žádný smysl.
- **OnParseCallback** - Funkce, která bude zavolána po naparsování všech
  parametrů v případě, že i tento byl naparsován. Jejím argumentem bude hodnota
  tohoto parametru.
- **Index** - Určuje očekávané pořadí ve vstupu (pokud se jedná o *Plain*
  parametr). Hodnota je relativní, tzn. při parsování budou všechny parametry
  seřazeny od nejmenšího indexu po největší. Pokud má více plain argumentů stejný
  index, jejich pořadí není definované.

Dále tu jsou metody `CreateDefault` resp. `CreateOptinal`, které vytvoří
příslušné instance parametrů. Příklad:

```cs
class Options
{
    /// <value>
    /// This parameter is required, because it's <see cref="Default{T}"/> and its default value is not set.
    /// </value>
    public Default<string> InputFile = new ParameterFactory<string>()
        .Identifier("input-file", 'i')
        .Description("(Required) The name of the input file.")
        .CreateDefault();

    const string defaultOutputFile = "output.csv";
    /// <value>
    /// This parameter has a default value. It seems optional to the user because he or she can overwrite it, but its in fact required from the programmer's point of view.
    /// </value>
    public Default<string> OutputFile = new ParameterFactory<string>()
        .Identifier("output-file", 'o')
        .Description("(Optional) The name of the output file. Default value: " + defaultOutputFile)
        .DefaultValue(defaultOutputFile)
        .CreateDefault();

    /// <value>
    /// This parameter is optional - if the user provides it, the behavior of the program will change.
    /// </value>
    public Optional<string> LogFile = new ParameterFactory<string>()
        .Identifier("log-file", 'l')
        .Description("(Optional) The name of the log file. If it's provided, the program will write some additional information here.")
        .CreateOptional();
}
```

### Argumenty ###

Argumenty začínající jednou nebo dvěma pomlčkami (`-`, `--`) jsou automaticky
parsovány jako identifikátory parametrů. Krátké identifikátory je možné sdružit
(např. `-asdf`), dlouhé je nutné psát zvlášť.

Parametry typu `bool` neakceptují žádné argumenty. Parametry všech ostatních
typů vždy akceptují právě jeden argument. Pro dlouhé identifikátory musí následovat bezprostředně za
znakem `=` (například `--file=myNewFile.txt`). Pro krátké identifikátory se vezme
první následující argument, který ještě nepatří jinému parametru. Pole je možné
zapsat opakováním daného parametru.

V argumentu nemohou být použity speciální znaky (mezera, uvozovky a nesmí začínat pomlčkou). Ty lze
případně zapsat v uvozovkách jako "-speciální argument". Uvozovky v samotném argumentu lze escapovat
pomocí \", lomítko se escapuje jako \\, pomlčka je tam normálně.

Všechny argumenty následující za -- jsou parsovány jako plain argumenty. U plain argumentů je nutné
zajistit jednoznačnost - konfigurace, kde je více než jeden plain argument typu `Array<T>`, není validní.

## Parser ##

Pro parsování stačí vytvořit nový objekt třídy `Parser<O>`, kde `O` je konfigurace (jakákoli třída,
obsahující veřejné proměnné typu `Default<T>` či `Optional<T>`, které budou identifikovány jako
parametry). V konstruktoru je nutné předat instanci této třídy.

Metody:
- **Check** - Zkontroluje, zda je objekt s konfigurací validní - například zda neobsahuje parametry
se stejnými identifikátory nebo více plain argumentů typu pole. Tuto metodu je vhodné volat po každé
změně konfigurace.
- **Parse** - Naparsuje předaný řetězec podle konfigurace a výše uvedených pravidel.
- **GetHelperText** - Vygeneruje nápovědu. Ta obsahuje identifikátory a popisy všech parametrů.

První dvě metody vrací objekty obsahující:
- **Status** - Zda operace uspěla či ne.
- **ErrorMessage** - Chybová hláška v případě neúspěchu. Zejména v případě parsování nemusí
být kompletní, parser se zpravidla zastaví na první nalezené chybě.
- **Options** (pouze u metody **Parse**) - výsledný objekt s naparsovanými hodnotami parametrů.

### Příklad použití ###

Ukázka programu, který by mohl být vytvořen pomocí konfigurace výše:

```cs
class Program {
    static void Main() {
        var parser = new Parser<Options>(new Options());
#if DEBUG
        // Let's perform consistency check first
        var check = parser.Check();
        if (!check.Status) {
            Console.WriteLine("Invalid options object!");
            Console.Write(check.ErrorMessage);
            return;
        }
#endif
        // Assume we have some arguments we need to parse
        string commandLineArguments = " ... ";
        var result = parser.Parse(commandLineArguments);

        // Handle invalid input
        if (!result.Status) {
            Console.Write(result.ErrorMessage);
            Console.Write(parser.GetHelperText());
            return;
        }

        // Now we can use parsed options
        var options = result.Options;
    }
}
```

# Další vývoj #

- Vlastnosti na parametrech, obsahující informace o tom, kolikátý (resp. zda
  vůbec) uživatel parametr zadal. Předpokládaný usecase je analýza chování
  uživatelů s cílem optimalizovat parametry programu, aby bylo jejich zadávání
  pro uživatele co nejjednodušší.
- Metoda na parseru, která umožní simulovat naparsování parametru z kódu v
  programu. To se bude hodit například při testování.