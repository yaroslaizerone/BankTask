using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTask
{
    class Helper
    {
        private static Module.Entities s_entities;
        public static Module.Entities getContext()
        {
            if (s_entities == null)
            {
                s_entities = new Module.Entities();
            }
            return s_entities;
        }

        public static void Create(Module.User user, Module.BankAccount account)
        {
            s_entities.User.Add(user);
            s_entities.BankAccount.Add(account);
            s_entities.SaveChanges();
        }

        public static int GetLastIDStaff()
        {
            int id = s_entities.User.OrderByDescending(user => user.IDUser).First().IDUser;

            return id + 1;
        }

        public static int GetLastIDAuth()
        {
            int id = s_entities.BankAccount.OrderByDescending(authorizations => authorizations.UserID).First().UserID;
            return id + 1;
        }
    }
}
