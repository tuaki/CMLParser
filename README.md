# Uživatelská dokumentace #

## Parametry ##

Základním prvkem jsou třídy `Default<T>` a `Optional<T>`, které reprezentují parametry programu. Obě mají vlastnost `T Value`, představující vlastní naparsovanou hodnotu parametru. Zároveň je pro ně definováno implicitní přetypování na `T`.

První jmenovaná třída má význam parametru, který má vždy nějakou hodnotu - buď je určena její defaultní hodnota, nebo je parametr povinný. Oproti tomu Trída `Optional<T>` představuje volitelný parametr, u kterého nás zajímá nejen hodnota, ale i to, zda jej vůbec uživatel zadal. K tomu slouží vlastnost `bool IsSet`.

### Podporované typy ###

Parametry jsou generické podle následujících typů:

- **bool**
- **int**
- **double**
- **string**
- **enum**

U všech kromě **bool** lze použít i odvozené pole, tedy např. `Array<int>`.

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
  parametrů v případě, že i tento byl naparsován.
- **Index** - Určuje očekávané pořadí ve vstupu (pokud se jedná o *Plain*
  parametr). Hodnota je relativní, tzn. při parsování budou všechny parametry
  seřazeny od nejmenšího indexu po největší.

Dále tu jsou metody `CreateDefault` resp. `CreateOptinal`, které vytvoří
příslušné instance parametrů. Příklad:

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
        if (!result.Status) {
            // Handle invalid input
            Console.Write(result.ErrorMessage);
            Console.Write(parser.GetHelperText());
            return;
        }
        // Now we can use parsed options
        var options = result.Options;
    }
}

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

### Vlastnosti ###

TODO

### Argumenty ###

Parametry typu **bool** neakceptují žádné argumenty. Parametry všech ostatních
typů vždy akceptují právě jeden argument, následující bezprostředně za
identifikátorem (výjimkou jsou složené krátké identifikátory). Pole je možné
zapsat opakováním daného parametru.

V argumentu nemohou být použity speciální znaky (mezera, uvozovky). Ty lze
případně zapsat jako "argument". Uvozovky v samotném argmuentu lze escapovat
pomocí \", lomítko se escapuje jako \\.

## Parser ##

TODO

# Další vývoj #

- Vlastnosti na parametrech, obsahující informace o tom, kolikátý (resp. zda
  vůbec) uživatel parametr zadal. Předpokládaný usecase je analýza chování
  uživatelů s cílem optimalizovat parametry programu, aby bylo jejich zadávání
  pro uživatele co nejjednodušší.
- Metoda na parseru, která umožní simulovat naparsování parametru z kódu v
  programu. To se bude hodit například při testování.