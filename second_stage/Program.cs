//TODO: * использовать isKnowAboutShares чтобы точно определять что всем известно и увеличивать кол-во покупателей
//      * добавить таймаут между действиями (не понял куда пихать)
//      * переписать говнокод :)

namespace second_stage
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logger.CreateLogger();
            Simulator.SimulateDay();
        }
    }
}
