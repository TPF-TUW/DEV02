using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Data;
using System.Collections;
using DevExpress.XtraGrid;
using System.ComponentModel;
using DevExpress.XtraGrid.Columns;
using System.Collections.Generic;
using System.Linq;

namespace DEV02
{
    public partial class DEV02 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        // Global Var
        goClass.dbConn db = new goClass.dbConn();
        goClass.ctool ct = new goClass.ctool();
        classHardQuery hq = new classHardQuery();
        SqlConnection mainConn = new goClass.dbConn().MDS();

        string global_oidSmpl = string.Empty;
        string global_Marking = string.Empty;

        //private Functionality.Function FUNC = new Functionality.Function();
        public DEV02()
        {
            InitializeComponent();
            UserLookAndFeel.Default.StyleChanged += MyStyleChanged;
            iniConfig = new IniFile("Config.ini");
            UserLookAndFeel.Default.SetSkinStyle(iniConfig.Read("SkinName", "DevExpress"), iniConfig.Read("SkinPalette", "DevExpress"));
        }

        private IniFile iniConfig;

        private void MyStyleChanged(object sender, EventArgs e)
        {
            UserLookAndFeel userLookAndFeel = (UserLookAndFeel)sender;
            LookAndFeelChangedEventArgs lookAndFeelChangedEventArgs = (DevExpress.LookAndFeel.LookAndFeelChangedEventArgs)e;
            //MessageBox.Show("MyStyleChanged: " + lookAndFeelChangedEventArgs.Reason.ToString() + ", " + userLookAndFeel.SkinName + ", " + userLookAndFeel.ActiveSvgPaletteName);
            iniConfig.Write("SkinName", userLookAndFeel.SkinName, "DevExpress");
            iniConfig.Write("SkinPalette", userLookAndFeel.ActiveSvgPaletteName, "DevExpress");
        }

        public class setGrid
        {
            public setGrid() { }
            public setGrid(string fbtype, string size, int aclCm, int qty, int lbodyCm, int lbodyM, int lbodyInc, int lbodyYrd, int weightMg, int weightPcs)
            {
                //Select '' as FBType, '' as Size, '' as [Actual Langth(cm)],'' as [Qty.(Pcs)], '' as [Length/Body(Cm)],'' as [Length/Body(M)],'' as [Length/Body(Inc)],'' as [Length/Body(Yrd)],'' as [Weight/M(g)],'' as [Weight/1Pcs(Kg)]
                FBType = fbtype;
                Size = size;
                ActualLangthCm = aclCm;
                QtyPcs = qty;
                LengthBodyCm = lbodyCm;
                LengthBodyM = lbodyM;
                LengthBodyInc = lbodyInc;
                LengthBodyYrd = lbodyYrd;
                WeightMg = weightMg;
                WeightPcs = weightPcs;
            }
            public string FBType { get; set; }
            public string Size { get; set; }
            public Int32 ActualLangthCm { get; set; }
            public Int32 QtyPcs { get; set; }
            public Int32 LengthBodyCm { get; set; }
            public Int32 LengthBodyM { get; set; }
            public Int32 LengthBodyInc { get; set; }
            public Int32 LengthBodyYrd { get; set; }
            public Int32 WeightMg { get; set; }
            public Int32 WeightPcs { get; set; }
        }

