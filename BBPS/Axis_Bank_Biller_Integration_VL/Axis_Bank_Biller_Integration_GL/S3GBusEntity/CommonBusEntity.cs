using System;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using System.Security.Cryptography;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace S3GBusEntity
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class AlwaysClosedTextFileTraceListener : CustomTraceListener
    {
        private string WeblogFilePath;
        private XmlDocument _xmlDoc;
        public string strProjectName
        {
            get { return _strProjectName; }
            set { _strProjectName = value; }
        }
        private string _strProjectName="";

        public AlwaysClosedTextFileTraceListener()
        {
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            string strFileName = (string)AppReader.GetValue("INIFILEPATH", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            if (File.Exists(strFileName))
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(File.ReadAllText(strFileName).Trim());
            }
            else
            {
                throw new FileNotFoundException("Configuration file not found");
            }

            WeblogFilePath = _xmlDoc.ChildNodes[0].SelectSingleNode("LogFiles").ChildNodes[0].Attributes["FilePath"].Value;// @"C:\S3Glog.txt";
            if (_strProjectName.ToUpper() == "SERVICE")
                WeblogFilePath = _xmlDoc.ChildNodes[0].SelectSingleNode("LogFiles").ChildNodes[1].Attributes["FilePath"].Value;// @"C:\S3Glog.txt";
            else if (_strProjectName.ToUpper() == "WINSERVICE")
                WeblogFilePath = _xmlDoc.ChildNodes[0].SelectSingleNode("LogFiles").ChildNodes[2].Attributes["FilePath"].Value;// @"C:\S3Glog.txt";
        }

        public override void Write(string message)
        {
            using (StreamWriter logFile = File.AppendText(WeblogFilePath))
            {
                logFile.Write(message);
                logFile.Flush();
                logFile.Close();
            }
        }

        public override void WriteLine(string message)
        {
            using (StreamWriter logFile = File.AppendText(WeblogFilePath))
            {
                //logFile.WriteLine("************************************");                
                logFile.WriteLine(message);
                logFile.Flush();
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry && this.Formatter != null)
            {
                WriteLine(this.Formatter.Format(data as LogEntry));
            }
            else
            {
                WriteLine(data.ToString());
            }
        }
    }


    [DataContractFormat]
    [Serializable]
    public class CreditParameterTransactionEntity
    {
        public int CompanyId { get; set; }
        public int AppraisalId { get; set; }
        public int CustomerId { get; set; }
        public int Document_Type { get; set; }
        public int EnquiryResponseId { get; set; }
        public string CreditParamNo { get; set; }
        public DateTime CreditParamDate { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        //public int Document_Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string XmlScoreDetails { get; set; }
        public string XmlOthers { get; set; }
        public string ErrorCode { get; set; }

    }
    public enum DALFlag
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        Query = 4
    }

    public enum SerializationMode
    {
        Binary,
        Xml,
        Soap,
    }

    public sealed class ClsPubCommCrypto
    {
        // Encrypt the text
        public static string EncryptText(string strText)
        {
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            string strEncryptKey = (string)AppReader.GetValue("EncryptKey", typeof(string));
            return Encrypt(strText, strEncryptKey);
        }

        //Decrypt the text 
        public static string DecryptText(string strText)
        {
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            string strEncryptKey = (string)AppReader.GetValue("EncryptKey", typeof(string));
            return Decrypt(strText, strEncryptKey);
        }

        //The function used to encrypt the text - 128 Bit Key
        private static string Encrypt(string strText, string strEncrKey)
        {
            byte[] byKey;
            byte[] IV = { 65, 6, 12, 34, 28, 232, 34, 158, 185, 192, 51, 74, 236, 28, 55, 9 };

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Trim());
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Unicode.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //The function used to decrypt the text - 128 Bit Key
        private static string Decrypt(string strText, string sDecrKey)
        {
            byte[] byKey;
            byte[] IV = { 65, 6, 12, 34, 28, 232, 34, 158, 185, 192, 51, 74, 236, 28, 55, 9 };
            byte[] inputByteArray = new byte[strText.Length];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Trim());
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.Unicode;

                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public sealed class ClsPubCommErrorLog
    {
        public static string strLogFilePath = string.Empty;
        private static StreamWriter sw = null;

        ///<summary>
        /// If the LogFile path is empty then, it will write the log entry to 
        /// LogFile.txt under application directory.
        /// If the LogFile.txt is not availble it will create it
        /// If the Log File path is not empty but the file is 
        /// not availble it will create it.
        /// <param name="objException"></param>
        /// <RETURNS>false if the problem persists</RETURNS>
        ///</summary>

        public static bool CustomErrorRoutine(Exception objException)
        {

            bool bReturn = true;

            // write the error log to that text file
            if (objException != null)
            {
                if (true != WriteErrorLog(objException))
                {
                    bReturn = false;
                }
            }
            return bReturn;
        }

        public static bool CustomErrorRoutine(Exception objException, string strPageName)
        {

            bool bReturn = true;

            // write the error log to that text file
            if (objException != null)
            {
                if (true != WriteErrorLog(objException, strPageName))
                {
                    bReturn = false;
                }
            }
            return bReturn;
        }

        public static bool CustomErrorRoutine(Exception objException, string strPageName, string strProjectName)
        {

            bool bReturn = true;

            // write the error log to that text file
            if (objException != null)
            {
                if (true != WriteErrorLog(objException, strPageName, strProjectName))
                {
                    bReturn = false;
                }
            }
            return bReturn;
        }


        private static bool WriteErrorLog(Exception objException, string strPageName)
        {
            bool bReturn = false;

            StackTrace st = new StackTrace(objException, true);
            StackFrame[] frames = st.GetFrames();
            int intFrameCount = st.FrameCount;

            LogEntry logEntry = new LogEntry();

            AlwaysClosedTextFileTraceListener obj = new AlwaysClosedTextFileTraceListener();
            obj.WriteLine("");
            obj.WriteLine("Timestamp \t: " + DateTime.Now.ToString());
            obj.WriteLine("Message \t: " + objException.Message);
            obj.WriteLine("File \t\t: " + frames[intFrameCount - 1].GetFileName());
            obj.WriteLine("Method \t\t: " + frames[intFrameCount - 1].GetMethod().Name);
            obj.WriteLine("Line Number \t: " + frames[intFrameCount - 1].GetFileLineNumber().ToString());
            if (intFrameCount > 1)
            {
                obj.WriteLine("Sub Method \t: " + frames[intFrameCount - 2].GetMethod().Name);
            }
            obj.WriteLine("Machine \t: " + logEntry.MachineName);
            obj.WriteLine("App Domain \t: " + logEntry.AppDomainName);
            obj.WriteLine("ProcessId \t: " + logEntry.ProcessId);
            obj.WriteLine("Process Name \t: " + logEntry.ProcessName);
            obj.WriteLine("Thread Name \t: " + logEntry.Win32ThreadId);
            bReturn = true;

            return bReturn;
        }

        private static bool WriteErrorLog(Exception objException, string strPageName, string strProjectName)
        {
            bool bReturn = false;

            StackTrace st = new StackTrace(objException, true);
            StackFrame[] frames = st.GetFrames();
            int intFrameCount = st.FrameCount;

            LogEntry logEntry = new LogEntry();

            AlwaysClosedTextFileTraceListener obj = new AlwaysClosedTextFileTraceListener();
            obj.WriteLine("");
            obj.WriteLine("Timestamp \t: " + DateTime.Now.ToString());
            obj.WriteLine("Message \t: " + objException.Message);
            obj.WriteLine("File \t\t: " + frames[intFrameCount - 1].GetFileName());
            obj.WriteLine("Method \t\t: " + frames[intFrameCount - 1].GetMethod().Name);
            obj.WriteLine("Line Number \t: " + frames[intFrameCount - 1].GetFileLineNumber().ToString());
            if (intFrameCount > 1)
            {
                obj.WriteLine("Sub Method \t: " + frames[intFrameCount - 2].GetMethod().Name);
            }
            obj.WriteLine("Machine \t: " + logEntry.MachineName);
            obj.WriteLine("App Domain \t: " + logEntry.AppDomainName);
            obj.WriteLine("ProcessId \t: " + logEntry.ProcessId);
            obj.WriteLine("Process Name \t: " + logEntry.ProcessName);
            obj.WriteLine("Thread Name \t: " + logEntry.Win32ThreadId);
            bReturn = true;

            return bReturn;
        }

        private static bool WriteErrorLog(Exception objException)
        {
            bool bReturn = false;

            StackTrace st = new StackTrace(objException, true);
            StackFrame[] frames = st.GetFrames();
            int intFrameCount = st.FrameCount;

            LogEntry logEntry = new LogEntry();

            AlwaysClosedTextFileTraceListener obj = new AlwaysClosedTextFileTraceListener();
            obj.WriteLine("");
            obj.WriteLine("Timestamp \t: " + DateTime.Now.ToString());
            obj.WriteLine("Message \t: " + objException.Message);
            obj.WriteLine("File \t\t: " + frames[intFrameCount - 1].GetFileName());
            obj.WriteLine("Method \t\t: " + frames[intFrameCount - 1].GetMethod().Name);
            obj.WriteLine("Line Number \t: " + frames[intFrameCount - 1].GetFileLineNumber().ToString());
            if (intFrameCount > 1)
            {
                obj.WriteLine("Sub Method \t: " + frames[intFrameCount - 2].GetMethod().Name);
            }
            obj.WriteLine("Machine \t: " + logEntry.MachineName);
            obj.WriteLine("App Domain \t: " + logEntry.AppDomainName);
            obj.WriteLine("ProcessId \t: " + logEntry.ProcessId);
            obj.WriteLine("Process Name \t: " + logEntry.ProcessName);
            obj.WriteLine("Thread Name \t: " + logEntry.Win32ThreadId);
            bReturn = true;

            return bReturn;
        }
    }

    public sealed class S3GLogger
    {

        ///<summary>
        /// If the LogFile path is empty then, it will write the log entry to 
        /// LogFile.txt under application directory.
        /// If the LogFile.txt is not availble it will create it
        /// If the Log File path is not empty but the file is 
        /// not availble it will create it.
        /// <param name="objException"></param>
        /// <RETURNS>false if the problem persists</RETURNS>
        ///</summary>

        public static void LogMessage(string Message, string strPageName)
        {

            // write the error log to that text file
            if (Message != null)
            {
                WriteLogMessage(Message, strPageName);

            }
        }

        private static void WriteLogMessage(string Message, string strPageName)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.EventId = 100;
            logEntry.Priority = 2;
            logEntry.Title = "S3G";
            // Create a StackTrace that captures
            // filename, line number, and column
            // information for the current thread.
            //StackTrace st = new StackTrace(true);
            //for (int i = 0; i < st.FrameCount; i++)
            //{
            //    // Note that high up the call stack, there is only
            //    // one stack frame.
            //    StackFrame sf = st.GetFrame(i);

            //    Message = Message + string.Format("\r\n High up the call stack, Method: {0}",
            //        sf.GetMethod());

            //    Message = Message + string.Format("\r\n High up the call stack, File: {0}",
            //       sf.GetFileName());

            //    Message = Message + string.Format("\r\n High up the call stack, Line Number: {0}",
            //        sf.GetFileLineNumber());
            //}
            logEntry.Message = Message;
            logEntry.ExtendedProperties.Add("Page Name", strPageName);
            logEntry.Categories.Add("General");
            Logger.Write(logEntry);

        }

        private static bool WriteErrorLog(Exception objException)
        {
            bool bReturn = false;

            StackTrace st = new StackTrace(objException, true);
            StackFrame[] frames = st.GetFrames();
            int intFrameCount = st.FrameCount;

            LogEntry logEntry = new LogEntry();

            AlwaysClosedTextFileTraceListener obj = new AlwaysClosedTextFileTraceListener();
            obj.WriteLine("");
            obj.WriteLine("Timestamp \t: " + DateTime.Now.ToString());
            obj.WriteLine("Message \t: " + objException.Message);
            obj.WriteLine("File \t\t: " + frames[intFrameCount - 1].GetFileName());
            obj.WriteLine("Method \t\t: " + frames[intFrameCount - 1].GetMethod().Name);
            obj.WriteLine("Line Number \t: " + frames[intFrameCount - 1].GetFileLineNumber().ToString());
            if (intFrameCount > 1)
            {
                obj.WriteLine("Sub Method \t: " + frames[intFrameCount - 2].GetMethod().Name);
            }
            obj.WriteLine("Machine \t: " + logEntry.MachineName);
            obj.WriteLine("App Domain \t: " + logEntry.AppDomainName);
            obj.WriteLine("ProcessId \t: " + logEntry.ProcessId);
            obj.WriteLine("Process Name \t: " + logEntry.ProcessName);
            obj.WriteLine("Thread Name \t: " + logEntry.Win32ThreadId);
            bReturn = true;

            return bReturn;
        }
    }

    public class ClsPubCommMail
    {
        #region properties

        public string ProFromRW
        {
            get;
            set;
        }
        public string ProTORW
        {
            get;
            set;
        }
        public string ProCCRW
        {
            get;
            set;
        }
        public string ProBCCRW
        {
            get;
            set;
        }
        public string ProSubjectRW
        {
            get;
            set;
        }
        public string ProMessageRW
        {
            get;
            set;
        }
        public ArrayList ProFileAttachementRW
        {
            get;
            set;
        }
        #endregion
    }

    public static class ClsPubConfigReader
    {

        /// <summary>
        /// Method to get Connection string from Config file by default 
        /// has a over load method which gets the required key's value
        /// from config file
        /// </summary>
        /// <returns>Connection String</returns>
        public static string FunPubReadConfig()
        {
            string key = String.Empty;
            key = ConfigurationSettings.AppSettings.Get("connectionString");
            return key;
        }

        /// <summary>
        /// Method to Read from Config file has a over load method which 
        /// by default get Connection string from config file
        /// </summary>
        /// <param name="KeyName"> Key name to search in config file</param>
        /// <returns>Key Value for GIven Key</returns>
        public static string FunPubReadConfig(string KeyName)
        {
            string key = String.Empty;
            key = ConfigurationSettings.AppSettings.Get(KeyName);
            return key;
        }

    }

    public static class ClsPubSerialize
    {

        public static byte[] Serialize(object ObjSerialize, SerializationMode SerMode)
        {
            using (MemoryStream msStream = new MemoryStream())
            {
                if (SerMode == SerializationMode.Xml)
                {
                    XmlSerializer xmSerializer = new XmlSerializer(ObjSerialize.GetType());
                    xmSerializer.Serialize(msStream, ObjSerialize);

                }
                if (SerMode == SerializationMode.Binary)
                {
                    BinaryFormatter binaryformatter = new BinaryFormatter();
                    binaryformatter.Serialize(msStream, ObjSerialize);
                }
                byte[] ObjSerialize_bytes = msStream.ToArray();
                msStream.Flush();
                return ObjSerialize_bytes;
            }
        }
        public static object ObjDE;
        public static object DeSerialize(byte[] ObjSerialize_bytes, SerializationMode SerMode, Type type)
        {
            var ObjDeSerialize = ObjDE;
            try
            {

                // ObjDeSerialize = null;
                if (SerMode == SerializationMode.Binary)
                {
                    using (MemoryStream msStream = new MemoryStream((ObjSerialize_bytes)))
                    {
                        //fileStreamObject = new FileStream(filename, FileMode.Open);
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        ObjDeSerialize = binaryFormatter.Deserialize(msStream);
                    }
                }
                if (SerMode == SerializationMode.Xml)
                {
                    using (MemoryStream msStream = new MemoryStream((ObjSerialize_bytes)))
                    {
                        //fileStreamObject = new FileStream(filename, FileMode.Open);
                        XmlSerializer xmlSerializer = new XmlSerializer(type);
                        ObjDeSerialize = xmlSerializer.Deserialize(msStream);
                    }
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex, "CommonBusEntity");
            }
            return ObjDeSerialize;
        }
    }


    [Serializable]
    [DataContractFormat]
    public class ClsSystemJournal
    {
        public int Company_ID { get; set; }
        public int Branch_ID { get; set; }
        public int LOB_ID { get; set; }
        public int Customer_ID { get; set; }
        public String Account_Link_Key { get; set; }
        public String Narration { get; set; }
        public DateTime Value_Date { get; set; }
        public String Txn_Currency_Code { get; set; }
        public int Program_ID { get; set; }
        public int Global_Dimension1_Code { get; set; }
        public String Global_Dimension1_No { get; set; }
        public int Report_ID { get; set; }
        public int Payment_Type { get; set; }
        public int JV_Status_Code { get; set; }
        public int Created_By { get; set; }
        public String Reference_Number { get; set; }
        public String Sub_Reference_Number { get; set; }
        public int Occurrence_No { get; set; }
        public String Txn_Amount { get; set; }
        public String GL_Account_Number { get; set; }
        public String Sub_GL_Account_Number { get; set; }
        public char Accounting_Flag { get; set; }
        public String Global_Dimension2_Code { get; set; }
        public String Global_Dimension2_No { get; set; }
        public String XMLSysJournal { get; set; }
    }

    [Serializable]
    [DataContractFormat]
    public class PagingValues
    {
        int intPageId = 0;
        public int ProCompany_ID
        {
            get;
            set;
        }
        public int ProUser_ID
        {
            get;
            set;
        }

        public int ProPageSize
        {
            get { return intPageId; }
            set { if (value == 0) intPageId = 1; else intPageId = value; }
        }
        public int ProTotalRecords
        {
            get;
            set;
        }
        public int ProCurrentPage
        {
            get;
            set;
        }
        public string ProSearchValue
        {
            get;
            set;
        }
        public string ProOrderBy
        {
            get;
            set;
        }
        public int ProProgram_ID //Added by Tamilselvan.S on 24/09/2011
        {
            get;
            set;
        }
    }

    /// <summary>
    /// This is for common to all,By Narayanan
    /// </summary>
    [DataContractFormat]
    [Serializable]
    public class CompanyHierarchyEntity
    {
        Int32 _CompanyID, _BranchID, _UserID, _LOBID, _ProductID;

        public string ProgramCode { get; set; }

        public Int32 CompanyID
        {
            get { return _CompanyID; }
            set { _CompanyID = value; }
        }
        public Int32 BranchID
        {
            get { return _BranchID; }
            set { _BranchID = value; }
        }
        public Int32 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public Int32 LOBID
        {
            get { return _LOBID; }
            set { _LOBID = value; }
        }
        public Int32 ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }

    }

    /// <summary>
    /// This is common constant to S3G_Status_LoopUp table By Narayanan
    /// </summary>
    public enum S3G_Statu_Lookup
    {
        CUSTOMER_TYPE,
        HOUSE_TYPE,
        COMPANY_TYPE,
        MARITAL_STATUS,
        APPLICATION_PROCESS,
        CREDIT_PARAMETER,
        CREDIT_PARAMETER_APPROVAL,
        GOVERNMENT,
        ACCOUNT_TYPE,
        CATEGORY_TYPE,
        TENURE_TYPE,
        PAY_TO,
        ORG_ROI_RULES_FREQUENCY,
        ORG_ROI_RULES_INSURANCE,
        ORG_ROI_RULES_IRR_REST,
        ORG_ROI_RULES_MARGIN,
        ORG_ROI_RULES_RATE,
        ORG_ROI_RULES_RATE_TYPE,
        ORG_ROI_RULES_REPAYMENT_MODE,
        ORG_ROI_RULES_RESIDUAL_VALUE,
        ORG_ROI_RULES_RETURN_PATTERN,
        ORG_ROI_RULES_TIME_VALUE,
        ALERT_TYPE,
        CASH_FLOW_FROM
    }

    /// <summary>
    /// This data contract for S3G Status look up sp Parameter.
    /// </summary>    
    [Serializable]
    [DataContractFormat]
    public class S3G_Status_Parameters
    {
        public int Option
        {
            get;
            set;
        }
        public string Param1
        {
            get;
            set;
        }
        public string Param2
        {
            get;
            set;
        }
        public string Param3
        {
            get;
            set;
        }
        public string Param4
        {
            get;
            set;
        }
        public string Param5
        {
            get;
            set;
        }
    }

    /// <summary>
    /// This data contract for Customer Master service
    /// </summary>
    [DataContractFormat]
    [Serializable]
    public class CustomerMasterBusEntity
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerType_ID { get; set; }
        public string GroupCode { get; set; }
        public string Groupname { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryName { get; set; }
        public int Constitution_ID { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public decimal CustomerPostingGroupCode_ID { get; set; }
        public string Comm_Address1 { get; set; }
        public string Comm_Address2 { get; set; }
        public string Comm_City { get; set; }
        public string Comm_State { get; set; }
        public string Comm_Country { get; set; }
        public string Comm_PINCode { get; set; }
        public string Comm_Mobile { get; set; }
        public string Comm_Telephone { get; set; }
        public string Comm_Email { get; set; }
        public string Comm_Website { get; set; }
        public string Perm_Address1 { get; set; }
        public string Perm_Address2 { get; set; }
        public string Perm_City { get; set; }
        public string Perm_State { get; set; }
        public string Perm_Country { get; set; }
        public string Perm_PINCode { get; set; }
        public string Perm_Mobile { get; set; }
        public string Perm_Telephone { get; set; }
        public string Perm_Email { get; set; }
        public string Perm_Website { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public int MaritalStatus_ID { get; set; }
        public string Qualification { get; set; }
        public string Profession { get; set; }
        public string SpouseName { get; set; }
        public decimal Children { get; set; }
        public decimal TotalDependents { get; set; }
        public DateTime WeddingAnniversaryDate { get; set; }
        public int HouseORFlat_ID { get; set; }
        public int ISOwn { get; set; }
        public string CurrentMarketValue { get; set; }
        public string RemainingLoanValue { get; set; }
        public string TotalNetMorth { get; set; }
        public int PublicCloselyheld_ID { get; set; }
        public decimal NoOfDirectors { get; set; }
        public string ListedAtStockExchange { get; set; }
        public decimal PaidupCapital { get; set; }
        public decimal FaceValueofShares { get; set; }
        public decimal BookValueofShares { get; set; }
        public string BusinessProfile { get; set; }
        public string Geographicalcoverage { get; set; }
        public decimal NoOfBranches { get; set; }
        public int GovernmentInstitutionalParticipation_ID { get; set; }
        public decimal PercentageOfStake { get; set; }
        public string JVPartnerName { get; set; }
        public decimal JVPartnerStake { get; set; }
        public string CEOName { get; set; }
        public decimal CEOAge { get; set; }
        public decimal CEOExperienceInYears { get; set; }
        public DateTime CEOWeddingDate { get; set; }
        public string ResidentialAddress { get; set; }

        //Modified By  :  Thanagm M
        //Reason       :  To add multiple Bank Details   

        //public int OwnBranch_ID { get; set; }
        //public int AccountType_ID { get; set; }
        //public string AccountNumber { get; set; }
        //public string BankName { get; set; }
        //public string BankBranch { get; set; }
        //public string MICRCode { get; set; }
        //public string BankAddress { get; set; }

        public string XmlBankDetails { get; set; }
        public int LOB_ID { get; set; }
        public int Type_ID { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ValidUpto { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public int Company_ID { get; set; }

        //Modified By  :  Saranya I
        //Reason       :  To add Relation Type  
        //Date         :  28/Jan/2012
        public bool Customer { get; set; }
        public bool Guarantor1 { get; set; }
        public bool Guarantor2 { get; set; }
        public bool CoApplicant { get; set; }
        //end
        public string XmlConstitutionalDocuments { get; set; }
        public string Mode { get; set; }

        public string XmlTrackDetails { get; set; }
        public string XmlCreditDetails { get; set; }

        //BCA Changes - Kuppu - Aug-17-2012
        public string Family_Name { get; set; }
        public string Notes { get; set; }
        //Ends

        //BDO changes - Thangam - 03-Oct-2012
        public int CreditType { get; set; }
        //End

        //BDO changes - Manikandan - 05-Oct-2012
        public bool IS_BlockListed { get; set; }
        //End 

        //BW changes - Saran - 24-Jul-2013
        public int Stock_Stmt_Frequency { get; set; }
        public int Is_BW { get; set; }
        //BW changes - Saran - 24-Jul-2013


        //Added By Ganapathy on 13-Nov-2013 BEGINS

        public int Enquiry_ID { get; set; }

        //Added By Ganapathy on 13-Nov-2013 ENDS

        public Int32 CRMID { get; set; }

	//Added by Vinodha M - To create Guarantor/Co-Applicant through CRM
        public Int32 Guarantor_CRMID { get; set; }

        public bool Account_Unlock { get; set; }

        //Added for GST Changes on 20-Aug-2018
        public string CGSTIN { get; set; }
        public DateTime CGST_Reg_Date { get; set; }

        public string SGSTIN { get; set; }
        public DateTime SGST_Reg_Date { get; set; }
        
    }

    /// <summary>
    /// This data contract for Customer constitution documents
    /// </summary>
    [DataContractFormat]
    [Serializable]
    public class CustomerConstitutionDocs
    {
        public Int32 Customer_ID { get; set; }
        public int ConstitutionDocuments_ID { get; set; }
        public int ISCollected { get; set; }
        public int ISScanned { get; set; }
        public string IdentityValues { get; set; }
        public int Created_By { get; set; }
        public int Company_ID { get; set; }

    }

    /// <summary>
    /// This data contract for Credit parameter
    /// </summary>
    [Serializable]
    [DataContractFormat]
    public class CreditParameterTransDTO
    {
        public Int32 ID { get; set; }
        public Int32 Customer_ID { get; set; }
        public string CreditParamNumber { get; set; }
        public string CreditParamDate { get; set; }
        public string CreditParamStatus { get; set; }
        public Int32 Company_ID { get; set; }
        public Int32 Created_By { get; set; }

    }

    /// <summary>
    /// This data contract for credit parameter transaction score details
    /// </summary>
    [Serializable]
    [DataContractFormat]
    public class CreditParamterTransScoreDTO
    {
        public Int32 CreditParamTrans_ID { get; set; }
        public Int32 CreditScore_Item_ID { get; set; }
        public Int32 GlobalParameter_ID { get; set; }
        public decimal PercentageImportance { get; set; }
        public decimal RequiredScore { get; set; }
        public decimal ActualScore { get; set; }
        public Int32 Created_By { get; set; }
    }

    [DataContractFormat]
    [Serializable]
    public class WorkFlowStatus
    {
        public int intProgramRefNo;
        public string strWorkSequenceId;
        public int intCompanyId;
        public int intUserId;
        public int intBranchId;
        public int intLOBId;
        public int intProductId;
        public string strTaskDocumentNo;
        public string strTaskStatus;
        public string strWorkPrgId;

    }

    [DataContractFormat]
    [Serializable]
    public class AccountCreationEntity
    {
        public int intAccountCreationId { get; set; }
        public int intCompanyId { get; set; }
        public int intUserId { get; set; }
        public int intLobId { get; set; }
        public int intBranchId { get; set; }
        public string strPANumber { get; set; }
        public int intProductId { get; set; }
        public int intApplicationProcessId { get; set; }
        public DateTime dtCreationDate { get; set; }
        public int intCustomerId { get; set; }
        public int intSalesPersonId { get; set; }
        public string Status { get; set; }

        //KLF
        public Int32 Lead_Source_Type { get; set; }
        public Int32 Lead_Source_ID { get; set; }
        //END

        public decimal dcmFinanceAmount { get; set; }
        public decimal dcmRefinanceConstract { get; set; }

        public int intConstitutionId { get; set; }
        public int intLeaseType { get; set; }
        public int intPAStatusTypeCode { get; set; }
        public int intPAStatusCode { get; set; }
        public int intTxnId { get; set; }
        public decimal dcmOfferResidualValue { get; set; }
        public decimal dcmOfferResidualValueAmount { get; set; }
        public decimal dcmOfferMargin { get; set; }
        public decimal dcmOfferMarginAmount { get; set; }
        public string strAccountNumber { get; set; }
        public string strSANum { get; set; }
        public string strConsSplitNo { get; set; }
        public string strSplit_RefNo { get; set; }
        public string XmlConstitutionDocDetails { get; set; }
        public string XmlAssetDetails { get; set; }
        public string XmlROIDetails { get; set; }
        public string XmlCashInflowDetails { get; set; }
        public string XmlOutFlowDetails { get; set; }
        public string XmlAlertDetails { get; set; }
        public string XmlFollowDetails { get; set; }
        public string XmlRepaymentDetails { get; set; }
        public string XmlMoratoriumDetails { get; set; }
        public string XmlGuarantorDetails { get; set; }
        public string XmlInvoiceDetails { get; set; }
        public string XmlCollateralDetails { get; set; }

        public int intROIRuleID { get; set; }

        public int intPaymentRuleId { get; set; }
        public int intFBDate { get; set; }
        public int intAdvanceInstallments { get; set; }
        public bool blnIsDORequired { get; set; }
        public DateTime dtLastODICalcDate { get; set; }




        public decimal dcmLoanAmount { get; set; }
        public int intTenureTypeCode { get; set; }
        public int intTenureCode { get; set; }
        public int intTenure { get; set; }
        public int intRepaymentTypecode { get; set; }
        public int intRepaymentCode { get; set; }
        public int intRepaymentTimeTypeCode { get; set; }
        public int intRepaymentTimeCode { get; set; }

        public decimal dcmBusinessIRR { get; set; }
        public decimal dcmCompanyIRR { get; set; }
        public decimal dcmAccountingIRR { get; set; }


        public string strSAInternal_code_Ref { get; set; }
        public string strSA_User_Name { get; set; }
        public string strSA_User_Address1 { get; set; }
        public string strSA_User_Address2 { get; set; }
        public string strSA_User_City { get; set; }
        public string strSA_User_State { get; set; }
        public string strSA_User_Country { get; set; }
        public string strSA_User_Pincode { get; set; }
        public string strSA_User_Phone { get; set; }
        public string strSA_User_Mobile { get; set; }
        public string strSA_User_Email { get; set; }
        public string strSA_User_Website { get; set; }

        public string XmlRepaymentStructure { get; set; }

        //Taken From Product for First Installment Date On 23-nov-2015

        public DateTime First_Install_Date { get; set; }
        public decimal CC_Rate { get; set; }

        //Taken From Product for First Installment Date On 23-nov-2015

        public int Lien_PASA_Ref_Id { get; set; }

        public string XML_Nominee { get; set; } /* Nominee */

        /*Valuation Details and Viability Details */
        public decimal ValuationAmount { get; set; }
        public string GridIBBValue { get; set; }
        public int ValuationGrade { get; set; }

        public string XMLViabilityDetails { get; set; }
        public int CustomerCategory { get; set; } 
    }

    [DataContractFormat]
    [Serializable]
    public class BillingEntity
    {
        public int intCompanyId { get; set; }
        public int intLobId { get; set; }
        public int intBranchId { get; set; }
        public long lngMonthYear { get; set; }
        public int intFrequency { get; set; }
        public DateTime dtBillingDate { get; set; }
        public DateTime dtStartDate { get; set; }
        public DateTime dtEndDate { get; set; }
        public DateTime dtScheduleDate { get; set; }
        public string strScheduleTime { get; set; }
        public int intUserId { get; set; }
        public string strXmlBranchDetails { get; set; }
        public string strXmlControlDataDetails { get; set; }
        public string strXmlCashFlowDetails { get; set; }

    }


}

namespace S3GBusEntity.EnquiryResponse
{
    [DataContractFormat]
    [Serializable]
    public class EnquiryResponseEntity
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public string Enquiry_No { get; set; }
        public Int32 Company_ID { get; set; }
        public string Customer_ID { get; set; }
        public string Status { get; set; }
        public string Response_Date { get; set; }
        public Int32 Responded_By { get; set; }
        public decimal Finance_Amount_Sought { get; set; }
        public decimal Residual_Margin_Amount { get; set; }
        public Int32 LOB_ID { get; set; }
        public Int32 Product_ID { get; set; }
        public Int32 Branch_ID { get; set; }
        public Int32 WorkFlow_Sequence { get; set; }
        public Int32 Offer_ROI_Rules_ID { get; set; }
        public Int32 Offer_Payment_RuleCard_ID { get; set; }
        public decimal Offer_ResidualValue { get; set; }
        public decimal Offer_ResidualValueAmount { get; set; }
        public decimal Offer_Margin { get; set; }
        public decimal Offer_Margin_Amount { get; set; }
        public decimal Repay_Block_Depriciation { get; set; }
        public decimal Repay_Book_Depriciation { get; set; }
        public decimal Repay_Accounting_IRR { get; set; }
        public decimal Repay_Company_IRR { get; set; }
        public decimal Repay_Business_IRR { get; set; }
        public Int32 Repay_Summary { get; set; }
        public Int32 ApplicationNumber { get; set; }
        public Int32 EnquiryResponseDetailId { get; set; }
        public string XmlAlertDetails { get; set; }
        public string XmlFollowDetails { get; set; }
        public string XmlRepaymentDetails { get; set; }
        public string XmlCashInflowDetails { get; set; }
        public string XmlOutFlowDetails { get; set; }
        public string XML_ROIRULE { get; set; }
        public string XML_REPAYSTRUCTURE { get; set; }
    }




    [DataContractFormat]

    [Serializable]
    public class S3G_ORG_CashFlow
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public string CashFlow_ID { get; set; }
        public string Date { get; set; }
        public Int32 Entity { get; set; }
        public decimal Amount { get; set; }
    }

    [DataContractFormat]
    [Serializable]
    public class S3G_ORG_EnquiryResponseAlertDetails
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public Int32 Alerts_Type { get; set; }
        public Int32 Alerts_UserContact { get; set; }
        public bool Alerts_SMS { get; set; }
        public bool Alerts_EMail { get; set; }
    }


    [DataContractFormat]
    [Serializable]
    public class S3G_ORG_Repayment
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public Int32 Enq_Res_Repay_ID { get; set; }
        public Int32 Repayment_CashFlow { get; set; }
        public decimal Amount { get; set; }
        public decimal Per_Installment_Amount { get; set; }
        public decimal Breakup_Percentage { get; set; }
        public decimal From_Installment { get; set; }
        public decimal To_Installment { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }

    }


    //[DataContractFormat]
    //public class S3G_ORG_EnquiryResponseFollowUpDetails
    //{
    //    public Int32 EnquiryResponse_ID { get; set; }
    //    public Int32 Enq_Res_FollowUp_ID { get; set; }
    //    public string Date { get; set; }
    //    public Int32 From_User_Id { get; set; }
    //    public Int32 To_User_Id { get; set; }
    //    public string Action { get; set; }
    //    public string Action_Date { get; set; }
    //    public string Customer_Response { get; set; }
    //    public string Remarks { get; set; }
    //}
    //[DataContractFormat]
    //public class S3G_ORG_EnquiryResponseOfferROIDetails
    //{
    //    public Int32 EnquiryResponse_ID { get; set; }
    //    public Int32 Enq_Res_Offer_Rule_ID { get; set; }
    //    public Int32 Serial_Number { get; set; }
    //    public string Model_Description { get; set; }
    //    public Int32 Rate_Type { get; set; }
    //    public string ROI_Rule_Number { get; set; }
    //    public Int32 Return_Pattern { get; set; }
    //    public Int32 Time_Value { get; set; }
    //    public Int32 Frequency { get; set; }
    //    public Int32 Repayment_Mode { get; set; }
    //    public decimal Rate { get; set; }
    //    public string IRR_Rest { get; set; }
    //    public Int32 Interest_Calculation { get; set; }
    //    public Int32 Interest_Levy { get; set; }
    //    public decimal Recovery_Pattern_Year1 { get; set; }
    //    public decimal Recovery_Pattern_Year2 { get; set; }
    //    public decimal Recovery_Pattern_Year3 { get; set; }
    //    public decimal Recovery_Pattern_Rest { get; set; }
    //    public Int32 Insurance { get; set; }
    //    public Int32 Residual_Value { get; set; }
    //    public Int32 Margin { get; set; }
    //    public decimal Margin_Percentage { get; set; }
    //}

    [DataContractFormat]
    [Serializable]
    public class S3G_ORG_EnquiryResponse
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public string Enquiry_No { get; set; }
        public Int32 Company_ID { get; set; }
        public string Customer_ID { get; set; }
        public string Status { get; set; }
        public string Response_Date { get; set; }
        public Int32 Responded_By { get; set; }
        public decimal Finance_Amount_Sought { get; set; }
        public decimal Residual_Margin_Amount { get; set; }
        public Int32 LOB_ID { get; set; }
        public Int32 Product_ID { get; set; }
        public Int32 Branch_ID { get; set; }
        public Int32 WorkFlow_Sequence { get; set; }
        public Int32 Offer_ROI_Rules_ID { get; set; }
        public Int32 Offer_Payment_RuleCard_ID { get; set; }
        public decimal Offer_ResidualValue { get; set; }
        public decimal Offer_ResidualValueAmount { get; set; }
        public decimal Offer_Margin { get; set; }
        public decimal Offer_Margin_Amount { get; set; }
        public decimal Repay_Block_Depriciation { get; set; }
        public decimal Repay_Book_Depriciation { get; set; }
        public Int32 Repay_Accounting_IRR { get; set; }
        public Int32 Repay_Company_IRR { get; set; }
        public Int32 Repay_Business_IRR { get; set; }
        public Int32 Repay_Summary { get; set; }
        public Int32 EnquiryResponseID { get; set; }

    }

    [DataContractFormat]
    [Serializable]
    public class Offer_ROI_Details
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public Int32 Serial_Number { get; set; }
        public string Model_Description { get; set; }
        public Int32 Rate_Type { get; set; }
        public string ROI_Rule_Number { get; set; }
        public Int32 Return_Pattern { get; set; }
        public Int32 Time_Value { get; set; }
        public Int32 Frequency { get; set; }
        public Int32 Repayment_Mode { get; set; }
        public decimal Rate { get; set; }
        public Int32 IRR_Rest { get; set; }
        public Int32 Interest_Calculation { get; set; }
        public Int32 Interest_Levy { get; set; }
        public decimal Recovery_Pattern_Year1 { get; set; }
        public decimal Recovery_Pattern_Year2 { get; set; }
        public decimal Recovery_Pattern_Year3 { get; set; }
        public decimal Recovery_Pattern_Rest { get; set; }
        public Int32 Insurance { get; set; }
        public Int32 Residual_Value { get; set; }
        public Int32 Margin { get; set; }
        public decimal Margin_Percentage { get; set; }
        public Int32 ROI_Rules_ID { get; set; }

    }

    [DataContractFormat]
    [Serializable]
    public class Offer_PaymentRuleCard
    {
        public Int32 EnquiryResponse_ID { get; set; }
        public Int32 Payment_RuleCard_ID { get; set; }
    }

    //[DataContractFormat]
    //public class FollowUp_Header
    //{
    //    public Int32 Program_ID { get; set; }
    //    public Int32 Program_PK_ID { get; set; }
    //    public Int32 LOB_ID { get; set; }
    //    public Int32 Branch_ID { get; set; }
    //    public Int32 Company_ID { get; set; }
    //    public Int32 Enquiry_Number { get; set; }
    //    public string Date { get; set; }
    //    public Int32 Created_By { get; set; }


    //}

    //[DataContractFormat]
    //public class FollowUp_Detail
    //{
    //    public Int32 Follow_Up_ID { get; set; }
    //    public Int32 From_UserID { get; set; }
    //    public Int32 To_UserID { get; set; }
    //    public string Action { get; set; }
    //    public string Date { get; set; }
    //    public string Action_Date { get; set; }
    //    public string Customer_Response { get; set; }
    //    public string Remarks { get; set; }
    //    public Int32 Created_By { get; set; }

    //}

}

