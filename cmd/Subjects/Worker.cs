using cmd.Abstract;
using static cmd.Units.Enums;

namespace cmd.Subjects
{
    internal class Worker : AbstractPerson
    {
        public bool IsBusy
        { get { return isBusy; } set { isBusy = value; } }

        private bool isBusy;
        private Company company;

        public Worker(string name, Company company, double balance = 0, places inPlace = places.OUTSIDE, bool isBusy = false)
        {
            this.name = name;
            this.Balance = balance;
            this.shares = company.Shares;
            this.inPlace = inPlace;
            this.isBusy = isBusy;
            this.company = company;
            this.sharesPrice = company.SharesPrice;
        }
    }
}