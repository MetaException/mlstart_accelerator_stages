namespace second_stage.Interface
{
    interface ICompany : IPaymentSubject
    {
        public int GetOpeningTime();

        public int GetClosingTime();
    }
}
