using second_stage.Abstract;
using second_stage.Interface;
using second_stage.Units;
using static second_stage.Units.Enums;

namespace second_stage.Subjects
{
    class Person : AbstractPerson, IPerson
    {
        bool isWantToBuy = false;
        bool isKnowAboutShares = false;

        public Person(string name, double balance, bool isWantToBuy = false, places inPlace = places.OUTSIDE)
        {
            this.name = name;
            this.balance = balance;
            this.shares = new List<Share>();
            this.inPlace = inPlace;
            this.isWantToBuy = isWantToBuy;
        }

        public bool IsWantToBuy()
        {
            return isWantToBuy;
        }

        public void SetWantToBuy(bool wantToBuy)
        {
            isWantToBuy = wantToBuy;
        }
    }
}
