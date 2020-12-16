>> DEV02 Marking Request Function หลักๆ

>> TAB : Marking
- GetData to grid > ListofSample						:: Wait..
- Save Marking											:: Wait..
- Update Marking										:: Wait..

+---------------------------------------------------------------------------------------------+

>> TAB : Marking Details
- GetData to grid > ListofMaterial						:: Wait..

txtPatternNo
rdoPatternSizeZone
txtVendFBCode
txtSize
txtSampleLotNo
txtFBType

>> PageChange
tabMARKING_SelectedPageChanged

>> tblField : vListOfFabric : SMPLNo,SMPLPatternNo,ptrnSizeZone,PatternSizeZone,VendFBCode,SMPLNo,FBType,Size
>> gvListofFabric_DoubleClick
>> Refresh
refreshMarkingDetail

>> RemoveRow Click
btnRemoveRow_Click

>> SPG Columns
Standard :: txtTotal_Standard , txUsable_Standard , txtWeight_Standard
Positive :: txtTotal_Positive , txtUsable_Positive , txtWeight_Positive
Negative :: txtTotal_Negative , txtUsable_Negative , txtWeight_Negative