namespace S3GBusEntity.ApplicationProcess
{
    [DataContractFormat]
    [Serializable]
    public class ApplicationProcess
    {
        public Int32 Application_Process_ID { get; set; }
        public string Application_Number { get; set; }
        public DateTime Date { get; set; }
        public string Business_Offer_Number { get; set; }
        public DateTime Offer_Date { get; set; }
        public string Status_ID { get; set; }
        public Int32 Customer_ID { get; set; }
        public Int32 Company_ID { get; set; }
        public Int32 LOB_ID { get; set; }
        public Int32 Branch_ID { get; set; }
        public Int32 Product_ID { get; set; }
        public Int32 Sales_Person_ID { get; set; }

        //KLF
        public Int32 Lead_Source_Type { get; set; }
        public Int32 Lead_Source_ID { get; set; }
        //END

        public decimal Business_IRR { get; set; }
        public decimal Company_IRR { get; set; }
        public decimal Accounting_IRR { get; set; }
        public decimal Finance_Amount { get; set; }
        public decimal Tenure { get; set; }
        public Int32 Tenure_Type { get; set; }
        public decimal Margin_Amount { get; set; }
        public decimal Residual_Value { get; set; }
        public Int32 Refinance_Contract { get; set; }
        public Int32 Constitution_ID { get; set; }
        public Int32 Lease_Type { get; set; }
        public decimal Asset_Margin_Percentage { get; set; }
        public decimal Asset_Margin_Amount { get; set; }
        public decimal Offer_Residual_Value { get; set; }
        public decimal Offer_Residual_Value_Amount { get; set; }
        public decimal Offer_Margin { get; set; }
        public decimal Offer_Margin_Amount { get; set; }
        public Int32 MLA_Applicable { get; set; }
        public Int32 MLA_Number { get; set; }
        public string MLA_Validity_To { get; set; }
        public string MLA_Validity_From { get; set; }
        public Int32 Created_By { get; set; }

