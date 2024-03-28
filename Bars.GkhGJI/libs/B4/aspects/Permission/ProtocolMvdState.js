Ext.define('B4.aspects.permission.ProtocolMvdState', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.protocolmvdstateperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                if (allowed) component.setReadOnly(false);
                else component.setReadOnly(true);
            }
            }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit', applyTo: '#sfMunicipalityProtocolMvd', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_Edit', applyTo: '#dfDateSupplyProtocolMvd', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#protocolMvdEditPanel' },

        //ArticleLaw
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#protocolMvdArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw.Edit', applyTo: '#btnSaveArticles', selector: '#protocolMvdArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMvdArticleLawGrid',
            applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //RealityObject
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#protocolMvdRealityObjectGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMvdRealityObjectGrid',
            applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //Annex
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#protocolMvdAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#protocolMvdAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMvdAnnexGrid',
            applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit', applyTo: '#documentDate', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit', applyTo: '#organMvd', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit', applyTo: '#municipality', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit', applyTo: '#dateOffense', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit', applyTo: '#timeOffense', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit', applyTo: '#cbExecutant', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit', applyTo: '#tfPhysPerson', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit', applyTo: '#birthDate', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit', applyTo: '#birthPlace', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit', applyTo: '#serialAndNumber', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit', applyTo: '#issueDate', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit', applyTo: '#issuingAuthority', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit', applyTo: '#taPhysPersonInfo', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit', applyTo: '#company', selector: '#protocolMvdEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View',
            applyTo: '#documentDate',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View',
            applyTo: '#organMvd',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View',
            applyTo: '#municipality',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View',
            applyTo: '#dateOffense',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View',
            applyTo: '#timeOffense',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View',
            applyTo: '#cbExecutant',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View',
            applyTo: '#birthDate',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View',
            applyTo: '#birthPlace',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View',
            applyTo: '#serialAndNumber',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View',
            applyTo: '#issueDate',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View',
            applyTo: '#issuingAuthority',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View',
            applyTo: '#taPhysPersonInfo',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View',
            applyTo: '#company',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View',
            applyTo: '#tfDocumentNumber',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View',
            applyTo: '#dfDateSupplyProtocolMvd',
            selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View', applyTo: '#tfPhysPerson', selector: '#protocolMvdEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
    ]
});