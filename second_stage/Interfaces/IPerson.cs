using second_stage.Interface;
using static second_stage.Units.Enums;

namespace second_stage.Interfaces
{
    public interface IPerson : IPaymentSubject
    {
        public void EnterTo(places places);
    }
}