        public Int32 Program_ID { get; set; }
        public Int32 Program_PK_ID { get; set; }
        public Int32 Payment_RuleCard_ID { get; set; }

        public string XML_PDD { get; set; }
        public string XML_ALERT { get; set; }
        public string XML_Guarantor { get; set; }
        public string XML_Moratorium { get; set; }
        public string XML_FollowDetail { get; set; }
        public string XML_Repayment { get; set; }
        public string XML_Inflow { get; set; }
        public string XML_OutFlow { get; set; }
        public string XML_AssetDetails { get; set; }
        public string XML_Constitution { get; set; }
        public string XML_Nominee { get; set; }
        public string XML_Invoice { get; set; }
        public string XML_Collateral { get; set; }
        public string XML_ROIRULE { get; set; }
        public string XMLRepaymentStructure { get; set; }

        public int intFBDate { get; set; }

        //Added on 03-Aug-2013 start
        public int Loan_Type { get; set; }
        //Added on 03-Aug-2013 end

        //Added on 26APR2014 for KLF CR002 start
        public int Leave_Period { get; set; }
        //Added on 26APR2014 for KLF CR002 end

        //Taken From Product for First Installment Date On 23-nov-2015

        public DateTime First_Install_Date { get; set; }
        public decimal CC_Rate { get; set; }

        //Taken From Product for First Installment Date On 23-nov-2015

        public int Lien_PA_SA_REF_ID { get; set; }  //Added for Hand Loan On 02-Jan-2016

        public Int64 CRM_ID { get; set; }

        //Added for Devaition CR on 07-Feb-2018 = > Code starts
        //public int Deviation_ID { get; set; }
        //public string Deviation_Remarks { get; set; }
        public string XMLDeviationDetails { get; set; }
        //Added for Devaition CR on 07-Feb-2018 = > Code Ends

        /*Valuation Details and Viability Details */
        public decimal ValuationAmount  { get; set; }
        public string GridIBBValue { get; set; }
        public int ValuationGrade { get; set; }
        
        public string XMLViabilityDetails { get; set; }

        public string XMLDeDupeCheck { get; set; }

        public int SegmentsOfUsage { get; set; }
        public decimal CIBILScore { get; set; }

        public int CustomerCategory { get; set; } 
    }




}


