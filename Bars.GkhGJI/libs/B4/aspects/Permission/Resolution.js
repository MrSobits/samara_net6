Ext.define('B4.aspects.permission.Resolution', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.resolutionperm',

    permissions: [
        /* name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол 
    */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNumber_Edit',
            applyTo: '#tfDocumentNumber',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNum_View',
            applyTo: '#nfDocumentNum',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentYear_View',
            applyTo: '#nfDocumentYear',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Resolution.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentSubNum_View',
            applyTo: '#nfDocumentSubNum',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_Edit', applyTo: '#dfDeliveryDate', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_Edit', applyTo: '#cbTypeInitOrg', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.SectorNumber_Edit', applyTo: '#tfSectorNumber', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.SectorNumber_View',
            applyTo: '#tfSectorNumber',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Official_Edit', applyTo: '#sfOfficial', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.FineMunicipality_Edit', applyTo: '#tsfFineMunicipality', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Municipality_Edit', applyTo: '#sfMunicipality', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Sanction_Edit', applyTo: '#cbSanction', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.Paided_Edit', applyTo: '#cbPaided', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_Edit', applyTo: '#nfPenaltyAmount', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DateTransferSSP_Edit', applyTo: '#dfDateTransferSsp', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNumSSP_Edit', applyTo: '#tfDocumentNumSsp', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.TypeTermination_Edit',
            applyTo: '#cbTermination',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //DateWriteOut
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DateWriteOut_Edit', applyTo: '#dfDateWriteOut', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DateWriteOut_View',
            applyTo: '#dfDateWriteOut',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //AbandonReason
        { name: 'GkhGji.DocumentsGji.Resolution.Field.AbandonReason_Edit', applyTo: '#tfAbandonReason', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.AbandonReason_View',
            applyTo: '#tfAbandonReason',
            selector: '#resolutionEditPanel',
            applyBy: function(component, allowed) {
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
            name: 'GkhGji.DocumentsGji.Resolution.Register.PayFine.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionPayFineGrid',
            applyBy: function(component, allowed) {
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
            name: 'GkhGji.DocumentsGji.Resolution.Register.Dispute.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionDisputeGrid',
            applyBy: function(component, allowed) {
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
            name: 'GkhGji.DocumentsGji.Resolution.Register.Definition.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionDefinitionGrid',
            applyBy: function(component, allowed) {
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
            name: 'GkhGji.DocumentsGji.Resolution.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionAnnexGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentDate_Edit', applyTo: '#documentDate', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.resolutionBaseName_Edit', applyTo: '#tfBaseName', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.GisUin_Edit', applyTo: '#tfGisUin', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.OffenderWas_Edit', applyTo: '#offenderWas', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.TypeTerminationBasement_Edit', applyTo: '#cbTermination', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.RulinFio_Edit', applyTo: '#rulinFio', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.RulingNumber_Edit', applyTo: '#rulingNumber', selector: '#resolutionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Resolution.Field.RulingDate_Edit', applyTo: '#rulingDate', selector: '#resolutionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentDate_View',
            applyTo: '#documentDate',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.resolutionBaseName_View',
            applyTo: '#tfBaseName',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.GisUin_View',
            applyTo: '#tfGisUin',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.OffenderWas_View',
            applyTo: '#offenderWas',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed && component.fromMvd) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_View',
            applyTo: '#dfDeliveryDate',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_View',
            applyTo: '#cbTypeInitOrg',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.FineMunicipality_View',
            applyTo: '#tsfFineMunicipality',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else {
                        component.hide();
                        component.allowBlank = true;
                    }
                }
            
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.Official_View',
            applyTo: '#sfOfficial',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.Municipality_View',
            applyTo: '#sfMunicipality',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.Sanction_View',
            applyTo: '#cbSanction',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.TypeTerminationBasement_View',
            applyTo: '#cbTermination',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.Paided_View',
            applyTo: '#cbPaided',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.Executant_View',
            applyTo: '#cbExecutant',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.RulinFio_View',
            applyTo: '#rulinFio',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.RulingNumber_View',
            applyTo: '#rulingNumber',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.RulingDate_View',
            applyTo: '#rulingDate',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNumber_View',
            applyTo: '#tfDocumentNumber',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DateTransferSsp_View',
            applyTo: '#dfDateTransferSsp',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.DocumentNumSsp_View',
            applyTo: '#tfDocumentNumSsp',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_View',
            applyTo: '#nfPenaltyAmount',
            selector: '#resolutionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});