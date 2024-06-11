using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S3GDALayer.Constants
{
    public class ClsPubDALConstants
    {
        #region Stored Procedure Names

        public const string SPGETLOBDETAILS = "S3G_RPT_GETLOBDETAILS";

        //public const string SPGETLOBDETAILS = "S3G_RPT_GETLOBDETAILS";

        public const string SPGETREPAYLOBDETAILS = "S3G_RPT_GetRepayLobDetails"; //Repayment

        public const string SPGETBRANCHDETAILS = "S3G_RPT_GET_BranchDetails";   //Repayment

        public const string SPGETREGBRANCHDETAILS = "S3G_RPT_BranchDetails"; //Based on region

        public const string SPRPTLANOL = "S3G_RPT_LANOL";       // LEASE ASSET NUMBER REGISTER REPORT

        public const string SPGETPACSUBAC = "S3G_RPT_Getpacsubac";  // LEASE ASSET NUMBER  SUB and PRIME AC NO

        public const string SPGETLANDESCRIPTIONDETAILS = "S3G_RPT_GetLANDescriptiondetails";//LEASE ASSET NUMBER REGISTER REPORT

        public const string SPGETLANDETAILS = "S3G_RPT_GetLANdetails";//LAN REGISTER DETAILS  

        public const string SPGETENQUIRYPERFORMANCEDETAILS = "S3G_RPT_GetEnquiryPerformanceDetails";//Enquiry Performance

        public const string SPGETENQUIRYRECEIVEDDETAILS = "S3G_RPT_GetEnquiryReceivedDetails";//Enquiry Performance

        public const string SPGETENQUIRYREJECTEDDETAILS = "S3G_RPT_GetEnquiryRejectedDetails";//Enquiry Performance

        public const string SPGETENQUIRYSUCCESSFULDETAILS = "S3G_RPT_GetEnquirySuccessfulDetails";//Enquiry Performance

        public const string SPGETENQUIRYFOLLOWUPDETAILS = "S3G_RPT_GetEnquiryFollowupDetails";//Enquiry Performance


        public const string SPGETREGION = "S3G_RPT_GetSanctionRegionDetails"; //Disbursement and SanctionDetails

        public const string SPGETPRODUCTDETAILS = "S3G_RPT_GetProductDetails";  //Repayment

        public const string SPGETPRODUCTDETAIL = "S3G_RPT_GetProductDetail";   //Credit Score Transaction

        public const string SPGETPASADETAILS = "S3G_RPT_GETPASADETAILS";

        public const string SPGETPANUMDETAILS = "S3G_RPT_GetPLASLA_AIE";        //Repayment

        public const string SPGETSANUMDETAILS = "S3G_RPT_GetPLASLA_AIE";        //Repayment

        public const string SPGETHEADERLOBBRANCHPRODUCTDETAILS = "S3G_RPT_GetHeaderLobBranchProductDetails";    //Repayment

        public const string SPGETASSETDETAILS = "S3G_RPT_GetAssetDetails";  //Repayment

        public const string SPGETREPAYMENTDETAILS = "S3G_RPT_GetRepaymentStructure";    //Repayment

        public const string SPGETCREDITSCOREDETAILS = "S3G_RPT_GetCreditScoreDetails";  //Credit Score Transaction

        public const string SPGETACCOUNTSDETAILS = "S3G_RPT_GetAccountsDetails";        //Credit Score Transaction

        public const string SPGETCUSTOMERSDETAILS = "S3G_RPT_GetCustomersDetails";      //Credit Score Transaction

        public const string SPGETTRANSACTIONDETAILS = "S3G_RPT_GetTransactionDetails";//Statement Of Account

        public const string SPGETSOAASSETDETAILS = "S3G_RPT_GetAsset";

        public const string SPGETSUMMARYDETAILS = "S3G_RPT_GetSummaryDetails";

        public const string SPGETMEMORANDUMDETAILS = "S3G_RPT_GetMemorandumDetails";

        public const string SPGETSANCTIONDETAILS = "S3G_RPT_GetSanctionDetails";    //Sanction Details

        public const string SPGETPDCNUMBER = "S3G_RPT_GetPDCNumber";        //PDC Acknowledgement 

        public const string SPGETPDCDETAILS = "S3G_RPT_GetPDCDetailsGrid";  //PDC Acknowledgement 

        public const string SPGETPANDETAILS = "S3G_RPT_GetPLASLA_Credit"; //PDC Acknowledgement 

        public const string SPGETSANDETAILS = "S3G_RPT_GetPLASLA_Credit"; //PDC Acknowledgement

        public const string SPGETDISBURSEMENTDETAILS = "S3G_RPT_GETDISBURSEMENTDETAILS";

        public const string SPGETFREQUENCY = "S3G_RPT_GETFREQUENCY";

        public const string SPGETASSETCATEGORIES = "S3G_RPT_GETASSETCATEGORIES";

        public const string SPGETGROUP = "S3G_RPT_GETCUSTOMERGROUP"; // Demand Collection Customer Level

        public const string SPGETDEMANDPAN = "S3G_RPT_GETDEMANDPAN"; //Demand Collection Customer Level

        public const string SPGETCUSTOMERGROUPPAN = "S3G_RPT_GetCustomerGroupPAN"; //Demand Collection Customer Level

        public const string SPGETDEMANDSAN = "S3G_RPT_GetDemandSAN"; //Demand Collection Customer Level

        public const string SPGETDEMANDCOLLECTION = "S3G_RPT_DEMANDCOLLECTION";

        public const string SPGETASSETWISEDEMANDCOLLECTION = "S3G_RPT_ASSETWISEDEMANDCOLLECTIONDETAILS";

        public const string SPGETDCCL = "S3G_RPT_DCCL";   //Demand Collection Customer Level

        public const string SPGETDCREGIONCCL = "S3G_RPT_DCREGIONCCLDETAILSNEW";

        public const string SPGETASSETCATEGORIESTYPE = "S3G_RPT_GETASSETCATEGORIESTYPE";

        public const string SPGETBUCKETCOUNT = "S3G_RPT_BUCKETGENERATION";

        public const string SPGETCOLLECTION = "S3G_RPT_CollectionPrecise"; //Collection Report

        public const string SPGETCOLLECTIONDETAILED = "S3G_RPT_CollectionDetailedReport"; //Collection Report

        public const string SPGETCOLLECTIONREPORT = "S3G_RPT_COLLECTIONREPORT"; //Collection Report

        public const string SPGETSTOCKRECEIVABLES = "S3G_RPT_STOCKRECEIVABLEDETAILSNEW"; //Stock and Receivables Report

        #region branchperformance

        public const string SPGETCOLLECTIONDETAILS = "S3G_RPT_GETCOLLECTION";//Branch Performance

        public const string SPGETBRANCHSTOCKDETAILS = "S3G_RPT_GETSTOCKDETAILS";//Branch Performance

        public const string SPGETBRANCHPAYMENTDETAILS = "S3G_RPT_PAYMENT";//Branch Performance

        public const string SPGETBRANCHNPADETAILS = "S3G_RPT_BRANCHNPADETAILS";//Branch Performance

        public const string SPGETBRANCHNPAOPENINGACCOUNTDETAILS = "S3G_RPT_GETOPENINGACCOUNT";

        public const string SPGETBRANCHNPAADDITIONACCOUNTDETAILS = "S3G_RPT_GETADDITIONACCOUNTS";

        public const string SPGETBRANCHNPADELETIONACCOUNTDETAILS = "S3G_RPT_GETDELETIONACCOUNTS";

        public const string SPGETBRANCHNPACLOSINGACCOUNTDETAILS = "S3G_RPT_GETCLOSINGACCOUNTS";

        public const string SPGETBRANCHCUMULATIVECOLLECTIONDETAILS = "S3G_RPT_GETCUMULATIVECOLLECTION";

        public const string SPGETBRANCHNOOFACCOUNTSDETAILS = "S3G_RPT_GETNOOFACCOUNTS";

        public const string SPGETREGIONBRANCH = "S3G_RPT_GETREGIONBRANCH";
        

        #endregion

        # region DemandStatement
        public const string SPGETCATEGORYDETAILS = "S3G_RPT_GETCATEGORY";

        public const string SPGETDEMANDSTATEMENTDETAILS = "S3G_RPT_GETDEMANDSTATEMENT";

        public const string SPGETDEBTCOLLECTORCODEDETAILS = "S3G_RPT_GETDEBTCOLLECTORRULECARD";

        #endregion

        #region DPD Report

        public const string SPGETDPDREPORT = "S3G_RPT_DPD_REPORT";

        #endregion

        #region Customer At A Glance

        public const string SPGETCUSTOMERGLANCEPRODUCTDETAILS = "S3G_RPT_GetCustomerGlanceProductDetails";

        public const string SPGETREGIONDETAILS = "S3G_RPT_GetRegionDetails";

        public const string SPGETBRANCHDETAILSBYREGION = "S3G_RPT_GetBranchDetails";

        public const string SPGETCUSTOMERATGLANCEDETAILS = "S3G_RPT_GETCUSTOMERATGLANCEDETAILSNew";

        public const string SPGETCUSTOMERGLANCEASSETDETAILS = "S3G_RPT_GetCustomerAtaGlanceAssetDetails";

        #endregion


        public const string SPGETLEVELDETAILS = "S3G_RPT_LEVEL";//COLLECTION Performance

        public const string SPGETLEVELVALUEDETAILS = "S3G_RPT_LEVELVALUE";//COLLECTION Performance

        public const string SPGETASSETTYPEVALUEDETAILS = "S3G_RPT_ASSETTYPEVALUE";//COLLECTION Performance

        public const string SPGETASSETTYPEDETAILS = "S3G_RPT_ASSETTYPE";//COLLECTION Performance

        public const string SPGETCOLLECTIONAMOUNT = "S3G_RPT_COLLECTIONANDCOMPAMOUNT";

        public const string SPGETCOMPARITIVEANALYSIS = "S3G_RPT_COMPARATIVEAMOUNT";






        #endregion

        #region Stored Procedure Parameter Names

        public const string INPUTPARAMMODE = "@MODE";

        public const string INPUTPARAMCOMPANYID = "@COMPANY_ID";

        public const string INPUTPARAMUSERID = "@USER_ID";

        public const string INPUTPARAMLOCATIONID1 = "@LOCATION_ID1";

        public const string INPUTPARAMLOCATIONID2 = "@LOCATION_ID2";

        public const string INPUTPARAMLOCATIONCODE = "@LOCATION_CODE";

        public const string INPUTPARAMLOCATION = "@LOCATION";

        public const string INPUTPARAMLOBID = "@LOB_ID";

        public const string INPUTPARAMPROGRAMID = "@PROGRAM_ID";

        public const string INPUTPARAMBUCKETCOUNT = "@BUCKET";

        public const string INPUTPARAMBRANCHID = "@BRANCH_ID";

        public const string INPUTPARAMPRODUCTID = "@PRODUCTID";

        public const string INPUTPARAMSTARTDATE = "@STARTDATE";

        public const string INPUTPARAMENDDATE = "@ENDDATE";

        public const string INPUTPARAMISACTIVE = "@IS_ACTIVE";

        public const string INPUTPARAMISACTIVATED = "@IS_ACTIVATED";

        public const string INPUTPARAMCHECKACCESS = "@CHECK_ACCESS";

        public const string INPUTPARAMCUSTOMERID = "@CUSTOMER_ID";

        public const string INPUTPARAMPANUM = "@PANUM";

        public const string INPUTPARAMSANUM = "@SANUM";

        public const string INPUTPARAMTYPE = "@TYPE";

        public const string INPUTPARAMPRIMEACCOUNTNO = "@PRIMEACCOUNTNO";//Statement Of Account

        public const string INPUTPARAMSUBACCOUNTNO = "@SUBACCOUNTNO";//Statement Of Account

        public const string XMLACCOUNTDETAILS = "@XMLACCOUNTDETAILS";

        public const string INPUTPARAMISSUMMARY = "@IS_SUMMARY";

        public const string INPUTPARAMDENOMINATION = "@DENOMINATION";

        public const string INPUTPARAMPDCNUMBER = "@PDC_NO";  //PDC Acknowledgement

        public const string INPUTPARAMGROUPID = "@GROUP_ID";

        public const string INPUTPARAMROLECODEID = "@ROLECODEID";

        //LEASE ASSET NUMBER REGISTER

        public const string INPUTPARAMLANNO = "@LEASEASSETNUMBER";

        public const string INPUTPARAMLANNOFROM = "@LANFROM";

        public const string INPUTPARAMLANNOTO = "@LANTO";

        public const string INPUTPARAMLAN = "@LAN";

        //Enquiry Performance

        public const string INPUTPARAMPRODID = "@PRODUCT_ID";

        public const string INPUTPARAMLOCATION_ID = "@LOCATIONID";

        //Customer At A Glance
        public const string INPUTPARAMREGIONID = "@REGION_ID";

        public const string INPUTPARAMSTATUS = "@STATUS";


        public const string INPUTPARAMFREQUENCYID = "@FREQUENCY_ID";

        public const string INPUTPARAMASSETTYPEID = "@ASSETTYPE_ID";

        public const string INPUTPARAMOPTION = "@OPTION"; //Demand Collection Customer Level

        public const string INPUTPARAMVALUE = "@VALUE";// Demand Collection Customer Level

        public const string INPUTPARAMFROMMONTH = "@FROMMONTH";

        public const string INPUTPARAMTOMONTH = "@TOMONTH";

        public const string INPUTPARAMPREFROMMONTH = "@PRE_FROMMONTH";

        public const string INPUTPARAMPRETOMONTH = "@PRE_TOMONTH";

        public const string INPUTPARAMCLASSID = "@CLASS_ID";

        public const string INPUTPARAMGROUPINGTYPE = "@GROUPINGCRITERIAID";//DEMAND COLLECTION REGION CUSTOMER CODE LEVEL

        public const string INPUTPARAMREFERENCEID = "@REFERENCE_ID";

        public const string INPUTPARAMINDUSTRYID = "@INDUSTRY_ID";

        public const string INPUTPARAMCUSTID = "@CUST_ID";

        //Demand Collection parameters
        public const string INPUTPARAMFINYEARSTARTMONTHSTARTDATE = "@FINYR_STARTMNTH_STARTDT";

        public const string INPUTPARAMFROMMONTHSTARTDATE = "@FROMMONTH_STARTDATE";

        public const string INPUTPARAMFROMMONTHPREMONTHENDDATE = "@FMMNTH_PREMNTH_ENDDT";

        //public const string INPUTPARAMTOMONTHENDDATE = "@TOMONTH_ENDDATE";

        public const string INPUTPARAMCOMPAREFINYEARSTARTMONTHSTARTDATE = "@COMPARE_FY_STRMNTH_STRDT";

        public const string INPUTPARAMCOMPAREFROMMONTHSTARTDATE = "@CMP_FROMMNTH_STARTDT";

        public const string INPUTPARAMCOMPAREFROMMONTHPREMONTHENDDATE = "@COMPARE_FMMNTH_PREMNTH_ENDDT";

        public const string INPUTPARAMCOMPARETOMONTHENDDATE = "@COMPARE_TOMONTH_ENDDATE";

        //BranchPerformance
        public const string INPUTPARAMCUTOFFYEAR = "@CUTOFFYEAR";

        public const string INPUTPARAMCUTOFFMONTH = "@CUTOFFMONTH";

        public const string INPUTPARAMFINANCIALYEARFROM = "@FINANCIAL_YEAR_FROM";

        public const string INPUTPARAMFINANCIALYEARSTARTDATE = "@FYSTARTDATE";

        public const string INPUTPARAMCUTOFFMONTHSTARTDATE = "@CMSTARTDATE";

        public const string INPUTPARAMCUTOFFMONTHENDDATE = "@CMENDDATE";

        public const string INPUTPARAMCUTOFFPREVIOUSMONTH = "@CUTOFFPREVIOUSMONTH";

        public const string INPUTPARAMFINANCIALMONTH = "@FINANCIAL_MONTH";

        //public const string INPUTPARAMCUTOFFYEAR = "@CUTOFFYEAR";

        //Payment

        public const string INPUTPARAMTOMONTHSTARTDATE = "@TOMONTH_STARTDATE";

        public const string INPUTPARAMTOMONTHENDDATE = "@TOMONTH_ENDDATE";

        public const string INPUTPARAMTOMONTHPREVIOUSMONTHSTARTDATE = "@TOMNTH_PRE_MNTH_STARTDT";

        public const string INPUTPARAMTOMONTHPREVIOUSMONTHENDDATE = "@TOMONTH_PRE_MNTH_ENDDT";

        public const string INPUTPARAMFORMMONTHSTARTDATE = "@FORMMONTH_STARTDATE";

        public const string INPUTPARAMFROMMONTHPREVIOUSYEARSTARTDATE = "@FMMNTH_PRE_YR_STARTDT";

        public const string INPUTPARAMTOMONTHPREVIOUSYEARENDDATE = "@TOMNTH_PRE_YR_ENDDT";

        public const string INPUTPARAMOPENINGMONTH = "@OPENING_MONTH";

        //DEMAND STATEMENT

        public const string INPUTPARAMDEBTCOLLECTORTYPE = "@DEBTCOLLECTOR_CODE";

        public const string INPUTPARAMCATEGORYCODE = "@CATEGORY_CODE";

        //public const string INPUTPARAMOPTION = "@Option";

        public const string INPUTPARAMDEMANDMONTH = "@DEMANDMONTH";

        public const string INPUTPARAMISDETAILED = "@ISDETAILED";


        #region DPD REPORT

        public const string INPUTPARAMBUCKETPARAM = "@BUCKETPARAM";

        public const string INPUTPARAMCUTOFF = "@CUTOFF";

        public const string INPUTPARAMLOB = "@LOB";

        public const string INPUTPARAMCOMPANY = "@COMPANY_ID";

        public const string INPUTPARAMRCPTUPTO = "@RCPT_UPTO";

        public const string INPUTPARAMBRANCH = "@BRANCH_ID";

        public const string INPUTPARAMDENOM = "@DENOM";

        public const string INPUTPARAMENTITYCODE = "@ENTITY_CODE";

        #endregion
        public const string INPUTPARAMLOCATIONID = "@Location_ID";

        public const string INPUTPARAMHIERARCHYTYPE = "@Hierarchy_Type";
        public const string INPUTPARAMLEVEL = "@LEVEL";
        public const string INPUTPARAMLEVELVALUE = "@LEVEL_VALUE";
        public const string INPUTPARAMASSETTYPE = "@ASSET_TYPE";
        public const string INPUTPARAMASSETCATID = "@ASSET_CAT_ID";
        public const string INPUTPARAMNORMALFROMEDATE = "@FROM_DATE";
        public const string INPUTPARAMNORMALTOEDATE = "@TO_DATE";
        public const string INPUTPARAMCOMPFROMEDATE = "@FROM_COMDATE";
        public const string INPUTPARAMCOMPTOEDATE = "@TO_COMDATE";
        //  public const string INPUTPARAMASSETTYPEID = "@AssetType_ID";


        //Journal Query
        public const string INPUTPARAMLEASEASSETNO = "@LEASE_ASSET_NO";
        public const string INPUTPARAMGLCODE = "@GL_CODE";
        
        //Collateral Report
        public const string INPUTPARAMCOLLATERALTYPEID = "@COLLATERALTYPEID";
        public const string INPUTPARAMCOLLATERALSTATUSID = "@COLLATERALSTATUSID";


        public const string INPUTPARAMDATE = "@DATE";

        #endregion

        // General
        #region Stored Procedure Column Names

        public const string LOBID = "LOB_ID";

        public const string PERIOD = "PERIOD";

        public const string LOBNAME = "LOB_NAME";

        public const string BRANCHID = "BRANCH_ID";

        public const string LOCATIONID = "LOCATION_ID";

        public const string LOCATIONCODE = "LOCATION_CODE";

        public const string LOCATIONNAME = "LOCATION_NAME";

        public const string LOCATION = "LOCATION";

        public const string CLASSID = "CLASS_ID";

        public const string BRANCH = "BRANCH";

        public const string PRODUCTID = "PRODUCT_ID";

        public const string PRODUCTS = "PRODUCT";

        public const string PRODUCTNAME = "PRODUCTNAME";

        public const string LOCATION_ID = "LOCATIONID";


        public const string PANUM = "PANUM";

        public const string SANUM = "SANUM";

        public const string PDCNO = "PDC_NO";

        public const string PDCDATE = "PDC_DATE";

        public const string CUSTOMERID = "CUSTOMER_ID";

        public const string GROUPID = "GROUP_ID";

        public const string GROUPNAME = "GROUP_NAME";

        public const string NAMEOFCUSTOMER = "CUSTOMER_NAME";

        // Asset Details Grid in Repayment

        public const string ASSETDETAILS = "ASSETDETAILS";

        public const string SLREGNO = "SLREGNO";

        public const string AMOUNTFINANCED = "AMOUNTFINANCED";

        public const string IRR = "IRR";

        public const string TERMS = "TERMS";

        public const string POLICYNO = "POLICYNO";

        public const string VALIDUPTO = "VALIDUPTO";

        public const string INSURER = "INSURER";

        public const string POLICYAMOUNT = "POLICYAMOUNT";


        // Sanction Details Grid

        public const string APPLICATIONPROCESSID = "APPLICATION_PROCESS_ID";

        public const string REGION = "REGION";

        public const string APPLICATIONNO = "APPLICATIONNO";

        public const string CUSTOMERCODE = "CUSTOMERCODE";

        public const string FINANCEAMOUNTSOUGHT = "FINANCEAMOUNTSOUGHT";

        public const string FINANCEAMOUNTOFFERED = "FINANCEAMOUNTOFFERED";

        public const string DISBURSABLEAMOUNT = "DISBURSABLE_AMOUNT";

        public const string DISBURSABLE_DATE = "DISBURSABLE_DATE";

        public const string DISBURSEDNO = "DISBURSED_NO";

        public const string DISBURSEDDATE = "DISBURSED_DATE";

        public const string DISBURSEDAMT = "DISBURSED_AMOUNT";

        public const string TOTALFINANCEAMOUNTSOUGHT = "FINANCEAMOUNTSOUGHT";

        public const string TOTALFINANCEAMOUNTOFFERED = "FINANCEAMOUNTOFFERED";

        public const string TOTALDISBURSABLEAMOUNT = "DISBURSABLE_AMOUNT";

        public const string TOTALDISBURSEDAMT = "DISBURSED_AMOUNT";

        //Statement Of Accounts

        public const string PRIMEACCOUNTNUMBER = "PRIMEACCOUNTNO";

        public const string SUMMARYACCOUNT = "SUMMARYACCOUNT";

        public const string SUBACCOUNTNUMBER = "SUBACCOUNTNO";

        public const string YETTOBEBILLED = "YETTOBEBILLED";

        public const string BILLED = "BILLED";

        public const string BALANCE = "BALANCE";

        public const string SOABALANCE = "SOABALANCE";

        public const string SOABAL = "SOABAL";

        public const string DOCUMENTDATE = "DOCUMENTDATE";

        public const string DOCUMENTNUMBER = "DOCUMENTNO";

        public const string VALUEDATE = "VALUEDATE";

        public const string DOCUMENTREFERENCE = "DOCUMENTREFERENCE";

        public const string DUES = "DUES";

        public const string RECEIPTS = "RECEIPTS";

        public const string TRANBALANCE = "TRANBALANCE";

        public const string NARRATION = "NARRATION";

        public const string DESCRIPTION = "DESCRIPTION";

        public const string INSTALLMENTDUE = "INSTALLMENTDUE";

        public const string INTERESTDUE = "INTERESTDUE";

        public const string INSURANCEDUE = "INSURANCEDUE";

        public const string ODIDUE = "ODIDUE";

        public const string CUMU = "Cumm";

        public const string CA_Exists = "CA_Exists";

        public const string OTHERDUE = "OTHERDUE";

        public const string DOCUMENTCHARGESDUE = "DOCUMENTCHARGESDUE";

        public const string CHEQUERETURNDUE = "CHEQUERETURNDUE";

        public const string VERIFICATIONCHARGESDUE = "VERIFICATIONCHARGESDUE";

        public const string LEGALCHARGESDUE = "LEGALCHARGESDUE";

        public const string REPOSSESSIONDUE = "REPOSSESSIONDUE";

        public const string OTHERDUES = "OTHERDUES";

        public const string GlACCOUNT = "GlACCOUNT";

        public const string LAN = "LAN";


        //DisbursementReport


        public const string APPROVEDAMOUNT = "APPROVED_AMOUNT";

        public const string REMAININGAMOUNT = "REMAINING_AMOUNT";

        public const string ACCOUNTYETAMOUNT = "ACCOUNT_YET_AMOUNT";

        public const string PAIDAMOUNT = "PAID_AMOUNT";

        public const string AGEING0DAYS = "AGEING0DAYS";

        public const string AGEING30DAYS = "AGEING30DAYS";

        public const string AGEING60DAYS = "AGEING60DAYS";


        // Repayment Details Grid in Repayment

        public const string INSTALLMENTNO = "INSTALLMENTNO";

        public const string INSTALLMENTDATE = "INSTALLMENTDATE";

        public const string INSTALLMENTAMOUNT = "INSTALLMENTAMOUNT";

        public const string PRINCIPALAMOUNT = "PRINCIPALAMOUNT";

        public const string FINANCECHARGES = "FINANCECHARGES";

        public const string UMFC = "UMFC";

        public const string INSURANCEAMOUNT = "INSURANCEAMOUNT";

        public const string OTHERS = "OTHERS";

        public const string VATRECOVERY = "VATRECOVERY";

        public const string TAXSETOFF = "TAXSETOFF";

        public const string TAX = "TAX";

        //Credit Score Details Grid in Credit Score Transaction

        public const string PRODUCT = "PRODUCT_NAME";

        public const string EQCTYPE = "EQCTYPE";

        public const string NOOFACCOUNTS = "NOOFACCOUNTS";

        public const string REQUIREDSCORE = "REQUIREDSCORE";

        public const string ACTUALSCORE = "ACTUALSCORE";

        public const string HYGIENE = "HYGIENE";

        public const string ACCEPTED = "ACCEPTED";

        public const string REJECTED = "REJECTED";

        //Customer Details Grid in Credit Score Transaction

        public const string CUSTOMERNAMEADDRESS = "CUSTOMER_NAME";

        //public const string ADDRESS = "CUSTOMER_ADDRESS";

        public const string LOANBORROWED = "FINANCE_AMOUNT";

        public const string ENQUIRYDATE = "ENQUIRY_DATE";

        public const string APPLICATIONDATE = "APPLICATION_DATE";

        public const string CONSTITUTION = "CONSTITUTION";

        public const string LOANREQUESTED = "FIN_AMOUNT";

        //Customer At A Glance

        public const string REGIONID = "REGION_ID";

        public const string REGIONNAME = "REGION_NAME";

        public const string BRANCHNAME = "BRANCH_NAME";

        public const string CUSTOMERNAME = "CUSTOMER_NAME";

        public const string STATUS = "STATUS";

        public const string DISBURSEDAMOUNT = "DISBURSEDAMOUNT";

        public const string APPLIEDAMOUNT = "APPLIEDAMOUNT";

        public const string SANCTIONEDAMOUNT = "SANCTIONEDAMOUNT";

        public const string GROSSEXPOSURE = "GROSSEXPOSURE";

        public const string NETEXPOSURE = "NETEXPOSURE";

        public const string MEMODUE = "MEMO_DUE";

        public const string COLLECTED = "COLLECTED";

        public const string COLLATERALVALUE = "COLLATERALVALUE";

        public const string AVERAGEDUEDATES = "AVERAGE_DUE_DATES";

        public const string PENDING = "Pending";

        //PDC Details Grid in PDC Acknowledgement

        public const string PDCSNO = "PDC_SNO";

        public const string CHEQUENUMBER = "CHEQUE_NUMBER";

        public const string CHEQUEDATE = "CHEQUE_DATE";

        public const string DRAWNBANK = "DRAWN_ON_BANK";

        public const string BANKINGDATE = "BANKING_DATE";

        public const string AMOUNT = "AMOUNT";

        //PDC Reminder Report

        public const string LOB = "LOB";

        public const string LASTCOLLECTEDPDCDATE = "LASTCOLLECTEDPDCDATE";

        public const string COMM_ADDRESS1 = "COMM_ADDRESS1";

        public const string COMM_ADDRESS2 = "COMM_ADDRESS2";

        public const string COMM_CITY = "COMM_CITY";

        public const string COMM_STATE = "COMM_STATE";

        public const string COMM_COUNTRY = "COMM_COUNTRY";

        public const string COMM_PINCODE = "COMM_PINCODE";

        public const string COMM_MOBILE = "COMM_MOBILE";

        public const string CUSTOMER_MAIL = "CUSTOMER_MAIL";

        public const string FUTUREPDCDATEFROM = "FUTUREPDCDATEFROM";

        public const string FUTUREPDCDATETO = "FUTUREPDCDATETO";

        public const string COMPANYNAME = "COMPANYNAME";

        public const string REPORTDATE = "REPORTDATE";

        public const string PDCDOCPATHID = "DOCUMENT_PATH_ID";

        public const string PDCDOCPATH = "DOCUMENT_PATH";
        

        // LEASE ASSET NUMBER Register Details

        public const string LANDOCUMENTDATE = "DOCUMENT_DATE";

        public string LANNOFROM = "LANFROM";

        public string LANNOTO = "LANTO";

        public string DATEFROM = "DATEFROM";

        public string DATETO = "DATETO";

        public const string LANDOCUMENTNUMBER = "DOCUMENT_NUMBER";

        public const string LEASEASSETNUMBER = "LEASE_ASSET_NO";

        public const string LANVALUEDATE = "VALUE_DATE";

        public const string DEBIT = "DEBIT";

        public const string CREDIT = "CREDIT";

        public const string LANDESCRIPTION = "DESCRIPTION";

        public const string ASSETCODE = "ASSET_CODE";

        public const string SERIALNO = "SERIAL_NUMBER";

        public const string REGISTRATIONNO = "REGN_NUMBER";

        public const string PERFORMINGSTATUS = "PERFORMINGSTATUS";

        public const string AVAILABILITYSTATUS = "AVAILABILITYSTATUS";
        public const string LANBOOKINGUPTO = "LANBOOKINGUPTO";

        public const string CHASISNUMBER = "CHASIS_NUMBER";

        public const string ASSETMODEL = "ASSETMODEL";

        public const string ENGINENO = "ENGINE_NUMBER";

        public const string ADESCRIPTION = "ASSET_DESCRIPTION";

        //Enquiry Performance

        //public const string PRODUCTS = "PRODUCT";

        public const string RECEIVEDCOUNT = "RECEIVEDCOUNT";

        public const string RECEIVEDVALUE = "RECEIVEDVALUE";

        public const string SUCCESSFULCOUNT = "SUCCESSFULCOUNT";

        public const string SUCCESSFULVALUE = "SUCCESSFULVALUE";

        public const string UNDERFOLLOWUPCOUNT = "UNDERFOLLOWUPCOUNT";

        public const string UNDERFOLLOWUPVALUE = "UNDERFOLLOWUPVALUE";

        public const string REJECTEDCOUNT = "REJECTEDCOUNT";

        public const string REJECTEDVALUE = "REJECTEDVALUE";

        public const string ENQUIRYNUMBER = "ENQUIRYNO";

        public const string ENQUIRYDAT = "ENQUIRYDATE";

        public const string CUSTOMERNAM = "CUSTOMERNAME";

        public const string FACILITYAMOUNT = "FACILITYAMOUNT";

        public const string REMARKS = "REMARKS";

        public const string STAGE = "STAGE";

        public const string PRIMEACCNO = "PRIMEACCNO";

        public const string SUBACCNO = "SUBACCNO";

        public const string ARREARDUE = "ARREAR_DUE";

        public const string CURRENTDUE = "CURRENT_DUE";


        // Demand collecion

        public const string FREQUENCYID = "ID";

        public const string FREQUENCYNAME = "NAME";

        public const string ASSETCLASSID = "CLASSID";

        public const string ASSETCLASS = "ASSETCLASS";

        public const string ASSETMAKEID = "MAKEID";

        public const string ASSETMAKE = "ASSETMAKE";

        public const string ASSETTYPEID = "TYPEID";

        public const string ASSETTYPE = "ASSETTYPE";

        public const string CATEGORIESID = "ID";

        public const string CATEGORIESNAME = "DESCRIPTION";


        //Demand Collection
        public const string FREQUENCY = "FREQUENCY";

        public const string MONTH = "MONTH";

        public const string DEMANDMONTH = "DEMANDMONTH";

        public const string ARREARSAMOUNT = "ARREARSAMOUNT";

        public const string OPENINGDEMAND = "OPENINGDEMAND";

        public const string OPENINGCOLLECTION = "OPENINGCOLLECTION";

        public const string OPENINGPERCENTAGE = "OPENINGPERCENTAGE";

        public const string MONTHLYDEMAND = "MONTHLYDEMAND";

        public const string MONTHLYCOLLECTION = "MONTHLYCOLLECTION";

        public const string MONTHLYPERCENTAGE = "MONTHLYPERCENTAGE";

        public const string CLOSINGDEMAND = "CLOSINGDEMAND";

        public const string CLOSINGCOLLECTION = "CLOSINGCOLLECTION";

        public const string CLOSINGPERCENTAGE = "CLOSINGPERCENTAGE";

        public const string CUSTOMERGROUPNAME = "GROUPCODE";


        //Branch Performance
        public const string FINANCIALYEAR = "FINANCIAL_YEAR";

        public const string LOCATIONDESC = "LOCATION_DESC";

        public const string CURRENTCOLLECTION = "CURRENTCOLLECTION";

        public const string ARREARCOLLECTION = "ARREARCOLLECTION";

        public const string TOTALCOLLECTION = "TOTALCOLLECTION";

        public const string STOCKONHIRE = "STOCKONHIRE";

        public const string ARREARASONCUTOFFMONTH = "ARREARASONCUTOFFMONTH";

        public const string TOTAL = "TOTAL";

        public const string MONTHAMT = "MONTHAMT";

        public const string YEARAMT = "YEARAMT";

        public const string BRANCHASSETCLASS = "ASSET_CLASS";

        public const string UNITMONTH = "UNIT_MONTH";

        public const string UNITYTM = "UNIT_YTM";

        public const string ASSETACCOUNTCLASS = "ASSETCLASS";

        public const string OPENINGACCOUNTS = "OPENINGACCOUNTS";

        public const string OPENINGSTOCK = "OPENINGSTOCK";

        public const string OPENINGARREARS = "OPENINGARREARS";

        public const string ADDITIONACCOUNTS = "ADDITIONACCOUNTS";

        public const string ADDITIONSTOCK = "ADDITIONSTOCK";

        public const string ADDITIONARREARS = "ADDITIONARREARS";

        public const string DELETIONACCOUNTS = "DELETIONACCOUNTS";

        public const string DELETIONSTOCK = "DELETIONSTOCK";

        public const string DELETIONARREARS = "DELETIONARREARS";

        public const string CLOSINGACCOUNTS = "CLOSINGACCOUNTS";

        public const string CLOSINGSTOCK = "CLOSINGSTOCK";

        public const string CLOSINGARREARS = "CLOSINGARREARS";

        public const string NPAPRIMEACCOUNTNUMBER = "PRIMEACCOUNTNUMBER";

        public const string NPASUBACCOUNTNUMBER = "SUBACCOUNTNUMBER";

        public const string CUMULATIVECOLLECTION = "CUMULATIVECOLLECTION";

        public const string STOCK = "STOCK";

        public const string ARREARS = "ARREARS";

        public const string ACCOUNTS = "ACCOUNTS";

        // Payment_DemandCollection_NPA

        public const string GROWTHPERCENTAGELASTMONTH = "GROWTHPERCENTAGELASTMONTH";
        public const string CUMULATIVE = "CUMULATIVE";
        public const string GROWTHPERCENTAGELASTYEAR = "GROWTHPERCENTAGELASTYEAR";
        public const string ARREARDEMAND = "ARREARDEMAND";
        //public const string ARREARCOLLECTION = "ARREARCOLLECTION";
        public const string ARREARCOLLECTIONPERCENTAGE = "ARREARCOLLECTIONPERCENTAGE";
        public const string CURRENTDEMAND = "CURRENTDEMAND";
        //public const string CURRENTCOLLECTION = "CURRENTCOLLECTION";
        public const string CURRENTPERCENTAGE = "CURRENTPERCENTAGE";
        public const string TOTALDEMAND = "TOTALDEMAND";
        //public const string TOTALCOLLECTION = "TOTALCOLLECTION";
        public const string TOTALPERCENTAGE = "TOTALPERCENTAGE";
        public const string BADDEBTS = "BADDEBTS";
        //public const string STOCK = "STOCK";
        public const string NPA = "NPA";
        public const string NPAPERCENTAGE = "NPAPERCENTAGE";
        public const string CLOSINGARREAR = "CLOSINGARREAR";
        public const string ARREARPERCENTAGE = "ARREARPERCENTAGE";


        //Demand Statement

        public const string CATEGORYID = "CATEGORYID";

        public const string CATEGORY = "CATEGORY";

        public const string PRIMEACCOUNT = "PRIMEACCOUNT";

        public const string SUBACCOUNT = "SUBACCOUNT";

        public const string DUESDESCRIPTION = "DUESDESCRIPTION";

        public const string CATEGORYNAME = "CATEGORYNAME";

        public const string BILLDATE = "BILLDATE";

        public const string RECEIPTDATE = "RECEIPTDATE";

        public const string DUEAMOUNT = "DUEAMOUNT";

        public const string DEBTCOLLECTORCODE = "DEBTCOLLECTORCODE";

       // public const string AGEING30DAYS = "AGEING30DAYS";

       // public const string AGEING60DAYS = "AGEING60DAYS";

        public const string AGEING90DAYS = "AGEING90DAYS";

        public const string AGEINGABOVE90DAYS = "AGEINGABOVE90DAYS";

        public const string DEBTCODE = "DEBTCODE";




        //DPD Reports 

        public const string BUCKETCOUNT = "BUCKETCOUNT";

        public const string BUCKETFROM = "BUCKETFROM";

        public const string BUCKETTO = "BUCKETTO";

        public const string BUCKETDEF = "BUCKETDEF";

        public const string CUTOFFDATE = "CUTOFFDATE";

        public const string RCPTUPTO = "RCPTUPTO";

        public const string DENOMINATION = "DENOMINATION";

        //public const string PANUM = "PANUM";

        //public const string SANUM = "SANUM";

        //public const string ARREARS = "ARREARS";

        public const string BUCKET1 = "BUCKET1";

        public const string BUCKET1PRC = "[BUCKET1%]";

        public const string BUCKET2 = "BUCKET2";

        public const string BUCKET2PRC = "[BUCKET2%]";

        public const string BUCKET3 = "BUCKET3";

        public const string BUCKET3PRC = "[BUCKET3%]";

        public const string BUCKET4 = "BUCKET1";

        public const string BUCKET4PRC = "[BUCKET4%]";

        public const string BUCKET5 = "BUCKET5";

        public const string BUCKET5PRC = "[BUCKET5%]";

        public const string BUCKET6 = "BUCKET6";

        public const string BUCKET6PRC = "[BUCKET6%]";

        public const string BUCKET7 = "BUCKET7";

        public const string BUCKET7PRC = "[BUCKET7%]";

        public const string BUCKET8 = "BUCKET8";

        public const string BUCKET8PRC = "[BUCKET8%]";

        public const string BUCKET9 = "BUCKET9";

        public const string BUCKET9PRC = "[BUCKET9%]";

        public const string TOTALPRC = "[TOTAL%]";


        //Collection Report

        public const string TOTALCOLLECTIONAMOUNT = "TOTALCOLLECTIONAMOUNT";

        public const string INTEREST = "INTEREST";

        public const string OVERDUEINTEREST = "OVERDUEINTEREST";

        public const string MEMOCHARGES = "MEMOCHARGES";

        public const string RECEIPTNUMBER = "RECEIPTNUMBER";

        public const string RECEIPTAMOUNT = "RECEIPTAMOUNT";

        //Stock and Receivables Report
        public const string LOB_ID = "LOB_ID";

       // public const string LOCATION_ID = "LOCATION_ID";

        public const string LOCATION_NAME = "LOCATION_NAME";

        public const string CUSTOMER_ID = "CUSTOMER_ID";
        
        public const string CUSTOMERCODENAME = "CUSTOMERCODENAME";

        public const string GROSSSTOCK = "GROSSSTOCK";

        public const string BILLEDUNCOLLECTEDPRINCIPAL = "BILLEDUNCOLLECTEDPRINCIPAL";

        public const string BILLEDUNCOLLECTEDFC = "BILLEDUNCOLLECTEDFC";

        public const string NOOFINSTALLMENT = "NOOFINSTALLMENT";

        public const string NETSTOCK = "NETSTOCK";

        public const string GROUP_ID = "GROUP_ID";

        public const string GROUP_NAME = "GROUP_NAME";

        public const string INDUSTRY_ID = "INDUSTRY_ID";

        public const string INDUSTRY_NAME = "INDUSTRY_NAME";

        public const string REFERENCE_ID = "REFERENCE_ID";

        public const string CUST_ID = "CUST_ID";

        public const string RECLOCATION_ID = "LOCATION_ID";


        //Collection Performance 

        public const string Hierachy = "Hierachy";
        public const string Location_Description = "Location_Description";
        public const string Location_Category_ID = "Location_Category_ID";
        public const string LocationCat_Description = "LocationCat_Description";
        public const string ASSETTYPE_ID = "ASSETTYPE_ID";
        public const string Asset_Description = "Asset_Description";
        public const string Asset_Category_ID = "Asset_Category_ID";
        public const string LOB_NAME = "LOB_NAME";
        public const string Customer_NAME = "Customer_NAME";
        public const string PANum = "PANum";
        public const string SANum = "SANum";
        public const string NOD = "NOD";
        public const string Ageing0to30 = "Ageing0to30";
        public const string Ageing31to60 = "Ageing31to60";
        public const string Ageing61to90 = "Ageing61to90";
        public const string AgeingAbove90 = "AgeingAbove90";
        public const string Chq_Ageing0to30 = "Chq_Ageing0to30";
        public const string Chq_Ageing31to60 = "Chq_Ageing31to60";
        public const string Chq_Ageing61to90 = "Chq_Ageing61to90";
        public const string Chq_AgeingAbove90 = "Chq_AgeingAbove90";

        public const string LevelName = "LEVEL_NAME";
        public const string AssetClass = "ASSET_CLASS";
        public const string AssetType = "ASSET_TYPE";
        public const string ProductName = "Product_Name";
        public const string DueAmount = "due_amount";
        public const string Period1DueAmount = "PERIOD1";
        public const string Period2DueAmount = "PERIOD2";
        public const string P1Amount030 = "Period10to30";
        public const string P1Amount3160 = "Period131to60";
        public const string P1Amount6190 = "Period161t90";
        public const string P1AmountAbove90 = "PAgeingAbove90";
        public const string P2Amount030 = "Period20t30";
        public const string P2Amount3160 = "Period231t60";
        public const string P2Amount6190 = "Period261t90";
        public const string P2AmountAbove90 = "Period2Above90";



        public const string Lan = "LAN";
        public const string AssetDescription = "ASSETDESCRIPTION";
        public const string RegnNumber = "REGNNUMBER";
        public const string ChasisNumber = "CHASISNUMBER";
        public const string EngineNumber = "ENGINENUMBER";
        public const string SerialNumber = "SERIALNUMBER";
        public const string PerformingStatus = "PERFORMINGSTATUS";
        public const string AvailabilityStatus = "AVAILABILITYSTATUS";
        public const string LanBookingUpto = "LANBOOKINGUPTO";


        //ASSET PERFORMANCE
        public const string LOBFILTEROPTION = "@OPTION";

        public const string IRRTYPE = "@IRRType";
        public const string REPORT_TYPE = "@ReportType";
        public const string CURRENT_DATE = "@CurrentDate";
        public const string CURRENT_MONTH_STARTDATE = "@CurrentMonthStartDate";
        public const string CURRENT_YEAR_STARTDATE = "@CurrentYearStartDate";

        public const string RPT_GENTIME = "@RptGenTime";


        public const string PERFASSETCODE = "Asset_Code";
        public const string NewOrOld = "NewOrOld";
        public const string BCD = "BCD";
        public const string BCDAMT = "BCDAmt";
        public const string BDD = "BDD";
        public const string BDDAMT = "BDDAmt";

        public const string BCM = "BCM";
        public const string BCMAMT = "BCMAmt";
        public const string BDM = "BDM";
        public const string BDMAMT = "BDMAmt";

        public const string BCY = "BCY";
        public const string BCYAMT = "BCYAmt";
        public const string BDY = "BDY";
        public const string BDYAMT = "BDYAmt";

        public const string DataRow1 = "DataRow1";
        public const string DataRow2 = "DataRow2";
        public const string ReportType = "ReportType";

        public const string Denomination = "Denomination";
        public const string Gpssuffix = "GPSSuffix";
        //Income Report
        public const string CutoffMonth_Date = "@CutoffMonth_Date";
        public const string FinYearstart = "@FinYearstart";
        public const string CurrentFinYear = "@CurrentFinYear";
        public const string PreviousFinYear = "@PreviousFinYear";
        public const string Year = "YEAR";
        public const string Method = "Method";
        public const string Method_Value = "Method_Value";

        public const string Period = "Period";
        public const string XMLLOB_ID = "@XML_LOBID";

        public const string LOB_ID1 = "@LOB_ID1";
        public const string LOB_ID2 = "@LOB_ID2";
        public const string LOB_ID3 = "@LOB_ID3";
        public const string LOB_ID4 = "@LOB_ID4";

        //Journal Query
        public const string BALDUE = "BALDUE";
        public const string PAID_DATE = "PAID_DATE";
        

        //Collateral Report
        public const string REF_NO = "REF_NO";
        public const string ASSET_DESC = "ASSET_DESC";
        public const string UNITS = "UNITS";
        public const string VALUE = "VALUE";
        public const string CITY = "CITY";
        public const string ADDRESS = "ADDRESS";
        public const string STORAGE1 = "STORAGE1";
        public const string STORAGE2 = "STORAGE2";
        public const string STORAGE3 = "STORAGE3";
        public const string COLLSTATUS = "STATUS";
        public const string STATUS_DATE = "STATUS_DATE";
        public const string COLLATERALTYPEID = "ID";
        public const string COLLATERALTYPENAME = "Description";
        public const string COMPANYADDRESS = "COMPANYADDRESS";
        public const string COLLATERALTYPE = "COLLATERALTYPE";


        // Factoring Maturity Report
        public const string FILNO = "FILNO";
        public const string FILDATE = "FILDATE";
        public const string CREDITDAYS = "CREDITDAYS";
        public const string MATURITYDATE = "MATURITYDATE";
        public const string PRINAMOUNT = "PRINAMOUNT";
        public const string VENDORNAME = "VENDORNAME";
        public const string INVOICENO = "INVOICENO";
        public const string INVOICEAMOUNT = "INVOICEAMOUNT";
        public const string MARGINAMOUNT = "MARGINAMOUNT";
        public const string DISCOUNTDATE = "DISCOUNTDATE";
        public const string CREDITAMOUNT = "Credit_Amount";
        public const string CREDITAVAILABLE = "CreditAvailable";
        public const string DISCOUNTAMOUNT = "DISCOUNT_AMOUNT";
        public const string INTERESTRATE = "INTEREST_RATE";
        public const string RECEIPTNO = "RECEIPT_NO";
        


         
                    
                    
                    
                    
 

        #endregion

    }
}
