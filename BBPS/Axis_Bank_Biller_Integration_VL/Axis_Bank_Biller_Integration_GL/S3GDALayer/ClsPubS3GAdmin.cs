#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Common
/// Screen Name			: Common DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 21-Jul-2010
/// Reason              : To implement code review points
/// Last Updated By		: Thalaiselvam N    
/// Last Updated Date   : 31-Aug-2011
/// Reason              : 1) Create "Password Validation" Region.
/// <Program Summary>
#endregion

#region namespaces
using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Data.OracleClient;
using S3GBusEntity;
using System.Collections;
using System.IO;
#endregion

namespace S3GDALayer
{
    namespace S3GAdminServices
    {
        public class ClsPubS3GAdmin
        {
            #region Intialization

            Database db;
            public ClsPubS3GAdmin()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            int intErrorCode;
            DataSet ds_Menu;
            StringBuilder strQuery;
            #endregion

            #region Common methods
            /// <summary>
            /// Common Method that will exeute the Required Procedure when send with 
            /// respective parameters and will return a datatable
            /// </summary>
            /// <param name="strProcName">Procedure name which will be executed</param>
            /// <param name="dctProcParams">dictionary Containing Parameters for the sp</param>
            /// <returns>Return a DataTable</returns>
            public DataTable FunPubFillDropdown(string strProcName, Dictionary<string, string> dctProcParams)
            {

                DataSet ds = new DataSet();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    //Database db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    command.CommandTimeout = 3600;
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        //db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                        if (!string.IsNullOrEmpty(ProcPair.Value) && ProcPair.Value.Contains("<Root>") && enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter(ProcPair.Key,
                              OracleType.Clob, ProcPair.Value.Length,
                              ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, string.IsNullOrEmpty(ProcPair.Value) ? " " : ProcPair.Value);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                        }
                    }

                    db.FunPubLoadDataSet(command, ds, "ListTable");
                    //db.LoadDataSet(command, ds, "ListTable");

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                //return ds.Tables["ListTable"];

