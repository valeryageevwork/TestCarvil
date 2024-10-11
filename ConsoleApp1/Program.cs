using Xunit;

namespace ConsoleApp1;


public sealed record CalculationResult(double CorrectedPriceWithNDS, double CorrectedPriceWithoutNDS);

public static class Calculation
{
    public static CalculationResult Calculate(double inputGrossPrice, double vat)
    {
        if (vat is < 0 or > 99)
        {
            throw new InvalidDataException("The percent must be between 0 and 99.");
        }

        var netPrice = inputGrossPrice / (1.0 + (vat / 100.0));
        var grossPrice = netPrice * (1.0 + (vat / 100.0));

        var grossPriceValue = Math.Round(grossPrice, 2, MidpointRounding.ToEven);
        var netPriceValue = Math.Round(netPrice, 2, MidpointRounding.ToEven);

        return new(grossPriceValue, netPriceValue);
    }
}


public class Tests
{
    [Theory]
    [InlineData(1.81000000001000000003, 20, 1.81, 1.51)]
    [InlineData(1.77, 18, 1.77, 1.5)]
    [InlineData(5.05, 27, 5.05, 3.98)]
    [InlineData(90.123456789, 3, 90.12, 87.5)]
    [InlineData(111190.123456789, 97, 111190.12, 56441.69)]
    [InlineData(111190.12945678912345678907, 1, 111190.13, 110089.24)]
    public void CalculationTests(double inputPriceWithNDS, double procNDS, double grossPremiumExpect, double netPremiumExpect)
    {
        var (grossPremium, netPremium) = Calculation.Calculate(inputPriceWithNDS, procNDS);

        Assert.Equal(grossPremium, grossPremiumExpect);
        Assert.Equal(netPremium, netPremiumExpect);
    }
}