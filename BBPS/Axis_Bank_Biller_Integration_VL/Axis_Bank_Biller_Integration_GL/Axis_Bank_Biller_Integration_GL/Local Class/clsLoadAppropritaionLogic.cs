using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using S3GDALayer.S3GAdminServices;
using System.Text;
using System.Globalization;

namespace Axis_Bank_Biller_Integration_GL.Local_Class
{
    public class clsLoadAppropritaionLogic
    {
        public DataTable funPriclsLoadAppropritaionLogic(string txtDocAmount,string ddlLOB, int intCompanyID,string ddlBranch,string hvfCustomerID,string strAccountsIn,int ProGpsSuffixRW)//btnAppropriationLogic_OnClick
        {
            DataTable dtApprFinal = new DataTable();
            try
            {
               
                string strAccounts = string.Empty;
                strAccounts += "'" + strAccountsIn + "',";
                ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();

                ClsAppropriationLogic ObjAppropriLogic = new ClsAppropriationLogic();

                //  DataTable dtAppropriationList = ObjAppropriLogic.FunPubReceiptAppropriationCalculation(this, Convert.ToDecimal(txtDocAmount.Text), Convert.ToInt32(ddlLOB.SelectedValue), intCompanyID, Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(hvfCustomerID.Value));


                DataTable dtAppropriationList = ObjAppropriLogic.FunPubReceiptAppropriationCalculation (Convert.ToDecimal(txtDocAmount), Convert.ToInt32(ddlLOB), intCompanyID, Convert.ToInt32(ddlBranch), Convert.ToInt32(hvfCustomerID), strAccounts, ProGpsSuffixRW);


                Decimal strAmount = Convert.ToDecimal(dtAppropriationList.Compute("sum(Amount)", "1=1"));

                if (Convert.ToDecimal(txtDocAmount) > strAmount)
                {
                    DataRow[] dr = dtAppropriationList.Select("AccountDescriptionId=23");
                    if (dr.Length > 0)
                    {
                        foreach (DataRow dr1 in dtAppropriationList.Select("AccountDescriptionId=23"))
                        {
                            dr1.BeginEdit();
                            dr1["Amount"] = Convert.ToString(Convert.ToDecimal(dr1["Amount"]) + Convert.ToDecimal(txtDocAmount) - Convert.ToDecimal(strAmount));
                            dr1.EndEdit();
                        }
                        dtAppropriationList.AcceptChanges();
                    }
                    else
                    {
                        Dictionary<string, string> Procparam = new Dictionary<string, string>();
                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@PANUM", strAccounts.Replace("'", "").TrimEnd(','));
                        //Procparam.Add("@PANUM", strAccounts.TrimEnd(','));
                        DataTable dtInstallments = DLayerAdminService.FunPubFillDropdown("S3G_GET_RECEIPT_INSTALLMENT_NO_CHRGS", Procparam);
                        DataRow dt = dtAppropriationList.NewRow();

                        if (dtInstallments.Rows.Count > 0)
                        {
                            dt["PrimeAccountNo"] = dtInstallments.Rows[0]["PrimeAccountNo"];
                            dt["SubAccountNo"] = dtInstallments.Rows[0]["SubAccountNo"];
                            dt["AccountDescriptionId"] = dtInstallments.Rows[0]["AccountDescriptionId"];
                            dt["AccountDescription"] = dtInstallments.Rows[0]["AccountDescription"];
                            dt["GLAccount"] = dtInstallments.Rows[0]["gl_code"];
                            dt["SLAccountId"] = dtInstallments.Rows[0]["SL_Code"];
                            dt["GLAccountId"] = dtInstallments.Rows[0]["GLAccountId"];
                            dt["SLAccountId"] = dtInstallments.Rows[0]["SLAccountId"];
                            dt["CFMID"] = dtInstallments.Rows[0]["CFMID"];
                            dt["Parent_CashFlow_Flag_ID"] = dtInstallments.Rows[0]["Parent_CashFlow_Flag_ID"];
                            dt["Amount"] = Convert.ToDecimal(txtDocAmount) - Convert.ToDecimal(strAmount);

                            dtAppropriationList.Rows.Add(dt);
                        }
                    }
                }
                if (dtAppropriationList != null && dtAppropriationList.Rows.Count > 0)
                {

                    dtApprFinal=FunPriCalculateServiceTaxAppropriation(dtAppropriationList, ddlLOB, ddlBranch, intCompanyID.ToString());

                
                }

              
            }
            catch (Exception ex)
            {
              
            }
            return dtApprFinal;
        }
        private DataTable FunPriCalculateServiceTaxAppropriation(DataTable dtReceiptDetails,string ddlLOB,string ddlBranch,string intCompanyID)
        {
            ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB));
            Procparam.Add("@Location_ID", Convert.ToString(ddlBranch));
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@XML_ReceiptDetails", FunPubFormXml(dtReceiptDetails,true));