        public BindingList<setGrid> dsListDetail()
        {
            BindingList<setGrid> ds = new BindingList<setGrid>();
            return ds;
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            tabMARKING.SelectedTabPageIndex = 0;
            //LoadData();   //Default Load Form
            //NewData();    //Clear Default Data

            //tabMARKING.SelectedTabPage    = lcgMark;
            //tabMarkDetail.SelectedTabPage = lcgStd;

            //Tab : Marking
            hq.ListOfSample(gcListOfSample); gvListOfSample.Columns["No"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left; gvListOfSample.Columns["No"].Width = 30; gvListOfSample.Columns["No"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            hq.ListOfMarking(gcMARK);
            hq.set_glBranch_Marking(glBranch); glBranch.EditValue = 1;
            hq.set_slSampleRequestNo(slSampleRequestNo);
            hq.set_glSeason(glSeason);
            hq.set_slCustomer(slCustomer);

            //txtMarkNo.Text = "";
            txtMarkingNo.EditValue = db.getMaxID("OIDMARK", "Marking");
            dtDocumentDate.EditValue = DateTime.Now;
            rdoMarkingRequestType.SelectedIndex = 0;

            //glBranch.Text = "";
            slSampleRequestNo.Text = "";
            dteRequestDate.EditValue = DateTime.Now;
            rdoSpecofSize.SelectedIndex = 0;

            glSeason.Text = "";
            slCustomer.Text = "";
            txtRequestBy.Text = "";
            dtDeliveryRequest.EditValue = DateTime.Now;
            rdoUseFor.SelectedIndex = 0;
            mmRemark.Text = "";

            txtItemNo.Text = "";
            txtModelName.Text = "";
            txtCategory.Text = "";
            txtStyleName.Text = "";
            txtSaleSection.Text = "";


            rdoCuttingFac.SelectedIndex = 1;
            rdoSawingFac.SelectedIndex = 1;

            txtCREATE.Text = "0";
            txtCDATE.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtUPDATE.Text = "0";
            txtUDATE.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // -------------------------------------------------------------------------------------------------------------------------------------

            //Tab : Marking Detail

            gcListofFabric.DataSource = null;

            //Detail
            gcSTD.DataSource = dsListDetail(); gvSTD.Columns["FBType"].OptionsColumn.AllowEdit = false; gvSTD.Columns["Size"].OptionsColumn.AllowEdit = false;
            gcPOS.DataSource = dsListDetail(); gvPOS.Columns["FBType"].OptionsColumn.AllowEdit = false; gvPOS.Columns["Size"].OptionsColumn.AllowEdit = false;
            gcNEG.DataSource = dsListDetail(); gvNEG.Columns["FBType"].OptionsColumn.AllowEdit = false; gvNEG.Columns["Size"].OptionsColumn.AllowEdit = false;

            txtPatternNo.Text = "";
            rdoPatternSizeZone.SelectedIndex = 0;
            txtVendFBCode.Text = "";
            txtSize.Text = "";
            txtSampleLotNo.Text = "";
            txtFBType.Text = "";
            gcMDT.DataSource = null;

            txtTotal_Standard.Text = "0.00";
            txtUsable_Standard.Text = "0.00";
            txtWeight_Standard.Text = "0.00";
            //gcSTD.DataSource = null;

            txtTotal_Positive.Text = "0.00";
            txtUsable_Positive.Text = "0.00";
            txtWeight_Positive.Text = "0.00";
            //gcPOS.DataSource = null;

            txtTotal_Negative.Text = "0.00";
            txtUsable_Negative.Text = "0.00";
            txtWeight_Negative.Text = "0.00";
            //gcNEG.DataSource = null;
        }

        private void LoadData()
        {
            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.Append("SELECT Branchs, OIDBranch AS ID FROM Branch ORDER BY ID ");
            //new ObjDevEx.setGridLookUpEdit(glueBranch, sbSQL, "Branchs", "ID").getData();

            //sbSQL.Clear();
            //sbSQL.Append("SELECT SMPLNo AS [SMPL No.], SMPLRevise AS [Revise], ReferenceNo AS [Reference No.], CONVERT(VARCHAR(10), RequestDate) AS [Request Date], Season, SMPLItem AS [SMPL Item], ModelName AS [Model Name], Status, OIDSMPL AS [SMPL ID] ");
            //sbSQL.Append("FROM SMPLRequest ");
            //sbSQL.Append("ORDER BY SMPLNo ");
            //new ObjDevEx.setSearchLookUpEdit(slueRequestNo, sbSQL, "SMPL No.", "SMPL No.").getData();


            //sbSQL.Clear();
            //sbSQL.Append("SELECT SRQ.OIDSMPL AS [SMPL ID], SRQ.Status, SRQ.SMPLNo AS [SMPL No.], SRQ.OIDBranch AS [BranchID], BN.Branch, CONVERT(VARCHAR(10), SRQ.RequestDate) AS [Request Date], ");
            ////sbSQL.Append("       SUBSTRING((SELECT ', ' + SIZEX.SizeName AS[text()] FROM SMPLQuantityRequired AS SQRX INNER JOIN ProductSize AS SIZEX ON SQRX.OIDSIZE = SIZEX.OIDSIZE WHERE(SQRX.OIDSMPL = SRQ.OIDSMPL) ORDER BY SQRX.OIDSMPLDT FOR XML PATH('')), 2, 1000) AS [Spec.of Size], ");
            //sbSQL.Append("       SRQ.SpecificationSize AS [SpecSizeID], CASE WHEN SRQ.SpecificationSize = 0 THEN 'Neccesary' ELSE 'Unneccesary' END AS [Spec.of Size], ");
            //sbSQL.Append("       SRQ.Season, SRQ.OIDCUST AS CustomerID, CUS.ShortName AS Customer, SRQ.UseFor AS UseForID, ");
            //sbSQL.Append("       CASE WHEN SRQ.UseFor = 0 THEN 'Application' ELSE CASE WHEN SRQ.UseFor = 1 THEN 'Take a photograp' ELSE CASE WHEN SRQ.UseFor = 2 THEN 'Monitor' ELSE CASE WHEN SRQ.UseFor = 3 THEN 'SMPL Meeting' ELSE CASE WHEN SRQ.UseFor = 4 THEN 'Each Color' ELSE CASE WHEN SRQ.UseFor = 5 THEN 'Other' ELSE '' END END END END END END AS [Use For], ");
            //sbSQL.Append("       SRQ.OIDCATEGORY AS CategoryID, CAT.CategoryName AS Category, SRQ.OIDSTYLE AS StyleID, PS.StyleName AS Style, ");
            //sbSQL.Append("       SRQ.SMPLItem AS [SMPL Item], SRQ.SMPLPatternNo AS [Pattern No.], ");
            //sbSQL.Append("       SRQ.PatternSizeZone AS PSZID, CASE WHEN SRQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN SRQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN SRQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS [Pattern Size Zone], ");
            //sbSQL.Append("       SRQ.CustApproved AS [Customer Approved], SRQ.ContactName AS [Contact Name], CONVERT(VARCHAR(10), SRQ.DeliveryRequest) AS [Delivery Request], SRQ.ModelName AS [Model Name], SRQ.OIDDEPT, DP.Department AS [Sales Section], SRQ.SMPLRevise AS [Revise] ");
            //sbSQL.Append("FROM   SMPLRequest AS SRQ INNER JOIN ");
            //sbSQL.Append("       Branch AS BN ON SRQ.OIDBranch = BN.OIDBranch INNER JOIN ");
            //sbSQL.Append("       Customer AS CUS ON SRQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
            //sbSQL.Append("       GarmentCategory AS CAT ON SRQ.OIDCATEGORY = CAT.OIDGCATEGORY INNER JOIN ");
            //sbSQL.Append("       ProductStyle AS PS ON SRQ.OIDSTYLE = PS.OIDSTYLE INNER JOIN ");
            //sbSQL.Append("       Department AS DP ON SRQ.OIDDEPT = DP.OIDDepartment ");
            //sbSQL.Append("WHERE (SRQ.Status = 0) ");
            //sbSQL.Append("ORDER BY OIDSMPL ");
            //new ObjDevEx.setGridControl(gcSQ, gvSQ, sbSQL).getDataShowOrder(false, false, false, true);

            //gvSQ.Columns[1].Visible = false; //SMPLID
            //gvSQ.Columns[4].Visible = false; //BranchID
            //gvSQ.Columns[7].Visible = false; //SpecSizeID
            //gvSQ.Columns[10].Visible = false; //CustomerID
            //gvSQ.Columns[12].Visible = false; //UseForID
            //gvSQ.Columns[14].Visible = false; //CategoryID
            //gvSQ.Columns[16].Visible = false; //StyleID
            //gvSQ.Columns[20].Visible = false; //PSZID --> Pattern Size Zone
            //gvSQ.Columns[23].Visible = false; //Contact Name
            //gvSQ.Columns[24].Visible = false; //Delivery Request
            //gvSQ.Columns[25].Visible = false; //Model Name
            //gvSQ.Columns[26].Visible = false; //OIDDEPT
            //gvSQ.Columns[27].Visible = false; //Sales Section
            //gvSQ.Columns[28].Visible = false; //Revise

            //gvSQ.Columns["NO"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            //gvSQ.Columns["Status"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            //gvSQ.Columns["SMPL No."].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //gvSQ.Columns["NO"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gvSQ.Columns["Status"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gvSQ.Columns["Season"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gvSQ.Columns["Request Date"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            //gvSQ.Appearance.HeaderPanel.Options.UseTextOptions = true;
            //gvSQ.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;


            //sbSQL.Clear();
            //sbSQL.Append("SELECT DISTINCT Season FROM SMPLRequest ");
            //new ObjDevEx.setGridLookUpEdit(glueSeason, sbSQL, "Season", "Season").getData();

            //sbSQL.Clear();
            //sbSQL.Append("SELECT DISTINCT Code AS [Customer Code], ShortName AS [Short Name], Name AS [Customer Name], OIDCUST AS [Customer ID] FROM Customer ");
            //new ObjDevEx.setSearchLookUpEdit(slueCustomer, sbSQL, "Short Name", "Customer ID").getData();
            ////slueCustomer

            //sbSQL.Clear();
            //sbSQL.Append("SELECT '' AS [Type], '' AS [Size], '' AS [Actual Length (cm.)], '' AS [Qty. (Pcs)], '' AS [Length/Body (cm.)], '' AS [Length/Body (M)], '' AS [Length/Body (Inch)], '' AS [Length/Body (Yard)], '' AS [Weight/M (g)], '' AS [Weight/1Pcs (kg)] ");
            //new ObjDevEx.setGridControl(gcSTD, gvSTD, sbSQL).getDataShowOrder(false, false, false, true);
            //new ObjDevEx.setGridControl(gcPOS, gvPOS, sbSQL).getDataShowOrder(false, false, false, true);
            //new ObjDevEx.setGridControl(gcNEG, gvNEG, sbSQL).getDataShowOrder(false, false, false, true);

        }

        private void NewData()
        {
            //Marking Tab
            txtMarkingNo.Text = "";
            dtDocumentDate.EditValue = DateTime.Now;
            rdoMarkingRequestType.SelectedIndex = 0;

            glBranch.Text = "";
            slSampleRequestNo.Text = "";
            dteRequestDate.EditValue = DateTime.Now;
            rdoSpecofSize.SelectedIndex = 0;

            glSeason.Text = "";
            slCustomer.Text = "";
            txtRequestBy.Text = "";
            dtDeliveryRequest.EditValue = DateTime.Now;
            rdoUseFor.SelectedIndex = 0;
            mmRemark.Text = "";

            txtItemNo.Text = "";
            txtModelName.Text = "";
            txtCategory.Text = "";
            txtStyleName.Text = "";
            txtSaleSection.Text = "";


            rdoCuttingFac.SelectedIndex = 0;
            rdoSawingFac.SelectedIndex = 0;

            txtCREATE.Text = "0";
            txtCDATE.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtUPDATE.Text = "0";
            txtUDATE.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //Marking Detail Tab
            gcListofFabric.DataSource = null;

            txtPatternNo.Text = "";
            rdoPatternSizeZone.SelectedIndex = 0;
            txtVendFBCode.Text = "";
            txtSize.Text = "";
            txtSampleLotNo.Text = "";
            txtFBType.Text = "";
            gcMDT.DataSource = null;

            txtTotal_Standard.Text = "";
            txtUsable_Standard.Text = "";
            txtWeight_Standard.Text = "";
            gcSTD.DataSource = null;

            txtTotal_Positive.Text = "";
            txtUsable_Positive.Text = "";
            txtWeight_Positive.Text = "";
            gcPOS.DataSource = null;

            txtTotal_Negative.Text = "";
            txtUsable_Negative.Text = "";
            txtWeight_Negative.Text = "";
            gcNEG.DataSource = null;
        }

        public void newMarking()
        {
            //ct.showInfoMessage("new Marking");
            slSampleRequestNo.EditValue = null;
        }

        public void newMarkingDetail()
        {
            ct.showInfoMessage("new MarkingDetail");
        }

        private void bbiNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadData();
            //NewData();

            if (tabMARKING.SelectedTabPageIndex == 0)
            {
                //newMarking
                newMarking();
            }
            else
            {
                // newMarkingDetail
                //newMarkingDetail();
                tabMARKING.SelectedTabPageIndex = 0;
                newMarking();
            }
        }

        private void gvGarment_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {

        }

        private void selectStatus(int value)
        {
            //switch (value)
            //{
            //    case 0:
            //        rgStatus.SelectedIndex = 0;
            //        break;
            //    case 1:
            //        rgStatus.SelectedIndex = 1;
            //        break;
            //    default:
            //        rgStatus.SelectedIndex = -1;
            //        break;
            //}
        }

        private bool chkDuplicate()
        {
            bool chkDup = true;
            //if (txeName.Text != "")
            //{
            //    txeName.Text = txeName.Text.Trim();
            //    if (txeName.Text.Trim() != "" && lblStatus.Text == "* Add Payment Term")
            //    {
            //        StringBuilder sbSQL = new StringBuilder();
            //        sbSQL.Append("SELECT TOP(1) Name FROM PaymentTerm WHERE (Name = N'" + txeName.Text.Trim() + "') ");
            //        if (new DBQuery(sbSQL).getString() != "")
            //        {
            //            FUNC.msgWarning("Duplicate payment term. !! Please Change.");
            //            txeName.Text = "";
            //            chkDup = false;
            //        }
            //    }
            //    else if (txeName.Text.Trim() != "" && lblStatus.Text == "* Edit Payment Term")
            //    {
            //        StringBuilder sbSQL = new StringBuilder();
            //        sbSQL.Append("SELECT TOP(1) OIDPayment ");
            //        sbSQL.Append("FROM PaymentTerm ");
            //        sbSQL.Append("WHERE (Name = N'" + txeName.Text.Trim() + "') ");
            //        string strCHK = new DBQuery(sbSQL).getString();
            //        if (strCHK != "" && strCHK != txeID.Text.Trim())
            //        {
            //            FUNC.msgWarning("Duplicate payment term. !! Please Change.");
            //            txeName.Text = "";
            //            chkDup = false;
            //        }
            //    }
            //}
            return chkDup;
        }

        private void txeName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    txeDescription.Focus();
            //}
        }

        private void txeName_LostFocus(object sender, EventArgs e)
        {
            //txeName.Text = txeName.Text.ToUpper().Trim();
            //bool chkDup = chkDuplicate();
            //if (chkDup == false)
            //{
            //    txeName.Text = "";
            //    txeName.Focus();
            //}
            //else
            //{
            //    txeDescription.Focus();
            //}
        }

        private void txeDescription_KeyDown(object sender, KeyEventArgs e)
        {
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        txeDueDate.Focus();
            //    }
        }

