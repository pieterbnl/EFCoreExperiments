using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCoreMovies.Entities.Conversions;

public class CurrencyToSymbolConvertor : ValueConverter<Currency, string>
{
    public CurrencyToSymbolConvertor()
        :base(value => MapCurrencyTostring(value),
            value => MapStringToCurrency(value))
    {
        
    }

    private static string MapCurrencyTostring(Currency value)
    {
        return value switch
        {
            Currency.Peso => "RD$",
            Currency.USDollar => "$",
            Currency.Euro => "€",
            _ => "" // empty string for default cases
        };
    }

    private static Currency MapStringToCurrency(string value)
    {
        return value switch
        {
            "RD$" => Currency.Peso,
            "$" => Currency.USDollar,
            "€" => Currency.Euro,
            _ => Currency.Uknown // uknown currency for default cases
        };
    }
}