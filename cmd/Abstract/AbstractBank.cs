using cmd.Interface;
using cmd.Interfaces;
using cmd.Managers;
using Microsoft.Extensions.Logging;

namespace cmd.Abstract
{
    internal abstract class AbstractBank : AbstractPaymentSubject, IBank
    {
        private class Vault : AbstractPaymentSubject
        {
            public Vault()
            {
                this.name = "Счёт в банке";
            }
        }

        private readonly Dictionary<string, Vault> balanceMap = new Dictionary<string, Vault>();

        public void StoreMoney(IPaymentSubject person, double balance)
        {
            if (balance == -1) // -1 сохранить весь баланс
                balance = person.Balance;

            if (balance == 0)
                return;

            var name = person.Name;

            if (balanceMap.ContainsKey(name))
            {
                var vault = balanceMap[name];
                TradeManager.TradeMoney(person, vault, balance);
                balanceMap[name] = vault;
            }
            else
            {
                var newVault = new Vault();
                TradeManager.TradeMoney(person, newVault, balance);
                balanceMap.Add(name, newVault);
            }

            Logger.logger.LogInformation("[День {@dayn}] [Час {@hourn}] {@person_name} сложил {@money_amount} денег в несгораемый шкаф. Всего денег на счёте: {@vault_balance}", Simulator.day, Simulator.hour, name, balance, balanceMap[name].Balance);
        }
    }
}