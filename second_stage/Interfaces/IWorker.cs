namespace second_stage.Interface
{
    internal interface IWorker : IPaymentSubject
    {
        void SetBusy(bool busy);
    }
}
