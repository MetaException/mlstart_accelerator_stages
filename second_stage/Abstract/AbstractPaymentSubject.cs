using second_stage.Units;

namespace second_stage.Abstract
{
    public abstract class AbstractPaymentSubject
    {
        protected List<Share> shares;
        protected double balance;
        protected double sharesPrice;
        protected string name;

        public List<Share> TakeShares(int count)
        {
            var takenShares = shares.Take(count).ToList();
            shares.RemoveRange(0, count);
            return takenShares;
        }

        public void GetMoney(double amount)
        {
            balance += amount;
        }

        public double GetMoney()
        {
            return balance;
        }

        public List<Share> GetShares()
        {
            return shares;
        }

        public int GetSharesCount()
        {
            return shares.Count;
        }

        public double TakeMoney(double amount)
        {
            balance -= amount; //...
            return amount;
        }

        public void GetShares(List<Share> sharesToAdd)
        {
            shares.AddRange(sharesToAdd);
        }

        public double GetSharesPrice()
        {
            return sharesPrice;
        }

        public string GetName()
        {
            return name;
        }
    }
}
