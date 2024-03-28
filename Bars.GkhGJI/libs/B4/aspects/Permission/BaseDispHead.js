Ext.define('B4.aspects.permission.BaseDispHead', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.basedispheadperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        //поля панели редактирования
        { name: 'GkhGji.Inspection.BaseDispHead.Field.DispHeadDate_Edit', applyTo: '#dfDispHeadDate', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.Head_Edit', applyTo: '#sflHead', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.Inspectors_Edit', applyTo: '#trfInspectors', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.TypeBaseDispHead_Edit', applyTo: '#cbTypeBase', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.PrevDocument_Edit', applyTo: '#dispHeadPrevDocumentSelectField', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.TypeForm_Edit', applyTo: '#cbTypeForm', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.TypeJurPerson_Edit', applyTo: '#cbTypeJurPerson', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.PersonInspection_Edit', applyTo: '#cbPersonInspection', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.Contragent_Edit', applyTo: '#sfContragent', selector: '#baseDispHeadEditPanel' },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.PhysicalPerson_Edit', applyTo: '#tfPhysicalPerson', selector: '#baseDispHeadEditPanel' },
        
        { name: 'GkhGji.Inspection.BaseDispHead.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#baseDispHeadRealityObjectGrid' },
        { name: 'GkhGji.Inspection.BaseDispHead.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#baseDispHeadRealityObjectGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.Inspection.BaseDispHead.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseDispHeadEditPanel' },
        {
            name: 'GkhGji.Inspection.BaseDispHead.Field.ReasonErpChecking_View', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseDispHeadEditPanel' ,
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
                component.setDisabled(!allowed);
            }
         }
    ]
});