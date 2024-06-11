#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Created By			: Thangam M
/// Created Date		: 05-Sep-2011
/// Purpose	            : To connect SQL/ORACLE databases.
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Data; 
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using S3GBusEntity;
using System.Data.OracleClient;
using System.Xml;
using System.IO;

namespace S3GDALayer
{
    public static partial class ClsPubCommonDB
    {
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="enumDBType">S3GBusEntity.S3GDBType</param>
        /// <returns></returns>
        public static int FunPubExecuteNonQuery(this Database db, DbCommand command)
        {
            int intReturnVal;
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                }

                intReturnVal = db.ExecuteNonQuery(command);
                
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    FunReplaceOracleParameterOut(ref command);
                }
                return intReturnVal;

            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteNonQuery with transaction
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="trans">System.Data.Common.DbTransaction</param>
        /// <param name="enumDBType">S3GBusEntity.S3GDBType</param>
        /// <returns></returns>
        public static int FunPubExecuteNonQuery(this Database db, DbCommand command, ref DbTransaction trans)
        {
            int intReturnVal;
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                }

                intReturnVal = db.ExecuteNonQuery(command, trans);
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    FunReplaceOracleParameterOut(ref command);
                }
                return intReturnVal;
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// LoadDataSet
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="DSet">System.Data.DataSet</param>
        /// <param name="strTableName">System.String</param>
        /// <param name="enumDBType">S3GBusEntity.S3GDBType</param>
        public static void FunPubLoadDataSet(this Database db, DbCommand command, DataSet DSet, string strTableName)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                    AddCursorOutParameter(ref command, db.ConnectionString);
                }

                db.LoadDataSet(command, DSet, strTableName);
                

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    DSet = FunRemoveDummyTable(DSet); 
                    FunReplaceOracleParameterOut(ref command);
                }
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// LoadDataSet
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="DSet">System.Data.DataSet</param>
        /// <param name="strTableName">System.String</param>
        /// <param name="enumDBType">S3GBusEntity.S3GDBType</param>
        public static void FunPubLoadDataSetStringQuery(this Database db, DbCommand command, DataSet DSet, string strTableName)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                db.LoadDataSet(command, DSet, strTableName);

                /*Commented due to issue on loading empty Datatable1-Column 
                 * & Row value = 0-- Kuppusamy B-16/02/2012 - Enquiry Assignment Form*/
                //if (enumDBType == S3GDALDBType.ORACLE)
                //{
                //    DSet = FunRemoveDummyTable(DSet);
                //}
                
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }
                
        /// <summary>
        /// LoadDataSet - Table Collections
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="DSet">System.Data.DataSet</param>
        /// <param name="strTableName">System.String</param>
        /// <param name="enumDBType">S3GBusEntity.S3GDBType</param>
        public static void FunPubLoadDataSet(this Database db, DbCommand command, DataSet DSet, string[] strTableName)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                    AddCursorOutParameter(ref command, db.ConnectionString);
                }

                db.LoadDataSet(command, DSet, strTableName);
                
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    DSet = FunRemoveDummyTable(DSet);
                    FunReplaceOracleParameterOut(ref command);
                }
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="enumDBType">S3GBusEntity.S3GDBType</param>
        /// <returns></returns>
        public static IDataReader FunPubExecuteReader(this Database db, DbCommand command)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                    AddCursorOutParameter(ref command, db.ConnectionString);
                }

                return db.ExecuteReader(command);
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <returns></returns>
        public static DataSet FunPubExecuteDataSet(this Database db, DbCommand command)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                DataSet ObjDataSet = new DataSet();

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                    AddCursorOutParameter(ref command, db.ConnectionString);
                }
                ObjDataSet = db.ExecuteDataSet(command);
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    ObjDataSet =FunRemoveDummyTable(ObjDataSet);
                    FunReplaceOracleParameterOut(ref command);
                }
                return ObjDataSet;
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static DataSet FunPubExecuteDataSet(this Database db, DbCommand command, ref DbTransaction trans)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                DataSet ObjDataSet = new DataSet();

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                    AddCursorOutParameter(ref command, db.ConnectionString);
                }

                ObjDataSet = db.ExecuteDataSet(command, trans);

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    ObjDataSet = FunRemoveDummyTable(ObjDataSet);
                    FunReplaceOracleParameterOut(ref command);
                }
                return ObjDataSet;

            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="db">Microsoft.Practices.EnterpriseLibrary.Data.Database</param>
        /// <param name="command">System.Data.Common.DbCommand</param>
        /// <returns></returns>
        public static object FunPubExecuteScalar(this Database db, DbCommand command)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    DataSet ObjDataSet = new DataSet();
                    command.FunPubGetORACLE_SP();
                    FunReplaceOracleParameter(ref command, enumDBType);
                    AddCursorOutParameter(ref command, db.ConnectionString);
                    ObjDataSet = FunRemoveDummyTable(db.ExecuteDataSet(command));
                    if (ObjDataSet.Tables.Count > 0)
                        return ObjDataSet.Tables[0].Rows[0][0].ToString().Trim();
                    else
                        return "";
                }
                else
                {
                    return db.ExecuteScalar(command);
                }
            }
            catch (Exception ex)
            {
                int intCountParam = command.Parameters.Count;
                FunGetParameterCommonErrorLogDB(db, command, intCountParam, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Get the ORACLE SP Name relevant to the SQL SP Name
        /// </summary>
        /// <param name="command"></param>
        public static void FunPubGetORACLE_SP(this DbCommand command)
        {
            try
            {
                S3GBusEntity.SPNames_SQL_ORA.ResourceManager.IgnoreCase = true;
                command.CommandText = S3GBusEntity.SPNames_SQL_ORA.ResourceManager.GetString(command.CommandText.Trim().Replace("[", "").Replace("]", "").Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Methods for ORACLE Commands

        public static string FunPubGetFormatedXML(string XML)
        {
            XmlDocument doc = new XmlDocument();
            doc.InnerXml = XML;

            StringBuilder strXML = new StringBuilder();
            strXML.Append("<Root>");

            foreach (XmlNode dtlNode in doc.ChildNodes[0].ChildNodes)
            {
                strXML.Append("<Details>");
                foreach (XmlAttribute dtlAttribute in dtlNode.Attributes)
                {
                    strXML.Append("<" + dtlAttribute.Name + ">" + dtlAttribute.Value + "</" + dtlAttribute.Name + ">");
                }
                strXML.Append("</Details>");
            }
            strXML.Append("</Root>");

            return strXML.ToString();
        }

        public static void FunReplaceOracleParameter(ref DbCommand dbCmd, S3GDALDBType enumDBType)
        {
            try
            {
                for (int i = 0; i <= dbCmd.Parameters.Count - 1; i++)
                {
                    if (dbCmd.Parameters[i].DbType == DbType.Boolean)
                    {
                        dbCmd.Parameters[i].DbType = DbType.Int32;
                        if (!string.IsNullOrEmpty(dbCmd.Parameters[i].Value.ToString()))
                        {   
                            if (Convert.ToBoolean(dbCmd.Parameters[i].Value) == true)
                            {
                                dbCmd.Parameters[i].Value = 1;
                            }
                            else
                            {
                                dbCmd.Parameters[i].Value = 0;
                            }
                        }
                    }
                    else if (dbCmd.Parameters[i].DbType == DbType.DateTime)
                    {
                        dbCmd.Parameters[i].DbType = DbType.String;
                        dbCmd.Parameters[i].Value = dbCmd.Parameters[i].Value.ToString(); 
                    }
                    dbCmd.Parameters[i].ParameterName = dbCmd.Parameters[i].ParameterName.Replace("@", "p_").Replace(" ","");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FunReplaceOracleParameterOut(ref DbCommand dbCmd)
        {
            try
            {
                for (int i = 0; i <= dbCmd.Parameters.Count - 1; i++)
                {
                    dbCmd.Parameters[i].ParameterName = "@" + dbCmd.Parameters[i].ParameterName.Trim().
                        Substring(2,dbCmd.Parameters[i].ParameterName.Trim().Length-2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddCursorOutParameter(ref DbCommand command, string dbCon)
        {
            try
            {
                AddParameter(command as OracleCommand, dbCon, OracleType.Cursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddParameter(OracleCommand command, string dbCon, OracleType oracleType, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
        {
            OracleConnection con = new OracleConnection(dbCon);

            try
            {
                con.Open();
                OracleCommand oracmd = new OracleCommand(command.CommandText, con);
                oracmd.CommandType = CommandType.StoredProcedure;
                OracleCommandBuilder.DeriveParameters(oracmd);

                for (int i = 0; i <= oracmd.Parameters.Count - 1; i++)
                {
                    OracleParameter pmt = oracmd.Parameters[i];
                    if (pmt.Direction == ParameterDirection.Output && pmt.OracleType == OracleType.Cursor)
                    {
                        OracleParameter param = new OracleParameter(pmt.ParameterName, oracleType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
                        param.OracleType = oracleType;
                        command.Parameters.Add(param);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static DataSet FunRemoveDummyTable(DataSet ObjDataSet)
        {
            for (int intTblCount = 0; intTblCount <= ObjDataSet.Tables.Count - 1; intTblCount++)
            {
                //Changed by Thangam M on 22/Feb/2012 to check for column count
                if (ObjDataSet.Tables[intTblCount].Columns.Count > 0 && ObjDataSet.Tables[intTblCount].Columns[0].ColumnName.ToUpper() == "ORA_DUMMYTABLE")
                {
                    ObjDataSet.Tables.RemoveAt(intTblCount);
                    intTblCount--;
                }
            }
            return ObjDataSet; 
        }

        #endregion

        private static void FunGetParameterCommonErrorLogDB(Database db, DbCommand dbCmd, int i, string strErrorMsg)
        {
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            try
            {
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                string strSendError = (string)AppReader.GetValue("SendError", typeof(string));

                if (strSendError == "1")
                {
                    int intReturnVal;
                    string strPramValues = "";
                    string strPramType = "";
                    for (int intcount = 0; intcount < i; intcount++)
                    {
                        if (dbCmd.Parameters[intcount].Value != null)
                        {
                            if (!string.IsNullOrEmpty(dbCmd.Parameters[intcount].Value.ToString()))
                            {
                                if (dbCmd.Parameters[intcount].DbType.ToString().Contains("String"))
                                {
                                    strPramValues = strPramValues + " " + dbCmd.Parameters[intcount].ParameterName + " := " + "'" + dbCmd.Parameters[intcount].Value.ToString().Replace("'", "''") + "';" + Environment.NewLine;
                                }
                                else
                                {
                                    strPramValues = strPramValues + " " + dbCmd.Parameters[intcount].ParameterName + " := " + dbCmd.Parameters[intcount].Value.ToString() + ";" + Environment.NewLine;
                                }
                            }
                            else
                            {
                                strPramValues = strPramValues + " " + dbCmd.Parameters[intcount].ParameterName + " := NULL;" + Environment.NewLine;
                            }
                        }
                        else
                        {
                            strPramValues = strPramValues + " " + dbCmd.Parameters[intcount].ParameterName + " := NULL;" + Environment.NewLine;
                        }

                        strPramType = strPramType + " " + dbCmd.Parameters[intcount].ParameterName + "\t :  " + dbCmd.Parameters[intcount].DbType.ToString() + Environment.NewLine;

                    }
                    //dictParams.Add("@USER_ID", string.Empty);
                    //dictParams.Add("@PROCEDURE_NAME", dbCmd.CommandText);
                    //dictParams.Add("@ERROR_MESSAGE", strErrorMsg);
                    //dictParams.Add("@PARAM_VALUES", strPramValues);
                    //dictParams.Add("@PARAM_TYPE", strPramType);
                    try
                    {
                        DbCommand dbErrorLogCommand = db.GetStoredProcCommand("S3G_INS_ERROR_LOG_DB");
                        db.AddInParameter(dbErrorLogCommand, "@USER_ID", DbType.String, string.Empty);
                        db.AddInParameter(dbErrorLogCommand, "@PROCEDURE_NAME", DbType.String, dbCmd.CommandText);
                        db.AddInParameter(dbErrorLogCommand, "@ERROR_MESSAGE", DbType.String, strErrorMsg);
                        db.AddInParameter(dbErrorLogCommand, "@PARAM_VALUES", DbType.String, strPramValues);
                        db.AddInParameter(dbErrorLogCommand, "@PARAM_TYPE", DbType.String, strPramType);
                        db.AddOutParameter(dbErrorLogCommand, "@ERRORCODE", DbType.Int32, 100);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            //command.FunPubGetORACLE_SP();
                            FunReplaceOracleParameter(ref dbErrorLogCommand, enumDBType);
                        }

                        intReturnVal = db.ExecuteNonQuery(dbErrorLogCommand);

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            FunReplaceOracleParameterOut(ref dbErrorLogCommand);
                        }
                    }
                    catch (Exception ae)
                    {
                        throw ae;
                    }
                }
                else
                {
                    XmlDocument conxmlDoc = Common.ClsIniFileAccess.xmlDoc;
                    //string strLogFile = conxmlDoc.ChildNodes[0].SelectSingleNode("LogFiles").ChildNodes[0].Attributes["S3GWebLogFile"].Value;
                    string strLogFile = (string)AppReader.GetValue("COMMONERRORLOG", typeof(string));

                    string strError = "";

                    strError = "DateTime : " + DateTime.Now;
                    strError = strError + strErrorMsg;

                    if (File.Exists(strLogFile))
                    {
                        File.AppendAllText(strLogFile, strError);
                    }
                }
            }
            catch (Exception ex)
            {
                //    int intCountParam = dbCmd.Parameters.Count;
                //    FunGetParameterCommonErrorLogDB(dbCmd, intCountParam, ex.Message);
                throw ex;
            }
        }


    }
    public enum S3GDALDBType
    {
        ORACLE = 0,
        SQL = 1,
    }
}
