using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;
using S3GBusEntity;
using System.Configuration;

namespace S3GDALayer.Common
{
    public class ClsIniFileAccess
    {
        public static void FunPubGetConnectionString()
        {
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();

            string strFileName = (string)AppReader.GetValue("INIFILEPATH", typeof(string));

            if (File.Exists(strFileName))
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(File.ReadAllText(strFileName).Trim());

                //_strConnectionString = xmlDoc.SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connectionString"].Value;
                //_strDataProvider = xmlDoc.SelectSingleNode("ConnectionStrings").ChildNodes[0].Attributes["providerName"].Value;

            }
            else
            {
                throw new FileNotFoundException("Configuration file not found");
            }
        }

        private static XmlDocument _xmlDoc;
        public static XmlDocument xmlDoc
        {
            get
            {
                FunPubGetConnectionString();
                return _xmlDoc;
            }
        }

        private static S3GDALDBType enumDBType;
        public static S3GDALDBType S3G_DBType
        {
            get
            {
                return enumDBType;
            }
            set
            {
                enumDBType = value;
            }
        }


        //private static string _strConnectionString = "";
        //public static string ConnectionString
        //{
        //    get 
        //    {
        //        FunPubGetConnectionString();
        //        return _strConnectionString;
        //    }            
        //}
        //private static string _strDataProvider = "";
        //public static string strDataProvider
        //{
        //    get
        //    {
        //        return _strDataProvider;
        //    }
        //}

        //public static void FunPubGetConnectionString()
        //{
        //    string strFileName = @"D:\s3g\ConFig.ini";

        //    if (File.Exists(strFileName))
        //    {
        //        string[] strFileContent = File.ReadAllLines(strFileName);
        //        if (strFileContent.Length > 0)
        //        {
        //            _strConnectionString = strFileContent[0].ToString();
        //        }
        //    }
        //}

        public static Database FunPubGetDatabase()
        {
            try
            {
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                S3G_DBType = S3GDALDBType.SQL;
                //Database ConfigDb = DatabaseFactory.CreateDatabase("S3GconnectionString");   
                //XmlDocument conxmlDoc = xmlDoc;

                string ConnectionString = (string)AppReader.GetValue("S3GconnectionString", typeof(string));
                string strDataProvider = (string)AppReader.GetValue("providerName", typeof(string));
                string strConType = (string)AppReader.GetValue("connnectionType", typeof(string));
                if (strConType.Trim().ToUpper() == "ORACLE")
                {
                    S3G_DBType = S3GDALDBType.ORACLE;
                }
                return new GenericDatabase(ConnectionString, DbProviderFactories.GetFactory(strDataProvider));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        #region Getting FinYear Start Month from Config.ini

        public static string FunPubFinYearStartMonth()
        {
            try
            {
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                string strFileName = (string)AppReader.GetValue("INIFILEPATH", typeof(string));
                string startMonth = "";
                if (File.Exists(strFileName))
                {
                    XmlDocument conxmlDoc = new XmlDocument();
                    conxmlDoc.LoadXml(File.ReadAllText(strFileName).Trim());
                    startMonth = conxmlDoc.ChildNodes[0].SelectSingleNode("FinancialStartMonth").ChildNodes[0].Attributes[1].Value;
                }
                return startMonth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        //public static Database FunPubGetDatabase(out S3GDALDBType enumDBType)
        //{
        //    try
        //    {

        //        //Database ConfigDb = DatabaseFactory.CreateDatabase("S3GconnectionString");   
        //        enumDBType = S3GDALDBType.SQL;
        //        XmlDocument conxmlDoc = xmlDoc;
        //        string ConnectionString = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connectionString"].Value;
        //        string strDataProvider = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["providerName"].Value;
        //        string strConType = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connnectionType"].Value;
        //        if (strConType.Trim().ToUpper() == "ORACLE")
        //        {
        //            enumDBType = S3GDALDBType.ORACLE;
        //        }
        //        return new GenericDatabase(ConnectionString, DbProviderFactories.GetFactory(strDataProvider));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //public void OpenConnection()
        //{
        //    DbConnection objConnection;
        //    try
        //    {
        //        //objConnection.ConnectionString = ConnectionString;
        //        //objConnection.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (objConnection.State == System.Data.ConnectionState.Open)
        //        //{
        //        //    objConnection.Close();
        //        //}
        //    }

        //}
    }

}
