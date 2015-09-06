using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.Services
{
    public class OperatorManager
    {
        #region Singleton

        private OperatorManager()
        {
            using (var context = new Context())
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                IsUserAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

                var name = Environment.UserDomainName.ToLower();

                var salesman = context.Salesmen.FirstOrDefault(x => x.Login.ToLower() == name);
                if (salesman == null) throw new Exception("Current user does not exist");

                CurrentUserID = salesman.ID;
            }
        }

        private static readonly OperatorManager _instance = new OperatorManager();

        public static OperatorManager Instance {
            get { return _instance; }
        }

        #endregion

        public int CurrentUserID { get; private set; }

        public bool IsUserAdmin { get; private set; }
    }
}
