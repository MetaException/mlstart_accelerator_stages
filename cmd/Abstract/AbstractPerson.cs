using cmd.Interfaces;
using Microsoft.Extensions.Logging;
using static cmd.Units.Enums;

namespace cmd.Abstract
{
    internal abstract class AbstractPerson : AbstractPaymentSubject, IPerson
    {
        public int HourWhenCome
        { get { return hourWhenCome; } set { hourWhenCome = value; } }
        public bool IsWantToBuy
        { get { return isWantToBuy; } set { isWantToBuy = value; } }
        public bool IsKnowAboutShares
        { get { return isKnowAboutShares; } set { isKnowAboutShares = value; } }

        protected places inPlace;
        protected int hourWhenCome;
        protected bool isWantToBuy = false;
        protected bool isKnowAboutShares = false;

        public void EnterTo(places places)
        {
            inPlace = places;
            Simulator.WaitDelay().Wait();
            if (places == places.OUTSIDE)
            {
                SimulatorLogger.Logger.Information("[День {@dayn}] [Час {@hourn}] {@p_name} удалился", Simulator.day, Simulator.hour, Name);
            }
            else
                SimulatorLogger.Logger.Information("[День {@dayn}] [Час {@hourn}] {@p_name} зашёл в {@place}", Simulator.day, Simulator.hour, Name, places.ToString());
        }
    }
}