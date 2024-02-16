using second_stage.Units;

namespace second_stage.Interface
{
    public interface IPaymentSubject
    {
        public double Balance { get; set; }
        public string Name { get; }
        public List<Share> Shares { get; }
        public double SharesPrice { get; set; }

        public List<Share> TakeShares(int count);

        public void AddMoney(double amount);

        public int GetSharesCount();

        public double TakeMoney(double amount);

        public void AddShares(List<Share> sharesToAdd);
    }
}