            DataTable dtAppropriation = DLayerAdminService.FunPubFillDropdown("S3G_Cln_GSTAppropriation", Procparam);

            if (dtAppropriation != null && dtAppropriation.Rows.Count > 0)
            {
                if (dtAppropriation.Columns.Contains("Exception_Msg"))
                {
                   
                }
            }

            return dtAppropriation;
        }
        public  string FunPubFormXml( DataTable DtXml, bool IsNeedUpperCase)
        {
            int intcolcount = 0;
            string strColValue = string.Empty;
            StringBuilder strbXml = new StringBuilder();
            strbXml.Append("<Root>");
            foreach (DataRow grvRow in DtXml.Rows)
            {
                intcolcount = 0;
                strbXml.Append(" <Details ");
                foreach (DataColumn dtCols in DtXml.Columns)
                {
                    strColValue = grvRow.ItemArray[intcolcount].ToString();
                    strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                    strColValue = strColValue.Replace("'", "\"");
                    if (!string.IsNullOrEmpty(strColValue))
                    {
                        if (grvRow.ItemArray[intcolcount].ToString() != "" || dtCols.ColumnName != string.Empty)
                        {
                            if (IsNeedUpperCase)
                            {
                                if (dtCols.ColumnName.ToUpper().Contains("DATE"))
                                    strbXml.Append(dtCols.ColumnName.ToUpper() + "='" + StringToDate(strColValue).ToString() + "' ");

                                else
                                    strbXml.Append(dtCols.ColumnName.ToUpper() + "='" + strColValue + "' ");
                            }
                            else
                            {
                                if (dtCols.ColumnName.ToUpper().Contains("DATE"))

                                    strbXml.Append(dtCols.ColumnName.ToLower() + "='" + StringToDate(strColValue).ToString() + "' ");

                                else
                                    strbXml.Append(dtCols.ColumnName.ToLower() + "='" + strColValue + "' ");
                            }

                        }
                    }
                    intcolcount++;
                }
                strColValue = "";
                strbXml.Append(" /> ");
            }
            strbXml.Append("</Root>");
            return strbXml.ToString();
        }
        public static DateTime StringToDate(string strInput)
        {
            string ProDateFormatRW= "MM/dd/yy";
            if (strInput == null)
                return System.DateTime.Now;
            DateTime dtValidDatetime;
            //if (strInput != string.Empty || strInput != "")
            //{
            if ((strInput.Contains(":")) && (IsDateTime(strInput)))
            {
                dtValidDatetime = Convert.ToDateTime(strInput);
            }
            else
            {
                //S3GSession ObjS3GSession = new S3GSession();
                DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
                if (ProDateFormatRW != null || ProDateFormatRW != string.Empty)
                {
                    dtformat.ShortDatePattern = ProDateFormatRW;
                }
                else
                {
                    dtformat.ShortDatePattern = "MM/dd/yy";
                }
                DateTime dt = DateTime.Parse(strInput, dtformat);
                dtValidDatetime = Convert.ToDateTime(dt.ToString("yyyy/MM/dd"));

            }
            return dtValidDatetime;
            //}
            //else
            //{
            //    return DateTime.Now;
            //}
        }
        public static bool IsDateTime(string strDateTime)
        {
            try
            {

                DateTime dtDatetime = Convert.ToDateTime(strDateTime);
                if (dtDatetime != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}