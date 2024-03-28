Ext.define('B4.aspects.permission.actionisolated.MotivatedPresentation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actionisolatedmotivatedpresentationperm',

    permissions: [
        {
            name: 'GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.Register.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#motivatedPresentationActionIsolatedAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#motivatedPresentationActionIsolatedAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});