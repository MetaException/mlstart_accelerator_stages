using second_stage.Interface;
using Serilog;

namespace second_stage.Managers;

static class TradeManager
{
    public static void PerformDeal(IPaymentSubject from, IPaymentSubject to, int countToTrade)
    {
        double priceToPay = CalculatePriceSum(from, countToTrade);

        if (to.GetMoney() < priceToPay || from.GetSharesCount() < countToTrade)
        {
            return;
        }

        TradeShares(from, to, countToTrade);
        TradeMoney(to, from, priceToPay);

        Log.Information("[День {@dayn}] [Час {@hourn}] {@from_name} продал {@shares_count} акций {@to_name} на сумму {@sum}", Logger.day, Logger.hour, from.GetName(), countToTrade, to.GetName(), priceToPay);
    }

    public static void TradeShares(IPaymentSubject from, IPaymentSubject to, int count)
    {
        if (from.GetSharesCount() < count)
        {
            count = from.GetSharesCount();
        }
        var shares = from.TakeShares(count);
        to.GetShares(shares);
        Log.Debug("[День {@dayn}] [Час {@hourn}] {@from_name} передал {@shares_count} акций {@to_name}", Logger.day, Logger.hour, from.GetName(), count, to.GetName());
    }

    public static void TradeMoney(IPaymentSubject from, IPaymentSubject to, double priceToPay)
    {
        if (priceToPay == -1) // -1 сохранить весь баланс
            priceToPay = from.GetMoney();

        var money = from.TakeMoney(priceToPay); //...
        to.GetMoney(money);
        Log.Debug("[День {@dayn}] [Час {@hourn}] {@from_name} передал {@shares_count} денег {@to_name}", Logger.day, Logger.hour, from.GetName(), priceToPay, to.GetName());
    }

    public static double CalculatePriceSum(IPaymentSubject company, int countToBuy)
    {
        return company.GetSharesPrice() * countToBuy;
    }
}