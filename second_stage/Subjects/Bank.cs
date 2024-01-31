﻿using second_stage.Abstract;
using second_stage.Interface;
using second_stage.Managers;
using Serilog;

namespace second_stage.Subjects
{
    internal class Bank : AbstractPaymentSubject, IPaymentSubject
    {
        private readonly Dictionary<string, Vault> balanceMap = new Dictionary<string, Vault>();

        class Vault : AbstractPaymentSubject, IPaymentSubject
        {
            public Vault() 
            {
                this.name = "Счёт в банке";
            }
        }

        public void StoreMoney(IPaymentSubject person, double balance)
        {
            if (balance == -1) // -1 сохранить весь баланс
                balance = person.GetMoney();

            if (balance == 0)
                return;

            var name = person.GetName();

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

            Log.Information("[День {@dayn}] [Час {@hourn}] {@person_name} сложил {@money_amount} денег в несгораемый шкаф. Всего денег на счёте: {@vault_balance}", Logger.day, Logger.hour, name, balance, balanceMap[name].GetMoney());
        }
    }
}
