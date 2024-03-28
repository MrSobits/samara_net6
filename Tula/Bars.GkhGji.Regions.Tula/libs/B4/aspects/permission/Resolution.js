Ext.define('B4.aspects.permission.Resolution', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.resolutionperm',

    permissions: [
   /* name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол 
    */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.Resolution.Field.BecameLegal_Edit', applyTo: '[name = BecameLegal]', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { 
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#resolutionEditPanel' },
        { 
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#resolutionEditPanel' },
        { 
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_Edit', applyTo: '#dfDeliveryDate', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_Edit', applyTo: '#cbTypeInitOrg', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.SectorNumber_Edit', applyTo: '#tfSectorNumber', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Official_Edit', applyTo: '#sfOfficial', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Municipality_Edit', applyTo: '#sfMunicipality', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Sanction_Edit', applyTo: '#cbSanction', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Paided_Edit', applyTo: '#cbPaided', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_Edit', applyTo: '#nfPenaltyAmount', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DateTransferSSP_Edit', applyTo: '#dfDateTransferSsp', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNumSSP_Edit', applyTo: '#tfDocumentNumSsp', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.TypeTermination_Edit', applyTo: '#cbTermination', selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //Payfine
        { name: 'GkhGji.DocumentsGji.Resolution.Register.PayFine.Create', applyTo: 'b4addbutton', selector: '#resolutionPayFineGrid' },
        { name: 'GkhGji.DocumentsGji.Resolution.Register.PayFine.Edit', applyTo: 'b4savebutton', selector: '#resolutionPayFineEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Register.PayFine.Delete', applyTo: 'b4deletecolumn', selector: '#resolutionPayFineGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //Dispute
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Dispute.Create', applyTo: 'b4addbutton', selector: '#resolutionDisputeGrid' },
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Dispute.Edit', applyTo: 'b4savebutton', selector: '#resolutionDisputeEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Register.Dispute.Delete', applyTo: 'b4deletecolumn', selector: '#resolutionDisputeGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //Definition
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Definition.Create', applyTo: 'b4addbutton', selector: '#resolutionDefinitionGrid' },
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Definition.Edit', applyTo: 'b4savebutton', selector: '#resolutionDefinitionEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Register.Definition.Delete', applyTo: 'b4deletecolumn', selector: '#resolutionDefinitionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //Annex
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#resolutionAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#resolutionAnnexEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#resolutionAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});