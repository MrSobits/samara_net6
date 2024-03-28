Ext.define('B4.aspects.permission.ResolutionRospotrebnadzor', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.resolutionrospperm',

    permissions: [
        /* name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол 
    */
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentDate_View',
            applyTo: '#documentDate',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentDate_Edit',
            applyTo: '#documentDate',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentNum_View',
            applyTo: '#tfDocumentNumber',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentNum_Edit',
            applyTo: '#tfDocumentNumber',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentYear_View',
            applyTo: '#nfDocumentYear',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentYear_Edit',
            applyTo: '#nfDocumentYear',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentReason_View',
            applyTo: '#tfDocumentReason',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentReason_Edit',
            applyTo: '#tfDocumentReason',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DeliveryDate_View',
            applyTo: '#dfDeliveryDate',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DeliveryDate_Edit',
            applyTo: '#dfDeliveryDate',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.GisUin_View',
            applyTo: '#tfGisUin',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.RevocationReason_View',
            applyTo: '#tfRevocationReason',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.RevocationReason_Edit',
            applyTo: '#tfRevocationReason',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.TypeInitiativeOrg_View',
            applyTo: '#tfTypeInitiativeOrg',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.ExpireReason_View',
            applyTo: '#cbExpireReason',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.ExpireReason_Edit',
            applyTo: '#cbExpireReason',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PenaltyAmount_View',
            applyTo: '#nfPenaltyAmount',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PenaltyAmount_Edit',
            applyTo: '#nfPenaltyAmount',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.SspDocumentNum_View',
            applyTo: '#tfSspDocumentNum',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Paided_View',
            applyTo: '#cbPaided',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Paided_Edit',
            applyTo: '#cbPaided',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.TransferToSspDate_View',
            applyTo: '#dfTransferToSspDate',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.TransferToSspDate_Edit',
            applyTo: '#dfTransferToSspDate',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPerson_View',
            applyTo: '#tfPhysicalPerson',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPerson_Edit',
            applyTo: '#tfPhysicalPerson',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPersonInfo_View',
            applyTo: '#taPhysPersonInfo',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPersonInfo_Edit',
            applyTo: '#taPhysPersonInfo',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.FineMunicipality_View',
            applyTo: '#tsfFineMunicipality',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.FineMunicipality_Edit',
            applyTo: '#tsfFineMunicipality',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Official_View',
            applyTo: '#sfOfficial',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Official_Edit',
            applyTo: '#sfOfficial',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.LocationMunicipality_View',
            applyTo: '#sfLocationMunicipality',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.LocationMunicipality_Edit',
            applyTo: '#sfLocationMunicipality',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Sanction_View',
            applyTo: '#cbSanction',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Sanction_Edit',
            applyTo: '#cbSanction',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Executant_View',
            applyTo: '#cbExecutant',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Executant_Edit',
            applyTo: '#cbExecutant',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Contragent_View',
            applyTo: '#sfContragent',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    if (allowed) { component.show(); }
                    else { component.hide(); }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Contragent_Edit',
            applyTo: '#sfContragent',
            selector: '#resolutionRospotrebnadzorEditPanel',
            applayBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute.Edit',
            applyTo: 'b4savebutton',
            selector: '#resolutionRospotrebnadzorDisputeEditWindow'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute.Create',
            applyTo: 'b4addbutton',
            selector: '#resolutionRospotrebnadzorDisputeGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionRospotrebnadzorDisputeGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition.Edit',
            applyTo: 'b4savebutton',
            selector: '#resolutionRospotrebnadzorDefinitionEditWindow'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition.Create',
            applyTo: 'b4addbutton',
            selector: '#resolutionRospotrebnadzorDefinitionGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionRospotrebnadzorDefinitionGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine.Edit',
            applyTo: 'b4savebutton',
            selector: '#resolutionRospotrebnadzorPayFineEditWindow'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine.Create',
            applyTo: 'b4addbutton',
            selector: '#resolutionRospotrebnadzorPayFineGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionRospotrebnadzorPayFineGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex.Edit',
            applyTo: 'b4savebutton',
            selector: '#resolutionRospotrebnadzorAnnexEditWindow'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#resolutionRospotrebnadzorAnnexGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionRospotrebnadzorAnnexEditWindow'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw.Edit',
            applyTo: 'button[name=saveButton]',
            selector: '#resolutionRospotrebnadzorArticleLawGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw.Create',
            applyTo: 'b4addbutton',
            selector: '#resolutionRospotrebnadzorArticleLawGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#resolutionRospotrebnadzorArticleLawGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Violation.Edit',
            applyTo: 'button[name=saveButton]',
            selector: '#resolutionRospotrebnadzorViolationGrid'
        },
    ]
});