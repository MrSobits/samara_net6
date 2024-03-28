Ext.define('B4.aspects.permission.specialobjectcr.PerformedWorkAct', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.performedworkactspecialobjectcrstateperm',

    permissions: [ 
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrperfactwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Work', applyTo: 'b4selectfield[name=TypeWorkCr]', selector: 'specialobjectcrperfactwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.DocumentNum', applyTo: 'textfield[name=DocumentNum]', selector: 'specialobjectcrperfactwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Sum', applyTo: 'gkhdecimalfield[name=Sum]', selector: 'specialobjectcrperfactwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Volume', applyTo: 'gkhdecimalfield[name=Volume]', selector: 'specialobjectcrperfactwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.DateFrom', applyTo: 'datefield[name=DateFrom]', selector: 'specialobjectcrperfactwin' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.CostFile', applyTo: 'b4filefield[name="CostFile"]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.DocumentFile', applyTo: 'b4filefield[name="DocumentFile"]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.AdditionFile', applyTo: 'b4filefield[name="AdditionFile"]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Rec.Edit', applyTo: 'button[actionName=btnSaveRecs]', selector: 'specialobjectcrperfworkactrecgrid' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Rec.Delete', applyTo: 'b4deletecolumn', selector: 'specialobjectcrperfworkactrecgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },        
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.RepresentativeSigned_Edit', applyTo: 'field[name=RepresentativeSigned]', selector: 'specialobjectcrperfactwin' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.RepresentativeSigned_View', applyTo: 'field[name=RepresentativeSigned]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.RepresentativeName_Edit', applyTo: '[name=RepresentativeNameContainer]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                var representativeSignedField = {},
                    commonAllowed = true;
                if (component) {
                    representativeSignedField = component.up('specialobjectcrperfactwin').down('b4enumcombo[name=RepresentativeSigned]');
                    if (representativeSignedField) {
                        commonAllowed = representativeSignedField.getValue() === B4.enums.YesNo.Yes;
                    }
                    Ext.each(component.query('field'), function (f) {
                        f.setDisabled(!(allowed && commonAllowed));
                    });
                    component.manualDisabled = !allowed;
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.RepresentativeName_View', applyTo: '[name=RepresentativeNameContainer]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.ExploitationAccepted_Edit', applyTo: 'b4enumcombo[name=ExploitationAccepted]', selector: 'specialobjectcrperfactwin' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.ExploitationAccepted_View', applyTo: 'b4enumcombo[name=ExploitationAccepted]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.WarrantyStartDate_Edit', applyTo: 'datefield[name=WarrantyStartDate]', selector: 'specialobjectcrperfactwin' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.WarrantyStartDate_View', applyTo: 'datefield[name=WarrantyStartDate]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.WarrantyEndDate_Edit', applyTo: 'datefield[name=WarrantyEndDate]', selector: 'specialobjectcrperfactwin' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.WarrantyEndDate_View', applyTo: 'datefield[name=WarrantyEndDate]', selector: 'specialobjectcrperfactwin',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        }
    ]
});