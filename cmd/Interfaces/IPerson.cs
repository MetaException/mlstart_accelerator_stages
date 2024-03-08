using cmd.Interface;
using static cmd.Units.Enums;

namespace cmd.Interfaces
{
    public interface IPerson : IPaymentSubject
    {
        public void EnterTo(places places);
    }
}