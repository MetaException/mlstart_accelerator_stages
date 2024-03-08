using cmd.Interface;

namespace cmd.Interfaces
{
    public interface IBank : IPaymentSubject
    {
        public void StoreMoney(IPaymentSubject person, double balance);
    }
}