using static second_stage.Program;

namespace second_stage.Interface
{
    interface IPerson : IPaymentSubject
    {
        public void EnterTo(places places);
        public bool IsWantToBuy();
        public void SetWantToBuy(bool wantToBuy);
        public int GetTimeWhenCome();
        public void SetTimeWhenCome(int timeWhenCome);
    }
}
