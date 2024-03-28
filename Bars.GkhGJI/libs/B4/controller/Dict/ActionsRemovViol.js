Ext.define('B4.controller.dict.ActionsRemovViol', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ActionsRemovViol'],
    stores: ['dict.ActionsRemovViol'],

    views: ['dict.actionsremovviol.Grid'],

    mainView: 'dict.actionsremovviol.Grid',
    mainViewSelector: 'actionsRemovViolGrid',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'actionsRemovViolGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'actionsRemovViolGrid',
            permissionPrefix: 'GkhGji.Dict.ActionsRemovViol'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'actionsRemovViolGridAspect',
            storeName: 'dict.ActionsRemovViol',
            modelName: 'dict.ActionsRemovViol',
            gridSelector: 'actionsRemovViolGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('actionsRemovViolGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ActionsRemovViol').load();
    }
});