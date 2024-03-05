﻿using cmd.Abstract;
using static cmd.Units.Enums;

namespace cmd.Subjects
{
    class Worker : AbstractPerson
    {
        public bool IsBusy { get { return isBusy; } set {  isBusy = value; } } 

        bool isBusy;
        Company company;

        public Worker(string name, Company company, double balance = 0, places inPlace = places.OUTSIDE, bool isBusy = false)
        {
            this.name = name;
            this.Balance = balance;
            this.shares = company.Shares;
            this.inPlace = inPlace;
            this.isBusy = isBusy;
            this.company = company;
            this.sharesPrice = company.SharesPrice;
        }
    }
}