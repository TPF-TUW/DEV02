using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEV02
{
    public class classHardQuery
    {
        goClass.dbConn db = new goClass.dbConn();
        goClass.ctool ct = new goClass.ctool();
        SqlConnection mainConn = new goClass.dbConn().MDS();

        // >> Tab : Marking
        // Set Grid
        public void ListOfSample(GridControl gcName)
        {
            string sql = "SELECT OIDSMPL as No,(case smpl.Status when 0 then 'New' when 1 then 'Wait Approve' when 2 then 'Approved' end) as Status, SMPLNo/*,ReferenceNo,ContactName,ModelName,Situation,StateArrangements*/ ,b.Name as Branch ,d.Name as SaleSection ,RequestDate ,(case SpecificationSize when 0 then 'Necessary' when 1 then 'Unnecessary' end) as SpecificationSize , Season ,/*c.ShortName as cusShortName,*/c.Name as Customer ,(case UseFor when 0 then 'Application' when 1 then 'Take a Photograph' when 2 then 'Monitor' when 3 then 'SMPLMeeting' when 4 then 'Other' end) as UseFor ,g.CategoryName as Category ,p.StyleName as Style,SMPLItem, SMPLPatternNo ,(case PatternSizeZone when 0 then 'Japan' when 1 then 'Europe' when 2 then 'US' end) as PatternSizeZone,(case CustApproved when 0 then 'Yes' when 1 then 'No' end) as CustApproved FROM SMPLRequest smpl left join Branchs b on b.OIDBranch = smpl.OIDBranch left join Departments d on d.OIDDEPT = smpl.OIDDEPT left join Customer c on c.OIDCUST = smpl.OIDCUST left join GarmentCategory g on g.OIDGCATEGORY = smpl.OIDCATEGORY left join ProductStyle p on p.OIDSTYLE = smpl.OIDSTYLE Where smpl.Status = 0 Order By smpl.Status";
            db.getDgv(sql, gcName, mainConn);
        }
        public void QuantityRequired(GridControl gcName, string OIDSMPL)
        {
            string sql = "Select ROW_NUMBER() over(Order by q.Quantity) as No,(case smpl.PatternSizeZone when 0 then 'Japan' when 1 then 'Europe' when 2 then 'US' end) as PatternSizeZone,SMPLPatternNo,c.ColorName as Color,s.SizeName as Size,Quantity,u.UnitName as Unit From SMPLQuantityRequired q left join ProductColor c on c.OIDCOLOR = q.OIDCOLOR left join ProductSize s on s.OIDSIZE = q.OIDSIZE left join Unit u on u.OIDUNIT = q.OIDUnit left join SMPLRequest smpl on smpl.OIDSMPL = q.OIDSMPL Where q.OIDSMPL = " + OIDSMPL + " ";
            db.getDgv(sql, gcName, mainConn);
        }
        public void ListOfMarking(GridControl glName)
        {
            string sql = "Select ROW_NUMBER() over(Order by OIDMARK) as No,OIDMARK as MarkingNo,SMPLNo,Season,c.Name as Customer,SMPLItem,s.StyleName as Style,SMPLPatternNo From Marking mark inner join SMPLRequest smpl on smpl.OIDSMPL = mark.OIDSMPL inner join Customer c on c.OIDCUST = smpl.OIDCUST inner join ProductStyle s on s.OIDSTYLE = smpl.OIDSTYLE ";
            db.getDgv(sql, glName, mainConn);
        }

        // Set Form -----------------------------------------------------------------------------------------------------
        public void set_glBranch_Marking(GridLookUpEdit glName)
        {
            db.getGl("Select OIDBranch,Name as Branch From Branchs", mainConn, glName, "OIDBranch", "Branch");
        }
        public void set_slSampleRequestNo(SearchLookUpEdit slName)
        {
            db.getSl("Select OIDSMPL, SMPLNo From SMPLRequest", mainConn, slName, "OIDSMPL", "SMPLNo");
        }
        public void set_glSeason(GridLookUpEdit glName)
        {
            db.getGl("Select distinct s.Season as Season From( Select SUBSTRING( cast(Year(GETDATE())-1 as nvarchar(4)) , 3 , 2)+SeasonNo as Season From Season union Select SUBSTRING( cast(Year(GETDATE()) as nvarchar(4)) , 3 , 2) +SeasonNo as Season From Season union Select SUBSTRING( cast(Year(GETDATE())+1 as nvarchar(4)) , 3 , 2)+SeasonNo as Season From Season) as s left join SMPLRequest as smpl on s.Season = smpl.Season", mainConn,glName, "Season", "Season");
        }
        public void set_slCustomer(SearchLookUpEdit slName)
        {
            db.getSl("Select OIDCUST,Name as Customer From Customer", mainConn,slName, "OIDCUST", "Customer");
        }


        // Tab : MarkingDetail ------------------------------
        public void getListofMaterialDetail(GridControl gc, string oidMark)
        {
            string sql = "Select ROW_NUMBER() over(order by markdt.OIDSIZE) as No, (case smpl.PatternSizeZone when 0 then 'Japan' when 1 then 'Europe' when 2 then 'US' end) as PatternSizeZone,smpl.SMPLPatternNo,markdt.* From MarkingDetails markdt inner join Marking mark on mark.OIDMARK = markdt.OIDMARK inner join SMPLRequest smpl on smpl.OIDSMPL = mark.OIDSMPL Where mark.OIDMARK = " + oidMark + " ";
            db.getDgv(sql, gc, mainConn);
        }
    }
}
