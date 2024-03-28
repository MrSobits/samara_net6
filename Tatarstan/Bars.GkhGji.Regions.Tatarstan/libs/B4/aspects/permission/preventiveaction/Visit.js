Ext.define('B4.aspects.permission.preventiveaction.Visit', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.preventiveactionvisitpermissions',
    
    applyOn: {
        event: 'aftersetpaneldata',
        selector: '#visitEditPanel'
    },

    permissions: [
        // Вкладка "Выявленные нарушения"
        {
            name: 'GkhGji.DocumentsGji.VisitSheet.Registry.ViolationInfo.View',
            applyTo: '#visitViolationInfoGrid',
            selector: '#visitEditPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.tab.show();
                } else {
                    component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.VisitSheet.Registry.ViolationInfo.Create',
            applyTo: 'b4addbutton',
            selector: '#visitViolationInfoGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.VisitSheet.Registry.ViolationInfo.Edit',
            applyTo: 'b4editcolumn',
            selector: '#visitViolationInfoGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.VisitSheet.Registry.ViolationInfo.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#visitViolationInfoGrid'
        },

        // Вкладка "Приложения"
        {
            name: 'GkhGji.DocumentsGji.VisitSheet.Registry.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#visitAnnexGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.VisitSheet.Registry.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#visitAnnexGrid'
        }
    ]
});