        private void txeDueDate_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    rgStatus.Focus();
            //}
        }

        private void gvPTerm_RowStyle(object sender, RowStyleEventArgs e)
        {

        }

        public void saveMarking()
        {
            if (ct.doConfirm("Save Marking") == true)
            {
                // Not Null
                string smplNo = ct.getVal_sl(slSampleRequestNo);
                string RequestBy = ct.getVal_text(txtRequestBy);
                // && g_oidSmpl != ""

                if (smplNo == "null") { ct.showWarningMessage("Please Select List of Sample Request !"); gcListOfSample.Focus(); return; }
                else if (RequestBy == "null") { ct.showWarningMessage("Please Key Request By !"); txtRequestBy.Focus(); return; }
                else
                {
                    int Status = 0;
                    string MarkingNo = db.getMaxID("OIDMARK", "Marking");
                    string DocumentDate = ct.getDateNow(dtDocumentDate);
                    string MarkingType = rdoMarkingRequestType.SelectedIndex.ToString();
                    string Branch = glBranch.EditValue.ToString();
                    string OIDSMPL = global_oidSmpl;
                    string CuttingFactory = rdoCuttingFac.SelectedIndex.ToString();
                    string SewingFactory = rdoSawingFac.SelectedIndex.ToString();
                    string Remark = (mmRemark.Text.ToString() == "") ? "null" : mmRemark.Text.Trim().ToString().Replace("'", "''");
                    int CreatedBy = 0;
                    string CreatedDate = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    int UpdatedBy = 0;
                    string UpdatedDate = CreatedDate;

                    string sql = "Insert into Marking(Status,MarkingNo,DocumentDate,MarkingType,Branch,OIDSMPL,CuttingFactory,SewingFactory,Remark,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) ";
                    sql += " Values(" + Status + ", " + MarkingNo + ", " + DocumentDate + ", " + MarkingType + ", " + Branch + ", " + OIDSMPL + ", " + CuttingFactory + ", " + SewingFactory + ", " + Remark + ", " + CreatedBy + ", " + CreatedDate + ", " + UpdatedBy + ", " + UpdatedDate + ")";
                    Console.WriteLine(sql);
                    int i = db.Query(sql, mainConn);
                    if (i > 0)
                    {
                        ct.showInfoMessage("Save Success.");
                        //tabMARKING.SelectedTabPageIndex = 1;
                        hq.ListOfMarking(gcMARK);
                    }
                }
            }
        }

        public void saveMarkingDetail()
        {
            if (ct.doConfirm("Save MarkingDetail ?") == true)
            {
                bool saveMKDTStatus = false;
                //chk gvFBPart : ต้องไม่ว่าง
                ArrayList rows = ct.getList_isChecked(gvFBPart);

                // TBL : listOfFabric   >> No,SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size
                // TBL : FBGPart        >> SOIDGParts AS ID, GarmentParts AS FBPart

                //check gvSTD : ต้องไม่ว่าง
                if (gvSTD.RowCount == 0) { ct.showWarningMessage("List of Detail is Empty ! Please Select Row Data !"); gvListofFabric.Focus(); return; }
                else if (rows.Count == 0) { ct.showWarningMessage("Please Select FBPart !"); gvFBPart.Focus(); return; }
                else
                {
                    //ct.showInfoMessage("ok pass.");

                    // Standard
                    if (txtTotal_Standard.Text.ToString().Trim() == "" || Convert.ToDecimal(txtTotal_Standard.Text.ToString()) == 0)
                    {
                        txtTotal_Standard.Focus(); return;
                    }
                    else if (txtUsable_Standard.Text.ToString().Trim() == "" || Convert.ToDecimal(txtUsable_Standard.Text.ToString()) == 0)
                    {
                        txtUsable_Standard.Focus(); return;
                    }
                    else if (txtWeight_Standard.Text.ToString().Trim() == "" || Convert.ToDecimal(txtWeight_Standard.Text.ToString()) == 0)
                    {
                        txtWeight_Standard.Focus(); return;
                    }

                    // Positive
                    else if (txtTotal_Positive.Text.ToString().Trim() == "" || Convert.ToDecimal(txtTotal_Positive.Text.ToString()) == 0)
                    {
                        tabMarkDetail.SelectedTabPageIndex = 1; txtTotal_Positive.Focus(); return;
                    }
                    else if (txtUsable_Positive.Text.ToString().Trim() == "" || Convert.ToDecimal(txtUsable_Positive.Text.ToString()) == 0)
                    {
                        tabMarkDetail.SelectedTabPageIndex = 1; txtUsable_Positive.Focus(); return;
                    }
                    else if (txtWeight_Positive.Text.ToString().Trim() == "" || Convert.ToDecimal(txtWeight_Positive.Text.ToString()) == 0)
                    {
                        tabMarkDetail.SelectedTabPageIndex = 1; txtWeight_Positive.Focus(); return;
                    }

                    // Negative
                    else if (txtTotal_Negative.Text.ToString().Trim() == "" || Convert.ToDecimal(txtTotal_Negative.Text.ToString()) == 0)
                    {
                        tabMarkDetail.SelectedTabPageIndex = 2; txtTotal_Negative.Focus(); return;
                    }
                    else if (txtUsable_Negative.Text.ToString().Trim() == "" || Convert.ToDecimal(txtUsable_Negative.Text.ToString()) == 0)
                    {
                        tabMarkDetail.SelectedTabPageIndex = 2; txtUsable_Negative.Focus(); return;
                    }
                    else if (txtWeight_Negative.Text.ToString().Trim() == "" || Convert.ToDecimal(txtWeight_Negative.Text.ToString()) == 0)
                    {
                        tabMarkDetail.SelectedTabPageIndex = 2; txtWeight_Negative.Focus(); return;
                    }
                    else
                    {
                        ct.showInfoMessage("ok chkPass");
                        //Loop Stuff OIDFBGPart

                        string stuff_FBPart = string.Empty;
                        for (int i = 0; i < rows.Count; i++)
                        {
                            DataRow row = rows[i] as DataRow;
                            stuff_FBPart += row["ID"].ToString() + ",";
                        }
                        string final_stuff_FBPart = stuff_FBPart.Substring(0, stuff_FBPart.Length - 1);
                        Console.WriteLine(final_stuff_FBPart);

                        // Include Variable
                        string OIDMARK = global_Marking;
                        string OIDITEM = "null";
                        string GPartsStuff = "'" + final_stuff_FBPart + "'";
                        string Details = "null";

                        // TBL : STD,POS,NEG : FBType , Size , ActualLangthCm , QtyPcs , LengthBodyCm , LengthBodyM , LengthBodyInc , LengthBodyYrd , WeightMg , WeightPcs

                        // Loop Mixed List : Standard
                        for (int j = 0; j < gvSTD.RowCount; j++)
                        {
                            string OIDSIZE = db.get_oneParameter("Select s.OIDSIZE From ProductSize s inner join SMPLQuantityRequired q on q.OIDSIZE = s.OIDSIZE inner join Marking mark on mark.OIDSMPL = q.OIDSMPL Where SizeName = '" + gvSTD.GetRowCellValue(j, "Size") + "' and OIDMARK = " + OIDMARK + " ", mainConn, "OIDSIZE");
                            string OIDSIZEZONE = rdoPatternSizeZone.SelectedIndex.ToString();
                            string OIDSMPLDTStuff = "'" + db.get_oneParameter("Select q.OIDSMPLDT From ProductSize s inner join SMPLQuantityRequired q on q.OIDSIZE = s.OIDSIZE inner join Marking mark on mark.OIDSMPL = q.OIDSMPL Where SizeName = '" + gvSTD.GetRowCellValue(j, "Size") + "' and OIDMARK = " + OIDMARK + " ", mainConn, "OIDSMPLDT") + "'";
                            string DetailsType = "0";
                            string TotalWidthSTD = txtTotal_Standard.EditValue.ToString();
                            string UsableWidth = txtUsable_Standard.EditValue.ToString();
                            string GM2 = txtWeight_Standard.EditValue.ToString();
                            /*------check this start col index 2-------------*/
                            string PracticalLengthCM = gvSTD.GetRowCellValue(j, "ActualLangthCm").ToString();
                            string QuantityPCS = gvSTD.GetRowCellValue(j, "QtyPcs").ToString();
                            string LengthPer1CM = gvSTD.GetRowCellValue(j, "LengthBodyCm").ToString();
                            string LengthPer1M = gvSTD.GetRowCellValue(j, "LengthBodyM").ToString();
                            string LengthPer1INCH = gvSTD.GetRowCellValue(j, "LengthBodyInc").ToString();
                            string LengthPer1YARD = gvSTD.GetRowCellValue(j, "LengthBodyYrd").ToString();
                            string WeightG = gvSTD.GetRowCellValue(j, "WeightMg").ToString();
                            string WeightKG = gvSTD.GetRowCellValue(j, "WeightPcs").ToString();

                            //if (Convert.ToInt32(PracticalLengthCM) <= 0)
                            //{
                            //    tabMarkDetail.SelectedTabPageIndex = 0; gvSTD.FocusedRowHandle = j; gvSTD.FocusedColumn = gvSTD.VisibleColumns[2];
                            //    gvSTD.SetColumnError(gvSTD.Columns[2], "The value must be greater than Units On Order"); gvSTD.ShowEditor(); return;
                            //}

                            string sql = "Insert Into MarkingDetails(OIDMARK,OIDITEM,OIDSIZE,OIDSIZEZONE,OIDSMPLDTStuff,GPartsStuff,DetailsType,Details,TotalWidthSTD,UsableWidth,GM2,PracticalLengthCM,QuantityPCS,LengthPer1CM,LengthPer1M,LengthPer1INCH,LengthPer1YARD,WeightG,WeightKG) ";
                            sql += " Values(" + OIDMARK + ", " + OIDITEM + ", " + OIDSIZE + ", " + OIDSIZEZONE + ", " + OIDSMPLDTStuff + ", " + GPartsStuff + ", " + DetailsType + ", " + Details + ", " + TotalWidthSTD + ", " + UsableWidth + ", " + GM2 + ", " + PracticalLengthCM + ", " + QuantityPCS + ", " + LengthPer1CM + ", " + LengthPer1M + ", " + LengthPer1INCH + ", " + LengthPer1YARD + ", " + WeightG + ", " + WeightKG + ")";
                            Console.WriteLine(sql);
                            db.Query(sql, mainConn);
                        }

                        // Loop Mixed List : Positive
                        for (int k = 0; k < gvPOS.RowCount; k++)
                        {
                            string OIDSIZE = db.get_oneParameter("Select s.OIDSIZE From ProductSize s inner join SMPLQuantityRequired q on q.OIDSIZE = s.OIDSIZE inner join Marking mark on mark.OIDSMPL = q.OIDSMPL Where SizeName = '" + gvPOS.GetRowCellValue(k, "Size") + "' and OIDMARK = " + OIDMARK + " ", mainConn, "OIDSIZE");
                            string OIDSIZEZONE = rdoPatternSizeZone.SelectedIndex.ToString();
                            string OIDSMPLDTStuff = "'" + db.get_oneParameter("Select q.OIDSMPLDT From ProductSize s inner join SMPLQuantityRequired q on q.OIDSIZE = s.OIDSIZE inner join Marking mark on mark.OIDSMPL = q.OIDSMPL Where SizeName = '" + gvPOS.GetRowCellValue(k, "Size") + "' and OIDMARK = " + OIDMARK + " ", mainConn, "OIDSMPLDT") + "'";
                            string DetailsType = "1";
                            string TotalWidthSTD = txtTotal_Standard.EditValue.ToString();
                            string UsableWidth = txtUsable_Standard.EditValue.ToString();
                            string GM2 = txtWeight_Standard.EditValue.ToString();
                            string PracticalLengthCM = gvPOS.GetRowCellValue(k, "ActualLangthCm").ToString();
                            string QuantityPCS = gvPOS.GetRowCellValue(k, "QtyPcs").ToString();
                            string LengthPer1CM = gvPOS.GetRowCellValue(k, "LengthBodyCm").ToString();
                            string LengthPer1M = gvPOS.GetRowCellValue(k, "LengthBodyM").ToString();
                            string LengthPer1INCH = gvPOS.GetRowCellValue(k, "LengthBodyInc").ToString();
                            string LengthPer1YARD = gvPOS.GetRowCellValue(k, "LengthBodyYrd").ToString();
                            string WeightG = gvPOS.GetRowCellValue(k, "WeightMg").ToString();
                            string WeightKG = gvPOS.GetRowCellValue(k, "WeightPcs").ToString();

                            string sql = "Insert Into MarkingDetails(OIDMARK,OIDITEM,OIDSIZE,OIDSIZEZONE,OIDSMPLDTStuff,GPartsStuff,DetailsType,Details,TotalWidthSTD,UsableWidth,GM2,PracticalLengthCM,QuantityPCS,LengthPer1CM,LengthPer1M,LengthPer1INCH,LengthPer1YARD,WeightG,WeightKG) ";
                            sql += " Values(" + OIDMARK + ", " + OIDITEM + ", " + OIDSIZE + ", " + OIDSIZEZONE + ", " + OIDSMPLDTStuff + ", " + GPartsStuff + ", " + DetailsType + ", " + Details + ", " + TotalWidthSTD + ", " + UsableWidth + ", " + GM2 + ", " + PracticalLengthCM + ", " + QuantityPCS + ", " + LengthPer1CM + ", " + LengthPer1M + ", " + LengthPer1INCH + ", " + LengthPer1YARD + ", " + WeightG + ", " + WeightKG + ")";
                            Console.WriteLine(sql);
                            db.Query(sql, mainConn);
                        }

                        // Loop Mixed List : Negative
                        for (int l = 0; l < gvNEG.RowCount; l++)
                        {
                            string OIDSIZE = db.get_oneParameter("Select s.OIDSIZE From ProductSize s inner join SMPLQuantityRequired q on q.OIDSIZE = s.OIDSIZE inner join Marking mark on mark.OIDSMPL = q.OIDSMPL Where SizeName = '" + gvNEG.GetRowCellValue(l, "Size") + "' and OIDMARK = " + OIDMARK + " ", mainConn, "OIDSIZE");
                            string OIDSIZEZONE = rdoPatternSizeZone.SelectedIndex.ToString();
                            string OIDSMPLDTStuff = "'" + db.get_oneParameter("Select q.OIDSMPLDT From ProductSize s inner join SMPLQuantityRequired q on q.OIDSIZE = s.OIDSIZE inner join Marking mark on mark.OIDSMPL = q.OIDSMPL Where SizeName = '" + gvNEG.GetRowCellValue(l, "Size") + "' and OIDMARK = " + OIDMARK + " ", mainConn, "OIDSMPLDT") + "'";
                            string DetailsType = "2";
                            string TotalWidthSTD = txtTotal_Standard.EditValue.ToString();
                            string UsableWidth = txtUsable_Standard.EditValue.ToString();
                            string GM2 = txtWeight_Standard.EditValue.ToString();
                            string PracticalLengthCM = gvNEG.GetRowCellValue(l, "ActualLangthCm").ToString();
                            string QuantityPCS = gvNEG.GetRowCellValue(l, "QtyPcs").ToString();
                            string LengthPer1CM = gvNEG.GetRowCellValue(l, "LengthBodyCm").ToString();
                            string LengthPer1M = gvNEG.GetRowCellValue(l, "LengthBodyM").ToString();
                            string LengthPer1INCH = gvNEG.GetRowCellValue(l, "LengthBodyInc").ToString();
                            string LengthPer1YARD = gvNEG.GetRowCellValue(l, "LengthBodyYrd").ToString();
                            string WeightG = gvNEG.GetRowCellValue(l, "WeightMg").ToString();
                            string WeightKG = gvNEG.GetRowCellValue(l, "WeightPcs").ToString();

                            string sql = "Insert Into MarkingDetails(OIDMARK,OIDITEM,OIDSIZE,OIDSIZEZONE,OIDSMPLDTStuff,GPartsStuff,DetailsType,Details,TotalWidthSTD,UsableWidth,GM2,PracticalLengthCM,QuantityPCS,LengthPer1CM,LengthPer1M,LengthPer1INCH,LengthPer1YARD,WeightG,WeightKG) ";
                            sql += " Values(" + OIDMARK + ", " + OIDITEM + ", " + OIDSIZE + ", " + OIDSIZEZONE + ", " + OIDSMPLDTStuff + ", " + GPartsStuff + ", " + DetailsType + ", " + Details + ", " + TotalWidthSTD + ", " + UsableWidth + ", " + GM2 + ", " + PracticalLengthCM + ", " + QuantityPCS + ", " + LengthPer1CM + ", " + LengthPer1M + ", " + LengthPer1INCH + ", " + LengthPer1YARD + ", " + WeightG + ", " + WeightKG + ")";
                            Console.WriteLine(sql);

                            int chkI = db.Query(sql, mainConn);
                            if (chkI > 0)
                            {
                                saveMKDTStatus = true;
                            }

                            //saveMKDTStatus = true;
                        }
                    }
                }

                if (saveMKDTStatus == true)
                {
                    ct.showInfoMessage("Save Success.");
                    //RefreshForm
                    refreshMarkingDetail();
                    // ดึง MaringDetail อีกที
                    hq.getListofMaterialDetail(gcMDT, global_Marking);
                }
                else
                {
                    ct.showErrorMessage("Error! Please Contact Administrator !");
                    return;
                }
            }//end if
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int tabIndex = tabMARKING.SelectedTabPageIndex;
            if (tabIndex == 0)
            {
                saveMarking();
            }
            else
            {
                saveMarkingDetail();
            }
        }

        private void bbiExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string pathFile = new ObjSet.Folder(@"C:\MDS\Export\").GetPath() + "PaymentTermList_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //gvPTerm.ExportToXlsx(pathFile);
            //System.Diagnostics.Process.Start(pathFile);
        }

        private void bbiPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gcPTerm.ShowPrintPreview();
        }

        private void bbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gcPTerm.Print();
        }

        private void drop__gvSQ_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //**** MARKING TAB *****
            //txtMarkNo.Text = "";
            //dteDocDate.EditValue = DateTime.Now;
            //rgMRT.SelectedIndex = 0; 
            //mmeRemark.EditValue = "";
            //rgCutting.SelectedIndex = 0;
            //rgSewing.SelectedIndex = 0;
            //gcMARK.DataSource = null;

            //glueBranch.EditValue = "";
            //slueRequestNo.EditValue = "";
            //dteRequestDate.EditValue = DateTime.Now;
            //rgSpecSize.SelectedIndex = 0;
            //glueSeason.EditValue = "";
            //slueCustomer.EditValue = "";
            //txeRequestBy.Text = "";
            //rgUseFor.SelectedIndex = 0;
            //txeItemNo.Text = "";
            //txeModelName.Text = "";
            //txeCategory.Text = "";
            //txeStyle.Text = "";
            //gcQR.DataSource = null;
            //txeSection.Text = "";


            //string SMPLNO = gvSQ.GetFocusedRowCellValue("SMPL No.").ToString();
            //string REVISE = gvSQ.GetFocusedRowCellValue("Revise").ToString();
            //slueRequestNo.EditValue = SMPLNO;


            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.Append("SELECT QR.OIDSMPLDT, QR.OIDSMPL, CASE WHEN RQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN RQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN RQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS Zone, RQ.SMPLPatternNo AS [Pattern No.], PC.ColorNo AS Color, PS.SizeNo AS Size, QR.Quantity ");
            //sbSQL.Append("FROM   SMPLRequest AS RQ INNER JOIN ");
            //sbSQL.Append("       SMPLQuantityRequired AS QR ON RQ.OIDSMPL = QR.OIDSMPL INNER JOIN ");
            //sbSQL.Append("       ProductColor AS PC ON QR.OIDCOLOR = PC.OIDCOLOR INNER JOIN ");
            //sbSQL.Append("       ProductSize AS PS ON QR.OIDSIZE = PS.OIDSIZE ");
            //sbSQL.Append("WHERE (RQ.SMPLNo = N'" + SMPLNO + "') AND(RQ.SMPLRevise = '" + REVISE + "') ");
            //sbSQL.Append("ORDER BY QR.OIDSMPLDT ");
            //new ObjDevEx.setGridControl(gcQR, gvQR, sbSQL).getDataShowOrder(false, false, false, true);

            //gvQR.Columns[1].Visible = false;
            //gvQR.Columns[2].Visible = false;

            //gvQR.Columns["NO"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gvQR.Columns["Quantity"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gvQR.Appearance.HeaderPanel.Options.UseTextOptions = true;
            //gvQR.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            //string SMPLID = gvSQ.GetFocusedRowCellValue("SMPL ID").ToString();
            //sbSQL.Clear();
            //sbSQL.Append("SELECT MK.OIDMARK AS MarkID, MK.MarkingNo AS [Marking No.], RQ.SMPLNo AS [SMPL No.], RQ.Season, CUS.ShortName AS Customer, RQ.SMPLItem AS [SMPL Item], PS.StyleName AS Style, RQ.SMPLPatternNo AS [Pattern No.]  ");
            //sbSQL.Append("FROM   Marking AS MK INNER JOIN ");
            //sbSQL.Append("       SMPLRequest AS RQ ON MK.OIDSMPL = RQ.OIDSMPL INNER JOIN ");
            //sbSQL.Append("       Customer AS CUS ON RQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
            //sbSQL.Append("       ProductStyle AS PS ON RQ.OIDSTYLE = PS.OIDSTYLE ");
            //sbSQL.Append("WHERE (MK.OIDSMPL = '" + SMPLID + "') ");
            //sbSQL.Append("ORDER BY MarkID ");
            //new ObjDevEx.setGridControl(gcMARK, gvMARK, sbSQL).getDataShowOrder(false, false, false, true);

            //gvMARK.Columns[1].Visible = false; //MarkID


            //string RequestDate = gvSQ.GetFocusedRowCellValue("Request Date").ToString();
            //dteRequestDate.EditValue = Convert.ToDateTime(RequestDate);

            //int SpecSizeID = Convert.ToInt32(gvSQ.GetFocusedRowCellValue("SpecSizeID").ToString());
            //rgSpecSize.EditValue = SpecSizeID;

            //int UseForID = Convert.ToInt32(gvSQ.GetFocusedRowCellValue("UseForID").ToString());
            //rgUseFor.EditValue = UseForID;

            //string Season = gvSQ.GetFocusedRowCellValue("Season").ToString();
            //glueSeason.EditValue = Season;

            //string CustomerID = gvSQ.GetFocusedRowCellValue("CustomerID").ToString();
            //slueCustomer.EditValue = CustomerID;

            //string ContactBy = gvSQ.GetFocusedRowCellValue("Contact Name").ToString();
            //txeRequestBy.Text = ContactBy;

            //string DeliveryRequest = gvSQ.GetFocusedRowCellValue("Delivery Request").ToString();
            //dteDeliveryRequest.EditValue = Convert.ToDateTime(DeliveryRequest);

            //string SMPLItem = gvSQ.GetFocusedRowCellValue("SMPL Item").ToString();
            //txeItemNo.Text = SMPLItem;

            //string ModelName = gvSQ.GetFocusedRowCellValue("Model Name").ToString();
            //txeModelName.Text = ModelName;

            //string Category = gvSQ.GetFocusedRowCellValue("Category").ToString();
            //txeCategory.Text = Category;

            //string Style = gvSQ.GetFocusedRowCellValue("Style").ToString();
            //txeStyle.Text = Style;

            //string SalesSection = gvSQ.GetFocusedRowCellValue("Sales Section").ToString();
            //txeSection.Text = SalesSection;

            //sbSQL.Clear();
            //sbSQL.Append("SELECT RFB.OIDSMPLFB AS [REC ID], RFB.VendFBCode AS [Vendor FB Code], RFB.Composition, RFB.FBWeight AS Weight, RFB.OIDCOLOR AS [Color ID], PC.ColorNo, RFB.SMPLotNo AS [Sample Lot No.], RFB.OIDVEND AS [Vendor ID], ");
            //sbSQL.Append("       VD.Code AS[Vendor Code], VD.Name AS[Vendor Name], '' AS[NAV Code] ");
            //sbSQL.Append("FROM   SMPLRequestFabric AS RFB INNER JOIN ");
            //sbSQL.Append("       ProductColor AS PC ON RFB.OIDCOLOR = PC.OIDCOLOR INNER JOIN ");
            //sbSQL.Append("       Vendor AS VD ON RFB.OIDVEND = VD.OIDVEND INNER JOIN ");
            //sbSQL.Append("       SMPLRequest AS RQ ON RFB.OIDSMPLDT = RQ.OIDSMPL ");
            //sbSQL.Append("WHERE (RQ.OIDSMPL = '" + SMPLID + "') ");
            //sbSQL.Append("ORDER BY[REC ID] ");
            //new ObjDevEx.setGridControl(gcFB, gvFB, sbSQL).getDataShowCheckBoxAndOrder(false, false, false, true);




            //sbSQL.Clear();
            //sbSQL.Append("SELECT MD.OIDMARKDT AS [Rec.ID], MD.OIDMARK AS MarkID,  ");
            //sbSQL.Append("       CASE WHEN MD.OIDSIZEZONE = 0 THEN 'Japan' ELSE CASE WHEN MD.OIDSIZEZONE = 1 THEN 'Europe' ELSE CASE WHEN MD.OIDSIZEZONE = 2 THEN 'US' ELSE '' END END END AS Zone, ");
            //sbSQL.Append("       RQ.SMPLPatternNo AS[Pattern No.], GP.GarmentParts AS[Fabric Parts], RFB.FBType AS Type, PS.SizeNo AS[Size No.], PS.SizeName AS Size, MD.TotalWidthSTD AS[Standard Width], MD.UsableWidth AS[Usable Width], ");
            //sbSQL.Append("       MD.GM2 AS[Weight(g / m2)], MD.PracticalLengthCM AS[Actual Length(cm.)], MD.QuantityPCS AS[Quantity(Pcs)], MD.LengthPer1CM AS[Length / Body(cm.)], MD.LengthPer1M AS[Length / Body(M)], ");
            //sbSQL.Append("       MD.LengthPer1INCH AS[Length / Body(Inch)], MD.LengthPer1YARD AS[Length / Body(Yard)], MD.WeightG AS[Weight / M(g)], MD.WeightKG AS[Weight / M(Kg)] ");
            //sbSQL.Append("FROM   MarkingDetails AS MD INNER JOIN ");
            //sbSQL.Append("       Marking AS M ON MD.OIDMARK = M.OIDMARK INNER JOIN ");
            //sbSQL.Append("       SMPLRequest AS RQ ON M.OIDSMPL = RQ.OIDSMPL INNER JOIN ");
            //sbSQL.Append("       GarmentParts AS GP ON MD.OIDGParts = GP.OIDGParts INNER JOIN ");
            //sbSQL.Append("       SMPLRequestFabric AS RFB ON RFB.OIDSMPLDT = RQ.OIDSMPL INNER JOIN ");
            //sbSQL.Append("       ProductSize AS PS ON MD.OIDSIZE = PS.OIDSIZE ");
            //sbSQL.Append("WHERE (MD.OIDMARK = '1') ");
            //sbSQL.Append("ORDER BY[Rec.ID] ");
            //new ObjDevEx.setGridControl(gcMDT, gvMDT, sbSQL).getDataShowOrder(false, false, false, true);

        }

        private void gcSQ_Click(object sender, EventArgs e)
        {

        }

        private void slueRequestNo_EditValueChanged(object sender, EventArgs e)
        {
            //if (slSampleRequestNo.Text.Trim() != "")
            //{
            //    StringBuilder sbSQL = new StringBuilder();
            //    sbSQL.Append("SELECT SRQ.OIDSMPL AS [SMPL ID], SRQ.Status, SRQ.SMPLNo AS [SMPL No.], SRQ.OIDBranch AS [BranchID], BN.Branch, CONVERT(VARCHAR(10), SRQ.RequestDate) AS [Request Date], ");
            //    sbSQL.Append("       SRQ.SpecificationSize AS [SpecSizeID], CASE WHEN SRQ.SpecificationSize = 0 THEN 'Neccesary' ELSE 'Unneccesary' END AS [Spec.of Size], ");
            //    sbSQL.Append("       SRQ.Season, SRQ.OIDCUST AS CustomerID, CUS.ShortName AS Customer, SRQ.UseFor AS UseForID, ");
            //    sbSQL.Append("       CASE WHEN SRQ.UseFor = 0 THEN 'Application' ELSE CASE WHEN SRQ.UseFor = 1 THEN 'Take a photograp' ELSE CASE WHEN SRQ.UseFor = 2 THEN 'Monitor' ELSE CASE WHEN SRQ.UseFor = 3 THEN 'SMPL Meeting' ELSE CASE WHEN SRQ.UseFor = 4 THEN 'Each Color' ELSE CASE WHEN SRQ.UseFor = 5 THEN 'Other' ELSE '' END END END END END END AS [Use For], ");
            //    sbSQL.Append("       SRQ.OIDCATEGORY AS CategoryID, CAT.CategoryName AS Category, SRQ.OIDSTYLE AS StyleID, PS.StyleName AS Style, ");
            //    sbSQL.Append("       SRQ.SMPLItem AS [SMPL Item], SRQ.SMPLPatternNo AS [Pattern No.], ");
            //    sbSQL.Append("       SRQ.PatternSizeZone AS PSZID, CASE WHEN SRQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN SRQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN SRQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS [Pattern Size Zone], ");
            //    sbSQL.Append("       SRQ.CustApproved AS [Customer Approved], SRQ.ContactName AS [Contact Name], CONVERT(VARCHAR(10), SRQ.DeliveryRequest) AS [Delivery Request], SRQ.ModelName AS [Model Name], SRQ.OIDDEPT, DP.Department AS [Sales Section], SRQ.SMPLRevise AS [Revise] ");
            //    sbSQL.Append("FROM   SMPLRequest AS SRQ INNER JOIN ");
            //    sbSQL.Append("       Branch AS BN ON SRQ.OIDBranch = BN.OIDBranch INNER JOIN ");
            //    sbSQL.Append("       Customer AS CUS ON SRQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
            //    sbSQL.Append("       GarmentCategory AS CAT ON SRQ.OIDCATEGORY = CAT.OIDGCATEGORY INNER JOIN ");
            //    sbSQL.Append("       ProductStyle AS PS ON SRQ.OIDSTYLE = PS.OIDSTYLE INNER JOIN ");
            //    sbSQL.Append("       Department AS DP ON SRQ.OIDDEPT = DP.OIDDepartment ");
            //    sbSQL.Append("WHERE (SRQ.SMPLNo = N'" + slSampleRequestNo.Text.Trim() + "') ");
            //    sbSQL.Append("ORDER BY OIDSMPL ");

            //    DataTable dtSMPL = new DBQuery(sbSQL).getDataTable();
            //    if (dtSMPL.Rows.Count > 0)
            //    {
            //        foreach (DataRow drSMPL in dtSMPL.Rows)
            //        {
            //            string BRANCH = drSMPL["BranchID"].ToString();
            //            glBranch.EditValue = BRANCH;

            //            string SMPLNO = drSMPL["SMPL No."].ToString();
            //            string REVISE = drSMPL["Revise"].ToString();
            //            sbSQL.Clear();
            //            sbSQL.Append("SELECT QR.OIDSMPLDT, QR.OIDSMPL, CASE WHEN RQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN RQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN RQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS Zone, RQ.SMPLPatternNo AS [Pattern No.], PC.ColorNo AS Color, PS.SizeNo AS Size, QR.Quantity ");
            //            sbSQL.Append("FROM   SMPLRequest AS RQ INNER JOIN ");
            //            sbSQL.Append("       SMPLQuantityRequired AS QR ON RQ.OIDSMPL = QR.OIDSMPL INNER JOIN ");
            //            sbSQL.Append("       ProductColor AS PC ON QR.OIDCOLOR = PC.OIDCOLOR INNER JOIN ");
            //            sbSQL.Append("       ProductSize AS PS ON QR.OIDSIZE = PS.OIDSIZE ");
            //            sbSQL.Append("WHERE (RQ.SMPLNo = N'" + SMPLNO + "') AND(RQ.SMPLRevise = '" + REVISE + "') ");
            //            sbSQL.Append("ORDER BY QR.OIDSMPLDT ");
            //            new ObjDevEx.setGridControl(gcQR, gvQR, sbSQL).getDataShowOrder(false, false, false, true);

            //            gvQR.Columns[1].Visible = false;
            //            gvQR.Columns[2].Visible = false;

            //            gvQR.Columns["NO"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //            gvQR.Columns["Quantity"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //            gvQR.Appearance.HeaderPanel.Options.UseTextOptions = true;
            //            gvQR.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            //            string SMPLID = drSMPL["SMPL ID"].ToString();
            //            sbSQL.Clear();
            //            sbSQL.Append("SELECT MK.OIDMARK AS MarkID, MK.MarkingNo AS [Marking No.], RQ.SMPLNo AS [SMPL No.], RQ.Season, CUS.ShortName AS Customer, RQ.SMPLItem AS [SMPL Item], PS.StyleName AS Style, RQ.SMPLPatternNo AS [Pattern No.]  ");
            //            sbSQL.Append("FROM   Marking AS MK INNER JOIN ");
            //            sbSQL.Append("       SMPLRequest AS RQ ON MK.OIDSMPL = RQ.OIDSMPL INNER JOIN ");
            //            sbSQL.Append("       Customer AS CUS ON RQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
            //            sbSQL.Append("       ProductStyle AS PS ON RQ.OIDSTYLE = PS.OIDSTYLE ");
            //            sbSQL.Append("WHERE (MK.OIDSMPL = '" + SMPLID + "') ");
            //            sbSQL.Append("ORDER BY MarkID ");
            //            new ObjDevEx.setGridControl(gcMARK, gvMARK, sbSQL).getDataShowOrder(false, false, false, true);

            //            gvMARK.Columns[1].Visible = false; //MarkID


            //            string RequestDate = drSMPL["Request Date"].ToString();
            //            dteRequestDate.EditValue = Convert.ToDateTime(RequestDate);

            //            int SpecSizeID = Convert.ToInt32(drSMPL["SpecSizeID"].ToString());
            //            rgSpecSize.EditValue = SpecSizeID;

            //            int UseForID = Convert.ToInt32(drSMPL["UseForID"].ToString());
            //            rgUseFor.EditValue = UseForID;

            //            string Season = drSMPL["Season"].ToString();
            //            glueSeason.EditValue = Season;

            //            string CustomerID = drSMPL["CustomerID"].ToString();
            //            slueCustomer.EditValue = CustomerID;

            //            string ContactBy = drSMPL["Contact Name"].ToString();
            //            txeRequestBy.Text = ContactBy;

            //            string DeliveryRequest = drSMPL["Delivery Request"].ToString();
            //            dteDeliveryRequest.EditValue = Convert.ToDateTime(DeliveryRequest);

            //            string SMPLItem = drSMPL["SMPL Item"].ToString();
            //            txeItemNo.Text = SMPLItem;

            //            string ModelName = drSMPL["Model Name"].ToString();
            //            txeModelName.Text = ModelName;

            //            string Category = drSMPL["Category"].ToString();
            //            txeCategory.Text = Category;

            //            string Style = drSMPL["Style"].ToString();
            //            txeStyle.Text = Style;

            //            string SalesSection = drSMPL["Sales Section"].ToString();
            //            txeSection.Text = SalesSection;

            //            sbSQL.Clear();
            //            sbSQL.Append("SELECT RFB.OIDSMPLFB AS [REC ID], RFB.VendFBCode AS [Vendor FB Code], RFB.Composition, RFB.FBWeight AS Weight, RFB.OIDCOLOR AS [Color ID], PC.ColorNo, RFB.SMPLotNo AS [Sample Lot No.], RFB.OIDVEND AS [Vendor ID], ");
            //            sbSQL.Append("       VD.Code AS[Vendor Code], VD.Name AS[Vendor Name], '' AS[NAV Code] ");
            //            sbSQL.Append("FROM   SMPLRequestFabric AS RFB INNER JOIN ");
            //            sbSQL.Append("       ProductColor AS PC ON RFB.OIDCOLOR = PC.OIDCOLOR INNER JOIN ");
            //            sbSQL.Append("       Vendor AS VD ON RFB.OIDVEND = VD.OIDVEND INNER JOIN ");
            //            sbSQL.Append("       SMPLRequest AS RQ ON RFB.OIDSMPLDT = RQ.OIDSMPL ");
            //            sbSQL.Append("WHERE (RQ.OIDSMPL = '" + SMPLID + "') ");
            //            sbSQL.Append("ORDER BY[REC ID] ");
            //            new ObjDevEx.setGridControl(gcFB, gvFB, sbSQL).getDataShowCheckBoxAndOrder(false, false, false, true);


            //            sbSQL.Clear();
            //            sbSQL.Append("SELECT MD.OIDMARKDT AS [Rec.ID], MD.OIDMARK AS MarkID,  ");
            //            sbSQL.Append("       CASE WHEN MD.OIDSIZEZONE = 0 THEN 'Japan' ELSE CASE WHEN MD.OIDSIZEZONE = 1 THEN 'Europe' ELSE CASE WHEN MD.OIDSIZEZONE = 2 THEN 'US' ELSE '' END END END AS Zone, ");
            //            sbSQL.Append("       RQ.SMPLPatternNo AS[Pattern No.], GP.GarmentParts AS[Fabric Parts], RFB.FBType AS Type, PS.SizeNo AS[Size No.], PS.SizeName AS Size, MD.TotalWidthSTD AS[Standard Width], MD.UsableWidth AS[Usable Width], ");
            //            sbSQL.Append("       MD.GM2 AS[Weight(g / m2)], MD.PracticalLengthCM AS[Actual Length(cm.)], MD.QuantityPCS AS[Quantity(Pcs)], MD.LengthPer1CM AS[Length / Body(cm.)], MD.LengthPer1M AS[Length / Body(M)], ");
            //            sbSQL.Append("       MD.LengthPer1INCH AS[Length / Body(Inch)], MD.LengthPer1YARD AS[Length / Body(Yard)], MD.WeightG AS[Weight / M(g)], MD.WeightKG AS[Weight / M(Kg)] ");
            //            sbSQL.Append("FROM   MarkingDetails AS MD INNER JOIN ");
            //            sbSQL.Append("       Marking AS M ON MD.OIDMARK = M.OIDMARK INNER JOIN ");
            //            sbSQL.Append("       SMPLRequest AS RQ ON M.OIDSMPL = RQ.OIDSMPL INNER JOIN ");
            //            sbSQL.Append("       GarmentParts AS GP ON MD.OIDGParts = GP.OIDGParts INNER JOIN ");
            //            sbSQL.Append("       SMPLRequestFabric AS RFB ON RFB.OIDSMPLDT = RQ.OIDSMPL INNER JOIN ");
            //            sbSQL.Append("       ProductSize AS PS ON MD.OIDSIZE = PS.OIDSIZE ");
            //            sbSQL.Append("WHERE (MD.OIDMARK = '1') ");
            //            sbSQL.Append("ORDER BY[Rec.ID] ");
            //            new ObjDevEx.setGridControl(gcMDT, gvMDT, sbSQL).getDataShowOrder(false, false, false, true);
            //        }
            //    }
            //}
        }

        private void tabMARKING_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            int tabIndex = tabMARKING.SelectedTabPageIndex;
            if (tabIndex == 1) //Tab : mrkDetail
            {
                if (global_Marking == "") { return; }

                //initialLoad
                tabMarkDetail.SelectedTabPageIndex = 0;
                db.getDgv("SELECT OIDGParts AS ID, GarmentParts AS FBPart FROM GarmentParts ORDER BY ID ", gcFBPart, mainConn);

                //ct.showInfoMessage(gvSTD.RowCount.ToString());
                if (gvSTD.RowCount == 0)
                {
                    btnRemoveRow.Enabled = false;
                }
                else
                {
                    btnRemoveRow.Enabled = true;
                }

                // * List of Fabric : ดึงข้อมูลมาจาก หน้าแรก By OIDMark
                //db.getDgv("Select ROW_NUMBER() over(Order By SMPLPatternNo) as No,SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size From vListOfFabric Where OIDSMPL = "+global_oidSmpl+" group by SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size order by SMPLPatternNo", gcListofFabric, mainConn);
                //ดึงรายการ Mark มาใส่ในตารางให้เลือก
                string sql = "Select ROW_NUMBER() over(Order By SMPLPatternNo) as No,SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size From vListOfFabric vFB inner join Marking mark on mark.OIDSMPL = vFB.OIDSMPL Where mark.OIDMARK = " + global_Marking + " group by SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size Order By SMPLPatternNo ";
                db.getDgv(sql, gcListofFabric, mainConn);
                ct.text_center(gvListofFabric, "No", 30);

                gvListofFabric.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                //ct.text_center(gvListofFabric, "OIDSMPLDT", 100);
                //ct.text_center(gvListofFabric, "OIDSMPLFB", 100);
                //ct.text_center(gvListofFabric, "OIDSMFBGParts", 100);
                //ct.text_center(gvListofFabric, "OIDGParts", 100);

                //List of Marking Detail
                //string sql2 = "Select ROW_NUMBER() over(Order by OIDMARKDT) as No,OIDMARKDT as RecID,smpl.PatternSizeZone,smpl.SMPLPatternNo,fbpart.OIDSMFBGParts,FBType,s.SizeName as Size,TotalWidthSTD as StandardWidth,markdt.UsableWidth,WeightG,LengthPer1CM,QuantityPCS,PracticalLengthCM as BodyLength,LengthPer1M,LengthPer1INCH,LengthPer1YARD,WeightG From MarkingDetails markdt inner join Marking mark on mark.OIDMARK = markdt.OIDMARK inner join SMPLRequest smpl on smpl.OIDSMPL = mark.OIDSMPL inner join SMPLQuantityRequired q on q.OIDSMPL = smpl.OIDSMPL inner join SMPLRequestFabric fb on fb.OIDSMPLDT = q.OIDSMPLDT inner join SMPLRequestFabricParts fbpart on fbpart.OIDSMPLFB = fb.OIDSMPLFB inner join ProductSize s on s.OIDSIZE = markdt.OIDSIZE";
                //string sql2 = "Select ROW_NUMBER() over(order by markdt.OIDSIZE) as No, (case smpl.PatternSizeZone when 0 then 'Japan' when 1 then 'Europe' when 2 then 'US' end) as PatternSizeZone,smpl.SMPLPatternNo,markdt.* From MarkingDetails markdt inner join Marking mark on mark.OIDMARK = markdt.OIDMARK inner join SMPLRequest smpl on smpl.OIDSMPL = mark.OIDSMPL Where mark.OIDMARK = " + global_Marking + " ";
                //db.getDgv(sql2, gcMDT, mainConn);
                hq.getListofMaterialDetail(gcMDT, global_Marking);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Step 1 : chkRow ว่ามีข้อมูลอยู่หรือไม่ > ถ้าว่าง ให้ Add เข้าไปได้ > ถ้าไม่ว่าง ให้ chkDup Size ก่อน ว่ามี Size ซ้ำในตารางแล้วหรือไม่
            // CountRow
            Console.WriteLine(gvSTD.RowCount.ToString());

            if (gvSTD.DataRowCount > -1 && txtPatternNo.Text.ToString() != "")
            {
                gvSTD.AddNewRow();
                gvSTD.SetRowCellValue(GridControl.NewItemRowHandle, gvSTD.Columns["Size"], txtSize.Text.ToString());
                gvSTD.SetRowCellValue(GridControl.NewItemRowHandle, gvSTD.Columns["FBType"], txtFBType.Text.ToString());

                gvPOS.AddNewRow();
                gvPOS.SetRowCellValue(GridControl.NewItemRowHandle, gvPOS.Columns["Size"], txtSize.Text.ToString());
                gvPOS.SetRowCellValue(GridControl.NewItemRowHandle, gvPOS.Columns["FBType"], txtFBType.Text.ToString());

                gvNEG.AddNewRow();
                gvNEG.SetRowCellValue(GridControl.NewItemRowHandle, gvNEG.Columns["Size"], txtSize.Text.ToString());
                gvNEG.SetRowCellValue(GridControl.NewItemRowHandle, gvNEG.Columns["FBType"], txtFBType.Text.ToString());
            }
        }

        public void refreshMarking()
        {
            //
        }

        public void refreshMarkingDetail()
        {
            txtPatternNo.EditValue = null;
            rdoPatternSizeZone.SelectedIndex = 0;
            txtVendFBCode.EditValue = null;
            txtSize.EditValue = null;
            txtSampleLotNo.EditValue = null;
            txtFBType.EditValue = null;
            db.getDgv("SELECT OIDGParts AS ID, GarmentParts AS FBPart FROM GarmentParts ORDER BY ID ", gcFBPart, mainConn);

            txtTotal_Standard.EditValue = null;
            txtTotal_Positive.EditValue = null;
            txtTotal_Negative.EditValue = null;

            txtUsable_Standard.EditValue = null;
            txtUsable_Positive.EditValue = null;
            txtUsable_Negative.EditValue = null;

            txtWeight_Standard.EditValue = null;
            txtWeight_Positive.EditValue = null;
            txtWeight_Negative.EditValue = null;

            gcSTD.DataSource = dsListDetail(); gvSTD.Columns["FBType"].OptionsColumn.AllowEdit = false; gvSTD.Columns["Size"].OptionsColumn.AllowEdit = false;
            gcPOS.DataSource = dsListDetail(); gvPOS.Columns["FBType"].OptionsColumn.AllowEdit = false; gvPOS.Columns["Size"].OptionsColumn.AllowEdit = false;
            gcNEG.DataSource = dsListDetail(); gvNEG.Columns["FBType"].OptionsColumn.AllowEdit = false; gvNEG.Columns["Size"].OptionsColumn.AllowEdit = false;
        }

        private void bbiRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (tabMARKING.SelectedTabPageIndex)
            {
                case 0: refreshMarking(); break;
                case 1: refreshMarkingDetail(); break;
                default: refreshMarking(); break;
            }
        }

        private void btnRemoveRow_Click(object sender, EventArgs e)
        {
            int delRow = gvSTD.FocusedRowHandle; gvPOS.FocusedRowHandle = delRow; gvNEG.FocusedRowHandle = delRow;
            //ct.showInfoMessage(gvSTD.FocusedRowHandle.ToString());
            //ct.showInfoMessage(gvPOS.FocusedRowHandle.ToString());
            //ct.showInfoMessage(gvNEG.FocusedRowHandle.ToString());
            gvSTD.DeleteRow(delRow);
            gvPOS.DeleteRow(delRow);
            gvNEG.DeleteRow(delRow);
        }

        private void gvListofFabric_DoubleClick(object sender, EventArgs e)
        {
            if (gvListofFabric.RowCount > 0)
            {
                // Set bbi
                bbiSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                bbiEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                //ct.showInfoMessage("ok > 0"); :: Set Text Detail
                txtPatternNo.EditValue = ct.getCellVal(sender, "SMPLPatternNo");
                rdoPatternSizeZone.SelectedIndex = Convert.ToInt32(ct.getCellVal(sender, "ptrnSizeZone"));
                txtVendFBCode.EditValue = ct.getCellVal(sender, "VendFBCode");
                txtSize.EditValue = ct.getCellVal(sender, "Size");
                txtSampleLotNo.EditValue = ct.getCellVal(sender, "SMPLPatternNo");
                txtFBType.EditValue = ct.getCellVal(sender, "FBType");

                //Add new Row to GridView
                gvSTD.AddNewRow();
                gvSTD.SetRowCellValue(GridControl.NewItemRowHandle, gvSTD.Columns["FBType"], ct.getCellVal(sender, "FBType"));
                gvSTD.SetRowCellValue(GridControl.NewItemRowHandle, gvSTD.Columns["Size"], ct.getCellVal(sender, "Size"));
                gcSTD.FocusedView.UpdateCurrentRow();

                gvPOS.AddNewRow();
                gvPOS.SetRowCellValue(GridControl.NewItemRowHandle, gvPOS.Columns["FBType"], ct.getCellVal(sender, "FBType"));
                gvPOS.SetRowCellValue(GridControl.NewItemRowHandle, gvPOS.Columns["Size"], ct.getCellVal(sender, "Size"));
                gcPOS.FocusedView.UpdateCurrentRow();

                gvNEG.AddNewRow();
                gvNEG.SetRowCellValue(GridControl.NewItemRowHandle, gvNEG.Columns["FBType"], ct.getCellVal(sender, "FBType"));
                gvNEG.SetRowCellValue(GridControl.NewItemRowHandle, gvNEG.Columns["Size"], ct.getCellVal(sender, "Size"));
                gcNEG.FocusedView.UpdateCurrentRow();

                if (gvSTD.RowCount > 0)
                {
                    btnRemoveRow.Enabled = true;
                }
                else
                {
                    btnRemoveRow.Enabled = false;
                }
            }
        }

        private void gvSTD_RowClick(object sender, RowClickEventArgs e)
        {
            //ct.showInfoMessage(gvSTD.FocusedRowHandle.ToString());
        }

        private void gvPOS_RowClick(object sender, RowClickEventArgs e)
        {
            //ct.showInfoMessage(gvPOS.FocusedRowHandle.ToString());
            gvSTD.FocusedRowHandle = gvPOS.FocusedRowHandle;
        }

        private void gvNEG_RowClick(object sender, RowClickEventArgs e)
        {
            //ct.showInfoMessage(gvNEG.FocusedRowHandle.ToString());
            gvSTD.FocusedRowHandle = gvNEG.FocusedRowHandle;
        }

        private void gvMARK_DoubleClick(object sender, EventArgs e)
        {
            //รับค่า OIDMark จากตาราง Marking และทำการ Set ค่า global_Marking
            global_Marking = ct.getCellVal(sender, "MarkingNo");

            //ไปที่ Tabindex 1
            gcListofFabric.Enabled = true;
            tabMARKING.SelectedTabPageIndex = 1;

            //ดึงรายการ Mark มาใส่ในตารางให้เลือก
            //string sql = "Select ROW_NUMBER() over(Order By SMPLPatternNo) as No,SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size From vListOfFabric vFB inner join Marking mark on mark.OIDSMPL = vFB.OIDSMPL Where mark.OIDMARK = "+ global_Marking + " group by SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size Order By SMPLPatternNo ";
            //db.getDgv(sql,gcMARK,mainConn);
        }
        private void gvListOfSample_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            // Readonly
            txtMarkingNo.Enabled = false;
            slSampleRequestNo.Enabled = false;
            dteRequestDate.Enabled = false;
            rdoSpecofSize.Enabled = false;
            glSeason.Enabled = false;
            slCustomer.Enabled = false;
            rdoUseFor.Enabled = false;
            txtItemNo.Enabled = false;
            txtModelName.Enabled = false;
            txtCategory.Enabled = false;
            txtStyleName.Enabled = false;
            txtSaleSection.Enabled = false;

            var s = sender;
            string OIDSMPL = ct.getCellVal(s, "No"); //(sender as GridView).GetFocusedRowCellValue("No").ToString(); 
            global_oidSmpl = OIDSMPL;
            rdoPatternSizeZone.SelectedIndex = Convert.ToInt32(db.get_oneParameter("Select PatternSizeZone From SMPLRequest Where OIDSMPL = " + OIDSMPL + " ", mainConn, "PatternSizeZone"));

            //get Quantity
            hq.QuantityRequired(gcQR, OIDSMPL);
            slSampleRequestNo.EditValue = OIDSMPL;
            dteRequestDate.EditValue = ct.getCellVal(s, "RequestDate");
            rdoSpecofSize.SelectedIndex = (ct.getCellVal(s, "SpecificationSize") == "Necessary") ? 0 : 1;
            glSeason.EditValue = OIDSMPL;
            glSeason.EditValue = ct.getCellVal(s, "Season");
            slCustomer.EditValue = db.get_oneParameter("Select c.OIDCUST From Customer c inner join SMPLRequest r on r.OIDCUST = c.OIDCUST Where r.OIDSMPL = " + OIDSMPL + " ", mainConn, "OIDCUST");
            rdoUseFor.SelectedIndex = Convert.ToInt32(db.get_oneParameter("Select UseFor From SMPLRequest Where OIDSMPL = " + OIDSMPL + " ", mainConn, "UseFor"));
            txtItemNo.EditValue = ct.getCellVal(s, "SMPLItem");
            txtModelName.EditValue = db.get_oneParameter("Select ModelName From SMPLRequest Where OIDSMPL = " + OIDSMPL + " ", mainConn, "ModelName");
            txtCategory.EditValue = ct.getCellVal(s, "Category");
            txtStyleName.EditValue = db.get_oneParameter("Select StyleName From SMPLRequest r inner join ProductStyle s on s.OIDSTYLE = r.OIDSTYLE Where OIDSMPL = " + OIDSMPL + " ", mainConn, "StyleName");
            txtSaleSection.EditValue = ct.getCellVal(s, "SaleSection");
        }

        private void gvSTD_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //TBL: STD,POS,NEG >> FBType , Size , ActualLangthCm , QtyPcs , LengthBodyCm , LengthBodyM , LengthBodyInc , LengthBodyYrd , WeightMg , WeightPcs

            //GridView view = sender as GridView;
            //if (view.FocusedColumn.FieldName == "ActualLangthCm")
            //{
            //    double val = 0;
            //    if (!Double.TryParse(e.Value as String, out val))
            //    {
            //        e.Valid = false;
            //        e.ErrorText = "Only numeric values are accepted.";
            //    }
            //}

            //GridColumn colModelPrice = gvSTD.Columns["ActualLangthCm"];
            //colModelPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //colModelPrice.DisplayFormat.FormatString = "c0";
        }

        private void gvMDT_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                // Set bbi
                //bbiEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                bbiSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                bbiEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                gcListofFabric.Enabled = false;

                string oidMarkingDT = ct.getCellVal(sender, "OIDMARKDT");
                txtPatternNo.EditValue = gvMDT.GetFocusedRowCellValue("SMPLPatternNo").ToString();
                rdoPatternSizeZone.SelectedIndex = ct.get_ID_PatternSizeZone(gvMDT.GetFocusedRowCellValue("PatternSizeZone").ToString());
                txtSize.EditValue = db.get_oneParameter("Select SizeName From ProductSize Where OIDSIZE = " + ct.getCellVal(sender, "OIDSIZE") + " ", mainConn, "SizeName");
                string sql = "Select VendFBCode,SMPLotNo,FBType From SMPLRequestFabric fb inner join SMPLQuantityRequired q on q.OIDSMPLDT = fb.OIDSMPLDT Where fb.OIDSMPLDT = " + ct.getCellVal(sender, "OIDSMPLDTStuff").Substring(0, 1) + " and q.OIDSIZE = " + ct.getCellVal(sender, "OIDSIZE") + " ";
                Console.WriteLine(sql);
                txtVendFBCode.EditValue = db.get_oneParameter(sql, mainConn, "VendFBCode");
                txtSampleLotNo.EditValue = db.get_oneParameter(sql, mainConn, "SMPLotNo");
                txtFBType.EditValue = db.get_oneParameter(sql, mainConn, "FBType");

                // Set Header to 3 Grid
                string ssql = "Select TotalWidthSTD,UsableWidth,GM2 From MarkingDetails mkdt inner join ProductSize s on s.OIDSIZE = mkdt.OIDSIZE Where s.OIDSIZE = "+ ct.getCellVal(sender, "OIDSIZE") + " ";
                txtTotal_Standard.EditValue = db.get_oneParameter(ssql+ " and DetailsType = 0", mainConn, "TotalWidthSTD");
                txtUsable_Standard.EditValue = db.get_oneParameter(ssql+ " and DetailsType = 0", mainConn, "UsableWidth");
                txtWeight_Standard.EditValue = db.get_oneParameter(ssql+ " and DetailsType = 0", mainConn, "GM2");

                txtTotal_Positive.EditValue = db.get_oneParameter(ssql + " and DetailsType = 1", mainConn, "TotalWidthSTD");
                txtUsable_Positive.EditValue = db.get_oneParameter(ssql + " and DetailsType = 1", mainConn, "UsableWidth");
                txtWeight_Positive.EditValue = db.get_oneParameter(ssql + " and DetailsType = 1", mainConn, "GM2");

                txtTotal_Negative.EditValue = db.get_oneParameter(ssql + " and DetailsType = 2", mainConn, "TotalWidthSTD");
                txtUsable_Negative.EditValue = db.get_oneParameter(ssql + " and DetailsType = 2", mainConn, "UsableWidth");
                txtWeight_Negative.EditValue = db.get_oneParameter(ssql + " and DetailsType = 2", mainConn, "GM2");

                //Set Data to 3 Grid
                string dsql = "Select '' as FBType,s.SizeName as Size,PracticalLengthCM as ActualLangthCm , QuantityPCS as QtyPcs , LengthPer1CM as LengthBodyCm,LengthPer1M as LengthBodyM ,LengthPer1INCH as LengthBodyInc , LengthPer1YARD as LengthBodyYrd,WeightG as WeightMg , WeightKG as WeightPcs";
                dsql += " From MarkingDetails mkdt inner join ProductSize s on s.OIDSIZE = mkdt.OIDSIZE";

                string sql1 = dsql + " Where DetailsType = 0 and s.OIDSIZE = " + ct.getCellVal(sender, "OIDSIZE") + " ";
                Console.WriteLine(sql1);
                db.getDgv(sql1, gcSTD, mainConn);

                string sql2 = dsql + " Where DetailsType = 1 and s.OIDSIZE = " + ct.getCellVal(sender, "OIDSIZE") + " ";
                Console.WriteLine(sql2);
                db.getDgv(sql2, gcPOS, mainConn);

                string sql3 = dsql + " Where DetailsType = 2 and s.OIDSIZE = " + ct.getCellVal(sender, "OIDSIZE") + " ";
                Console.WriteLine(sql3);
                db.getDgv(sql3, gcNEG, mainConn);

                //set chkBox FBGPart
                // Clear FBPart
                db.getDgv("Select OIDGParts,GarmentParts as FBPart From GarmentParts", gcFBPart, mainConn);

                // Split String to List
                List<string> strList = ct.getCellVal(sender, "GPartsStuff").Split(',').ToList();

                GridView gv = gvFBPart;
                for (int i = 0; i < gv.RowCount; i++)
                {
                    //>> Read Data in Datatable :: วนรอบข้อมูลใน DataTable
                    foreach (string val in strList)
                    {
                        string OIDGParts1 = gv.GetRowCellValue(i, "OIDGParts").ToString();
                        string OIDGParts2 = val.ToString();
                        if (OIDGParts1 == OIDGParts2) //ถ้าตรงกัน ก็ให้ทำการ Check ซะ
                        {
                            gv.SelectRow(i);
                        }
                    }
                }
            }
        }

        public void updateMarkingDetail()
        {
            if (ct.doConfirm("Update MarkingDetail") == true)
            {
                ct.showInfoMessage("Update Success.");
                refreshMarkingDetail();
            }
        }

        private void bbiEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (tabMARKING.SelectedTabPageIndex)
            {
                case 1: updateMarkingDetail(); break;
            }
        }
    }
}