//      * добавить таймаут между действиями (не понял куда пихать)

namespace cmd
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Logger.CreateLogger();

            var simulator = new Simulator();

            while (true) // По заданию должен быть день сурка
            {
                simulator.SimulateDay();
            }
        }
    }
}