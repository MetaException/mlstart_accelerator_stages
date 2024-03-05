using cmd.Abstract;
using cmd.Units;
using Serilog;
using static cmd.Units.Enums;

namespace cmd.Subjects
{
    class Person : AbstractPerson
    {
        public Person(string name, double balance = 0, bool isWantToBuy = false, places inPlace = places.OUTSIDE)
        {
            this.name = name;
            this.Balance = balance;
            this.shares = new List<Share>();
            this.inPlace = inPlace;
            this.isWantToBuy = isWantToBuy;
        }
    }
}
