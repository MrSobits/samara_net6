Ext.define('B4.controller.dict.MaterialRoof', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'materialroofgrid',
            permissionPrefix: 'Gkh.Dictionaries.MaterialRoof'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'materialroofgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.materialroof.Grid'],

    mainView: 'dict.materialroof.Grid',
    mainViewSelector: 'materialroofgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'materialroofgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('materialroofgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});