using System.ComponentModel.DataAnnotations;

public class CurrencyExchangeViewModel
{
    private string _sourceCurrencyName;
    private string _targetCurrencyName;

    // Input fields
    [Required(ErrorMessage = "The date is required.")]
    [DataType(DataType.Date)] // Ensures proper date format
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "The source currency is required.")]
    public string SourceCurrencyName
    {
        get => _sourceCurrencyName;
        set => _sourceCurrencyName = value?.ToLower();
    }

    [Required(ErrorMessage = "The target currency is required.")]
    public string TargetCurrencyName
    {
        get => _targetCurrencyName;
        set => _targetCurrencyName = value?.ToLower();
    }

    [Required(ErrorMessage = "The amount is required.")]
    public decimal SourceCurrencyAmount { get; set; }

    // Output fields
    public decimal? ConvertedAmount { get; set; }
    public decimal? ExchangeRate { get; set; }
}