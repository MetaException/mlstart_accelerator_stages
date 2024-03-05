using cmd.Abstract;
using cmd.Units;

namespace cmd.Subjects
{
    internal class Bank : AbstractBank
    {
        public Bank(string name, int balance, int sharesPrice, List<Share> shares = null)
        {
            this.name = name;
            this.sharesPrice = sharesPrice;
            this.balance = balance;
            this.shares = shares is not null ? shares : new List<Share>();
        }
    }
}
