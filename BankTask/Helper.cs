using BankTask.Module;
using System.Linq;
using System.Windows;

namespace BankTask
{
    class Helper
    {
        private static Entities1 s_entities;
        public static Entities1 getContext()
        {
            if (s_entities == null)
            {
                s_entities = new Entities1();
            }
            return s_entities;
        }

        public static void Create(Contract contract1)
        {
            s_entities.Contract.Add(contract1);
            s_entities.SaveChanges();
            MessageBox.Show("Запись добавлена успешно");
        }

        public static int GetLastIDStaff()
        {
            int id = s_entities.User.OrderByDescending(user => user.IDUser).First().IDUser;

            return id + 1;
        }

        public static int GetLastIDAuth()
        {
            int id = s_entities.BankAccount.OrderByDescending(authorizations => authorizations.IDUser).First().IDUser;
            return id + 1;
        }
    }
}
