//TODO: * использовать isKnowAboutShares чтобы точно определять что всем известно и увеличивать кол-во покупателей
//      * добавить таймаут между действиями (не понял куда пихать)
//      * переписать говнокод :)

using Microsoft.Extensions.Logging;
using Serilog;

namespace cmd
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logger.CreateLogger();

            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);

            var logger = loggerFactory.CreateLogger<Simulator>();

            var simulator = new Simulator(logger);

            while (true) // По заданию должен быть день сурка
            {
                simulator.SimulateDay();
            }
        }
    }
}
