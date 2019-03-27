using Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.Helper
{
    public class notificationhelper
    {
        public void Notify(Account OldBalance, Account Newbalance)
        {
            var oldBalance = OldBalance.CurrentBalance;
            var newBalance = Newbalance.CurrentBalance;

            if (oldBalance == newBalance)
                return;

            var newNotify = new Notification
            {
                Warning = true,
                Body = ""

            };



        } 

       
    }
}