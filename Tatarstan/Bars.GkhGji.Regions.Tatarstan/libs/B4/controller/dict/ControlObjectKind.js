Ext.define('B4.controller.dict.ControlObjectKind', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ControlObjectKind'],
    stores: ['dict.ControlObjectKind'],

    views: ['dict.controlobjectkind.Grid'],

    mainView: 'dict.controlobjectkind.Grid',
    mainViewSelector: 'controlobjectkind',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            name: 'controlobjectkindinlinegridaspect',
            gridSelector: 'controlobjectkind',
            permissionPrefix: 'GkhGji.Dict.ControlObjectKind'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'controlobjectkind',
            storeName: 'dict.ControlObjectKind',
            modelName: 'dict.ControlObjectKind',
            requiredFields: true
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});