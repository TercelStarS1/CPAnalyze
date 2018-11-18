using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZORM;
namespace EntityInfos
{
    public partial class DB
    {
        protected override void Initialization()
        {
            //DB context = this;
            //UserInfo user = new EntityInfos.UserInfo();
            //user.UserName = "star";
            //user.UserPassword = MD51.StrMD5("hello" + "star");
            //user.IsLogin = "01";
            //context.UserInfo.Add(user);
            //context.Save();
        }

        protected override void UpdataDBExce()
        {
            if (DBVersion == 0)
            {
                UPDBVersion(DBUP.UPVersion1(this));
            }
            if (DBVersion == 1)
            {
                UPDBVersion(DBUP.UPVersion2(this));
            }
        }

    }
    public partial class StarOracle
    {
        protected override void Initialization()
        {

        }

        protected override void UpdataDBExce()
        {

        }
    } 
    
}
