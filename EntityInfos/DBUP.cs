using System; 
using SZORM;

namespace EntityInfos
{
    internal class DBUP
    {
        internal static SZORM_Upgrade UPVersion1(DB context)
        {
            SZORM_Upgrade up = new SZORM_Upgrade();
            up.ReleaceTime = new DateTime(2015, 10, 10);
            up.Version = 1;


            
            return up;
        }
        internal static SZORM_Upgrade UPVersion2(DB context)
        {
            SZORM_Upgrade up = new SZORM_Upgrade();
            up.ReleaceTime = new DateTime(2015, 10, 13);
            up.Version = 2;
          
            return up;
        }
         
    }

}
