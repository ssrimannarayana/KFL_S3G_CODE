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
                DataTable dtUpdateLoanStatus = DLayerAdminService.FunPubFillDropdown("S3G_BBPS_GET_ACCOUNT_INFO_DTL", strProparm);
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

                        funPriUpdateResponse(Request_ID, JsonConvert.SerializeObject(ObjclsLoanQuery), "2", "Failed");
                    }
                    else
                    {
                        ObjclsLoanQuery.Loan_Numer = Loan_Numer.ToString();
                        ObjclsLoanQuery.status = HttpStatusCode.OK.ToString();
                        ObjclsLoanQuery.errorCode = "100";
                        ObjclsLoanQuery.customerName = dtUpdateLoanStatus.Rows[0]["CustomerName"].ToString();
                        ObjclsLoanQuery.amountDue = dtUpdateLoanStatus.Rows[0]["Amount"].ToString();
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
                funPriUpdateResponse(Request_ID, JsonConvert.SerializeObject(ObjclsLoanQuery), "2", "Exception");
                return JsonConvert.SerializeObject(ObjclsLoanQuery);
            }
        }
        private void funPriUpdateResponse(string Request_ID, string ResponseJosn, string Option, string API_Status)
        {
            try
            {
                ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
                Dictionary<string, string> strProparm = new Dictionary<string, string>();
                strProparm.Add("@Option", Option);
                strProparm.Add("@Request_ID", Request_ID.ToString());
                strProparm.Add("@ResponseJosn", ResponseJosn);
                strProparm.Add("@API_Status", API_Status);
                
                DataTable dtUpdateLoanStatus = DLayerAdminService.FunPubFillDropdown("S3G_BBPS_GET_ACCOUNT_INFO_DTL", strProparm);

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
                ClsPubS3GAdmin DLayerAdminService = new ClsPubS3GAdmin();
                Dictionary<string, string> strProparm = new Dictionary<string, string>();
                int i = 0;
                string strErroStatus = string.Empty;
                clsBillPost objclsBillPost = new clsBillPost();
                S3GDALayer.API_Post.clsAPIpost clsAPIpost = new S3GDALayer.API_Post.clsAPIpost();
                i = clsAPIpost.FunPubPostReceiptDetails(1, objclsclsBillPost.Request_ID, objclsclsBillPost.loan_number, objclsclsBillPost.amountPaid, objclsclsBillPost.transactionId, objclsclsBillPost.paymentMode, objclsclsBillPost.paymentDate, out strErroStatus);
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
                objclsclsBillPost.loan_number = objclsclsBillPost.loan_number;
                objclsclsBillPost.status = HttpStatusCode.BadRequest.ToString();
                objclsclsBillPost.errorCode = "101";
                objclsclsBillPost.errorCode_Status = ex.ToString() ;
                funPriUpdateResponse(objclsclsBillPost.Request_ID.ToString(), JsonConvert.SerializeObject(objclsclsBillPost), "3", "Exception");
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
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
        public string additionalInfo { get; set; }

    }
    public class clsLoanBillFetchAdditionalInfo
    {
        public string Pledge_Date { get; set; }
        public string Gross_Weight { get; set; }
        public string Pledge_amount { get; set; }
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
                    lstClsObjclsLoanBillFetchAdditionalInfo.Add(new clsLoanBillFetchAdditionalInfo() { Pledge_Date = dr["Pledge_Date"].ToString(), Gross_Weight = dr["Gross_Weight"].ToString(), Pledge_amount = dr["Pledge_amount"].ToString(), Phone_number = dr["Phone_number"].ToString() });
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




