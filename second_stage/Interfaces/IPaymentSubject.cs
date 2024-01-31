using second_stage.Units;

namespace second_stage.Interface
{
    public interface IPaymentSubject
    {
        public List<Share> TakeShares(int count);

        public void GetMoney(double amount);

        public double GetMoney();

        public List<Share> GetShares();

        public int GetSharesCount();

        public double TakeMoney(double amount);

        public void GetShares(List<Share> sharesToAdd);

        public double GetSharesPrice();

        public string GetName();
    }
}
