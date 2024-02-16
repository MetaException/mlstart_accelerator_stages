using second_stage.Interface;
using second_stage.Units;

namespace second_stage.Abstract
{
    public abstract class AbstractPaymentSubject : IPaymentSubject
    {
        public double Balance { get { return balance; } set { balance = value; } }
        public string Name { get { return name; } }
        public List<Share> Shares { get { return shares; }}
        public double SharesPrice { get { return sharesPrice; } }

        protected double balance;
        protected List<Share> shares;
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
    }
}
