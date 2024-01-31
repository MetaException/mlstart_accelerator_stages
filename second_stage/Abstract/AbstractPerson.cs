using Serilog;
using static second_stage.Program;

namespace second_stage.Abstract
{
    abstract class AbstractPerson : AbstractPaymentSubject
    {
        protected places inPlace;
        protected int hourWhenCome;

        public int GetTimeWhenCome()
        {
            return hourWhenCome;
        }

        public void SetTimeWhenCome(int hour)
        {
            hourWhenCome = hour;
        }

        public void EnterTo(places places)
        {
            inPlace = places;
            Log.Information("[День {@dayn}] [Час {@hourn}] {@p_name} зашёл в {@place}", Logger.day, Logger.hour, this.name, places.ToString());
        }
    }
}
