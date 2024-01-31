using second_stage.Abstract;
using second_stage.Interface;
using second_stage.Units;
using Serilog;
using static second_stage.Units.Enums;

namespace second_stage.Subjects
{
    class Worker : AbstractPerson, IWorker
    {
        bool isBusy;
        Company company;

        public Worker(string name, double balance, Company company, places inPlace = places.OUTSIDE, bool isBusy = false)
        {
            this.name = name;
            this.balance = balance;
            this.shares = new List<Share>();
            this.inPlace = inPlace;
            this.isBusy = isBusy;
            this.company = company;
            this.sharesPrice = company.GetSharesPrice();
        }

        public void SetBusy(bool isBusy)
        {
            this.isBusy = isBusy;
        }

        public bool IsBusy()
        {
            return isBusy;
        }
    }
}
