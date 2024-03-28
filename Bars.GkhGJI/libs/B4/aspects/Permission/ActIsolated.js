Ext.define('B4.aspects.permission.ActIsolated', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actisolatedperm',
    applyOn: {
                event: 'aftersetpaneldata',
                selector: 'actisolatededitpanel'
            },
    permissions: [
        { name: 'GkhGji.DocumentsGji.ActIsolated.Edit', applyTo: 'b4savebutton', selector: 'actisolatededitpanel' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Delete', applyTo: '#btnDelete', selector: 'actisolatededitpanel' },

        //поля панели редактирования ActIsolated
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentNumber_Edit',
            applyTo: '#tfDocumentNumber',
            selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentNumber_View',
            applyTo: '#tfDocumentNumber',
            selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: 'actisolatededitpanel' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentNum_View',
            applyTo: '#nfDocumentNum',
            selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: 'actisolatededitpanel' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: 'actisolatededitpanel' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentSubNum_View',
            applyTo: '#nfDocumentSubNum',
            selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: 'actisolatededitpanel' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentDate_View',
            applyTo: '#dfDocumentDate',
            selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: 'actisolatededitpanel' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //Реквизиты
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Requisites.View', applyTo: 'actisolatedannexgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },

        //ActIsolatedAnnex
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Annex.View', applyTo: 'actisolatedannexgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Annex.Create', applyTo: 'b4addbutton', selector: 'actisolatedannexgrid' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Annex.Edit', applyTo: 'b4savebutton', selector: 'actisolatedannexeditwindow' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actisolatedannexgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActIsolatedInspectedPart
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.InspectedPart.View', applyTo: 'actisolatedinspectedpartgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.InspectedPart.Create', applyTo: 'b4addbutton', selector: 'actisolatedinspectedpartgrid' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.InspectedPart.Edit', applyTo: 'b4savebutton', selector: 'actisolatedinspectedparteditwindow' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.InspectedPart.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actisolatedinspectedpartgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActIsolatedDefinition
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Definition.View', applyTo: 'actisolateddefinitiongrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Definition.Create', applyTo: 'b4addbutton', selector: 'actisolateddefinitiongrid' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Definition.Edit', applyTo: 'b4savebutton', selector: 'actisolateddefinitioneditwindow' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Definition.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actisolateddefinitiongrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActIsolatedProvidedDoc
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.ProvidedDoc.View', applyTo: 'actisolatedprovideddocgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.ProvidedDoc.Create', applyTo: 'b4addbutton', selector: 'actisolatedprovideddocgrid' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.ProvidedDoc.Edit', applyTo: '#actProvidedDocGridSaveButton', selector: 'actisolatedprovideddocgrid' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.ProvidedDoc.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actisolatedprovideddocgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        //ActIsolatedPeriod
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Period.View', applyTo: 'actisolatedperiodgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Period.Create', applyTo: 'b4addbutton', selector: 'actisolatedperiodgrid' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Period.Edit', applyTo: 'b4savebutton', selector: 'actisolatedperiodeditwindow' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Period.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actisolatedperiodgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActIsolatedWitness
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Witness.View', applyTo: 'actisolatedwitnessgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Witness.Create', applyTo: 'b4addbutton', selector: 'actisolatedwitnessgrid' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.Witness.Edit', applyTo: '#actIsolatedWitnessSaveButton', selector: 'actisolatededitpanel' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.Witness.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actisolatedwitnessgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActIsolatedRealObj
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj.View', applyTo: 'actisolatedrealityobjectgrid', selector: 'actisolatededitpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj.Edit', applyTo: '#actRealObjEditWindowSaveButton', selector: 'actisolatedrealityobjecteditwindow' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj.Edit', applyTo: 'b4savebutton', selector: 'actisolatedeventeditwindow' },
        { name: 'GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj.Edit', applyTo: '#realObjMeasureSaveButton', selector: 'actisolatedmeasuregrid' },
        {
            name: 'GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj.Delete', applyTo: 'b4deletecolumn', selector: 'actisolatedrealityobjectgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
    ]
});