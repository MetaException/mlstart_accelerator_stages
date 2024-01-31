using second_stage.Interface;
using second_stage.Managers;
using second_stage.Subjects;
using second_stage.Units;
using Serilog;
using static second_stage.Units.Enums;

//TODO: * использовать iswanttobuy чтобы точно определять что всем известно
//      * добавить таймаут между действиями
//      * добавить класс record
//      * переписать говнокод :)


namespace second_stage
{
    internal class Program
    {
        private static readonly Random rnd = new Random();

        static void Main(string[] args)
        {
            Logger.CreateLogger();

            var bank = new Bank();
            var company = new Company(1500);

            var shorty = new Person("Коротышка", 100);
            var dunnoAndGoaty = new Worker("Незнайка и Козлик", 0, company);
            var miga = new Worker("Мига", 0, company);

            var otherCitizens = GenerateRandomCitizens(200);
            var comeEarlierCount = 0;

            for (int day = 1; company.GetSharesCount() > 0; day++)
            {
                Logger.day = day;

                if (day == 1)
                    shorty.SetWantToBuy(true);

                GenerateDemand(otherCitizens, comeEarlierCount);

                var wantToBuy = GetClients(otherCitizens, company, ref comeEarlierCount);

                for (int hour = 7; hour <= 19; hour++)
                {
                    Logger.hour = hour;

                    if (hour == 8)
                        dunnoAndGoaty.EnterTo(places.COMPANY);
                    else if (hour == 19)
                        dunnoAndGoaty.EnterTo(places.OUTSIDE);

                    if (hour >= company.GetOpeningTime() && hour <= company.GetClosingTime())
                    {
                        foreach (var client in wantToBuy.Where(x => x.GetTimeWhenCome() == hour))
                        {
                            client.EnterTo(places.COMPANY);
                            dunnoAndGoaty.SetBusy(true);

                            if (company.GetSharesCount() != 0)
                            {
                                int countToBuy = rnd.Next(1, 1 + (int)(client.GetMoney() / company.GetSharesPrice()));
                                TradeManager.TradeShares(company, dunnoAndGoaty, countToBuy);
                                TradeManager.PerformDeal(dunnoAndGoaty, client, countToBuy);
                                dunnoAndGoaty.SetBusy(false);
                            }
                            else
                                Log.Information("[День {@dayn}] Акции распроданы", day);

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
            var dontWantToBuy = otherCitizens.Where(x => !x.IsWantToBuy()).ToArray();

            for (int i = 0; i < Math.Min(dontWantToBuy.Length / 3 + comeEarlierCount, dontWantToBuy.Length); i++)
            {
                dontWantToBuy[i].SetWantToBuy(true);
                dontWantToBuy[i].SetTimeWhenCome(rnd.Next(7, 19));
            }

            if (dontWantToBuy.Length == 0)
                Log.Information("[День {@dayn}] Всем в городе стало известно про акции", Logger.day);
        }

        private static List<Person> GetClients(List<Person> otherCitizens, Company company, ref int comeEarlierCount)
        {
            var wantToBuy = otherCitizens.Where(x => x.IsWantToBuy()).ToList();

            Log.Information("[День {@dayn}] Количество желающих приобрести акции выросло: {@wcount}", Logger.day, wantToBuy.Count());

            var comeEarly = wantToBuy.Where(x => x.GetTimeWhenCome() < company.GetOpeningTime());
            comeEarlierCount = comeEarly.Count();

            if (comeEarlierCount > 0)
                Log.Information("[День {@dayn}] Раньше пришло {@cp} человек", Logger.day, comeEarlierCount);

            return wantToBuy;
        }
    }
}
