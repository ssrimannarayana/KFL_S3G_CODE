#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: API_Post
/// Screen Name			: API Post
/// Created By			: Sathish R
/// Created Date		: 20-Aug-2021
/// <Program Summary>
#endregion

using System;
using S3GDALayer.S3GAdminServices;
using System.Data;
using S3GBusEntity;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.OracleClient;

namespace S3GDALayer.API_Post
{
    public class clsAPIpost
    {
        int intRowsAffected;


        //Code added for getting common connection string  from config file
        Database db;
        public clsAPIpost()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        public int FunPubPostReceiptDetails(int Option, string Request_ID, string strLoan_Numer, decimal amountPaid, string transactionId, string strpaymentMode, string paymentDate, out string strErroStatus,string XMLACCOUNTDETAILS)
        {
            strErroStatus = string.Empty;
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_BBPS_POST_ACCOUNT_INFO_INS_VL");
                db.AddInParameter(command, "@Option", DbType.Int32, Option);
                db.AddInParameter(command, "@Request_ID", DbType.String, Request_ID);
                db.AddInParameter(command, "@Loan_Numer", DbType.String, strLoan_Numer);
                db.AddInParameter(command, "@amountPaid", DbType.String, amountPaid);
                db.AddInParameter(command, "@transactionId", DbType.String, transactionId);
                db.AddInParameter(command, "@paymentMode", DbType.String, strpaymentMode);
                db.AddInParameter(command, "@paymentDate", DbType.String, paymentDate);
                db.AddInParameter(command, "@XMLACCOUNTDETAILS", DbType.String, XMLACCOUNTDETAILS);
                db.AddOutParameter(command, "@ErroStatus", DbType.String, 100);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(command, ref trans);
                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            strErroStatus = (string)command.Parameters["@ErroStatus"].Value;
                        }
                        else
                        {
                            strErroStatus = (string)command.Parameters["@ErroStatus"].Value;
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (intRowsAffected == 0)
                            intRowsAffected = 50;
                        ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        strErroStatus = "Something went Wrong";
                        trans.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                intRowsAffected = 50;
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return intRowsAffected;
        }
    }
}
