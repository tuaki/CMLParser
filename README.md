## Temp ##
### Požadavky na parametry ###
- required/defautl value - viz **Typy parametrů**
- typ, což souvisí i s počtem argumentů
- identifikátor:
	- Identifier:
		- dvě pomlčky a znaky (a-zA-Z0-9_) až do mezery, nejdou kombinovat
		- povinné

	- ShortIdentifier:
		- pomlčka a jeden znak (a-zA-Z0-9)
		- dají se kombinovat dohromady
			- ale jen poslední ve skupině může mít parammetry
		- nepovinné

- callback

### Podporované typy ###
- bool
- int
- double
- string
- enum
#- Array<int>
#- Array<double>
#- Array<string>
#- Array<enum>

### Počet argumentů ###
- žádný (jen pro *bool*)
- jeden (*int*, *double*, *string*, *enum*)
- všechny až do dalšího příkazu (*Array* varianty)

### Typy parametrů ###
- required, default value - neexistuje
- required - musí být zadán v CML - (specifikuje se tak, že se mu nenastaví defaultní hodnota)
- default value - má defaultní hodnotu a tedy nemusí být zadán - nastaví se mu defaultní hodnota
- optional - není required, nemá defaultní hodnotu

- plain - od předchozích se liší tím, že nemá identifikátor ... potom by ale měl být required/default, protože jinak by to fakt nebylo přehledné
	- druhá možnost je říct, že předchozí typy nemusí specifikovat identifikátor a potom z nich automaticky budou plain - to by možná bylo jednodušší