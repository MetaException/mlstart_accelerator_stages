//      * добавить таймаут между действиями (не понял куда пихать)

namespace cmd
{
    public class Program
    {
        private static void Main(string[] args)
        {
            while (true) // По заданию должен быть день сурка
            {
                Simulator.SimulateDay();
            }
        }
    }
}