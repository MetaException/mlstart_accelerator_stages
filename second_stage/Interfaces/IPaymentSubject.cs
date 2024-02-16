using second_stage.Units;

namespace second_stage.Interface
{
    public interface IPaymentSubject
    {
        public double Balance { get; set; }
        public string Name { get; }
        public List<Share> Shares {get;}
        public double SharesPrice { get; }

        public List<Share> TakeShares(int count);

        public void GetMoney(double amount);

        public int GetSharesCount();

        public double TakeMoney(double amount);

        public void GetShares(List<Share> sharesToAdd);
    }
}
