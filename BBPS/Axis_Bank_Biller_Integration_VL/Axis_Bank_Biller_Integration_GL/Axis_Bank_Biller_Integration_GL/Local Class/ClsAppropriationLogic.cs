#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Collection
/// Screen Name         :   Appropriation Logic
/// Created By          :   Tamilselvan.S
/// Created Date        :   17-March-2011
/// Purpose             :   Appropriation Logic for Receipt
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :    
/// <Program Summary>
#endregion

#region [Namespaces]

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using S3GBusEntity;
using System.Text;
//using System.Data.Linq;
using S3GDALayer.S3GAdminServices;
#endregion [Namespaces]

/// <summary>
/// Summary description for AppropriationLogic
/// </summary>

public class ClsAppropriationLogic
{
    ClsSelectDistinctDataTable clsSelectDistinct = new ClsSelectDistinctDataTable();
    public string[] strColumnsDistictAccNo = new string[2] { "PrimeAccountNo", "SubAccountNo" };
    //S3GSession objsession = new S3GSession();
    decimal decSuffixValue = Convert.ToDecimal(0.9);

    /*
     *  Added By Senthilkumar P 
     *  For Login apply based on selected account
     */


    /// <summary>
    /// Selected Account will passed through this area.
    /// 
    /// </summary>
    /// <param name="thisPage"></param>
    /// <param name="decReceiptAmt"></param>
    /// <param name="intLOB_ID"></param>
    /// <param name="intCompanyId"></param>
    /// <param name="intBranch_ID"></param>
    /// <param name="intCustomer_ID"></param>
    /// <param name="dtAccount"></param>
    /// <returns></returns>
    public DataTable FunPubReceiptAppropriationCalculation( decimal decReceiptAmt, int intLOB_ID, int intCompanyId, int intBranch_ID, int intCustomer_ID, String strAccounts,int ProGpsSuffixRW)
    {
        DataTable dtAppropriationCalList = new DataTable();
        try
        {
            ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
            DataSet dtAppropriationList1 = new DataSet();
            DataSet dtAppropriationList = new DataSet();

            dtAppropriationList1 = FunPubGetApprppriationLogicDetails(intLOB_ID, intCompanyId, intBranch_ID, intCustomer_ID);


            dtAppropriationList.Tables.Add(dtAppropriationList1.Tables[0].Copy());

            DataView dv = new DataView(dtAppropriationList1.Tables[1], "PrimeAccountNo in (" + strAccounts + ")", "", DataViewRowState.CurrentRows);

            if (dv.Count > 0)
            {
                dtAppropriationList.Tables.Add(dv.ToTable());
                dtAppropriationList.AcceptChanges();
            }
            else
            {
                Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
                dictProcParam.Add("@PANUM", strAccounts.Replace("'", "").TrimEnd(','));
                DataTable dtInstallments = DLayerAdminService.FunPubFillDropdown("S3G_GET_RECEIPT_INSTALLMENT_NO_CHRGS", dictProcParam);

                //Utility.FunShowAlertMsg(thisPage, "No Pending Installments in Current date for selected Customer");

                return dtInstallments;
            }

            if (dtAppropriationList != null)
            {


                if (dtAppropriationList.Tables[0].Rows.Count == 0)
                {
                    //Utility.FunShowAlertMsg(thisPage, "Define an Appropriation Logic for selected Line of Business");
                }
                if (dtAppropriationList.Tables[1].Rows.Count == 0)
                {
                    //Utility.FunShowAlertMsg(thisPage, "No Pending Installments in Current date for selected Customer");
                }
                if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtAppropriationList.Tables[1].Rows.Count > 0)
                {
                    decSuffixValue = ProGpsSuffixRW == 1 ? Convert.ToDecimal(0.9) : ProGpsSuffixRW == 2 ? Convert.ToDecimal(0.09) : ProGpsSuffixRW == 3 ? Convert.ToDecimal(0.009) : Convert.ToDecimal(0.0009);
                    if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationType"]) == "1" && Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationTypeDescription"]).Contains("Vertical"))  ///Vertical
                    {
                        if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["Is_FullDue"]) == "1")//Full Due
                            dtAppropriationCalList = FunPubReceiptAppropriationVerticalFullDue(decReceiptAmt, dtAppropriationList);
                        else
                            dtAppropriationCalList = FunPubReceiptAppropriationVertical(decReceiptAmt, dtAppropriationList);
                    }
                    else if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationType"]) == "2" && Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationTypeDescription"]).Contains("Horizontal"))  ///Horizontal
                    {
                        if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["Is_FullDue"]) == "1")//Full Due
                            dtAppropriationCalList = FunPubReceiptAppropriationHorizontalFullDue(decReceiptAmt, dtAppropriationList);
                        else
                            dtAppropriationCalList = FunPubReceiptAppropriationHorizontal(decReceiptAmt, dtAppropriationList);
                    }
                }
            }
            else
            {
                //Utility.FunShowAlertMsg(thisPage, "Define an Appropriation Logic for selected Line of Business");
            }



        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;

    }




    /// <summary>
    /// Created by Tamilselvan.s
    /// Created Date 23/03/2011
    /// </summary>
    /// <param name="decReceiptAmt"></param>
    /// <param name="intLOB_ID"></param>
    /// <param name="intCompanyId"></param>
    /// <param name="dtAccountNoDetails"></param>
    /// <returns></returns>
    public DataTable FunPubReceiptAppropriationCalculation(Page thisPage, decimal decReceiptAmt, int intLOB_ID, int intCompanyId, int intBranch_ID, int intCustomer_ID,int ProGpsSuffixRW)
    {
        DataTable dtAppropriationCalList = new DataTable();
        try
        {
            DataSet dtAppropriationList = new DataSet();

            dtAppropriationList = FunPubGetApprppriationLogicDetails(intLOB_ID, intCompanyId, intBranch_ID, intCustomer_ID);
            if (dtAppropriationList != null)
            {
                if (dtAppropriationList.Tables[0].Rows.Count == 0)
                {
                    //Utility.FunShowAlertMsg(thisPage, "Define an Appropriation Logic for selected Line of Business");
                }
                if (dtAppropriationList.Tables[1].Rows.Count == 0)
                {
                    //Utility.FunShowAlertMsg(thisPage, "No Pending Installments in Current date for selected Customer");
                }
                if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtAppropriationList.Tables[1].Rows.Count > 0)
                {
                    decSuffixValue = ProGpsSuffixRW == 1 ? Convert.ToDecimal(0.9) : ProGpsSuffixRW == 2 ? Convert.ToDecimal(0.09) : ProGpsSuffixRW == 3 ? Convert.ToDecimal(0.009) : Convert.ToDecimal(0.0009);
                    if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationType"]) == "1" && Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationTypeDescription"]).Contains("Vertical"))  ///Vertical
                    {
                        if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["Is_FullDue"]) == "1")//Full Due
                            dtAppropriationCalList = FunPubReceiptAppropriationVerticalFullDue(decReceiptAmt, dtAppropriationList);
                        else
                            dtAppropriationCalList = FunPubReceiptAppropriationVertical(decReceiptAmt, dtAppropriationList);
                       
                    }
                    else if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationType"]) == "2" && Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["AppropriationTypeDescription"]).Contains("Horizontal"))  ///Horizontal
                    {
                        if (Convert.ToString(dtAppropriationList.Tables[0].Rows[0]["Is_FullDue"]) == "1")//Full Due
                          dtAppropriationCalList = FunPubReceiptAppropriationHorizontalFullDue(decReceiptAmt, dtAppropriationList);
                        else
                            dtAppropriationCalList = FunPubReceiptAppropriationHorizontal(decReceiptAmt, dtAppropriationList);
                    }
                }
            }
            else
            {
                //Utility.FunShowAlertMsg(thisPage, "Define an Appropriation Logic for selected Line of Business");
            }
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;
        //var r = from c in dtAppropriationCalList.AsEnumerable()
        //        orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo") ascending
        //        select c;
        //if (r.Count() > 0)
        //    return r.CopyToDataTable();  // dtAppropriationCalList;
        //else
        //    return null;
    }

    /// <summary>
    /// Created By Tamilselvan.s
    /// Created Date 17/03/2011
    /// Modifed by Tamilselvan.S on 2/05/2011
    /// For Receipt Appropriation Logic Calculation only for Vertical its based on Account by Account
    /// </summary>
    /// <param name="decReceiptAmt"></param>
    /// <param name="intLOB_ID"></param>
    /// <param name="intCompanyId"></param>
    /// <param name="dtAccountNoDetails"></param>
    /// <returns></returns>
    public DataTable FunPubReceiptAppropriationVertical(decimal decReceiptAmt, DataSet dtAppropriationList)
    {
        DataTable dtAppropriationCalList = new DataTable();
        DataTable dtAccountNoDetails = new DataTable();
        DataTable dtCurrentPaymentStruct = new DataTable();
        decimal decPaymentAmt = decReceiptAmt;
        decimal decInstallmentAmt = 0;
        try
        {
            dtAppropriationCalList = FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
            dtAppropriationList.Tables[0].Columns.Add("OnPaidAmount", typeof(decimal)).DefaultValue = 0;
            //var varAccList = from al in dtAppropriationList.Tables[1].AsEnumerable()
            //                 //orderby al.Field<String>("PrimeAccountNo"), al.Field<String>("SubAccountNo") ascending
            //                 select al;
            dtAccountNoDetails = dtAppropriationList.Tables[1].Copy();
            dtCurrentPaymentStruct = dtAccountNoDetails.Copy();

            if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtAppropriationList.Tables[1].Rows.Count > 0)
            {
                if (dtCurrentPaymentStruct.Rows.Count > 0)
                {
                    var varAcc = (from acc in dtAppropriationList.Tables[1].AsEnumerable()
                                  select new { PrimeAccountNo = acc.Field<String>("PrimeAccountNo"), SubAccountNo = acc.Field<String>("SubAccountNo") }).Distinct();
                    //foreach (DataRow drAccNo in dtAppropriationList.Tables[1].Rows)    //Check account by Account
                    foreach (var drAccNo in varAcc)
                    {
                    Step1:
                        decimal decAmountAllocation = 0;
                        foreach (DataRow drow in dtAppropriationList.Tables[0].Rows)
                        {
                            drow.BeginEdit();
                            drow["OnPaidAmount"] = Math.Round(Convert.ToDecimal((decPaymentAmt * Convert.ToDecimal(drow["Percentage"])) / 100), 0);// objsession.ProGpsSuffixRW);
                            drow.EndEdit();
                            drow.AcceptChanges();
                            decAmountAllocation += Math.Round(Convert.ToDecimal((decPaymentAmt * Convert.ToDecimal(drow["Percentage"])) / 100), 0);// objsession.ProGpsSuffixRW);
                        }
                        if (decAmountAllocation != decPaymentAmt)
                        {
                            dtAppropriationList.Tables[0].Rows[0].BeginEdit();
                            dtAppropriationList.Tables[0].Rows[0]["OnPaidAmount"] = Convert.ToDecimal(dtAppropriationList.Tables[0].Rows[0]["OnPaidAmount"]) + (decPaymentAmt - decAmountAllocation);
                            dtAppropriationList.Tables[0].Rows[0].EndEdit();
                            dtAppropriationList.Tables[0].Rows[0].AcceptChanges();
                        }

                        DataRow[] drCurrentAccountList = dtCurrentPaymentStruct.Select("PrimeAccountNo='" + Convert.ToString(drAccNo.PrimeAccountNo) + "' AND SubAccountNo='" + Convert.ToString(drAccNo.SubAccountNo) + "'");

                        DataTable dtCurrent = drCurrentAccountList.CopyToDataTable();
                        //DataRow[] drCurrentCashFlowList = dtAppropriationList.Tables[0].Select("AccountDescriptionId='" + Convert.ToString(drAccNo.PrimeAccountNo) + "' AND SubAccountNo='" + Convert.ToString(drAccNo.SubAccountNo) + "'");

                        var varCashflowList = from c in dtAppropriationList.Tables[0].AsEnumerable()
                                              join d in dtCurrent.AsEnumerable() on c.Field<String>("AccountDescriptionId") equals d.Field<String>("Due_Flag")
                                              select c;
                        DataTable dtNewTablelist = varCashflowList.CopyToDataTable();
                        decimal decOnTotalPaidAmount=Convert.ToDecimal((from c in dtAppropriationList.Tables[0].AsEnumerable()
                                                                       select c.Field<decimal>("OnPaidAmount")).Sum());
                        int LoopStopcount = 0;
                        foreach (DataRow drCurrentAcc in drCurrentAccountList)
                        {
                            foreach (DataRow drApproLogic in dtAppropriationList.Tables[0].Rows)      //Check CashFlow type by type 
                            {
                                if (Convert.ToInt32(drApproLogic["AccountDescriptionId"]) == Convert.ToInt32(drCurrentAcc["Due_Flag"]))
                                {                                    
                                    decInstallmentAmt = Convert.ToDecimal(drApproLogic["OnPaidAmount"]);
                                    
                                    if (decInstallmentAmt == 0 && dtNewTablelist.Rows.Count == 1 && decOnTotalPaidAmount > 0)
                                    {
                                        decInstallmentAmt = decOnTotalPaidAmount;
                                        LoopStopcount = 1;
                                    }
                                    if (decInstallmentAmt > 0)
                                    {
                                        //decInstallmentAmt = Math.Ceiling(Convert.ToDecimal((decReceiptAmt * Convert.ToDecimal(drApproLogic["Percentage"])) / 100));
                                        decimal decCurrentAmt = 0;
                                        if (Convert.ToDecimal(drCurrentAcc["Amount"]) <= decInstallmentAmt)
                                        {
                                            decCurrentAmt = Convert.ToDecimal(drCurrentAcc["Amount"]);
                                            DataRow[] dr = dtAccountNoDetails.Select("ID='" + drCurrentAcc["ID"] + "'");
                                            dtAccountNoDetails.Rows.Remove(dr[0]);
                                        }
                                        else
                                        {
                                            decCurrentAmt = Convert.ToDecimal(decInstallmentAmt);
                                            DataRow[] dr = dtAccountNoDetails.Select("ID='" + drCurrentAcc["ID"] + "'");
                                            dr[0].BeginEdit();
                                            dr[0]["Amount"] = Convert.ToString(Convert.ToDecimal(dr[0]["Amount"]) - decInstallmentAmt);
                                            dr[0].EndEdit();
                                            dr[0].AcceptChanges();
                                        }
                                        DataRow[] drContains = dtAppropriationCalList.Select("SNo='" + drCurrentAcc["ID"] + "' AND AccountDescriptionId='" + Convert.ToString(drCurrentAcc["Due_Flag"]) + "'");
                                        if (drContains.Count() != 0)
                                        {                                       //This part is Already Existing balance Amount updated
                                            drContains[0].BeginEdit();
                                            decimal decExistingAmt = Convert.ToDecimal(drContains[0]["Amount"]);
                                            drContains[0]["Amount"] = Convert.ToDecimal(decExistingAmt + decCurrentAmt);
                                            drContains[0].EndEdit();
                                            drContains[0].AcceptChanges();
                                        }
                                        else
                                        {                                      // This Part Newly Inserted 
                                            DataRow drAdd = dtAppropriationCalList.NewRow();
                                            drAdd["SNo"] = Convert.ToInt32(drCurrentAcc["ID"]);
                                            drAdd["PrimeAccountNo"] = drAccNo.PrimeAccountNo;// ["PrimeAccountNo"].ToString();
                                            drAdd["SubAccountNo"] = drAccNo.SubAccountNo;// ["SubAccountNo"].ToString();
                                            drAdd["AccountDescriptionId"] = Convert.ToString(drApproLogic["AccountDescriptionId"]);
                                            drAdd["AccountDescription"] = Convert.ToString(drApproLogic["AccountDescription"]);
                                            drAdd["Installment_No"] = "";
                                            drAdd["GLAccountId"] = Convert.ToString(drCurrentAcc["GL_AccountId"]);
                                            drAdd["SLAccountId"] = Convert.ToString(drCurrentAcc["SL_AccountId"]);
                                            drAdd["GLAccount"] = Convert.ToString(drCurrentAcc["GL_Account"]);
                                            drAdd["SLAccount"] = Convert.ToString(drCurrentAcc["SL_Account"]);
                                            drAdd["Amount"] = Convert.ToDecimal(decCurrentAmt);
                                            drAdd["CFMID"] = Convert.ToString(drCurrentAcc["CFMID"]);
                                            drAdd["Parent_CashFlow_Flag_ID"] = Convert.ToString(drCurrentAcc["Parent_CashFlow_Flag_ID"]);      //Added on 29-Jun-2018 WRF 11748_175
                                            drAdd["Is_GST"] = Convert.ToString(drCurrentAcc["Is_GST"]);      //Added on 13-Aug-2018 
                                            dtAppropriationCalList.Rows.Add(drAdd);
                                        }
                                        drApproLogic.BeginEdit();
                                        drApproLogic["OnPaidAmount"] = decInstallmentAmt - decCurrentAmt;
                                        drApproLogic.EndEdit();
                                        drApproLogic.AcceptChanges();
                                    }
                                }
                            }
                            if (LoopStopcount == 1)
                                break;
                        }
                        if (LoopStopcount == 1)
                            break;
                        decimal decCurrentAccPayment = Convert.ToDecimal((from c in dtAppropriationCalList.AsEnumerable()
                                                                          where c.Field<string>("PrimeAccountNo") == drAccNo.PrimeAccountNo && c.Field<string>("SubAccountNo") == drAccNo.SubAccountNo
                                                                          select c.Field<decimal>("Amount")).Sum());
                        decimal decTotalPendAccPayment = Convert.ToDecimal((from c in dtAppropriationList.Tables[1].AsEnumerable()
                                                                            where c.Field<string>("PrimeAccountNo") == drAccNo.PrimeAccountNo && c.Field<string>("SubAccountNo") == drAccNo.SubAccountNo
                                                                            select c.Field<decimal>("Amount")).Sum());
                        decimal decCurrentPaidAmt = Convert.ToDecimal((from c in dtAppropriationCalList.AsEnumerable()
                                                                       select c.Field<decimal>("Amount")).Sum());
                        if (decCurrentAccPayment < decTotalPendAccPayment && decReceiptAmt > decCurrentPaidAmt)
                        {
                            dtCurrentPaymentStruct = dtAccountNoDetails.Copy();
                            decPaymentAmt = decReceiptAmt - decCurrentPaidAmt;
                            goto Step1;
                        }
                        else if (decReceiptAmt > decCurrentPaidAmt && dtAppropriationCalList.Rows.Count > 0)
                        {
                            dtCurrentPaymentStruct = dtAccountNoDetails.Copy();
                            decPaymentAmt = decReceiptAmt - decCurrentPaidAmt;
                        }
                        else if (decReceiptAmt == decCurrentPaidAmt)
                            break;

                        //The above condition is check 
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;
    }

/// <summary>
/// To Handle Full Due in Vertical Method
/// </summary>
/// <param name="decReceiptAmt"></param>
/// <param name="dtAppropriationList"></param>
/// <returns></returns>
    public DataTable FunPubReceiptAppropriationVerticalFullDue(decimal decReceiptAmt, DataSet dtAppropriationList)
    {
        DataTable dtAppropriationCalList, dtAccountNoDetails = new DataTable();
        decimal decPaymentAmt = decReceiptAmt;
        decimal decInstallmentAmt = 0;
        try
        {
            DataTable dtCurrentPaymentStruct = dtAppropriationList.Tables[1].Copy();
            dtAppropriationCalList = FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
            var varAccList = from al in dtAppropriationList.Tables[1].AsEnumerable()
                             //orderby al.Field<Int32>("Due_Flag"), al.Field<String>("PrimeAccountNo"), al.Field<String>("SubAccountNo") ascending
                             select al;
            dtAccountNoDetails = varAccList.CopyToDataTable();
            dtCurrentPaymentStruct = dtAccountNoDetails.Copy();

            if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtCurrentPaymentStruct.Rows.Count > 0 && decReceiptAmt != 0)
            {
                decimal decPaidAmt = 0;
                bool bolisFirst = false;
                //For ISFC
                decInstallmentAmt = decPaymentAmt;
            Step1:
                foreach (DataRow drowAccNo in dtCurrentPaymentStruct.Rows)   //Check account by Account
                {

                    if (decPaymentAmt == 0)    //if payment amount is zero then break the loop.
                        break;
                    if (decPaymentAmt < Convert.ToDecimal(1))
                    {
                        decInstallmentAmt = decPaymentAmt;
                        decPaymentAmt = 0;
                    }
                    else if (bolisFirst == false)
                        decInstallmentAmt = decInstallmentAmt;// objsession.ProGpsSuffixRW);
                    else
                        decInstallmentAmt = decPaymentAmt;

                    foreach (DataRow drow in dtAppropriationList.Tables[0].Rows)   //Check CashFlow type by type
                    {
                        if (Convert.ToInt32(drow["AccountDescriptionId"]) == Convert.ToInt32(drowAccNo["Due_Flag"]))
                        {
                            decInstallmentAmt = Math.Round(Convert.ToDecimal((decInstallmentAmt * Convert.ToDecimal(drow["Percentage"])) / 100), 2);
                            if (decInstallmentAmt == 0)   //if installment amount is zero then break the loop.
                                break;
                            decimal decCurrrentAmt = 0;
                            if (Convert.ToDecimal(drowAccNo["Amount"]) <= decInstallmentAmt)
                            {                                                               //To Deleting the Full Paid Amount in Original Structure.
                                decCurrrentAmt = Convert.ToDecimal(drowAccNo["Amount"]);
                                decInstallmentAmt = decInstallmentAmt - Convert.ToDecimal(drowAccNo["Amount"]);
                                DataRow[] dr = dtAppropriationList.Tables[1].Select("ID='" + drowAccNo["ID"] + "'");
                                dtAppropriationList.Tables[1].Rows.Remove(dr[0]);
                            }
                            else
                            {                                                            //To Update the Amount in Original Structure based on Current Payment                          
                                decCurrrentAmt = Convert.ToDecimal(decInstallmentAmt);
                                DataRow[] dr = dtAppropriationList.Tables[1].Select("ID='" + drowAccNo["ID"] + "'");
                                dr[0].BeginEdit();
                                dr[0]["Amount"] = Convert.ToString(Convert.ToDecimal(drowAccNo["Amount"]) - decInstallmentAmt);
                                dr[0].EndEdit();
                                dr[0].AcceptChanges();
                                decInstallmentAmt = 0;
                            }
                            DataRow[] drContains = dtAppropriationCalList.Select("SNo='" + drowAccNo["ID"] + "' AND AccountDescriptionId='" + Convert.ToString(drowAccNo["Due_Flag"]) + "'");
                            if (drContains.Count() != 0)
                            {                                       //This part is Already Existing balance Amount updated
                                drContains[0].BeginEdit();
                                decimal decExistingAmt = Convert.ToDecimal(drContains[0]["Amount"]);
                                drContains[0]["Amount"] = Convert.ToDecimal(decExistingAmt + decCurrrentAmt);
                                drContains[0].EndEdit();
                                drContains[0].AcceptChanges();
                            }
                            else
                            {                                      // This Part Newly Inserted 
                                DataRow drAdd = dtAppropriationCalList.NewRow();
                                drAdd["SNo"] = Convert.ToInt32(drowAccNo["ID"]);
                                drAdd["PrimeAccountNo"] = drowAccNo["PrimeAccountNo"].ToString();
                                drAdd["SubAccountNo"] = drowAccNo["SubAccountNo"].ToString();
                                drAdd["AccountDescriptionId"] = Convert.ToString(drow["AccountDescriptionId"]);
                                drAdd["AccountDescription"] = Convert.ToString(drow["AccountDescription"]);
                                drAdd["Installment_No"] = "";// Convert.ToString(drowAccNo["InstallmentNo"]);
                                drAdd["Amount"] = Convert.ToDecimal(decCurrrentAmt);
                                drAdd["GLAccountId"] = Convert.ToString(drowAccNo["GL_AccountId"]);
                                drAdd["SLAccountId"] = Convert.ToString(drowAccNo["SL_AccountId"]);
                                drAdd["GLAccount"] = Convert.ToString(drowAccNo["GL_Account"]);
                                drAdd["SLAccount"] = Convert.ToString(drowAccNo["SL_Account"]);
                                drAdd["CFMID"] = Convert.ToString(drowAccNo["CFMID"]);
                                drAdd["Parent_CashFlow_Flag_ID"] = Convert.ToString(drowAccNo["Parent_CashFlow_Flag_ID"]);      //Added on 29-Jun-2018 WRF 11748_175
                                drAdd["Is_GST"] = Convert.ToString(drowAccNo["Is_GST"]);
                                drAdd["RegistrationNumber"] = Convert.ToString(drowAccNo["RegistrationNumber"]);     //Added for binding Registration Number On 14-Aug-2015
                                dtAppropriationCalList.Rows.Add(drAdd);
                            }
                        }
                    }
                    if (bolisFirst)
                    {
                        if (decInstallmentAmt == 0)
                            break;
                        decPaymentAmt = decInstallmentAmt;
                    }
                }
                bolisFirst = true;
                decPaidAmt = 0;
                if (dtAppropriationCalList.Rows.Count > 0)
                {
                    decPaidAmt = Convert.ToDecimal((from c in dtAppropriationCalList.AsEnumerable()
                                                    select c.Field<decimal>("Amount")).Sum());
                }
                if (decReceiptAmt > decPaidAmt && dtAppropriationList.Tables[1].Rows.Count > 0)    //This Part is Pending amount is paid for unpaid amount then go to step1.
                {
                    dtCurrentPaymentStruct = dtAppropriationList.Tables[1].Copy();
                    decPaymentAmt = decReceiptAmt - decPaidAmt;
                    if (decPaymentAmt > Convert.ToDecimal(0.9))
                        goto Step1;
                }
            }
            var r = from c in dtAppropriationCalList.AsEnumerable()
                    orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo"), c.Field<Int32>("SNo") ascending   //c.Field<String>("AccountDescriptionId")
                    select c;
            if (r.Count() > 0)
                return r.CopyToDataTable();  // dtAppropriationCalList;
            else
                return null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;
    }


    /// <summary>
    /// Created By Tamilselvan.s
    /// Created Date 17/03/2011
    /// Modified By Tamilselvan.S on 30/04/2011
    /// For Receipt Appropriation Logic Calculation only for Horizontal
    /// </summary>
    /// <param name="decReceiptAmt"></param>
    /// <param name="intLOB_ID"></param>
    /// <param name="intCompanyId"></param>
    /// <param name="dtAccountNoDetails"></param>
    /// <returns></returns>
    public DataTable FunPubReceiptAppropriationHorizontal(decimal decReceiptAmt, DataSet dtAppropriationList)
    {
        DataTable dtAppropriationCalList, dtAccountNoDetails = new DataTable();
        decimal decPaymentAmt = decReceiptAmt;
        decimal decInstallmentAmt = 0;
        try
        {
            DataTable dtCurrentPaymentStruct = dtAppropriationList.Tables[1].Copy();
            dtAppropriationCalList = FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
            var varAccList = from al in dtAppropriationList.Tables[1].AsEnumerable()
                             // orderby al.Field<Int32>("Due_Flag"), al.Field<String>("PrimeAccountNo"), al.Field<String>("SubAccountNo") ascending
                             select al;
            dtAccountNoDetails = varAccList.CopyToDataTable();
            dtCurrentPaymentStruct = dtAccountNoDetails.Copy();

            if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtCurrentPaymentStruct.Rows.Count > 0 && decReceiptAmt != 0)
            {
                decimal decPaidAmt = 0;
                bool bolisFirst = false;
            Step1:

                foreach (DataRow drow in dtAppropriationList.Tables[0].Rows)   //Check CashFlow type by type
                {
                    if (decPaymentAmt == 0)    //if payment amount is zero then break the loop.
                        break;
                    if (decPaymentAmt < Convert.ToDecimal(1))
                    {
                        decInstallmentAmt = decPaymentAmt;
                        decPaymentAmt = 0;
                    }
                    else if (bolisFirst == false)
                        decInstallmentAmt = Math.Round(Convert.ToDecimal((decPaymentAmt * Convert.ToDecimal(drow["Percentage"])) / 100), 0);// objsession.ProGpsSuffixRW);
                    else
                        decInstallmentAmt = decPaymentAmt;
                    foreach (DataRow drowAccNo in dtCurrentPaymentStruct.Rows)   //Check account by Account
                    {
                        if (Convert.ToInt32(drow["AccountDescriptionId"]) == Convert.ToInt32(drowAccNo["Due_Flag"]))
                        {
                            if (decInstallmentAmt == 0)   //if installment amount is zero then break the loop.
                                break;
                            decimal decCurrrentAmt = 0;
                            if (Convert.ToDecimal(drowAccNo["Amount"]) <= decInstallmentAmt)
                            {                                                               //To Deleting the Full Paid Amount in Original Structure.
                                decCurrrentAmt = Convert.ToDecimal(drowAccNo["Amount"]);
                                decInstallmentAmt = decInstallmentAmt - Convert.ToDecimal(drowAccNo["Amount"]);
                                DataRow[] dr = dtAppropriationList.Tables[1].Select("ID='" + drowAccNo["ID"] + "'");
                                dtAppropriationList.Tables[1].Rows.Remove(dr[0]);
                            }
                            else
                            {                                                            //To Update the Amount in Original Structure based on Current Payment                          
                                decCurrrentAmt = Convert.ToDecimal(decInstallmentAmt);
                                DataRow[] dr = dtAppropriationList.Tables[1].Select("ID='" + drowAccNo["ID"] + "'");
                                dr[0].BeginEdit();
                                dr[0]["Amount"] = Convert.ToString(Convert.ToDecimal(drowAccNo["Amount"]) - decInstallmentAmt);
                                dr[0].EndEdit();
                                dr[0].AcceptChanges();
                                decInstallmentAmt = 0;
                            }
                            DataRow[] drContains = dtAppropriationCalList.Select("SNo='" + drowAccNo["ID"] + "' AND AccountDescriptionId='" + Convert.ToString(drowAccNo["Due_Flag"]) + "'");
                            if (drContains.Count() != 0)
                            {                                       //This part is Already Existing balance Amount updated
                                drContains[0].BeginEdit();
                                decimal decExistingAmt = Convert.ToDecimal(drContains[0]["Amount"]);
                                drContains[0]["Amount"] = Convert.ToDecimal(decExistingAmt + decCurrrentAmt);
                                drContains[0].EndEdit();
                                drContains[0].AcceptChanges();
                            }
                            else
                            {                                      // This Part Newly Inserted 
                                DataRow drAdd = dtAppropriationCalList.NewRow();
                                drAdd["SNo"] = Convert.ToInt32(drowAccNo["ID"]);
                                drAdd["PrimeAccountNo"] = drowAccNo["PrimeAccountNo"].ToString();
                                drAdd["SubAccountNo"] = drowAccNo["SubAccountNo"].ToString();
                                drAdd["AccountDescriptionId"] = Convert.ToString(drow["AccountDescriptionId"]);
                                drAdd["AccountDescription"] = Convert.ToString(drow["AccountDescription"]);
                                drAdd["Installment_No"] = "";// Convert.ToString(drowAccNo["InstallmentNo"]);
                                drAdd["Amount"] = decCurrrentAmt;
                                drAdd["GLAccountId"] = Convert.ToString(drowAccNo["GL_AccountId"]);
                                drAdd["SLAccountId"] = Convert.ToString(drowAccNo["SL_AccountId"]);
                                drAdd["GLAccount"] = Convert.ToString(drowAccNo["GL_Account"]);
                                drAdd["SLAccount"] = Convert.ToString(drowAccNo["SL_Account"]);
                                drAdd["CFMID"] = Convert.ToString(drowAccNo["CFMID"]);
                                drAdd["Parent_CashFlow_Flag_ID"] = Convert.ToString(drowAccNo["Parent_CashFlow_Flag_ID"]);      //Added on 29-Jun-2018 WRF 11748_175
                                drAdd["Is_GST"] = Convert.ToString(drowAccNo["Is_GST"]);      //Added on 13-Aug-2018 
                                dtAppropriationCalList.Rows.Add(drAdd);
                            }
                        }
                    }
                    if (bolisFirst)
                    {
                        if (decInstallmentAmt == 0)
                            break;
                        decPaymentAmt = decInstallmentAmt;
                    }
                }
                bolisFirst = true;
                decPaidAmt = 0;
                if (dtAppropriationCalList.Rows.Count > 0)
                {
                    decPaidAmt = Convert.ToDecimal((from c in dtAppropriationCalList.AsEnumerable()
                                                    select c.Field<decimal>("Amount")).Sum());
                }
                if (decReceiptAmt > decPaidAmt && dtAppropriationList.Tables[1].Rows.Count > 0)    //This Part is Pending amount is paid for unpaid amount then go to step1.
                {
                    dtCurrentPaymentStruct = dtAppropriationList.Tables[1].Copy();
                    decPaymentAmt = decReceiptAmt - decPaidAmt;
                    if (decPaymentAmt > Convert.ToDecimal(decSuffixValue))
                        goto Step1;
                }
            }
            var r = from c in dtAppropriationCalList.AsEnumerable()
                    orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo"), c.Field<Int32>("SNo") ascending   //c.Field<String>("AccountDescriptionId")
                    select c;
            if (r.Count() > 0)
                return r.CopyToDataTable();  // dtAppropriationCalList;
            else
                return null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;
    }

    /// <summary>
    /// To Handle Full Due in Horizontal Method
    /// </summary>
    /// <param name="decReceiptAmt"></param>
    /// <param name="dtAppropriationList"></param>
    /// <returns></returns>
    public DataTable FunPubReceiptAppropriationHorizontalFullDue(decimal decReceiptAmt, DataSet dtAppropriationList)
    {
        DataTable dtAppropriationCalList, dtAccountNoDetails = new DataTable();
        decimal decPaymentAmt = decReceiptAmt;
        decimal decInstallmentAmt = 0;
        try
        {
            DataTable dtCurrentPaymentStruct = dtAppropriationList.Tables[1].Copy();
            dtAppropriationCalList = FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
            var varAccList = from al in dtAppropriationList.Tables[1].AsEnumerable()
                             // orderby al.Field<Int32>("Due_Flag"), al.Field<String>("PrimeAccountNo"), al.Field<String>("SubAccountNo") ascending
                             select al;
            dtAccountNoDetails = varAccList.CopyToDataTable();
            dtCurrentPaymentStruct = dtAccountNoDetails.Copy();

            if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtCurrentPaymentStruct.Rows.Count > 0 && decReceiptAmt != 0)
            {
                decimal decPaidAmt = 0;
                bool bolisFirst = false;
                //For ISFC
                decInstallmentAmt = decPaymentAmt;
            Step1:

                foreach (DataRow drow in dtAppropriationList.Tables[0].Rows)   //Check CashFlow type by type
                {
                    if (decPaymentAmt == 0)    //if payment amount is zero then break the loop.
                        break;
                    if (decPaymentAmt < Convert.ToDecimal(1))
                    {
                        decInstallmentAmt = decPaymentAmt;
                        decPaymentAmt = 0;
                    }
                    else if (bolisFirst == false)
                        decInstallmentAmt = Math.Round(Convert.ToDecimal((decInstallmentAmt * Convert.ToDecimal(drow["Percentage"])) / 100), 0);// objsession.ProGpsSuffixRW);
                    else
                        decInstallmentAmt = decPaymentAmt;
                    foreach (DataRow drowAccNo in dtCurrentPaymentStruct.Rows)   //Check account by Account
                    {
                        if (Convert.ToInt32(drow["AccountDescriptionId"]) == Convert.ToInt32(drowAccNo["Due_Flag"]))
                        {
                            if (decInstallmentAmt == 0)   //if installment amount is zero then break the loop.
                                break;
                            decimal decCurrrentAmt = 0;
                            if (Convert.ToDecimal(drowAccNo["Amount"]) <= decInstallmentAmt)
                            {                                                               //To Deleting the Full Paid Amount in Original Structure.
                                decCurrrentAmt = Convert.ToDecimal(drowAccNo["Amount"]);
                                decInstallmentAmt = decInstallmentAmt - Convert.ToDecimal(drowAccNo["Amount"]);
                                DataRow[] dr = dtAppropriationList.Tables[1].Select("ID='" + drowAccNo["ID"] + "'");
                                dtAppropriationList.Tables[1].Rows.Remove(dr[0]);
                            }
                            else
                            {                                                            //To Update the Amount in Original Structure based on Current Payment                          
                                decCurrrentAmt = Convert.ToDecimal(decInstallmentAmt);
                                DataRow[] dr = dtAppropriationList.Tables[1].Select("ID='" + drowAccNo["ID"] + "'");
                                dr[0].BeginEdit();
                                dr[0]["Amount"] = Convert.ToString(Convert.ToDecimal(drowAccNo["Amount"]) - decInstallmentAmt);
                                dr[0].EndEdit();
                                dr[0].AcceptChanges();
                                decInstallmentAmt = 0;
                            }
                            DataRow[] drContains = dtAppropriationCalList.Select("SNo='" + drowAccNo["ID"] + "' AND AccountDescriptionId='" + Convert.ToString(drowAccNo["Due_Flag"]) + "'");
                            if (drContains.Count() != 0)
                            {                                       //This part is Already Existing balance Amount updated
                                drContains[0].BeginEdit();
                                decimal decExistingAmt = Convert.ToDecimal(drContains[0]["Amount"]);
                                drContains[0]["Amount"] = Convert.ToDecimal(decExistingAmt + decCurrrentAmt);
                                drContains[0].EndEdit();
                                drContains[0].AcceptChanges();
                            }
                            else
                            {                                      // This Part Newly Inserted 
                                DataRow drAdd = dtAppropriationCalList.NewRow();
                                drAdd["SNo"] = Convert.ToInt32(drowAccNo["ID"]);
                                drAdd["PrimeAccountNo"] = drowAccNo["PrimeAccountNo"].ToString();
                                drAdd["SubAccountNo"] = drowAccNo["SubAccountNo"].ToString();
                                drAdd["AccountDescriptionId"] = Convert.ToString(drow["AccountDescriptionId"]);
                                drAdd["AccountDescription"] = Convert.ToString(drow["AccountDescription"]);
                                drAdd["Installment_No"] = "";// Convert.ToString(drowAccNo["InstallmentNo"]);
                                drAdd["Amount"] = decCurrrentAmt;
                                drAdd["GLAccountId"] = Convert.ToString(drowAccNo["GL_AccountId"]);
                                drAdd["SLAccountId"] = Convert.ToString(drowAccNo["SL_AccountId"]);
                                drAdd["GLAccount"] = Convert.ToString(drowAccNo["GL_Account"]);
                                drAdd["SLAccount"] = Convert.ToString(drowAccNo["SL_Account"]);
                                drAdd["CFMID"] = Convert.ToString(drowAccNo["CFMID"]);
                                drAdd["Parent_CashFlow_Flag_ID"] = Convert.ToString(drowAccNo["Parent_CashFlow_Flag_ID"]);      //Added on 29-Jun-2018 WRF 11748_175
                                drAdd["Is_GST"] = Convert.ToString(drowAccNo["Is_GST"]);      //Added on 13-Aug-2018
                                dtAppropriationCalList.Rows.Add(drAdd);
                            }
                        }
                    }
                    if (bolisFirst)
                    {
                        if (decInstallmentAmt == 0)
                            break;
                        decPaymentAmt = decInstallmentAmt;
                    }
                }
                bolisFirst = true;
                decPaidAmt = 0;
                if (dtAppropriationCalList.Rows.Count > 0)
                {
                    decPaidAmt = Convert.ToDecimal((from c in dtAppropriationCalList.AsEnumerable()
                                                    select c.Field<decimal>("Amount")).Sum());
                }
                if (decReceiptAmt > decPaidAmt && dtAppropriationList.Tables[1].Rows.Count > 0)    //This Part is Pending amount is paid for unpaid amount then go to step1.
                {
                    dtCurrentPaymentStruct = dtAppropriationList.Tables[1].Copy();
                    decPaymentAmt = decReceiptAmt - decPaidAmt;
                    if (decPaymentAmt > Convert.ToDecimal(decSuffixValue))
                        goto Step1;
                }
            }
            var r = from c in dtAppropriationCalList.AsEnumerable()
                    orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo"), c.Field<Int32>("SNo") ascending   //c.Field<String>("AccountDescriptionId")
                    select c;
            if (r.Count() > 0)
                return r.CopyToDataTable();  // dtAppropriationCalList;
            else
                return null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;
    }

    public DataSet FunPubGetApprppriationLogicDetails(int intLOB_ID, int intCompanyId, int intBranch_ID, int intCustomer_ID)
    {
        ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
        dictProcParam.Add("@Company_ID", intCompanyId.ToString());
        dictProcParam.Add("@LOB_ID", intLOB_ID.ToString());
        dictProcParam.Add("@Location_ID", intBranch_ID.ToString());
        dictProcParam.Add("@Customer_ID", intCustomer_ID.ToString());
        return DLayerAdminService.FunPubFillDataset("S3G_CLN_GetPendingInstallmentWithAppropriationLogic", dictProcParam);
    }

    public DataTable FunPubSetDataTableStructureForAppropriationLogic()
    {
        DataTable dtAppropriationCalList = new DataTable();
        dtAppropriationCalList.Columns.Add("SNo", typeof(Int32));
        dtAppropriationCalList.Columns.Add("PrimeAccountNo", typeof(String));
        dtAppropriationCalList.Columns.Add("SubAccountNo", typeof(String));
        dtAppropriationCalList.Columns.Add("Installment_No", typeof(string));
        dtAppropriationCalList.Columns.Add("AccountDescriptionId", typeof(String));
        dtAppropriationCalList.Columns.Add("AccountDescription", typeof(String));
        dtAppropriationCalList.Columns.Add("GLAccountId", typeof(String));
        dtAppropriationCalList.Columns.Add("GLAccount", typeof(String));
        dtAppropriationCalList.Columns.Add("SLAccountId", typeof(String));
        dtAppropriationCalList.Columns.Add("SLAccount", typeof(String));
        dtAppropriationCalList.Columns.Add("Amount", typeof(Decimal));
        dtAppropriationCalList.Columns.Add("CFMID", typeof(String));
        dtAppropriationCalList.Columns.Add("RegistrationNumber", typeof(String));
        dtAppropriationCalList.Columns.Add("Remarks", typeof(String));                      //              Added by Senthilkumar P     08-May-2015
        dtAppropriationCalList.Columns.Add("Parent_CashFlow_Flag_ID", typeof(String));      //Added on 29-Jun-2018 WRF 11748_175
        dtAppropriationCalList.Columns.Add("Is_GST", typeof(String));      //Added on 13-Aug-2018 
        return dtAppropriationCalList;
    }

    public string FunPriGetXMLAcountNo(Dictionary<string, string> dictAccountNo)
    {
        StringBuilder strbXmlAccountNo = new StringBuilder();
        strbXmlAccountNo.Insert(0, "<Root>");
        foreach (KeyValuePair<string, string> kvp in dictAccountNo)
        {
            strbXmlAccountNo.Append("<Details PANum='" + kvp.Key + "'");
            strbXmlAccountNo.Append(" SANum='" + kvp.Value + "'/>");
        }
        strbXmlAccountNo.Append("</Root>");
        return strbXmlAccountNo.ToString();
    }

    public string FunPriGetXMLAcountNo(DataTable dtDistictAccountNo)
    {
        StringBuilder strbXmlAccountNo = new StringBuilder();
        strbXmlAccountNo.Insert(0, "<Root>");
        foreach (DataRow dr in dtDistictAccountNo.Rows)
        {
            strbXmlAccountNo.Append("<Details PANum='" + dr["PrimeAccountNo"].ToString() + "'");
            strbXmlAccountNo.Append(" SANum='" + dr["SubAccountNo"].ToString() + "'/>");
        }
        strbXmlAccountNo.Append("</Root>");
        return strbXmlAccountNo.ToString();
    }
}

public class ClsSelectDistinctDataTable
{
    public DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
    {
        object[] lastValues;
        DataTable newTable;
        DataRow[] orderedRows;

        if (FieldNames == null || FieldNames.Length == 0)
            throw new ArgumentNullException("FieldNames");

        lastValues = new object[FieldNames.Length];
        newTable = new DataTable();

        foreach (string fieldName in FieldNames)
            newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

        orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

        foreach (DataRow row in orderedRows)
        {
            if (!fieldValuesAreEqual(lastValues, row, FieldNames))
            {
                newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                setLastValues(lastValues, row, FieldNames);
            }
        }

        return newTable;
    }

    private bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
    {
        bool areEqual = true;

        for (int i = 0; i < fieldNames.Length; i++)
        {
            if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
            {
                areEqual = false;
                break;
            }
        }

        return areEqual;
    }

    private DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
    {
        foreach (string field in fieldNames)
            newRow[field] = sourceRow[field];

        return newRow;
    }

    private void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
    {
        for (int i = 0; i < fieldNames.Length; i++)
            lastValues[i] = sourceRow[fieldNames[i]];
    }


}

#region [Changed code part on 29/04/2011]

public class ChangedCodePartOnDATE29042011
{
    /// <summary>
    /// Created By Tamilselvan.s
    /// Created Date 17/03/2011
    /// To get Apprppriation Logic Details for Receipt Appropriation 
    /// </summary>
    /// <param name="intLOB_ID"></param>
    /// <param name="strAppropriationType"></param>
    /// <returns></returns>
    public DataSet FunPubGetApprppriationLogicDetails(int intLOB_ID, int intCompanyId, int intAppropriationType, string XmlAccountNo)
    {
        ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
        dictProcParam.Add("@Company_ID", intCompanyId.ToString());
        dictProcParam.Add("@LOB_ID", intLOB_ID.ToString());
        dictProcParam.Add("@AppropriationType", intAppropriationType.ToString());
        dictProcParam.Add("@XMLAccountNo", XmlAccountNo);
        return DLayerAdminService.FunPubFillDataset("S3G_CLN_GetAppropriationLogicDetails", dictProcParam);
    }


}
#endregion [Changed code part on 29/04/2011]


#region Unused Code1 replaced on 23/03/2011

///// <summary>
//  /// Created By Tamilselvan.s
//  /// Created Date 17/03/2011
//  /// For Receipt Appropriation Logic Calculation only for Vertical
//  /// </summary>
//  /// <param name="decReceiptAmt"></param>
//  /// <param name="intLOB_ID"></param>
//  /// <param name="intCompanyId"></param>
//  /// <param name="dtAccountNoDetails"></param>
//  /// <returns></returns>
//  public DataTable FunPubReceiptAppropriationVertical(Dictionary<string, string> dictAccountNo, decimal decReceiptAmt, int intLOB_ID, int intCompanyId, DataTable dtAccountNoDetails)
//  {
//      DataSet dtAppropriationList = new DataSet();
//      DataTable dtAppropriationCalList = new DataTable();
//      decimal decRemainingAmt = 0;
//      decimal decInstallmentAmt = 0;
//      try
//      {
//          dtAppropriationCalList = FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
//          if (dtAccountNoDetails.Rows.Count > 0)
//          {
//              var varlsits = from al in dtAccountNoDetails.AsEnumerable()
//                             orderby al.Field<String>("PrimeAccountNo"), al.Field<String>("SubAccountNo"), al.Field<String>("AccountDescriptionId") ascending
//                             select al;
//              DataView dv = dtAccountNoDetails.DefaultView;
//              DataTable dtDistictAccountNo = dv.ToTable(true, strColumnsDistictAccNo);
//              dtAccountNoDetails = varlsits.CopyToDataTable();
//              dtAppropriationList = FunPubGetApprppriationLogicDetails(intLOB_ID, intCompanyId, 1, FunPriGetXMLAcountNo(dtDistictAccountNo));  //Get Appropriation logic list and Repayment structure details
//              if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtAppropriationList.Tables[1].Rows.Count > 0)
//              {
//                  string strNextPayment = string.Empty;
//                  int intMaxInstallment = 0;
//                  int intCount = 0;
//              Step1:
//                  foreach (DataRow drAccNo in dtAccountNoDetails.Rows)    //Check account by Account
//                  {
//                      DataTable dtRepayStructure = ((DataRow[])dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "'")).CopyToDataTable();  // AND [Installment No] not like " + strInsNo);  //Get account by Account from the repayment structure details
//                      decimal decTotalInsAmount = 0;
//                      if (dtRepayStructure.Rows.Count > intCount)
//                      {
//                          strNextPayment = dtRepayStructure.Rows[intCount]["Installment_No"].ToString();
//                          if (intMaxInstallment < Convert.ToInt32(dtRepayStructure.Rows[dtRepayStructure.Rows.Count - 1]["Installment_No"]))
//                              intMaxInstallment = Convert.ToInt32(dtRepayStructure.Rows[dtRepayStructure.Rows.Count - 1]["Installment_No"]);
//                          var varRepayStructure = from c in dtRepayStructure.AsEnumerable()
//                                                  where (Convert.ToString(c.Field<Int32>("Installment_No")) == strNextPayment)   //(c.Field<Boolean>("Status") == false) && 
//                                                  select c;
//                          var varCurrentStructure = varRepayStructure.First();

//                          foreach (DataRow drApproLogic in dtAppropriationList.Tables[0].Rows)      //Check CashFlow type by type
//                          {
//                              if (decReceiptAmt <= 0)
//                                  break;
//                              DataTable dtExistDetails = dtAppropriationCalList.Clone();
//                              DataRow[] drExistingDetails = null;
//                              if (dtAppropriationCalList.Rows.Count > 0)
//                                  drExistingDetails = dtAppropriationCalList.Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "' AND AccountDescriptionId='" + drApproLogic["AccountDescriptionId"] + "'");
//                              if (drExistingDetails != null && drExistingDetails.Count() > 0)
//                                  dtExistDetails = drExistingDetails.CopyToDataTable();
//                              if (dtExistDetails.Rows.Count == 0 || (Convert.ToString(dtExistDetails.Rows[0]["AccountDescription"]).Contains("Instal")))
//                              {
//                                  if (Convert.ToInt32(drApproLogic["AccountDescriptionId"]) == Convert.ToInt32(drAccNo["AccountDescriptionId"]))
//                                  {
//                                      decInstallmentAmt = Math.Round(Convert.ToDecimal((decReceiptAmt * Convert.ToDecimal(drApproLogic["Percentage"])) / 100));
//                                      decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
//                                      decTotalInsAmount += decInstallmentAmt;
//                                      DataRow drAdd = dtAppropriationCalList.NewRow();
//                                      drAdd["SNo"] = dtAppropriationCalList.Rows.Count + 1;
//                                      drAdd["PrimeAccountNo"] = drAccNo["PrimeAccountNo"].ToString();
//                                      drAdd["SubAccountNo"] = drAccNo["SubAccountNo"].ToString();
//                                      drAdd["AccountDescriptionId"] = Convert.ToInt32(drApproLogic["AccountDescriptionId"]);
//                                      drAdd["AccountDescription"] = Convert.ToString(drApproLogic["AccountDescription"]);
//                                      drAdd["Installment_No"] = Convert.ToInt32(varCurrentStructure["Installment_No"]);
//                                      drAdd["GLAccountId"] = Convert.ToInt32(drAccNo["GLAccountId"]);
//                                      drAdd["GLAccount"] = Convert.ToString(drAccNo["GLAccount"]);
//                                      drAdd["SLAccountId"] = Convert.ToInt32(drAccNo["SLAccountId"]);
//                                      drAdd["SLAccount"] = Convert.ToString(drAccNo["SLAccount"]);
//                                      drAdd["Amount"] = Convert.ToDecimal(decTotalInsAmount);
//                                      drAdd["RemainingAmt"] = decReceiptAmt = Convert.ToDecimal(decRemainingAmt);
//                                      dtAppropriationCalList.Rows.Add(drAdd);
//                                  }
//                              }
//                          }
//                          if (decReceiptAmt <= 0)
//                              break;
//                          if (dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "'AND Installment_No='" + Convert.ToString(varCurrentStructure["Installment_No"]) + "'").CopyToDataTable().Rows.Count > 0)
//                          {
//                              dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "'AND Installment_No='" + Convert.ToString(varCurrentStructure["Installment_No"]) + "'")[0].BeginEdit();
//                              dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "'AND Installment_No='" + Convert.ToString(varCurrentStructure["Installment_No"]) + "'")[0]["Status"] = true;
//                              dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "'AND Installment_No='" + Convert.ToString(varCurrentStructure["Installment_No"]) + "'")[0].EndEdit();
//                              dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drAccNo["SubAccountNo"].ToString() + "'AND Installment_No='" + Convert.ToString(varCurrentStructure["Installment_No"]) + "'")[0].AcceptChanges();
//                          }
//                      }
//                  }
//                  if (decReceiptAmt > 0)
//                  {
//                      intCount++;
//                      if (intMaxInstallment > intCount)
//                          goto Step1;
//                  }
//              }
//          }
//      }
//      catch (Exception ex)
//      {
//            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
//          throw;
//      }
//      var r = from c in dtAppropriationCalList.AsEnumerable()
//              orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo") ascending
//              select c;
//      return r.CopyToDataTable();
//  }

//  /// <summary>
//  /// Created By Tamilselvan.s
//  /// Created Date 17/03/2011
//  /// For Receipt Appropriation Logic Calculation only for Horizontal
//  /// </summary>
//  /// <param name="decReceiptAmt"></param>
//  /// <param name="intLOB_ID"></param>
//  /// <param name="intCompanyId"></param>
//  /// <param name="dtAccountNoDetails"></param>
//  /// <returns></returns>
//  public DataTable FunPubReceiptAppropriationHorizontal(decimal decReceiptAmt, int intLOB_ID, int intCompanyId, DataTable dtAccountNoDetails)
//  {
//      DataSet dtAppropriationList = new DataSet();
//      DataTable dtAppropriationCalList = new DataTable();
//      decimal decRemainingAmt = 0;
//      decimal decInstallmentAmt = 0;
//      try
//      {
//          dtAppropriationCalList = FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
//          if (dtAccountNoDetails.Rows.Count > 0)
//          {
//              DataView dv = dtAccountNoDetails.DefaultView;
//              DataTable dtDistictAccountNo = dv.ToTable(true, strColumnsDistictAccNo);

//              dtAppropriationList = FunPubGetApprppriationLogicDetails(intLOB_ID, intCompanyId, 2, FunPriGetXMLAcountNo(dtDistictAccountNo));
//              if (dtAppropriationList.Tables[0].Rows.Count > 0 && dtAppropriationList.Tables[1].Rows.Count > 0 && decReceiptAmt != 0)
//              {
//                  foreach (DataRow drow in dtAppropriationList.Tables[0].Rows)   //Check CashFlow type by type
//                  {
//                      decimal TotalAmt = Convert.ToDecimal(dtAppropriationCalList.AsEnumerable().Sum(x => x.Field<Decimal>("Amount")));
//                      if (TotalAmt == decReceiptAmt)
//                          break;
//                      foreach (DataRow drowAccNo in dtAccountNoDetails.Rows)   //Check account by Account
//                      {
//                          //DataTable dtRepayStructure = ((DataRow[])dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + drowAccNo["PrimeAccountNo"].ToString() + "' AND SubAccountNo='" + drowAccNo["SubAccountNo"].ToString() + "'")).CopyToDataTable();  //Get account by Account from the repayment structure details

//                          if (Convert.ToInt32(drow["AccountDescriptionId"]) == Convert.ToInt32(drowAccNo["AccountDescriptionId"]))
//                          {
//                              decInstallmentAmt = Math.Round(Convert.ToDecimal((decReceiptAmt * Convert.ToDecimal(drow["Percentage"])) / 100));

//                              TotalAmt = Convert.ToDecimal(dtAppropriationCalList.AsEnumerable().Sum(x => x.Field<Decimal>("Amount")));
//                              if (TotalAmt >= decReceiptAmt)
//                                  break;
//                              if (TotalAmt + decInstallmentAmt > decReceiptAmt)
//                                  decInstallmentAmt = decReceiptAmt - TotalAmt;
//                              DataRow drAdd = dtAppropriationCalList.NewRow();
//                              drAdd["SNo"] = dtAppropriationCalList.Rows.Count + 1;
//                              drAdd["PrimeAccountNo"] = drowAccNo["PrimeAccountNo"].ToString();
//                              drAdd["SubAccountNo"] = drowAccNo["SubAccountNo"].ToString();
//                              drAdd["AccountDescriptionId"] = Convert.ToInt32(drow["AccountDescriptionId"]);
//                              drAdd["AccountDescription"] = Convert.ToString(drow["AccountDescription"]);
//                              drAdd["Installment_No"] = "1";// Convert.ToInt32(varCurrentStructure["Installment_No"]);
//                              drAdd["Amount"] = Convert.ToDecimal(decInstallmentAmt);
//                              drAdd["RemainingAmt"] = Convert.ToDecimal(decRemainingAmt);
//                              dtAppropriationCalList.Rows.Add(drAdd);
//                              if (Convert.ToDecimal(dtAppropriationCalList.AsEnumerable().Sum(x => x.Field<Decimal>("Amount"))) == decReceiptAmt)
//                                  break;
//                          }
//                      }
//                  }
//              }
//          }
//      }
//      catch (Exception ex)
//      {
//            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
//          throw;
//      }
//      var r = from c in dtAppropriationCalList.AsEnumerable()
//              orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo") ascending
//              select c;
//      return r.CopyToDataTable();
//  }

#endregion Unused code1


#region [Unused]

/// <summary>
/// Code is changed based on input this for old one
/// </summary>
public class UnUsedCode
{
    ClsAppropriationLogic clsApplogic = new ClsAppropriationLogic();
    public DataTable FunPubReceiptAppropriationVertical(Dictionary<string, string> dictAccountNo, decimal decReceiptAmt, int intLOB_ID, int intCompanyId, DataTable dtAccountNoDetails)
    {
        DataSet dtAppropriationList = new DataSet();
        DataTable dtAppropriationCalList = new DataTable();
        decimal decRemainingAmt = 0;
        decimal decInstallmentAmt = 0;
        try
        {
            dtAppropriationCalList = clsApplogic.FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
            if (dtAccountNoDetails.Rows.Count > 0)
            {
                DataView dv = dtAccountNoDetails.DefaultView;
                DataTable dtDistictAccountNo = dv.ToTable(true, clsApplogic.strColumnsDistictAccNo);

                dtAppropriationList = clsApplogic.FunPubGetApprppriationLogicDetails(intLOB_ID, intCompanyId, 1, 1);  //Get Appropriation logic list and Repayment structure details
                if (dtAppropriationList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drAccNo in dtAccountNoDetails.Rows)
                    {
                        foreach (KeyValuePair<string, string> kvp in dictAccountNo)   //Check account by Account
                        {
                            var varapplist = from l in dtAppropriationCalList.AsEnumerable()
                                             select new { Installment_No = l.Field<Int32>("Installment_No") };
                            string strInsNo = string.Empty;
                            foreach (var var1 in varapplist)
                            {
                                if (strInsNo == string.Empty)
                                    strInsNo += var1.Installment_No.ToString();
                                else
                                    strInsNo += "," + var1.Installment_No.ToString();
                            }

                            DataRow[] drRepayStructure = dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + kvp.Key.ToString() + "' AND SubAccountNo='" + kvp.Value.ToString() + "' AND [Installment No] not like " + strInsNo);  //Get account by Account from the repayment structure details
                            DataTable dtRepayStructure = drRepayStructure.CopyToDataTable();
                            decimal decTotalInsAmount = 0;
                            var varRepayStructure = from c in dtRepayStructure.AsEnumerable()
                                                    // where c.Field<Boolean>("Status") == false
                                                    select c;
                            var varCurrentStructure = varRepayStructure.First();

                            foreach (DataRow dr in dtAppropriationList.Tables[0].Rows)      //Check CashFlow type by type
                            {
                                decInstallmentAmt = Math.Round(Convert.ToDecimal((decReceiptAmt * Convert.ToDecimal(dr["Percentage"])) / 100));
                                decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
                                decTotalInsAmount += decInstallmentAmt;
                            }
                            //check Installment amount Vs Calculated amount in appropriation logic
                            if (decInstallmentAmt > Convert.ToDecimal(varCurrentStructure.Field<Decimal>("InstallmentAmount")))
                            {
                                decInstallmentAmt = Math.Round(Convert.ToDecimal(varCurrentStructure.Field<Decimal>("InstallmentAmount")));
                                decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
                            }

                            DataRow drAdd = dtAppropriationCalList.NewRow();
                            drAdd["PrimeAccountNo"] = kvp.Key.ToString();
                            drAdd["SubAccountNo"] = kvp.Value.ToString();
                            drAdd["Due_Flag"] = 0;// Convert.ToInt32(dr["Due_Flag"]);
                            drAdd["Due_FlagName"] = "";// Convert.ToString(dr["Due_FlagName"]);
                            drAdd["Installment_No"] = Convert.ToInt32(varCurrentStructure["Installment No"]);
                            drAdd["Amount"] = Convert.ToDecimal(decTotalInsAmount);
                            drAdd["RemainingAmt"] = decReceiptAmt = Convert.ToDecimal(decRemainingAmt);
                            dtAppropriationCalList.Rows.Add(drAdd);

                            varCurrentStructure.BeginEdit();
                            varCurrentStructure["Status"] = true;
                            varCurrentStructure.EndEdit();
                            varCurrentStructure.AcceptChanges();

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }
        return dtAppropriationCalList;
    }

    /// <summary>
    /// Created By Tamilselvan.s
    /// Created Date 17/03/2011
    /// For Receipt Appropriation Logic Calculation only for Horizontal
    /// </summary>
    /// <param name="dictAccountNo"></param>
    /// <param name="decReceiptAmt"></param>
    /// <param name="intLOB_ID"></param>
    /// <param name="intCompanyId"></param>
    /// <returns></returns>
    public DataTable FunPubReceiptAppropriationHorizontal(Dictionary<string, string> dictAccountNo, decimal decReceiptAmt, int intLOB_ID, int intCompanyId, DataTable dtAccountNoDetails)
    {
        DataSet dtAppropriationList = new DataSet();
        DataTable dtAppropriationCalList = new DataTable();
        decimal decRemainingAmt = 0;
        decimal decInstallmentAmt = 0;
        try
        {
            dtAppropriationCalList = clsApplogic.FunPubSetDataTableStructureForAppropriationLogic();   //Creating table structure
            if (dtAccountNoDetails.Rows.Count > 0)
            {
                DataView dv = dtAccountNoDetails.DefaultView;
                DataTable dtDistictAccountNo = dv.ToTable(true, clsApplogic.strColumnsDistictAccNo);

                dtAppropriationList = clsApplogic.FunPubGetApprppriationLogicDetails(intLOB_ID, intCompanyId, 2, 1);
                if (dtAppropriationList.Tables[0].Rows.Count > 0)
                {
                    //dtAppropriationList.Tables[1].Columns.Add("Status", typeof(Boolean)).DefaultValue = false;
                    //dtAppropriationList.Tables[1].Columns.Add("CurrentRow", typeof(Int32)).DefaultValue = 0;
                    if (dictAccountNo.Count > 1)
                    {
                        int intLoopCount = 0;
                        foreach (DataRow drow in dtAppropriationList.Tables[0].Rows)   //Check CashFlow type by type
                        {
                            if (decReceiptAmt == 0)
                                break;
                            foreach (KeyValuePair<string, string> kvp in dictAccountNo)   //Check account by Account
                            {
                                DataRow[] drRepayStructure = dtAppropriationList.Tables[1].Select("PrimeAccountNo='" + kvp.Key.ToString() + "' AND SubAccountNo='" + kvp.Value.ToString() + "'");  //Get account by Account from the repayment structure details
                                DataTable dtRepayStructure = drRepayStructure.CopyToDataTable();
                                if (intLoopCount == 0)
                                {
                                    var varRepayStructure = from c in dtRepayStructure.AsEnumerable()
                                                            // where c.Field<Boolean>("Status") == false
                                                            select c;
                                    var varCurrentStructure = varRepayStructure.First();

                                    decInstallmentAmt = Math.Round(Convert.ToDecimal((decReceiptAmt * Convert.ToDecimal(drow["Percentage"])) / 100));
                                    if (decReceiptAmt <= decInstallmentAmt)
                                    {
                                        decInstallmentAmt = decReceiptAmt;
                                        decRemainingAmt = 0;
                                    }
                                    else
                                        decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
                                    if (decInstallmentAmt > Convert.ToDecimal(varCurrentStructure.Field<Decimal>("InstallmentAmount")))
                                    {
                                        decInstallmentAmt = Math.Round(Convert.ToDecimal(varCurrentStructure.Field<Decimal>("InstallmentAmount")));
                                        decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
                                    }
                                    DataRow drAdd = dtAppropriationCalList.NewRow();
                                    drAdd["PrimeAccountNo"] = kvp.Key.ToString();
                                    drAdd["SubAccountNo"] = kvp.Value.ToString();
                                    drAdd["Due_Flag"] = Convert.ToInt32(drow["Due_Flag"]);
                                    drAdd["Due_FlagName"] = Convert.ToString(drow["Due_FlagName"]);
                                    drAdd["Installment_No"] = Convert.ToInt32(varCurrentStructure["Installment No"]);
                                    drAdd["Amount"] = Convert.ToDecimal(decInstallmentAmt);
                                    drAdd["RemainingAmt"] = decReceiptAmt = Convert.ToDecimal(decRemainingAmt);
                                    dtAppropriationCalList.Rows.Add(drAdd);
                                    if (decReceiptAmt == 0)
                                        break;
                                }
                                else
                                {
                                    var varRepayStructure = from c in dtRepayStructure.AsEnumerable()
                                                            //where c.Field<Boolean>("Status") == true
                                                            select c;
                                    var varCurrentStructure = varRepayStructure.First();

                                    decInstallmentAmt = Math.Round(Convert.ToDecimal((decReceiptAmt * Convert.ToDecimal(drow["Percentage"])) / 100));
                                    if (decReceiptAmt <= decInstallmentAmt)
                                    {
                                        decInstallmentAmt = decReceiptAmt;
                                        decRemainingAmt = 0;
                                    }
                                    else
                                        decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
                                    if (decInstallmentAmt > Convert.ToDecimal(varCurrentStructure.Field<Decimal>("InstallmentAmount")))
                                    {
                                        decInstallmentAmt = Math.Round(Convert.ToDecimal(varCurrentStructure.Field<Decimal>("InstallmentAmount")));
                                        decRemainingAmt = Convert.ToDecimal(decReceiptAmt - decInstallmentAmt);
                                    }
                                    DataRow[] drAdd = dtAppropriationCalList.Select("PrimeAccountNo='" + kvp.Key.ToString() + "' AND SubAccountNo='" + kvp.Value.ToString() + "'");// AND Due_Flag='" + Convert.ToInt32(drow["Due_Flag"]) + "'");
                                    //DataRow[] drAdd = drList.Where(t => t.Field<Int32>("Due_Flag") == Convert.ToInt32(drow["Due_Flag"])).AsEnumerable();
                                    //DataRow[] drAdd = drList.CopyToDataTable().DefaultView.RowFilter = "Due_Flag=" + Convert.ToInt32(drow["Due_Flag"]);
                                    drAdd[0].BeginEdit();
                                    drAdd[0]["Amount"] = Convert.ToDecimal(Convert.ToDecimal(drAdd[0]["Amount"]) + decInstallmentAmt);
                                    drAdd[0]["RemainingAmt"] = decReceiptAmt = Convert.ToDecimal(decRemainingAmt);
                                    drAdd[0].EndEdit();
                                    drAdd[0].AcceptChanges();
                                    if (decReceiptAmt == 0)
                                        break;
                                }
                            }
                            intLoopCount++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            throw;
        }

        return dtAppropriationCalList;
    }
}

#endregion [Unused]