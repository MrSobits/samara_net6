Ext.define('B4.aspects.permission.actionisolated.ActActionIsolated', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actactionisolatedperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Edit', applyTo: 'b4savebutton', selector: '#actActionIsolatedEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Delete', applyTo: '#btnDelete', selector: '#actActionIsolatedEditPanel'},
        
        //TODO Временно ограничил функционал кнопок
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Edit', applyTo: 'gjidocumentcreatebutton', selector: '#actActionIsolatedEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Edit', applyTo: 'gkhbuttonprint', selector: '#actActionIsolatedEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Edit', applyTo: '#btnState', selector: '#actActionIsolatedEditPanel' },
        
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.ActionResult.Create', applyTo: 'b4addbutton', selector: '#actActionIsolatedRealityObjectGrid' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.ActionResult.Edit', applyTo: 'b4editcolumn', selector: '#actActionIsolatedRealityObjectGrid' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.ActionResult.Delete', applyTo: 'b4deletecolumn', selector: '#actActionIsolatedRealityObjectGrid' },

        // Вкладка "Задание/Акт/Действие"
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Action.View',
            applyTo: 'actactionisolatedactiongrid',
            selector: '#actActionIsolatedEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed && component.recordAllowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Action.Create',
            applyTo: 'b4addbutton',
            selector: '#actActionIsolatedActionGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Action.Edit',
            applyTo: 'b4editcolumn',
            selector: '#actActionIsolatedActionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Action.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#actActionIsolatedActionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        // Вкладка "Задание/Акт/Действие" окна редактирования действий
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Action.InspectionAction.Edit',
            applyTo: 'b4savebutton',
            selector: 'inspectionactactionisolatededitwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    this.setActionEditWindowDisabledItems(component, allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Action.InstrExamAction.Edit',
            applyTo: 'b4savebutton',
            selector: 'instrexamactactionisolatededitwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    this.setActionEditWindowDisabledItems(component, allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition.Create',
            applyTo: 'b4addbutton',
            selector: 'actactionisolateddefinitionpanel'
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition.View',
            applyTo: 'b4editcolumn',
            selector: 'actactionisolateddefinitionpanel'
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'actactionisolateddefinitionpanel'
        },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Annex.View', applyTo: '#actActionIsolatedAnnexGrid', selector: '#actActionIsolatedEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#actActionIsolatedAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Annex.Edit', applyTo: 'b4editcolumn', selector: '#actActionIsolatedAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ActActionIsolated.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#actActionIsolatedAnnexGrid' },
        {
            name: 'GkhGji.DocumentsGji.ActActionIsolated.AcquaintInfo.View',
            applyTo: '[name=AcquaintInfo]',
            selector: '#actActionIsolatedEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if(allowed){
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        }
    ],

    setActionEditWindowDisabledItems: function (component, allowed) {
        var form = component?.up('#actActionIsolatedActionAddWindow');

        if (component && form) {
            form.setDisabledItems(component, allowed);
        }
    }
});