using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using S3GDALayer.S3GAdminServices;

namespace Axis_Bank_Biller_Integration_GL.Controllers
{

    //Created By:Sathish R
    //Created on:09-Aug-2021
    public class BPPSController : ApiController
    {
        [HttpGet]
        public string Get(string Request_ID, string Loan_Numer)
        {
            ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
            clsLoanBillFetch ObjclsLoanQuery = new clsLoanBillFetch();
            try
            {
                string strArray = string.Empty;
                clsLoanBillFetchAdditionalInfo ObjclsLoanBillFetchAdditionalInfo = new clsLoanBillFetchAdditionalInfo();
                ObjclsLoanQuery.Request_ID = Request_ID;
                Dictionary<string, string> strProparm = new Dictionary<string, string>();
                strProparm.Add("@Option", "1");
                strProparm.Add("@Request_ID", Request_ID);
                strProparm.Add("@Loan_Numer", Loan_Numer);
                DataTable dtUpdateLoanStatus = DLayerAdminService.FunPubFillDropdown("S3G_BBPS_GET_ACCOUNT_INFO_DTL_VL", strProparm);
                if (dtUpdateLoanStatus.Rows.Count > 0)
                {
                    if (dtUpdateLoanStatus.Rows[0]["Error_Code"].ToString() != "00")
                    {
                        if (Loan_Numer != null)
                        {
                            ObjclsLoanQuery.Loan_Numer = Loan_Numer.ToString();
                        }
                        ObjclsLoanQuery.status = HttpStatusCode.BadRequest.ToString();
                        ObjclsLoanQuery.errorCode = dtUpdateLoanStatus.Rows[0]["Error_Code"].ToString();
                        ObjclsLoanQuery.errorCode_Status = dtUpdateLoanStatus.Rows[0]["Error_Code_Status"].ToString();

                        funPriUpdateResponse(Request_ID, JsonConvert.SerializeObject(ObjclsLoanQuery),"2","Failed");
                    }
                    else
                    {
                        ObjclsLoanQuery.Loan_Numer = Loan_Numer.ToString();
                        ObjclsLoanQuery.status = HttpStatusCode.OK.ToString();
                        ObjclsLoanQuery.errorCode = "100";
                        ObjclsLoanQuery.customerName = dtUpdateLoanStatus.Rows[0]["CustomerName"].ToString();
                        ObjclsLoanQuery.amountDue = dtUpdateLoanStatus.Rows[0]["Amount"].ToString();
                        ObjclsLoanQuery.billDate = dtUpdateLoanStatus.Rows[0]["billDate"].ToString();
                        ObjclsLoanQuery.dueDate = dtUpdateLoanStatus.Rows[0]["dueDate"].ToString();
                        ObjclsLoanQuery.billPeriod = dtUpdateLoanStatus.Rows[0]["billPeriod"].ToString();
                        ObjclsLoanQuery.errorCode_Status = dtUpdateLoanStatus.Rows[0]["Error_Code_Status"].ToString();
                        ObjclsLoanQuery.additionalInfo = strArray = ObjclsLoanBillFetchAdditionalInfo.FunPriFormAdditionalInfor(dtUpdateLoanStatus);
                        funPriUpdateResponse(Request_ID, JsonConvert.SerializeObject(ObjclsLoanQuery), "2","Success");
                    }
                }
               
                return JsonConvert.SerializeObject(ObjclsLoanQuery);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                ObjclsLoanQuery.Loan_Numer = Loan_Numer.ToString();
                ObjclsLoanQuery.status = HttpStatusCode.BadRequest.ToString();
                ObjclsLoanQuery.errorCode = "101";
                funPriUpdateResponse(Request_ID, JsonConvert.SerializeObject(ObjclsLoanQuery),"2","Exception");
                return JsonConvert.SerializeObject(ObjclsLoanQuery);
            }
        }
        private void funPriUpdateResponse(string Request_ID, string ResponseJosn,string Option,string API_Status)
        {
            try
            {
                ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
                Dictionary<string, string> strProparm = new Dictionary<string, string>();
                strProparm.Add("@Option", Option);
                strProparm.Add("@Request_ID", Request_ID.ToString());
                strProparm.Add("@ResponseJosn", ResponseJosn);
                strProparm.Add("@API_Status", API_Status);
                
                DataTable dtUpdateLoanStatus = DLayerAdminService.FunPubFillDropdown("S3G_BBPS_GET_ACCOUNT_INFO_DTL_VL", strProparm);

            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
        }
        //[HttpPost]
        //public string Post([FromBody] clsBillPost objclsclsBillPost)
        //{
        //    ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
        //    Dictionary<string, string> strProparm = new Dictionary<string, string>();
        //    strProparm.Add("@Option", "1");
        //    strProparm.Add("@Request_ID", objclsclsBillPost.Request_ID);
        //    strProparm.Add("@Loan_Numer", objclsclsBillPost.loan_number);
        //    strProparm.Add("@amountPaid", objclsclsBillPost.amountPaid);
        //    strProparm.Add("@transactionId", objclsclsBillPost.transactionId);
        //    strProparm.Add("@paymentMode", objclsclsBillPost.paymentMode);
        //    strProparm.Add("@paymentDate", objclsclsBillPost.paymentDate);
        //    DataTable dtUpdateLoanStatus = DLayerAdminService.FunPubFillDropdown("S3G_BBPS_POST_ACCOUNT_INFO_INS", strProparm);
        //    if (dtUpdateLoanStatus.Rows[0]["Error_Code"].ToString() != "00")
        //    {
        //        objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
        //        objclsclsBillPost.status = HttpStatusCode.BadRequest.ToString();
        //        objclsclsBillPost.errorCode = dtUpdateLoanStatus.Rows[0]["Error_Code"].ToString();
        //        funPriUpdateResponse(objclsclsBillPost.Request_ID, JsonConvert.SerializeObject(objclsclsBillPost));
        //    }
        //    else
        //    {
        //        objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
        //        objclsclsBillPost.status = HttpStatusCode.OK.ToString();
        //        objclsclsBillPost.errorCode = "100";

        //    }
        //    return JsonConvert.SerializeObject(objclsclsBillPost);
        //}
        [HttpPost]
        public string Post([FromBody] clsBillPost objclsclsBillPost)
        {
            try
            {
                DataTable dtFinalApprDet = new DataTable();
                ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
                Dictionary<string, string> strProparm = new Dictionary<string, string>();
                int i = 0;
                string strErroStatus = string.Empty;

                strProparm.Add("@Request_ID", objclsclsBillPost.Request_ID);
                strProparm.Add("@Loan_Numer", objclsclsBillPost.loan_number);
                strProparm.Add("@amountPaid", objclsclsBillPost.amountPaid.ToString());
                strProparm.Add("@transactionId", objclsclsBillPost.transactionId.ToString());
                strProparm.Add("@paymentMode", objclsclsBillPost.paymentMode.ToString());
                strProparm.Add("@paymentDate", objclsclsBillPost.paymentDate.ToString());
                DataTable dtUpdateLoanStatus = DLayerAdminService.FunPubFillDropdown("S3G_BBPS_POST_VALIDATE_VL", strProparm);
                if (dtUpdateLoanStatus.Rows.Count > 0)
                {
                    if (dtUpdateLoanStatus.Rows[0]["ErrorCode"].ToString() != "100")
                    {
                        objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
                        objclsclsBillPost.status = HttpStatusCode.BadRequest.ToString();
                        objclsclsBillPost.errorCode = i.ToString();
                        objclsclsBillPost.errorCode_Status = dtUpdateLoanStatus.Rows[0]["ErroStatus"].ToString();
                        funPriUpdateResponse(objclsclsBillPost.Request_ID.ToString(), JsonConvert.SerializeObject(objclsclsBillPost), "3", "Failed");
                        return JsonConvert.SerializeObject(objclsclsBillPost);
                    }
                    else
                    {
                        Local_Class.clsLoadAppropritaionLogic LoadlocalCalss = new Local_Class.clsLoadAppropritaionLogic();
                        dtFinalApprDet=LoadlocalCalss.funPriclsLoadAppropritaionLogic(dtUpdateLoanStatus.Rows[0]["amountPaid"].ToString(), dtUpdateLoanStatus.Rows[0]["LOB_ID"].ToString(),Convert.ToInt32( dtUpdateLoanStatus.Rows[0]["Company_ID"].ToString()), dtUpdateLoanStatus.Rows[0]["LOCAITON_ID"].ToString(), dtUpdateLoanStatus.Rows[0]["CUSTOMER_ID"].ToString(), objclsclsBillPost.loan_number,Convert.ToInt32( dtUpdateLoanStatus.Rows[0]["Currency_Max_Dec_Digit"].ToString()));
                        if (dtFinalApprDet.Rows.Count == 0)
                        {
                            objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
                            objclsclsBillPost.status = HttpStatusCode.BadRequest.ToString();
                            objclsclsBillPost.errorCode = i.ToString();
                            objclsclsBillPost.errorCode_Status = "No Pending due for this account";
                            funPriUpdateResponse(objclsclsBillPost.Request_ID.ToString(), JsonConvert.SerializeObject(objclsclsBillPost), "3", "Failed");
                            return JsonConvert.SerializeObject(objclsclsBillPost);
                        }
                    }
                }

               
                clsBillPost objclsBillPost = new clsBillPost();
                string strAccountDetails = string.Empty;
                Local_Class.clsLoadAppropritaionLogic objclsLoadAppropritaionLogic = new Local_Class.clsLoadAppropritaionLogic();
                S3GDALayer.API_Post.clsAPIpost clsAPIpost = new S3GDALayer.API_Post.clsAPIpost();
                strAccountDetails = objclsLoadAppropritaionLogic.FunPubFormXml(dtFinalApprDet,true);
                i = clsAPIpost.FunPubPostReceiptDetails(1, objclsclsBillPost.Request_ID, objclsclsBillPost.loan_number, objclsclsBillPost.amountPaid, objclsclsBillPost.transactionId, objclsclsBillPost.paymentMode, objclsclsBillPost.paymentDate, out strErroStatus, strAccountDetails);
                if (i != 0)
                {
                    objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
                    objclsclsBillPost.status = HttpStatusCode.BadRequest.ToString();
                    objclsclsBillPost.errorCode = i.ToString();
                    objclsclsBillPost.errorCode_Status = strErroStatus;
                    funPriUpdateResponse(objclsclsBillPost.Request_ID.ToString(), JsonConvert.SerializeObject(objclsclsBillPost), "3", "Failed");
                }
                else
                {
                    objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
                    objclsclsBillPost.status = HttpStatusCode.OK.ToString();
                    objclsclsBillPost.errorCode = i.ToString();
                    objclsclsBillPost.errorCode_Status = strErroStatus;
                    funPriUpdateResponse(objclsclsBillPost.Request_ID.ToString(), JsonConvert.SerializeObject(objclsclsBillPost), "3", "Success");

                }
                return JsonConvert.SerializeObject(objclsclsBillPost);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
                objclsclsBillPost.status = HttpStatusCode.BadRequest.ToString();
                objclsclsBillPost.errorCode = "101";
                objclsclsBillPost.errorCode_Status = "Exception";
                funPriUpdateResponse(objclsclsBillPost.Request_ID.ToString(), JsonConvert.SerializeObject(objclsclsBillPost), "3", "Exception");
                return JsonConvert.SerializeObject(objclsclsBillPost);
            }
        }

    }
    public class clsLoanBillFetch
    {
        public clsLoanBillFetch()
        {

        }
        public string Request_ID { get; set; }
        public string Loan_Numer { get; set; }
        public string status { get; set; }
        public string errorCode { get; set; }
        public string errorCode_Status { get; set; }
        public string customerName { get; set; }
        public string amountDue { get; set; }
        public string billDate { get; set; }
        public string dueDate { get; set; }
        public string billPeriod { get; set; }
        public string additionalInfo { get; set; }

    }
    public class clsLoanBillFetchAdditionalInfo
    {
        public string Vehicle_number { get; set; }
        public string Arrear_Amount { get; set; }
        public string Other_Memos { get; set; }
        public string Phone_number { get; set; }
        public clsLoanBillFetchAdditionalInfo()
        {

        }
        public string FunPriFormAdditionalInfor(DataTable dtAsset)
        {
            string strAsset = string.Empty;
            List<clsLoanBillFetchAdditionalInfo> lstClsObjclsLoanBillFetchAdditionalInfo = new List<clsLoanBillFetchAdditionalInfo>();
            if (dtAsset.Rows.Count > 0)
            {

                foreach (DataRow dr in dtAsset.Rows)
                {
                    lstClsObjclsLoanBillFetchAdditionalInfo.Add(new clsLoanBillFetchAdditionalInfo() { Vehicle_number = dr["Vehicle_number"].ToString(), Arrear_Amount = dr["Arrear_Amount"].ToString(), Other_Memos = dr["Other_Memos"].ToString(), Phone_number = dr["Phone_number"].ToString() });
                }

                strAsset = JsonConvert.SerializeObject(lstClsObjclsLoanBillFetchAdditionalInfo);
            }
            else
            {
                strAsset = JsonConvert.SerializeObject(lstClsObjclsLoanBillFetchAdditionalInfo);
            }
            return strAsset;
        }

    }
    public class clsBillPost
    {
        public string Request_ID { get; set; }
        public string status { get; set; }
        public string errorCode { get; set; }
        public string errorCode_Status { get; set; }
        public string loan_number { get; set; }
        public decimal amountPaid { get; set; }
        public string transactionId { get; set; }
        public string paymentMode { get; set; }
        public string paymentDate { get; set; }
        public clsBillPost()
        {

        }
    }
}




