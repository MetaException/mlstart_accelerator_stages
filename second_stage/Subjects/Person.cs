using second_stage.Abstract;
using second_stage.Units;
using Serilog;
using static second_stage.Units.Enums;

namespace second_stage.Subjects
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
