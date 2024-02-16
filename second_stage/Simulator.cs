using second_stage.Managers;
using second_stage.Subjects;
using Serilog;
using static second_stage.Units.Enums;

namespace second_stage
{
    public static class Simulator
    {
        private static readonly Random rnd = new Random();

        public static int day;
        public static int hour;

        public static void SimulateDay()
        {
            var bank = new Bank();
            var company = new Company(name: "Общество гигантских растений", sharesCount: 1500);

            var shorty = new Person("Коротышка", 100);
            var dunnoAndGoaty = new Worker("Незнайка и Козлик", company);
            var miga = new Worker("Мига", company);

            var otherCitizens = GenerateRandomCitizens(300);
            var comeEarlierCount = 0;

            for (int dayi = 1; company.GetSharesCount() > 0; dayi++)
            {
                day = dayi;
                if (dayi == 1)
                    shorty.IsWantToBuy = true;

                GenerateDemand(otherCitizens, comeEarlierCount);

                var wantToBuy = GetClients(otherCitizens, company, ref comeEarlierCount);

                for (int houri = 7; houri <= 19; houri++)
                {
                    hour = houri;
                    if (houri == 8)
                        dunnoAndGoaty.EnterTo(places.COMPANY);
                    else if (houri == 19)
                        dunnoAndGoaty.EnterTo(places.OUTSIDE);

                    if (houri >= company.OpeningTime && houri <= company.ClosingTime)
                    {
                        foreach (var client in wantToBuy.Where(x => x.HourWhenCome == houri))
                        {
                            client.EnterTo(places.COMPANY);
                            dunnoAndGoaty.IsBusy = true;

                            if (company.GetSharesCount() != 0)
                            {
                                int countToBuy = rnd.Next(1, 1 + (int)(client.Balance / company.SharesPrice));
                                TradeManager.PerformDeal(dunnoAndGoaty, client, countToBuy);
                                dunnoAndGoaty.IsBusy = false;
                            }
                            else
                                Log.Information("[День {@dayn}] Акции распроданы", dayi);

                            client.EnterTo(places.OUTSIDE);
                        }
                    }
                }

                miga.EnterTo(places.COMPANY);

                TradeManager.TradeMoney(dunnoAndGoaty, miga, -1);

                miga.EnterTo(places.OUTSIDE);
                miga.EnterTo(places.BANK);

                bank.StoreMoney(miga, -1);

                miga.EnterTo(places.OUTSIDE);
            }
        }

        private static List<Person> GenerateRandomCitizens(int count)
        {
            var citizens = Enumerable.Range(0, count)
                .Select(i => new Person($"person-{i}", 300 + new Random().NextDouble() * 1000))
                .ToList();

            return citizens;
        }

        private static void GenerateDemand(List<Person> otherCitizens, int comeEarlierCount)
        {
            var dontWantToBuy = otherCitizens.Where(x => !x.IsWantToBuy).ToArray();

            for (int i = 0; i < Math.Min(dontWantToBuy.Length / 3 + comeEarlierCount, dontWantToBuy.Length); i++)
            {
                dontWantToBuy[i].IsWantToBuy = true;
                dontWantToBuy[i].HourWhenCome = rnd.Next(7, 19);
                dontWantToBuy[i].IsKnowAboutShares = true;
            }

            if (dontWantToBuy.Length == 0)
                Log.Information("[День {@dayn}] Всем в городе стало известно про акции", day);
        }

        private static List<Person> GetClients(List<Person> otherCitizens, Company company, ref int comeEarlierCount)
        {
            var wantToBuy = otherCitizens.Where(x => x.IsWantToBuy).ToList();

            Log.Information("[День {@dayn}] Количество желающих приобрести акции выросло: {@wcount}", day, wantToBuy.Count());

            var comeEarly = wantToBuy.Where(x => x.HourWhenCome < company.OpeningTime);
            comeEarlierCount = comeEarly.Count();

            if (comeEarlierCount > 0)
                Log.Information("[День {@dayn}] Раньше пришло {@cp} человек", day, comeEarlierCount);

            return wantToBuy;
        }
    }
}
