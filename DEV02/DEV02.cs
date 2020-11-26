using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Extensions;
using DBConnection;
using System.Data;
using DevExpress.XtraEditors.Controls;
using System.CodeDom;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;


namespace DEV02
{
    public partial class DEV02 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Functionality.Function FUNC = new Functionality.Function();
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

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            LoadData();
            NewData();

            tabMARKING.SelectedTabPage = lcgMark;
            tabMarkDetail.SelectedTabPage = lcgStd;
        }

        private void LoadData()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT Branch, OIDBranch AS ID FROM Branch ORDER BY ID ");
            new ObjDevEx.setGridLookUpEdit(glueBranch, sbSQL, "Branch", "ID").getData();

            sbSQL.Clear();
            sbSQL.Append("SELECT SMPLNo AS [SMPL No.], SMPLRevise AS [Revise], ReferenceNo AS [Reference No.], CONVERT(VARCHAR(10), RequestDate) AS [Request Date], Season, SMPLItem AS [SMPL Item], ModelName AS [Model Name], Status, OIDSMPL AS [SMPL ID] ");
            sbSQL.Append("FROM SMPLRequest ");
            sbSQL.Append("ORDER BY SMPLNo ");
            new ObjDevEx.setSearchLookUpEdit(slueRequestNo, sbSQL, "SMPL No.", "SMPL No.").getData();


            sbSQL.Clear();
            sbSQL.Append("SELECT SRQ.OIDSMPL AS [SMPL ID], SRQ.Status, SRQ.SMPLNo AS [SMPL No.], SRQ.OIDBranch AS [BranchID], BN.Branch, CONVERT(VARCHAR(10), SRQ.RequestDate) AS [Request Date], ");
            //sbSQL.Append("       SUBSTRING((SELECT ', ' + SIZEX.SizeName AS[text()] FROM SMPLQuantityRequired AS SQRX INNER JOIN ProductSize AS SIZEX ON SQRX.OIDSIZE = SIZEX.OIDSIZE WHERE(SQRX.OIDSMPL = SRQ.OIDSMPL) ORDER BY SQRX.OIDSMPLDT FOR XML PATH('')), 2, 1000) AS [Spec.of Size], ");
            sbSQL.Append("       SRQ.SpecificationSize AS [SpecSizeID], CASE WHEN SRQ.SpecificationSize = 0 THEN 'Neccesary' ELSE 'Unneccesary' END AS [Spec.of Size], ");
            sbSQL.Append("       SRQ.Season, SRQ.OIDCUST AS CustomerID, CUS.ShortName AS Customer, SRQ.UseFor AS UseForID, ");
            sbSQL.Append("       CASE WHEN SRQ.UseFor = 0 THEN 'Application' ELSE CASE WHEN SRQ.UseFor = 1 THEN 'Take a photograp' ELSE CASE WHEN SRQ.UseFor = 2 THEN 'Monitor' ELSE CASE WHEN SRQ.UseFor = 3 THEN 'SMPL Meeting' ELSE CASE WHEN SRQ.UseFor = 4 THEN 'Each Color' ELSE CASE WHEN SRQ.UseFor = 5 THEN 'Other' ELSE '' END END END END END END AS [Use For], ");
            sbSQL.Append("       SRQ.OIDCATEGORY AS CategoryID, CAT.CategoryName AS Category, SRQ.OIDSTYLE AS StyleID, PS.StyleName AS Style, ");
            sbSQL.Append("       SRQ.SMPLItem AS [SMPL Item], SRQ.SMPLPatternNo AS [Pattern No.], ");
            sbSQL.Append("       SRQ.PatternSizeZone AS PSZID, CASE WHEN SRQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN SRQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN SRQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS [Pattern Size Zone], ");
            sbSQL.Append("       SRQ.CustApproved AS [Customer Approved], SRQ.ContactName AS [Contact Name], CONVERT(VARCHAR(10), SRQ.DeliveryRequest) AS [Delivery Request], SRQ.ModelName AS [Model Name], SRQ.OIDDEPT, DP.Department AS [Sales Section], SRQ.SMPLRevise AS [Revise] ");
            sbSQL.Append("FROM   SMPLRequest AS SRQ INNER JOIN ");
            sbSQL.Append("       Branch AS BN ON SRQ.OIDBranch = BN.OIDBranch INNER JOIN ");
            sbSQL.Append("       Customer AS CUS ON SRQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
            sbSQL.Append("       GarmentCategory AS CAT ON SRQ.OIDCATEGORY = CAT.OIDGCATEGORY INNER JOIN ");
            sbSQL.Append("       ProductStyle AS PS ON SRQ.OIDSTYLE = PS.OIDSTYLE INNER JOIN ");
            sbSQL.Append("       Department AS DP ON SRQ.OIDDEPT = DP.OIDDepartment ");
            sbSQL.Append("WHERE (SRQ.Status = 0) ");
            sbSQL.Append("ORDER BY OIDSMPL ");
            new ObjDevEx.setGridControl(gcSQ, gvSQ, sbSQL).getDataShowOrder(false, false, false, true);

            gvSQ.Columns[1].Visible = false; //SMPLID
            gvSQ.Columns[4].Visible = false; //BranchID
            gvSQ.Columns[7].Visible = false; //SpecSizeID
            gvSQ.Columns[10].Visible = false; //CustomerID
            gvSQ.Columns[12].Visible = false; //UseForID
            gvSQ.Columns[14].Visible = false; //CategoryID
            gvSQ.Columns[16].Visible = false; //StyleID
            gvSQ.Columns[20].Visible = false; //PSZID --> Pattern Size Zone
            gvSQ.Columns[23].Visible = false; //Contact Name
            gvSQ.Columns[24].Visible = false; //Delivery Request
            gvSQ.Columns[25].Visible = false; //Model Name
            gvSQ.Columns[26].Visible = false; //OIDDEPT
            gvSQ.Columns[27].Visible = false; //Sales Section
            gvSQ.Columns[28].Visible = false; //Revise

            gvSQ.Columns["NO"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gvSQ.Columns["Status"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gvSQ.Columns["SMPL No."].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            gvSQ.Columns["NO"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvSQ.Columns["Status"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvSQ.Columns["Season"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvSQ.Columns["Request Date"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            gvSQ.Appearance.HeaderPanel.Options.UseTextOptions = true;
            gvSQ.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;


            sbSQL.Clear();
            sbSQL.Append("SELECT DISTINCT Season FROM SMPLRequest ");
            new ObjDevEx.setGridLookUpEdit(glueSeason, sbSQL, "Season", "Season").getData();

            sbSQL.Clear();
            sbSQL.Append("SELECT DISTINCT Code AS [Customer Code], ShortName AS [Short Name], Name AS [Customer Name], OIDCUST AS [Customer ID] FROM Customer ");
            new ObjDevEx.setSearchLookUpEdit(slueCustomer, sbSQL, "Short Name", "Customer ID").getData();
            //slueCustomer

            sbSQL.Clear();
            sbSQL.Append("SELECT '' AS [Type], '' AS [Size], '' AS [Actual Length (cm.)], '' AS [Qty. (Pcs)], '' AS [Length/Body (cm.)], '' AS [Length/Body (M)], '' AS [Length/Body (Inch)], '' AS [Length/Body (Yard)], '' AS [Weight/M (g)], '' AS [Weight/1Pcs (kg)] ");
            new ObjDevEx.setGridControl(gcSTD, gvSTD, sbSQL).getDataShowOrder(false, false, false, true);
            new ObjDevEx.setGridControl(gcPOS, gvPOS, sbSQL).getDataShowOrder(false, false, false, true);
            new ObjDevEx.setGridControl(gcNEG, gvNEG, sbSQL).getDataShowOrder(false, false, false, true);

        }

        private void NewData()
        {
            //Marking Tab
            txtMarkNo.Text = "";
            dteDocDate.EditValue = DateTime.Now;
            rgMRT.SelectedIndex = 0;

            glueBranch.Text = "";
            slueRequestNo.Text = "";
            dteRequestDate.EditValue = DateTime.Now;
            rgSpecSize.SelectedIndex = 0;

            glueSeason.Text = "";
            slueCustomer.Text = "";
            txeRequestBy.Text = "";
            dteDeliveryRequest.EditValue = DateTime.Now;
            rgUseFor.SelectedIndex = 0;
            mmeRemark.Text = "";

            txeItemNo.Text = "";
            txeModelName.Text = "";
            txeCategory.Text = "";
            txeStyle.Text = "";
            txeSection.Text = "";


            rgCutting.SelectedIndex = 0;
            rgSewing.SelectedIndex = 0;

            txeCREATE.Text = "0";
            txeCDATE.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txeUPDATE.Text = "0";
            txeUDATE.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //Marking Detail Tab
            gcFB.DataSource = null;
            LoadDataParts();

            txePattern.Text = "";
            rgZone.SelectedIndex = 0;
            txeVendor.Text = "";
            txeFabricColor.Text = "";
            txeSampleLot.Text = "";
            txeFabricType.Text = "";
            gcMDT.DataSource = null;

            txeStdTotal.Text = "";
            txeStdUsable.Text = "";
            txeStdWeight.Text = "";
            gcSTD.DataSource = null;

            txePosTotal.Text = "";
            txePosUsable.Text = "";
            txePosWeight.Text = "";
            gcPOS.DataSource = null;

            txeNegTotal.Text = "";
            txeNegUsable.Text = "";
            txeNegWeight.Text = "";
            gcNEG.DataSource = null;
        }

        private void bbiNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
            NewData();
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

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (txeName.Text.Trim() == "")
            //{
            //    FUNC.msgWarning("Please name.");
            //    txeName.Focus();
            //}
            //else if (txeDescription.Text.Trim() == "")
            //{
            //    FUNC.msgWarning("Please input description.");
            //    txeDescription.Focus();
            //}
            //else
            //{
            //    if (FUNC.msgQuiz("Confirm save data ?") == true)
            //    {
            //        StringBuilder sbSQL = new StringBuilder();
            //        string strCREATE = "0";
            //        if (txeCREATE.Text.Trim() != "")
            //        {
            //            strCREATE = txeCREATE.Text.Trim();
            //        }

            //        bool chkGMP = chkDuplicate();
            //        if (chkGMP == true)
            //        {
            //            string Status = "NULL";
            //            if (rgStatus.SelectedIndex != -1)
            //            {
            //                Status = rgStatus.Properties.Items[rgStatus.SelectedIndex].Value.ToString();
            //            }

            //            if (lblStatus.Text == "* Add Payment Term")
            //            {
            //                sbSQL.Append("  INSERT INTO PaymentTerm(Name, Description, DueDateCalculation, Status, CreatedBy, CreatedDate) ");
            //                sbSQL.Append("  VALUES(N'" + txeName.Text.Trim().Replace("'", "''") + "', N'" + txeDescription.Text.Trim().Replace("'", "''") + "', N'" + txeDueDate.Text.Trim().Replace("'", "''") + "', " + Status + ", '" + strCREATE + "', GETDATE()) ");
            //            }
            //            else if (lblStatus.Text == "* Edit Payment Term")
            //            {
            //                sbSQL.Append("  UPDATE PaymentTerm SET ");
            //                sbSQL.Append("      Name=N'" + txeName.Text.Trim().Replace("'", "''") + "', ");
            //                sbSQL.Append("      Description=N'" + txeDescription.Text.Trim().Replace("'", "''") + "', ");
            //                sbSQL.Append("      DueDateCalculation=N'" + txeDueDate.Text.Trim().Replace("'", "''") + "', ");
            //                sbSQL.Append("      Status=" + Status + " ");
            //                sbSQL.Append("  WHERE(OIDPayment = '" + txeID.Text.Trim() + "') ");
            //            }

            //            //MessageBox.Show(sbSQL.ToString());
            //            if (sbSQL.Length > 0)
            //            {
            //                try
            //                {
            //                    bool chkSAVE = new DBQuery(sbSQL).runSQL();
            //                    if (chkSAVE == true)
            //                    {
            //                        FUNC.msgInfo("Save complete.");
            //                        bbiNew.PerformClick();
            //                    }
            //                }
            //                catch (Exception)
            //                { }
            //            }
            //        }
            //    }
            //}
        }

        private void bbiExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string pathFile = new ObjSet.Folder(@"C:\MDS\Export\").GetPath() + "PaymentTermList_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //gvPTerm.ExportToXlsx(pathFile);
            //System.Diagnostics.Process.Start(pathFile);
        }

        private void gvPTerm_RowClick(object sender, RowClickEventArgs e)
        {
            //lblStatus.Text = "* Edit Payment Term";
            //lblStatus.ForeColor = Color.Red;

            //txeID.Text = gvPTerm.GetFocusedRowCellValue("No").ToString();
            //txeName.Text = gvPTerm.GetFocusedRowCellValue("Name").ToString();
            //txeDescription.Text = gvPTerm.GetFocusedRowCellValue("Description").ToString();
            //txeDueDate.Text = gvPTerm.GetFocusedRowCellValue("DuedateCalculation").ToString();

            //int status = -1;
            //if (gvPTerm.GetFocusedRowCellValue("Status").ToString() != "")
            //{
            //    status = Convert.ToInt32(gvPTerm.GetFocusedRowCellValue("Status").ToString());
            //}

            //selectStatus(status);

            //txeCREATE.Text = gvPTerm.GetFocusedRowCellValue("CreatedBy").ToString();
            //txeDATE.Text = gvPTerm.GetFocusedRowCellValue("CreatedDate").ToString();
        }

        private void bbiPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gcPTerm.ShowPrintPreview();
        }

        private void bbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gcPTerm.Print();
        }

        private void gvSQ_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //**** MARKING TAB *****
            txtMarkNo.Text = "";
            dteDocDate.EditValue = DateTime.Now;
            rgMRT.SelectedIndex = 0; 
            mmeRemark.EditValue = "";
            rgCutting.SelectedIndex = 0;
            rgSewing.SelectedIndex = 0;
            gcMARK.DataSource = null;

            glueBranch.EditValue = "";
            slueRequestNo.EditValue = "";
            dteRequestDate.EditValue = DateTime.Now;
            rgSpecSize.SelectedIndex = 0;
            glueSeason.EditValue = "";
            slueCustomer.EditValue = "";
            txeRequestBy.Text = "";
            rgUseFor.SelectedIndex = 0;
            txeItemNo.Text = "";
            txeModelName.Text = "";
            txeCategory.Text = "";
            txeStyle.Text = "";
            gcQR.DataSource = null;
            txeSection.Text = "";


            string SMPLNO = gvSQ.GetFocusedRowCellValue("SMPL No.").ToString();
            string REVISE = gvSQ.GetFocusedRowCellValue("Revise").ToString();
            slueRequestNo.EditValue = SMPLNO;


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
            if (slueRequestNo.Text.Trim() != "")
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append("SELECT SRQ.OIDSMPL AS [SMPL ID], SRQ.Status, SRQ.SMPLNo AS [SMPL No.], SRQ.OIDBranch AS [BranchID], BN.Branch, CONVERT(VARCHAR(10), SRQ.RequestDate) AS [Request Date], ");
                sbSQL.Append("       SRQ.SpecificationSize AS [SpecSizeID], CASE WHEN SRQ.SpecificationSize = 0 THEN 'Neccesary' ELSE 'Unneccesary' END AS [Spec.of Size], ");
                sbSQL.Append("       SRQ.Season, SRQ.OIDCUST AS CustomerID, CUS.ShortName AS Customer, SRQ.UseFor AS UseForID, ");
                sbSQL.Append("       CASE WHEN SRQ.UseFor = 0 THEN 'Application' ELSE CASE WHEN SRQ.UseFor = 1 THEN 'Take a photograp' ELSE CASE WHEN SRQ.UseFor = 2 THEN 'Monitor' ELSE CASE WHEN SRQ.UseFor = 3 THEN 'SMPL Meeting' ELSE CASE WHEN SRQ.UseFor = 4 THEN 'Each Color' ELSE CASE WHEN SRQ.UseFor = 5 THEN 'Other' ELSE '' END END END END END END AS [Use For], ");
                sbSQL.Append("       SRQ.OIDCATEGORY AS CategoryID, CAT.CategoryName AS Category, SRQ.OIDSTYLE AS StyleID, PS.StyleName AS Style, ");
                sbSQL.Append("       SRQ.SMPLItem AS [SMPL Item], SRQ.SMPLPatternNo AS [Pattern No.], ");
                sbSQL.Append("       SRQ.PatternSizeZone AS PSZID, CASE WHEN SRQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN SRQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN SRQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS [Pattern Size Zone], ");
                sbSQL.Append("       SRQ.CustApproved AS [Customer Approved], SRQ.ContactName AS [Contact Name], CONVERT(VARCHAR(10), SRQ.DeliveryRequest) AS [Delivery Request], SRQ.ModelName AS [Model Name], SRQ.OIDDEPT, DP.Department AS [Sales Section], SRQ.SMPLRevise AS [Revise] ");
                sbSQL.Append("FROM   SMPLRequest AS SRQ INNER JOIN ");
                sbSQL.Append("       Branch AS BN ON SRQ.OIDBranch = BN.OIDBranch INNER JOIN ");
                sbSQL.Append("       Customer AS CUS ON SRQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
                sbSQL.Append("       GarmentCategory AS CAT ON SRQ.OIDCATEGORY = CAT.OIDGCATEGORY INNER JOIN ");
                sbSQL.Append("       ProductStyle AS PS ON SRQ.OIDSTYLE = PS.OIDSTYLE INNER JOIN ");
                sbSQL.Append("       Department AS DP ON SRQ.OIDDEPT = DP.OIDDepartment ");
                sbSQL.Append("WHERE (SRQ.SMPLNo = N'" + slueRequestNo.Text.Trim() + "') ");
                sbSQL.Append("ORDER BY OIDSMPL ");

                DataTable dtSMPL = new DBQuery(sbSQL).getDataTable();
                if (dtSMPL.Rows.Count > 0)
                {
                    foreach (DataRow drSMPL in dtSMPL.Rows)
                    {
                        string BRANCH = drSMPL["BranchID"].ToString();
                        glueBranch.EditValue = BRANCH;

                        string SMPLNO = drSMPL["SMPL No."].ToString();
                        string REVISE = drSMPL["Revise"].ToString();
                        sbSQL.Clear();
                        sbSQL.Append("SELECT QR.OIDSMPLDT, QR.OIDSMPL, CASE WHEN RQ.PatternSizeZone = 0 THEN 'Japan' ELSE CASE WHEN RQ.PatternSizeZone = 1 THEN 'Europe' ELSE CASE WHEN RQ.PatternSizeZone = 2 THEN 'US' ELSE '' END END END AS Zone, RQ.SMPLPatternNo AS [Pattern No.], PC.ColorNo AS Color, PS.SizeNo AS Size, QR.Quantity ");
                        sbSQL.Append("FROM   SMPLRequest AS RQ INNER JOIN ");
                        sbSQL.Append("       SMPLQuantityRequired AS QR ON RQ.OIDSMPL = QR.OIDSMPL INNER JOIN ");
                        sbSQL.Append("       ProductColor AS PC ON QR.OIDCOLOR = PC.OIDCOLOR INNER JOIN ");
                        sbSQL.Append("       ProductSize AS PS ON QR.OIDSIZE = PS.OIDSIZE ");
                        sbSQL.Append("WHERE (RQ.SMPLNo = N'" + SMPLNO + "') AND(RQ.SMPLRevise = '" + REVISE + "') ");
                        sbSQL.Append("ORDER BY QR.OIDSMPLDT ");
                        new ObjDevEx.setGridControl(gcQR, gvQR, sbSQL).getDataShowOrder(false, false, false, true);

                        gvQR.Columns[1].Visible = false;
                        gvQR.Columns[2].Visible = false;

                        gvQR.Columns["NO"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvQR.Columns["Quantity"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvQR.Appearance.HeaderPanel.Options.UseTextOptions = true;
                        gvQR.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                        string SMPLID = drSMPL["SMPL ID"].ToString();
                        sbSQL.Clear();
                        sbSQL.Append("SELECT MK.OIDMARK AS MarkID, MK.MarkingNo AS [Marking No.], RQ.SMPLNo AS [SMPL No.], RQ.Season, CUS.ShortName AS Customer, RQ.SMPLItem AS [SMPL Item], PS.StyleName AS Style, RQ.SMPLPatternNo AS [Pattern No.]  ");
                        sbSQL.Append("FROM   Marking AS MK INNER JOIN ");
                        sbSQL.Append("       SMPLRequest AS RQ ON MK.OIDSMPL = RQ.OIDSMPL INNER JOIN ");
                        sbSQL.Append("       Customer AS CUS ON RQ.OIDCUST = CUS.OIDCUST INNER JOIN ");
                        sbSQL.Append("       ProductStyle AS PS ON RQ.OIDSTYLE = PS.OIDSTYLE ");
                        sbSQL.Append("WHERE (MK.OIDSMPL = '" + SMPLID + "') ");
                        sbSQL.Append("ORDER BY MarkID ");
                        new ObjDevEx.setGridControl(gcMARK, gvMARK, sbSQL).getDataShowOrder(false, false, false, true);

                        gvMARK.Columns[1].Visible = false; //MarkID


                        string RequestDate = drSMPL["Request Date"].ToString();
                        dteRequestDate.EditValue = Convert.ToDateTime(RequestDate);

                        int SpecSizeID = Convert.ToInt32(drSMPL["SpecSizeID"].ToString());
                        rgSpecSize.EditValue = SpecSizeID;

                        int UseForID = Convert.ToInt32(drSMPL["UseForID"].ToString());
                        rgUseFor.EditValue = UseForID;

                        string Season = drSMPL["Season"].ToString();
                        glueSeason.EditValue = Season;

                        string CustomerID = drSMPL["CustomerID"].ToString();
                        slueCustomer.EditValue = CustomerID;

                        string ContactBy = drSMPL["Contact Name"].ToString();
                        txeRequestBy.Text = ContactBy;

                        string DeliveryRequest = drSMPL["Delivery Request"].ToString();
                        dteDeliveryRequest.EditValue = Convert.ToDateTime(DeliveryRequest);

                        string SMPLItem = drSMPL["SMPL Item"].ToString();
                        txeItemNo.Text = SMPLItem;

                        string ModelName = drSMPL["Model Name"].ToString();
                        txeModelName.Text = ModelName;

                        string Category = drSMPL["Category"].ToString();
                        txeCategory.Text = Category;

                        string Style = drSMPL["Style"].ToString();
                        txeStyle.Text = Style;

                        string SalesSection = drSMPL["Sales Section"].ToString();
                        txeSection.Text = SalesSection;

                        sbSQL.Clear();
                        sbSQL.Append("SELECT RFB.OIDSMPLFB AS [REC ID], RFB.VendFBCode AS [Vendor FB Code], RFB.Composition, RFB.FBWeight AS Weight, RFB.OIDCOLOR AS [Color ID], PC.ColorNo, RFB.SMPLotNo AS [Sample Lot No.], RFB.OIDVEND AS [Vendor ID], ");
                        sbSQL.Append("       VD.Code AS[Vendor Code], VD.Name AS[Vendor Name], '' AS[NAV Code] ");
                        sbSQL.Append("FROM   SMPLRequestFabric AS RFB INNER JOIN ");
                        sbSQL.Append("       ProductColor AS PC ON RFB.OIDCOLOR = PC.OIDCOLOR INNER JOIN ");
                        sbSQL.Append("       Vendor AS VD ON RFB.OIDVEND = VD.OIDVEND INNER JOIN ");
                        sbSQL.Append("       SMPLRequest AS RQ ON RFB.OIDSMPLDT = RQ.OIDSMPL ");
                        sbSQL.Append("WHERE (RQ.OIDSMPL = '" + SMPLID + "') ");
                        sbSQL.Append("ORDER BY[REC ID] ");
                        new ObjDevEx.setGridControl(gcFB, gvFB, sbSQL).getDataShowCheckBoxAndOrder(false, false, false, true);


                        sbSQL.Clear();
                        sbSQL.Append("SELECT MD.OIDMARKDT AS [Rec.ID], MD.OIDMARK AS MarkID,  ");
                        sbSQL.Append("       CASE WHEN MD.OIDSIZEZONE = 0 THEN 'Japan' ELSE CASE WHEN MD.OIDSIZEZONE = 1 THEN 'Europe' ELSE CASE WHEN MD.OIDSIZEZONE = 2 THEN 'US' ELSE '' END END END AS Zone, ");
                        sbSQL.Append("       RQ.SMPLPatternNo AS[Pattern No.], GP.GarmentParts AS[Fabric Parts], RFB.FBType AS Type, PS.SizeNo AS[Size No.], PS.SizeName AS Size, MD.TotalWidthSTD AS[Standard Width], MD.UsableWidth AS[Usable Width], ");
                        sbSQL.Append("       MD.GM2 AS[Weight(g / m2)], MD.PracticalLengthCM AS[Actual Length(cm.)], MD.QuantityPCS AS[Quantity(Pcs)], MD.LengthPer1CM AS[Length / Body(cm.)], MD.LengthPer1M AS[Length / Body(M)], ");
                        sbSQL.Append("       MD.LengthPer1INCH AS[Length / Body(Inch)], MD.LengthPer1YARD AS[Length / Body(Yard)], MD.WeightG AS[Weight / M(g)], MD.WeightKG AS[Weight / M(Kg)] ");
                        sbSQL.Append("FROM   MarkingDetails AS MD INNER JOIN ");
                        sbSQL.Append("       Marking AS M ON MD.OIDMARK = M.OIDMARK INNER JOIN ");
                        sbSQL.Append("       SMPLRequest AS RQ ON M.OIDSMPL = RQ.OIDSMPL INNER JOIN ");
                        sbSQL.Append("       GarmentParts AS GP ON MD.OIDGParts = GP.OIDGParts INNER JOIN ");
                        sbSQL.Append("       SMPLRequestFabric AS RFB ON RFB.OIDSMPLDT = RQ.OIDSMPL INNER JOIN ");
                        sbSQL.Append("       ProductSize AS PS ON MD.OIDSIZE = PS.OIDSIZE ");
                        sbSQL.Append("WHERE (MD.OIDMARK = '1') ");
                        sbSQL.Append("ORDER BY[Rec.ID] ");
                        new ObjDevEx.setGridControl(gcMDT, gvMDT, sbSQL).getDataShowOrder(false, false, false, true);
                    }
                }

            }
            
        }

        private void LoadDataParts()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT OIDGParts AS ID, GarmentParts AS Parts ");
            sbSQL.Append("FROM GarmentParts ");
            sbSQL.Append("ORDER BY ID ");
            DataTable drParts = new DBQuery(sbSQL).getDataTable();
            clbParts.ValueMember = "ID";
            clbParts.DisplayMember = "Parts";
            clbParts.DataSource = drParts;

        }

        private void gvFB_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            LoadDataParts();

            txePattern.Text = "";
            rgZone.SelectedIndex = 0;
            txeVendor.Text = "";
            txeFabricColor.Text = "";
            txeSampleLot.Text = "";
            txeFabricType.Text = "";
            gcMDT.DataSource = null;

            txeStdTotal.Text = "";
            txeStdUsable.Text = "";
            txeStdWeight.Text = "";
            gcSTD.DataSource = null;

            txePosTotal.Text = "";
            txePosUsable.Text = "";
            txePosWeight.Text = "";
            gcPOS.DataSource = null;

            txeNegTotal.Text = "";
            txeNegUsable.Text = "";
            txeNegWeight.Text = "";
            gcNEG.DataSource = null;

        }
    }
}