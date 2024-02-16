using second_stage.Interfaces;
using Serilog;
using static second_stage.Units.Enums;

namespace second_stage.Abstract
{
    abstract class AbstractPerson : AbstractPaymentSubject, IPerson
    {
        public int HourWhenCome { get { return hourWhenCome; } set { hourWhenCome = value; } }
        public bool IsWantToBuy { get { return isWantToBuy; } set { isWantToBuy = value; } }
        public bool IsKnowAboutShares { get { return isKnowAboutShares; } set { isKnowAboutShares = value; } }

        protected places inPlace;
        protected int hourWhenCome;
        protected bool isWantToBuy = false;
        protected bool isKnowAboutShares = false;

        public void EnterTo(places places)
        {
            inPlace = places;
            if (places == places.OUTSIDE)
            {
                Log.Information("[День {@dayn}] [Час {@hourn}] {@p_name} удалился", Simulator.day, Simulator.hour, Name);
            }
            else
                Log.Information("[День {@dayn}] [Час {@hourn}] {@p_name} зашёл в {@place}", Simulator.day, Simulator.hour, Name, places.ToString());
        }
    }
}
