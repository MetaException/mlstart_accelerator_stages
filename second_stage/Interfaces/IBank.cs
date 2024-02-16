using second_stage.Interface;

namespace second_stage.Interfaces
{
    public interface IBank : IPaymentSubject
    {
        public void StoreMoney(IPaymentSubject person, double balance);
    }
}
