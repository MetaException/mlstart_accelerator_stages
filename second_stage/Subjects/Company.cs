using second_stage.Abstract;
using second_stage.Interface;
using second_stage.Units;

namespace second_stage.Subjects
{
    class Company : AbstractPaymentSubject, ICompany
    {
        int openingTime;
        int closingTime;

        public Company(string name, int sharesCount)
        {
            sharesPrice = 100d;
            openingTime = 9;
            closingTime = 19;
            this.name = name;
            shares = Enumerable.Range(1, sharesCount).Select(x => new Share(sharesPrice)).ToList();
        }

        public int GetOpeningTime()
        {
            return openingTime;
        }

        public int GetClosingTime()
        {
            return closingTime;
        }
    }
}
