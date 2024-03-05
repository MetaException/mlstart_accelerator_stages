using cmd.Abstract;
using cmd.Units;

namespace cmd.Subjects
{
    class Company : AbstractPaymentSubject
    {
        public int OpeningTime { get { return openingTime; } }
        public int ClosingTime { get { return closingTime; } }

        private readonly int openingTime;
        private readonly int closingTime;

        public Company(string name, int sharesCount)
        {
            sharesPrice = 100d;
            openingTime = 9;
            closingTime = 19;
            this.name = name;
            shares = Enumerable.Range(1, sharesCount).Select(x => new Share(sharesPrice)).ToList();
        }
    }
}
