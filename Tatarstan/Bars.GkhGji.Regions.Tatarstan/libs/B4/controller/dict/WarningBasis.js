Ext.define('B4.controller.dict.WarningBasis', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.warningbasis.Grid'],

    mainView: 'dict.warningbasis.Grid',
    mainViewSelector: 'warningbasisgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'warningbasisgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'warningbasisgrid',
            permissionPrefix: 'GkhGji.Dict.WarningBasis'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            gridSelector: 'warningbasisgrid',
            listeners: {
                beforesave: function (asp, store) {
                    var modifiedRecords = store.getModifiedRecords(),
                        validate = true;

                    Ext.each(modifiedRecords, function(rec) {
                        if (!rec.get('Name') || !rec.get('Code')) {
                            validate = false;
                        }
                    });

                    if (!validate) {
                        Ext.Msg.alert('Ошибка сохранения', 'Необходимо заполнить поля');
                    }

                    return validate;
                }
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});