                return ds.Tables.Count == 0 ? null : ds.Tables[0];

            }
            public DataTable PubExecuteSQL(Dictionary<string, string> ProcParams)
            {
                DataSet dsSQL = new DataSet();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_GetDDLValue);
                    foreach (KeyValuePair<string, string> ProcPair in ProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }

                    db.FunPubLoadDataSet(command, dsSQL, "ListTable");
                    //db.LoadDataSet(command, dsSQL, "ListTable");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //return dsSQL.Tables["ListTable"];
                return dsSQL.Tables.Count == 0 ? null : dsSQL.Tables[0];
                //return dsSQL.Tables[0];
            }

            /// <summary>
            /// Common Method that will exeute the Required Procedure when send with 
            /// respective parameters and will return a DataSet
            /// </summary>
            /// <param name="strProcName">Procedure name which will be executed</param>
            /// <param name="dctProcParams">dictionary Containing Parameters for the sp</param>
            /// <returns>Return a DataSet</returns>
            public DataSet FunPubFillDataset(string strProcName, Dictionary<string, string> dctProcParams)
            {
                DataSet ds = new DataSet();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        if (ProcPair.Value != null)
                        {                            
                            if (ProcPair.Value.ToUpper().Contains("<ROOT>") && enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter(ProcPair.Key,
                                  OracleType.Clob, ProcPair.Value.Length,
                                  ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, string.IsNullOrEmpty(ProcPair.Value) ? " " : ProcPair.Value);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                            }
                        }
                        //db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    command.CommandTimeout = 1800;//Added on 17-Dec-2014
                    db.FunPubLoadDataSet(command, ds, "ListTable");
                    //db.LoadDataSet(command, ds, "ListTable");

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds;

            }

            //Added by Kali on 17-Feb-2011 for Output Parameter Option

            public DataTable FunPubFillOutputDataTable(string strProcName, Dictionary<string, string> dctProcParams, Dictionary<string, string> dctProcParamsOutput, out ArrayList arrOutput)
            {

                DataSet ds = new DataSet();
                arrOutput = new ArrayList();
                StringBuilder strbuildOutput = new StringBuilder();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }

                    foreach (KeyValuePair<string, string> ProcPair in dctProcParamsOutput)
                    {
                        db.AddOutParameter(command, ProcPair.Key, DbType.String, 100);
                    }

                    ds = db.FunPubExecuteDataSet(command);
                    //ds = db.FunPubExecuteDataSet(command);

                    foreach (KeyValuePair<string, string> ProcPair in dctProcParamsOutput)
                    {
                        if (command.Parameters[ProcPair.Key].Value != null)
                        {
                            arrOutput.Add((string)command.Parameters[ProcPair.Key].Value);

                        }
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds.Tables[0];

            }

            public string FunValidateMonthClosure(string strProcName, Dictionary<string, string> dctProcParams)
            {
                try
                {
                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 10);
                    db.FunPubExecuteNonQuery(command);

                    return Convert.ToString(command.Parameters["@ErrorCode"].Value);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }

            }

            public string FunGetScalarValue(string strProcName, Dictionary<string, string> dctProcParams)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }

                    return Convert.ToString(db.FunPubExecuteScalar(command));
                    //return Convert.ToString(db.FunPubExecuteScalar(command));

                    //if (db.FunPubExecuteScalar(command) != null)
                    //{
                    //    return (db.FunPubExecuteScalar(command)).ToString();
                    //}
                    //else
                    //{
                    //    return "";
                    //}
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }

            }
            /// <summary>
            /// Function Mainly for Gridview Binding with paging it should have predefined Paging object
            /// passed to it.
            /// </summary>
            /// <param name="strProcName">Procedure name used for grid binding </param>
            /// <param name="dctProcParams">Parameter for the Procedure</param>
            /// <param name="intTotalRecords">Total records that the query return</param>
            /// <param name="ObjPaging"> Paging object that has page related details</param>
            /// <returns>DataTable</returns>
            public DataTable FunPubFillGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                DataSet ds = new DataSet();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    if (ObjPaging.ProPageSize == 0)
                    {
                        ObjPaging.ProPageSize = 1;
                    }
                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPaging.ProUser_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    //if (ObjPaging.ProProgram_ID > 0)
                        //db.AddInParameter(command, "@Program_Id", DbType.String, ObjPaging.ProProgram_ID);
                    db.FunPubLoadDataSet(command, ds, "GridTable");
                    //db.LoadDataSet(command, ds, "GridTable");

                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds.Tables["GridTable"];

            }
            public DataTable FunPubFillGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, out int intErrorCode, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                intErrorCode = 0;
                DataSet ds = new DataSet();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPaging.ProUser_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                    db.FunPubLoadDataSet(command, ds, "GridTable");
                    //db.LoadDataSet(command, ds, "GridTable");

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds.Tables["GridTable"];

            }

            #endregion

            #region Menu Loading Code
            /// <summary>
            /// Function To get user Menu as Dataset
            /// </summary>
            /// <param name="intUserId">User Id for which menu to be loaded</param>
            /// <param name="strUserName"> User Name of the user</param>
            /// <returns>DataSet</returns>
            public DataSet FunPubGetUserMenu(int intUserId, string strUserName, string strUserType)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                try
                {
                    ds_Menu = new DataSet();
                    if (strUserType == "USER")
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_UserMenuItems");
                        db.AddInParameter(command, "@User_Id", DbType.Int32, intUserId);
                        db.AddInParameter(command, "@Option", DbType.Int32, 3);

                        db.FunPubLoadDataSet(command, ds_Menu, "Module_Header");
                        //db.LoadDataSet(command, ds_Menu, "Module_Header");

                        for (int i = 0; i < ds_Menu.Tables["Module_Header"].Rows.Count; i++)
                        {
                            command = db.GetStoredProcCommand("S3G_Get_UserMenuItems");
                            db.AddInParameter(command, "@Option", DbType.Int32, 4);
                            db.AddInParameter(command, "@User_Id", DbType.Int32, intUserId);
                            db.AddInParameter(command, "@Module_Id", DbType.Int32, ds_Menu.Tables["Module_Header"].Rows[i].ItemArray[0].ToString());
                            string strHeader = ds_Menu.Tables["Module_Header"].Rows[i].ItemArray[2].ToString();

                            db.FunPubLoadDataSet(command, ds_Menu, strHeader);
                            //db.LoadDataSet(command, ds_Menu, strHeader);
                        }
                        if (ds_Menu.Tables["Module_Header"].Rows.Count == 0)
                        {
                            switch (strUserName)
                            {
                                case "S3GUSER":
                                    command = db.GetStoredProcCommand("S3G_Get_UserMenuItems");
                                    db.AddInParameter(command, "@Option", DbType.Int32, 1);

                                    db.FunPubLoadDataSet(command, ds_Menu, "System Admin");
                                    //db.LoadDataSet(command, ds_Menu, "System Admin");
                                    break;
                                case "SYSADMIN":
                                    command = db.GetStoredProcCommand("S3G_Get_UserMenuItems");
                                    db.AddInParameter(command, "@Option", DbType.Int32, 2);

                                    db.FunPubLoadDataSet(command, ds_Menu, "System Admin");
                                    //db.LoadDataSet(command, ds_Menu, "System Admin");
                                    break;
                            }
                        }
                    }
                    else if (strUserType == "UTPA")
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_UserMenuItems");
                        db.AddInParameter(command, "@Option", DbType.Int32, 5);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, intUserId);

                        db.FunPubLoadDataSet(command, ds_Menu, "Menu");
                        //db.LoadDataSet(command, ds_Menu, "Menu");
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }

                return ds_Menu;
            }
            #endregion

            #region Login Validations
            /// <summary>
            /// 
            /// </summary>
            /// <param name="strUserLoginCode"></param>
            /// <param name="strPassword"></param>
            /// <param name="intCompanyID"></param>
            /// <param name="intUserID"></param>
            /// <param name="intUser_Level_ID"></param>
            /// <param name="strUserName"></param>
            /// <param name="strCompanyName"></param>
            /// <param name="strCompanyCode"></param>
            /// <param name="LastLoginDate"></param>
            /// <param name="strTheme"></param>
            /// <param name="strAccess"></param>
            /// <param name="strCountryName"></param>
            /// <param name="strUserType"></param>
            /// <returns>Return error code Default i.e if no error it will return 0</returns>
            public int FunPubValidateLogin(string strUserLoginCode, out string strPassword, out int intCompanyID, out int intUserID, out int intUser_Level_ID, out string strUserName, out string strCompanyName, out string strCompanyCode, out DateTime LastLoginDate, out string strTheme, out string strAccess, out string strCountryName, out string strUserType, out string strMarquee, int SSOEnabled)
            {
                intCompanyID = 0;
                intUserID = 0;
                intUser_Level_ID = 0;
                strCompanyName = "";
                strCompanyCode = "";
                strUserName = "";
                strTheme = "";
                strAccess = "";
                strCountryName = string.Empty;
                strUserType = string.Empty;
                strMarquee = string.Empty;
                LastLoginDate = DateTime.Now;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Validate_Login_new");
                    db.AddInParameter(command, "@UserLoginID", DbType.String, strUserLoginCode);
                    db.AddOutParameter(command, "@Password", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@Last_LoginDate", DbType.DateTime, 200);
                    db.AddOutParameter(command, "@User_Theme", DbType.String, 50);
                    db.AddOutParameter(command, "@CompanyID", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@UserID", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@User_Level_ID", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@UserName", DbType.String, 100);
                    db.AddOutParameter(command, "@CompanyName", DbType.String, 80);
                    db.AddOutParameter(command, "@CompanyCode", DbType.String, 3);
                    db.AddOutParameter(command, "@LevelAccess", DbType.String, 50);
                    db.AddOutParameter(command, "@CountryName", DbType.String, 60);
                    db.AddOutParameter(command, "@UserType", DbType.String, 60);
                    db.AddOutParameter(command, "@Marquee_Text", DbType.String, 1000);
                    db.AddInParameter(command, "@SSOEnabled", DbType.String, SSOEnabled);

                    db.FunPubExecuteNonQuery(command);
                    //db.FunPubExecuteNonQuery(command);
                    intErrorCode = (command.Parameters["@ErrorCode"].Value != DBNull.Value) ? (int)command.Parameters["@ErrorCode"].Value : 0;
                    intCompanyID = (command.Parameters["@CompanyID"].Value != DBNull.Value) ? (int)command.Parameters["@CompanyID"].Value : 0;
                    intUserID = (command.Parameters["@UserID"].Value != DBNull.Value) ? (int)command.Parameters["@UserID"].Value : 0;
                    strPassword = Convert.ToString((command.Parameters["@Password"].Value != DBNull.Value) ? command.Parameters["@Password"].Value : 0);
                    intUser_Level_ID = (command.Parameters["@User_Level_ID"].Value != DBNull.Value) ? (int)command.Parameters["@User_Level_ID"].Value : 0;
                    strUserName = Convert.ToString((command.Parameters["@UserName"].Value != DBNull.Value) ? command.Parameters["@UserName"].Value : 0);
                    strCompanyName = Convert.ToString((command.Parameters["@CompanyName"].Value != DBNull.Value) ? command.Parameters["@CompanyName"].Value : 0);
                    strCompanyCode = Convert.ToString((command.Parameters["@CompanyCode"].Value != DBNull.Value) ? command.Parameters["@CompanyCode"].Value : 0);
                    LastLoginDate = Convert.ToDateTime((command.Parameters["@Last_LoginDate"].Value != DBNull.Value) ? command.Parameters["@Last_LoginDate"].Value : DateTime.Now);
                    strTheme = Convert.ToString((command.Parameters["@User_Theme"].Value != DBNull.Value) ? command.Parameters["@User_Theme"].Value : string.Empty);
                    strAccess = Convert.ToString((command.Parameters["@LevelAccess"].Value != DBNull.Value) ? command.Parameters["@LevelAccess"].Value : string.Empty);
                    strCountryName = Convert.ToString((command.Parameters["@CountryName"].Value != DBNull.Value) ? command.Parameters["@CountryName"].Value : string.Empty);
                    strUserType = Convert.ToString((command.Parameters["@UserType"].Value != DBNull.Value) ? command.Parameters["@UserType"].Value : string.Empty);
                    strMarquee = Convert.ToString((command.Parameters["@Marquee_Text"].Value != DBNull.Value) ? command.Parameters["@Marquee_Text"].Value : string.Empty);
                }

                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }
            #endregion

            #region Login Validations
            /// <summary>
            /// 
            /// </summary>
            /// <param name="strUserLoginCode"></param>
            /// <param name="strPassword"></param>
            /// <param name="intCompanyID"></param>
            /// <param name="intUserID"></param>
            /// <param name="intUser_Level_ID"></param>
            /// <param name="strUserName"></param>
            /// <param name="strCompanyName"></param>
            /// <param name="strCompanyCode"></param>
            /// <param name="LastLoginDate"></param>
            /// <param name="strTheme"></param>
            /// <param name="strAccess"></param>
            /// <param name="strCountryName"></param>
            /// <param name="strUserType"></param>
            /// <returns>Return error code Default i.e if no error it will return 0</returns>
            public int FunPubValidateDemoLogin(int intNewUser, string strUserLoginCode, string strFullName, string strMobile, string strEmail, string strDesignation, string strCompany_Name, string strBriefAbt, string strCurSys, out string strPassword, out int intCompanyID, out int intUserID, out int intUser_Level_ID, out string strUserName, out string strCompanyName, out string strCompanyCode, out DateTime LastLoginDate, out string strTheme, out string strAccess, out string strCountryName, out string strUserType, out string strMarquee)
            {
                intCompanyID = 0;
                intUserID = 0;
                intUser_Level_ID = 0;
                strCompanyName = "";
                strCompanyCode = "";
                strUserName = "";
                strTheme = "";
                strAccess = "";
                strCountryName = string.Empty;
                strUserType = string.Empty;
                strMarquee = string.Empty;
                LastLoginDate = DateTime.Now;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Sysad_Validate_Demo_User");

                    db.AddInParameter(command, "@intNewUser", DbType.Int32, intNewUser);
                    db.AddInParameter(command, "@Name", DbType.String, strFullName);
                    db.AddInParameter(command, "@Mobile_number", DbType.String, strMobile);
                    db.AddInParameter(command, "@Email_ID", DbType.String, strEmail);
                    db.AddInParameter(command, "@Desgination", DbType.String, strDesignation);
                    db.AddInParameter(command, "@Company_Name", DbType.String, strCompany_Name);
                    db.AddInParameter(command, "@Operations", DbType.String, strBriefAbt);
                    db.AddInParameter(command, "@Current_Lending_Company", DbType.String, strCurSys);

                    db.AddInParameter(command, "@UserLoginID", DbType.String, strUserLoginCode);
                    db.AddOutParameter(command, "@Password", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@Last_LoginDate", DbType.DateTime, 200);
                    db.AddOutParameter(command, "@User_Theme", DbType.String, 50);
                    db.AddOutParameter(command, "@CompanyID", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@UserID", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@User_Level_ID", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@UserName", DbType.String, 100);
                    db.AddOutParameter(command, "@CompanyName", DbType.String, 80);
                    db.AddOutParameter(command, "@CompanyCode", DbType.String, 3);
                    db.AddOutParameter(command, "@LevelAccess", DbType.String, 50);
                    db.AddOutParameter(command, "@CountryName", DbType.String, 60);
                    db.AddOutParameter(command, "@UserType", DbType.String, 60);
                    db.AddOutParameter(command, "@Marquee_Text", DbType.String, 1000);

                    db.FunPubExecuteNonQuery(command);
                    //db.FunPubExecuteNonQuery(command);
                    intErrorCode = (command.Parameters["@ErrorCode"].Value != DBNull.Value) ? (int)command.Parameters["@ErrorCode"].Value : 0;
                    intCompanyID = (command.Parameters["@CompanyID"].Value != DBNull.Value) ? (int)command.Parameters["@CompanyID"].Value : 0;
                    intUserID = (command.Parameters["@UserID"].Value != DBNull.Value) ? (int)command.Parameters["@UserID"].Value : 0;
                    strPassword = Convert.ToString((command.Parameters["@Password"].Value != DBNull.Value) ? command.Parameters["@Password"].Value : 0);
                    intUser_Level_ID = (command.Parameters["@User_Level_ID"].Value != DBNull.Value) ? (int)command.Parameters["@User_Level_ID"].Value : 0;
                    strUserName = Convert.ToString((command.Parameters["@UserName"].Value != DBNull.Value) ? command.Parameters["@UserName"].Value : 0);
                    strCompanyName = Convert.ToString((command.Parameters["@CompanyName"].Value != DBNull.Value) ? command.Parameters["@CompanyName"].Value : 0);
                    strCompanyCode = Convert.ToString((command.Parameters["@CompanyCode"].Value != DBNull.Value) ? command.Parameters["@CompanyCode"].Value : 0);
                    LastLoginDate = Convert.ToDateTime((command.Parameters["@Last_LoginDate"].Value != DBNull.Value) ? command.Parameters["@Last_LoginDate"].Value : DateTime.Now);
                    strTheme = Convert.ToString((command.Parameters["@User_Theme"].Value != DBNull.Value) ? command.Parameters["@User_Theme"].Value : string.Empty);
                    strAccess = Convert.ToString((command.Parameters["@LevelAccess"].Value != DBNull.Value) ? command.Parameters["@LevelAccess"].Value : string.Empty);
                    strCountryName = Convert.ToString((command.Parameters["@CountryName"].Value != DBNull.Value) ? command.Parameters["@CountryName"].Value : string.Empty);
                    strUserType = Convert.ToString((command.Parameters["@UserType"].Value != DBNull.Value) ? command.Parameters["@UserType"].Value : string.Empty);
                    strMarquee = Convert.ToString((command.Parameters["@Marquee_Text"].Value != DBNull.Value) ? command.Parameters["@Marquee_Text"].Value : string.Empty);
                }

                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }
            #endregion

            #region Workflow ToDo List

            public DataSet FunPubGetWorkflowDocuments(int intCompanyId, int intUserId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_GEN_GetWorkFlowWaitingRecords");
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@UserId", DbType.Int32, intUserId);

                    db.FunPubLoadDataSet(command, ObjDS, "WorkflowToDoList");
                    //db.LoadDataSet(command, ObjDS, "WorkflowToDoList");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataSet FunPubGetWorkflowToDoList(int intCompanyId, int intUserId, bool ShowAll, DateTime? FromDate, DateTime? ToDate)
            {
                try
                {
                    DataSet ObjDS = new DataSet();

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_GetWorkFlowRecords);
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUserId);
                    db.AddInParameter(command, "@ShowAll", DbType.Boolean, ShowAll);
                    if (FromDate != null && FromDate != DateTime.MaxValue)
                        db.AddInParameter(command, "@FromDate", DbType.DateTime, FromDate);
                    if (ToDate != null && ToDate != DateTime.MaxValue)
                        db.AddInParameter(command, "@ToDate", DbType.DateTime, ToDate);

                    db.FunPubLoadDataSet(command, ObjDS, "WorkflowToDoList");
                    //db.LoadDataSet(command, ObjDS, "WorkflowToDoList");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            #endregion

            #region Journal Entry
            public void FunPubSysJournalEntry(ClsSystemJournal ObjSysJournal)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertSysJournal");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSysJournal.Company_ID);
                    db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjSysJournal.Branch_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSysJournal.LOB_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjSysJournal.Customer_ID);
                    db.AddInParameter(command, "@Account_Link_Key", DbType.String, ObjSysJournal.Account_Link_Key);
                    db.AddInParameter(command, "@Narration", DbType.String, ObjSysJournal.Narration);
                    db.AddInParameter(command, "@Value_Date", DbType.String, ObjSysJournal.Value_Date);
                    db.AddInParameter(command, "@Txn_Currency_Code", DbType.String, ObjSysJournal.Txn_Currency_Code);
                    db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjSysJournal.Program_ID);
                    db.AddInParameter(command, "@Global_Dimension1_Code", DbType.Int32, ObjSysJournal.Global_Dimension1_Code);
                    db.AddInParameter(command, "@Global_Dimension1_No", DbType.Int32, ObjSysJournal.Global_Dimension1_No);
                    db.AddInParameter(command, "@JV_Status_Code", DbType.Int32, ObjSysJournal.JV_Status_Code);
                    db.AddInParameter(command, "@Reference_Number", DbType.Int32, ObjSysJournal.Reference_Number);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, ObjSysJournal.Created_By);
                    db.AddInParameter(command, "@XMLSysJournal", DbType.String, ObjSysJournal.XMLSysJournal);

                    db.FunPubExecuteNonQuery(command);
                    //db.FunPubExecuteNonQuery(command);

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }

            }
            #endregion

            #region [Location Master Details]

            /// <summary>
            /// Created By Tamilselvan.S
            /// Created Date 04/05/2011
            /// To Insert the Location Category Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesLocation_Datatable"></param>
            /// <param name="strErrorCode"></param>
            //public int FunPubInsertLocationCategory(SerializationMode SerMode, byte[] bytesLocation_Datatable, out string strExistingCode, out string strExistingDescription)
            //{
            //    intErrorCode = 0;
            //    strExistingCode = strExistingDescription = string.Empty;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable ObjS3G_Location_Category_DataTable;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryRow ObjS3g_Location_Category_Row;
            //    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //    DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_InsertLocationCategory");
            //    try
            //    {
            //        ObjS3G_Location_Category_DataTable = (SystemAdmin.S3G_SYSAD_LocationCategoryDataTable)ClsPubSerialize.DeSerialize(bytesLocation_Datatable, SerMode, typeof(S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable));
            //        ObjS3g_Location_Category_Row = ObjS3G_Location_Category_DataTable[0];

            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjS3g_Location_Category_Row.Company_ID);
            //        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjS3g_Location_Category_Row.Created_By);
            //        db.AddInParameter(command, "@XMLLocationDetails", DbType.String, ObjS3g_Location_Category_Row.XMLLocationDetails);
            //        db.AddInParameter(command, "@XMLAddressDetails", DbType.String, ObjS3g_Location_Category_Row.XMLAddressDetails);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
            //        db.AddOutParameter(command, "@ExistingMappingCode", DbType.String, 500);
            //        db.AddOutParameter(command, "@ExistingMappingDescription", DbType.String, 1000);

            //        db.FunPubExecuteNonQuery(command);
            //        //db.FunPubExecuteNonQuery(command);
            //        if (command.Parameters["@ErrorCode"].Value != null)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //        if (command.Parameters["@ExistingMappingCode"].Value != null)
            //        {
            //            strExistingCode = Convert.ToString(command.Parameters["@ExistingMappingCode"].Value);
            //        }
            //        if (command.Parameters["@ExistingMappingDescription"].Value != null)
            //        {
            //            strExistingDescription = Convert.ToString(command.Parameters["@ExistingMappingDescription"].Value);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        if (command.Parameters["@ErrorCode"].Value != null && (int)command.Parameters["@ErrorCode"].Value != 0)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //        else
            //        {
            //            intErrorCode = 20;
            //        }
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }
            //    return intErrorCode;
            //}

            /// <summary>
            /// Created By Tamilselvan.S
            /// Created Date 09/05/2011
            /// To Update the Location Category Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesLocation_Datatable"></param>
            /// <returns></returns>
            //public int FunPubUpdateLocationCategory(SerializationMode SerMode, byte[] bytesLocation_Datatable)
            //{
            //    intErrorCode = 0;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable ObjS3G_Location_Category_DataTable;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryRow ObjS3g_Location_Category_Row;
            //    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //    DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_UpdateLocationCategory");
            //    try
            //    {
            //        ObjS3G_Location_Category_DataTable = (SystemAdmin.S3G_SYSAD_LocationCategoryDataTable)ClsPubSerialize.DeSerialize(bytesLocation_Datatable, SerMode, typeof(S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable));
            //        ObjS3g_Location_Category_Row = ObjS3G_Location_Category_DataTable[0];
            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjS3g_Location_Category_Row.Company_ID);
            //        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjS3g_Location_Category_Row.Modified_By);
            //        db.AddInParameter(command, "@Location_Category_ID", DbType.String, ObjS3g_Location_Category_Row.Location_Category_ID);
            //        //Changed by bhuvana.C
            //        db.AddInParameter(command, "@LOCATIONCAT_DESCRIPTION", DbType.String, ObjS3g_Location_Category_Row.Location_Description);
            //        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjS3g_Location_Category_Row.Is_Active);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

            //        db.AddInParameter(command, "@XMLAddressDetails", DbType.String, ObjS3g_Location_Category_Row.XMLAddressDetails);
            //        db.FunPubExecuteNonQuery(command);
            //        //db.FunPubExecuteNonQuery(command);
            //        if (command.Parameters["@ErrorCode"].Value != null)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        if (command.Parameters["@ErrorCode"].Value != null && (int)command.Parameters["@ErrorCode"].Value != 0)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //        else
            //        {
            //            intErrorCode = 20;
            //        }
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }
            //    return intErrorCode;
            //}
            
            /// <summary>
            /// Created by Tamilselvan.S
            /// Created date 06/05/2011
            /// </summary>
            /// <param name="intCompany_Id"></param>
            /// <param name="intUser_ID"></param>
            /// <param name="intLocationCategory_Id"></param>
            /// <returns></returns>
            //public S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable FunPubQueryLocationCategoryDetails(int intCompany_Id, int intUser_ID, int intLocationCategory_Id)
            //{
            //    S3GBusEntity.SystemAdmin dsLocCat = new SystemAdmin();
            //    try
            //    {
            //        //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //        DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_GetLocationCategoryDetails");
            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
            //        db.AddInParameter(command, "@User_ID", DbType.Int32, intUser_ID);
            //        db.AddInParameter(command, "@Location_Category_ID", DbType.Int32, intLocationCategory_Id);

            //        db.FunPubLoadDataSet(command, dsLocCat, dsLocCat.S3G_SYSAD_LocationCategory.TableName);
            //        //db.LoadDataSet(command, dsLocCat, dsLocCat.S3G_SYSAD_LocationCategory.TableName);
            //    }
            //    catch (Exception ex)
            //    {
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //    }
            //    return dsLocCat.S3G_SYSAD_LocationCategory;
            //}

            /// <summary>
            /// Created By Tamilselvan.S
            /// Created Date 06/05/2011
            /// To Insert the Location Master details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesLocation_Datatable"></param>
            /// <param name="strExistingMapping"></param>
            /// <returns></returns>
            //public int FunPubInsertLocationMaster(SerializationMode SerMode, byte[] bytesLocation_Datatable, out string strExistingMapping)
            //{
            //    intErrorCode = 0;
            //    strExistingMapping = string.Empty;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterDataTable ObjS3G_Location_Master_DataTable;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterRow ObjS3g_Location_Master_Row;
            //    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //    DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_InsertLocationMaster");
            //    try
            //    {
            //        ObjS3G_Location_Master_DataTable = (SystemAdmin.S3G_SYSAD_LocationMasterDataTable)ClsPubSerialize.DeSerialize(bytesLocation_Datatable, SerMode, typeof(S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterDataTable));
            //        ObjS3g_Location_Master_Row = ObjS3G_Location_Master_DataTable[0];

            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjS3g_Location_Master_Row.Company_ID);
            //        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjS3g_Location_Master_Row.Created_By);
            //        db.AddInParameter(command, "@XmlLocationMasterDetails", DbType.String, ObjS3g_Location_Master_Row.XMLLocationMasterDetails);
            //        db.AddOutParameter(command, "@ExistingMapping", DbType.String, 1000);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

            //        db.FunPubExecuteNonQuery(command);
            //        //db.FunPubExecuteNonQuery(command);
            //        if (command.Parameters["@ErrorCode"].Value != null)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //        if (command.Parameters["@ExistingMapping"].Value != null)
            //        {
            //            strExistingMapping = Convert.ToString(command.Parameters["@ExistingMapping"].Value);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        if (command.Parameters["@ErrorCode"].Value != null && (int)command.Parameters["@ErrorCode"].Value != 0)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //        else
            //        {
            //            intErrorCode = 20;
            //        }
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }
            //    return intErrorCode;
            //}

            /// <summary>
            /// Created By Tamilselvan.S
            /// Created Date 09/05/2011
            /// To Update the Location Mapping Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesLocation_Datatable"></param>
            /// <returns></returns>
            //public int FunPubUpdateLocationMapping(SerializationMode SerMode, byte[] bytesLocation_Datatable)
            //{
            //    intErrorCode = 0;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterDataTable ObjS3G_Location_Master_DataTable;
            //    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterRow ObjS3g_Location_Master_Row;
            //    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //    DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_UpdateLocationMapping");
            //    try
            //    {
            //        ObjS3G_Location_Master_DataTable = (SystemAdmin.S3G_SYSAD_LocationMasterDataTable)ClsPubSerialize.DeSerialize(bytesLocation_Datatable, SerMode, typeof(S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterDataTable));
            //        ObjS3g_Location_Master_Row = ObjS3G_Location_Master_DataTable[0];
            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjS3g_Location_Master_Row.Company_ID);
            //        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjS3g_Location_Master_Row.Modified_By);
            //        db.AddInParameter(command, "@Location_Mapping_ID", DbType.String, ObjS3g_Location_Master_Row.Location_ID);
            //        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjS3g_Location_Master_Row.Is_Active);
            //        db.AddInParameter(command, "@Is_Operational", DbType.Boolean, ObjS3g_Location_Master_Row.Is_Operational);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

            //        db.FunPubExecuteNonQuery(command);
            //        //db.FunPubExecuteNonQuery(command);
            //        if (command.Parameters["@ErrorCode"].Value != null)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        if (command.Parameters["@ErrorCode"].Value != null && (int)command.Parameters["@ErrorCode"].Value != 0)
            //        {
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //        else
            //        {
            //            intErrorCode = 20;
            //        }
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }
            //    return intErrorCode;
            //}


            #endregion [Location Master Details]

            //public int FunPubCreateAuditTrial(SerializationMode SerMode, byte[] bytesObjS3G_AuditTrial_DataTable)
            //{
                
            //    S3GBusEntity.AuditTrial.S3G_SYSAD_AuditTrialDataTable objAuditTrial_DAL;
            //    S3GBusEntity.AuditTrial.S3G_SYSAD_AuditTrialRow objAuditTrialRow = null;
            //    try
            //    {
            //        //objAuditTrial_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable)
            //        //   ClsPubSerialize.DeSerialize(bytesObjS3G_LEGAL_RepossessionNote_DataTable, SerMode,
            //        //   typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable));

            //        objAuditTrial_DAL = (S3GBusEntity.AuditTrial.S3G_SYSAD_AuditTrialDataTable)
            //           ClsPubSerialize.DeSerialize(bytesObjS3G_AuditTrial_DataTable, SerMode,
            //           typeof(S3GBusEntity.AuditTrial.S3G_SYSAD_AuditTrialDataTable));

            //        //objRepossessionNoteRow = objRepossessionNote_DAL.Rows[0] as
            //        //   S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteRow;

            //        objAuditTrialRow = objAuditTrial_DAL.Rows[0] as
            //           S3GBusEntity.AuditTrial.S3G_SYSAD_AuditTrialRow;

            //        DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_InsertAuditTrial");
            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAuditTrialRow.Company_ID);
            //        db.AddInParameter(command, "@XMLAuditTrialData", DbType.String, objAuditTrialRow.XmlAuditTrial);
            //        db.AddInParameter(command, "@Created_By ", DbType.Int32, objAuditTrialRow.Created_By);
            //        db.AddInParameter(command, "@Modified_By", DbType.Int32, objAuditTrialRow.Modified_By);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
            //        using (DbConnection conn = db.CreateConnection())
            //        {
            //            conn.Open();
            //            DbTransaction trans = conn.BeginTransaction();
            //            try
            //            {

            //                db.FunPubExecuteNonQuery(command);
            //                //db.FunPubExecuteNonQuery(command);
            //                if ((int)command.Parameters["@ErrorCode"].Value > 0 || (int)command.Parameters["@ErrorCode"].Value < 0)
            //                {
            //                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //                    //throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
            //                }
            //                if ((int)command.Parameters["@ErrorCode"].Value == 0)
            //                {
            //                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //                    trans.Commit();
            //                    // strDSNo = (string)command.Parameters["@DSNO"].Value;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                if (intErrorCode == 0)
            //                    intErrorCode = 50;
            //                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //            }
            //            finally
            //            {
            //                conn.Close();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        intErrorCode = 50;
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }

            //    return intErrorCode;
            //}

            #region Password Validation

            public int FunPubPasswordValidation(Int32 intUserID, out string strPassword)
            {
                intErrorCode = 0;
                strPassword = String.Empty;
                DbCommand command = db.GetStoredProcCommand("S3G_SYSADM_GetLoginUserPassword");
                try
                {
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUserID);
                    db.AddOutParameter(command, "@Password", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

                    db.FunPubExecuteNonQuery(command);
                    //db.FunPubExecuteNonQuery(command);
                    if (command.Parameters["@ErrorCode"].Value != null)
                    {
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    }

                    if (command.Parameters["@Password"].Value != null)
                    {
                        strPassword = Convert.ToString(command.Parameters["@Password"].Value);
                    }
                }
                catch (Exception ex)
                {
                    if (command.Parameters["@ErrorCode"].Value != null && (int)command.Parameters["@ErrorCode"].Value != 0)
                    {
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    }
                    else
                    {
                        intErrorCode = 20;
                    }
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            #endregion


            #region EMI Calculation Save


            public int FunPubSaveEMICalc(SerializationMode SerMode, byte[] bytesDictionary)
            {
                int strErrorCode = 0;
                try
                {
                    Dictionary<string, string> dctProcParams = new Dictionary<string, string>();
                    dctProcParams = (Dictionary<string, string>)ClsPubSerialize.DeSerialize(bytesDictionary, SerMode, typeof(Dictionary<string, string>));

                    DbCommand command = db.GetStoredProcCommand("S3G_Common_InsertEMICalc");
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        strErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return strErrorCode;

            }

            #endregion

            #region ErrorLog
            public string FunPubSysErrorLog(string strProcName, Dictionary<string, string> dctProcParams)
            {
                try
                {
                    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                    string strSendError = (string)AppReader.GetValue("SendError", typeof(string));
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    if (strSendError == "1")
                    {
                        DbCommand command = db.GetStoredProcCommand(strProcName);

                        foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                        {
                            //Thangam M to avoid recursive call on 29/Jul/2013
                            if (ProcPair.Value != null)
                            {
                                //db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                                if (enumDBType == S3GDALDBType.ORACLE)
                                {
                                    OracleParameter param = new OracleParameter(ProcPair.Key,
                                          OracleType.Clob, ProcPair.Value.Length,
                                          ParameterDirection.Input, true, 0, 0, String.Empty,
                                          DataRowVersion.Default, string.IsNullOrEmpty(ProcPair.Value) ? " " : ProcPair.Value);
                                    command.Parameters.Add(param);
                                }
                                else
                                {
                                    db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                                }
                            }
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 10);
                        db.FunPubExecuteNonQuery(command);

                        return Convert.ToString(command.Parameters["@ErrorCode"].Value);
                    }
                    else
                    {
                        string strLogFile = (string)AppReader.GetValue("COMMONERRORLOG", typeof(string));

                        string strError = "";

                        strError = Environment.NewLine;
                        strError = strError + FunProFormatedSpace("DateTime", 17) + ": " + DateTime.Now;

                        foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                        {
                            if (ProcPair.Value != null)
                            {
                                strError = strError + Environment.NewLine + FunProFormatedSpace(ProcPair.Key.Replace("@", ""), 17) + ": " + ProcPair.Value;
                            }
                        }

                        strError = strError + Environment.NewLine; 

                        if (File.Exists(strLogFile))
                        {
                            File.AppendAllText(strLogFile, strError);
                        }
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    //By Thangam M to avoid recursive calling - 23/July/2013
                    //ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
            }

            protected string FunProFormatedSpace(string strVal, int intFxdLength)
            {
                int intLen = 0;
                string strRetValue = string.Empty;
                intLen = strVal.Length;
                strRetValue = strVal;

                for (int i = 0; i < intFxdLength - intLen; i++)
                {
                    strRetValue = strRetValue + " ";
                }

                return strRetValue;
            }

            #endregion
            /// <summary>
            /// By Chandu 7-Aug-2013
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="byteUserDetails"></param>
            /// <returns></returns>
            //public int FunPubInsUserLoginDet(SerializationMode SerMode, byte[] byteUserDetails)
            //{
            //    try
            //    {
            //        SystemAdmin.S3G_SYSAD_UserLoginDetailsDataTable ObjUserLoginDetails_Datatable_DAL;
            //        DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_USERLOGINDETAILS_ADD");
            //        ObjUserLoginDetails_Datatable_DAL = (S3GBusEntity.SystemAdmin.S3G_SYSAD_UserLoginDetailsDataTable)ClsPubSerialize.DeSerialize(byteUserDetails, SerMode, typeof(S3GBusEntity.SystemAdmin.S3G_SYSAD_UserLoginDetailsDataTable));
            //        SystemAdmin.S3G_SYSAD_UserLoginDetailsRow ObjTargetRow = ObjUserLoginDetails_Datatable_DAL.NewS3G_SYSAD_UserLoginDetailsRow();
            //        ObjTargetRow = ObjUserLoginDetails_Datatable_DAL.Rows[0] as S3GBusEntity.SystemAdmin.S3G_SYSAD_UserLoginDetailsRow;

            //        db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, ObjTargetRow.COMPANY_ID);
            //        db.AddInParameter(command, "@USER_ID", DbType.Int32, ObjTargetRow.USER_ID);
            //        db.AddInParameter(command, "@HOST_NAME", DbType.String, ObjTargetRow.HOST_NAME);
            //        db.AddInParameter(command, "@IP_ADDRESS", DbType.String, ObjTargetRow.IP_ADDRESS);
            //        db.AddInParameter(command, "@Session_ID", DbType.String, ObjTargetRow.Session_ID);
            //        db.AddInParameter(command, "@XMLUserLoginDetails", DbType.String, ObjTargetRow.XMLUserLoginDetails);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
            //        db.FunPubExecuteNonQuery(command);
            //        if ((int)command.Parameters["@ErrorCode"].Value > 0)
            //            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //    }
            //    catch (Exception ex)
            //    {
            //        ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //    }
            //    return intErrorCode;
            //}
            public int FunPubUpdateUserIsActive(DateTime ActiveDateTime, int User_ID, int Company_ID, int Manual_Logged_Out)
            {
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_SA_UPD_USERACTIVE_DTLS");
                    db.AddInParameter(command, "@Company_ID", DbType.String, Company_ID);
                    db.AddInParameter(command, "@User_ID", DbType.String, User_ID);
                    db.AddInParameter(command, "@Last_Active_Time", DbType.String, ActiveDateTime);
                    db.AddInParameter(command, "@Manual_Logged_Out", DbType.String, Manual_Logged_Out);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

                    db.FunPubExecuteNonQuery(command);
                    intErrorCode = (command.Parameters["@ErrorCode"].Value != DBNull.Value) ? (int)command.Parameters["@ErrorCode"].Value : 0;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            public int FunPubUpdateLogOutFlags(int intCompanyID, int intUserID, String Session_ID, String UserIDs, int LogOut_Status_Code)
            {
                String UserList = UserIDs;

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_UPDATELOGOUTDETAILS");
                    db.AddInParameter(command, "@Company_ID", DbType.String, intCompanyID);
                    db.AddInParameter(command, "@User_ID", DbType.String, intUserID);
                    db.AddInParameter(command, "@Session_ID", DbType.String, Session_ID);
                    db.AddInParameter(command, "@User_List", DbType.String, UserList);
                    db.AddInParameter(command, "@LogOut_Status_Code", DbType.String, LogOut_Status_Code);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

                    db.FunPubExecuteNonQuery(command);
                    intErrorCode = (command.Parameters["@ErrorCode"].Value != DBNull.Value) ? (int)command.Parameters["@ErrorCode"].Value : 0;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            /// <summary>
            /// By Chandu 7-Aug-2013
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="byteUserDetails"></param>
            /// <returns></returns>
        }
    }
}
