Ext.define('B4.aspects.permission.appealcits.MotivatedPresentation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.motivatedpresentationappealcitsperm',

    permissions: [
        // Вкладка "Приложения"
        {
            name: 'GkhGji.DocumentsGji.MotivatedPresentationAppealCits.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#motivatedPresentationAppealCitsAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivatedPresentationAppealCits.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#motivatedPresentationAppealCitsAnnexGrid',
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