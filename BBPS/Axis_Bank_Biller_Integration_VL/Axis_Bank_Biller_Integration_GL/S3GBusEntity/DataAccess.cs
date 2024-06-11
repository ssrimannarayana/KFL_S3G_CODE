using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using Microsoft.VisualBasic;
using System.Globalization;
using Microsoft.Office;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace S3GBusEntity
{
    public class ClsPubDataAccess
    {
        string strConnectionString = "";
        string strDatabase = "";
        public Database objDatabase;

        public ClsPubDataAccess()
        {
            if (ConfigurationSettings.AppSettings["S3GconnectionString"] != null)
            {
                throw new ApplicationException("Define the ConnectionString");
            }
            strConnectionString = Convert.ToString(ConfigurationSettings.AppSettings["S3GconnectionString"]);
            objDatabase = DatabaseFactory.CreateDatabase(strConnectionString);
        }
        
        public void FunPubLoadDataSet(string strProcedureName,Dictionary<string,string> objParameters)
        {
            try
            {
                DbCommand command = objDatabase.GetStoredProcCommand(strProcedureName);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
        
    }
   
}
