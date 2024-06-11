#region namespaces
using System;using S3GDALayer.S3GAdminServices;using S3GDALayer.S3GAdminServices;
using System.Web;
using System.Globalization;
using S3GBusEntity;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Resources;
using System.Reflection;
using System.Configuration;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ServiceModel;
#endregion

namespace S3GDALayer
{
    namespace S3GAdminServices
    {
        public class ClsPubS3GAdminErrLog
        {
            Database db;

            public ClsPubS3GAdminErrLog()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

        }
        public sealed class ClsPubCommErrorLogDal
        {
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
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjErrorLog = new ClsPubS3GAdmin();
                Dictionary<string, string> dictParams = new Dictionary<string, string>();
                StackTrace st = new StackTrace(objException, true);
                StackFrame[] frames = st.GetFrames();
                int intFrameCount = st.FrameCount;
                LogEntry logEntry = new LogEntry();
                dictParams.Add("@User_ID", string.Empty);
                dictParams.Add("@Error_Message", objException.Message);
                dictParams.Add("@Category_Name", string.Empty);
                dictParams.Add("@Priority", string.Empty);
                dictParams.Add("@Event_ID", string.Empty);
                dictParams.Add("@Severity", string.Empty);
                dictParams.Add("@Title", string.Empty);
                dictParams.Add("@Machine", logEntry.MachineName);
                dictParams.Add("@App_Domain", logEntry.AppDomainName);
                dictParams.Add("@Process_ID", logEntry.ProcessId);
                dictParams.Add("@Process_Name", logEntry.ProcessName);
                dictParams.Add("@Thread_Name", logEntry.Win32ThreadId);
                dictParams.Add("@Win32_ThreadID", string.Empty);
                dictParams.Add("@Extended_Properties", string.Empty);

                string strLastIncomeDate;
                strLastIncomeDate = ObjErrorLog.FunPubSysErrorLog("S3G_INS_ERROR_LOG", dictParams);
                bReturn = true;
                return bReturn;
            }

            private static bool WriteErrorLog(Exception objException, string strPageName, string strProjectName)
            {
                bool bReturn = false;
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjErrorLog = new ClsPubS3GAdmin();
                Dictionary<string, string> dictParams = new Dictionary<string, string>();
                StackTrace st = new StackTrace(objException, true);
                StackFrame[] frames = st.GetFrames();
                int intFrameCount = st.FrameCount;
                LogEntry logEntry = new LogEntry();
                dictParams.Add("@User_ID", string.Empty);
                dictParams.Add("@Error_Message", objException.Message);
                dictParams.Add("@Category_Name", string.Empty);
                dictParams.Add("@Priority", string.Empty);
                dictParams.Add("@Event_ID", string.Empty);
                dictParams.Add("@Severity", string.Empty);
                dictParams.Add("@Title", string.Empty);
                dictParams.Add("@Machine", logEntry.MachineName);
                dictParams.Add("@App_Domain", logEntry.AppDomainName);
                dictParams.Add("@Process_ID", logEntry.ProcessId);
                dictParams.Add("@Process_Name", logEntry.ProcessName);
                dictParams.Add("@Thread_Name", logEntry.Win32ThreadId);
                dictParams.Add("@Win32_ThreadID", string.Empty);
                dictParams.Add("@Extended_Properties", string.Empty);

                string strLastIncomeDate;
                strLastIncomeDate = ObjErrorLog.FunPubSysErrorLog("S3G_INS_ERROR_LOG", dictParams);
                bReturn = true;
                return bReturn;
            }

            private static bool WriteErrorLog(Exception objException)
            {
                bool bReturn = false;
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjErrorLog = new ClsPubS3GAdmin();
                Dictionary<string, string> dictParams = new Dictionary<string, string>();
                StackTrace st = new StackTrace(objException, true);
                StackFrame[] frames = st.GetFrames();
                int intFrameCount = st.FrameCount;
                LogEntry logEntry = new LogEntry();

                dictParams.Add("@User_ID", string.Empty);
                dictParams.Add("@Error_Message", objException.Message);
                dictParams.Add("@Category_Name", string.Empty);
                dictParams.Add("@Priority", string.Empty);
                dictParams.Add("@Event_ID", string.Empty);
                dictParams.Add("@Severity", string.Empty);
                dictParams.Add("@Title", string.Empty);
                dictParams.Add("@Machine", logEntry.MachineName);
                dictParams.Add("@App_Domain", logEntry.AppDomainName);
                dictParams.Add("@Process_ID", logEntry.ProcessId);
                dictParams.Add("@Process_Name", logEntry.ProcessName);
                dictParams.Add("@Thread_Name", logEntry.Win32ThreadId);
                dictParams.Add("@Win32_ThreadID", string.Empty);
                dictParams.Add("@Extended_Properties", string.Empty);

                string strLastIncomeDate;
                strLastIncomeDate = ObjErrorLog.FunPubSysErrorLog("S3G_INS_ERROR_LOG", dictParams);
                bReturn = true;
                return bReturn;
            }
        }
    }

}