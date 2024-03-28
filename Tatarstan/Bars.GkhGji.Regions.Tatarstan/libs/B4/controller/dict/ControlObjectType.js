Ext.define('B4.controller.dict.ControlObjectType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.ControlObjectType'],
    stores: ['dict.ControlObjectType'],

    views: ['dict.controlobjecttype.Grid'],

    mainView: 'dict.controlobjecttype.Grid',
    mainViewSelector: 'controlobjecttype',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            name: 'controlobjecttypeinlinegridaspect',
            gridSelector: 'controlobjecttype',
            permissionPrefix: 'GkhGji.Dict.ControlObjectType'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'controlobjecttype',
            storeName: 'dict.ControlObjectType',
            modelName: 'dict.ControlObjectType',
            requiredFields: true
        }
    ],
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        
        me.application.deployView(view);
        view.getStore().load();
    }
});