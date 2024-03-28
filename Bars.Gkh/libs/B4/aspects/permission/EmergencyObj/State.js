Ext.define('B4.aspects.permission.emergencyobj.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.emergencyobjstateperm',

    permissions: [
        { name: 'Gkh.EmergencyObject.Field.RealityObject_Edit', applyTo: '#sfRealityObjectEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ResettlementProgram_Edit', applyTo: '#sfResettlementProgEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ActualInfoDate_Edit', applyTo: '#dfActualInfoDateEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ReasonInexpedient_Edit', applyTo: '#sfReasInexpedientEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ResettlementFlatArea_Edit', applyTo: '#nfResettlementFlatAreaEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.CadastralNumber_Edit', applyTo: '#tfCadastralNumberEmerObj', selector: '#emergencyObjEditPanel' },

        { name: 'Gkh.EmergencyObject.Field.DemolitionDate_Edit', applyTo: '#dfDemolitionDateEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ResettlementDate_Edit', applyTo: '#dfResettlementDateEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.FactDemolitionDate_Edit', applyTo: '#dfFactDemolitionDateEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.FactResettlementDate_Edit', applyTo: '#dfFactResettlementDateEmerObj', selector: '#emergencyObjEditPanel' },


        { name: 'Gkh.EmergencyObject.Field.IsRepairExpedient_Edit', applyTo: '#cbIsRepairExpedientEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ResettlementFlatAmount_Edit', applyTo: '#nfRessetlementAmountEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.LandArea_Edit', applyTo: '#nfLandAreaEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.InhabitantNumber_Edit', applyTo: '#nfInhabitantNumberEmerObj', selector: '#emergencyObjEditPanel' },

        { name: 'Gkh.EmergencyObject.Field.FurtherUse_Edit', applyTo: '#sfFurtherUseEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.ConditionHouse_Edit', applyTo: '#cbConditionHouseEmerObj', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.Description_Edit', applyTo: '#taDescription', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.File_Edit', applyTo: '#ffFileInfo', selector: '#emergencyObjEditPanel' },

        { name: 'Gkh.EmergencyObject.Field.EmergencyDocumentName_Edit', applyTo: '#tfEmergencyDocumentName', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.EmergencyDocumentNumber_Edit', applyTo: '#tfEmergencyDocumentNumber', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.EmergencyDocumentDate_Edit', applyTo: '#dfEmergencyDocumentDate', selector: '#emergencyObjEditPanel' },
        { name: 'Gkh.EmergencyObject.Field.EmergencyFileInfo_Edit', applyTo: '#ffEmergencyFileInfo', selector: '#emergencyObjEditPanel' },

        { name: 'Gkh.EmergencyObject.Field.State_Edit', applyTo: '#btnState', selector: '#emergencyObjEditPanel' }
    ]
});