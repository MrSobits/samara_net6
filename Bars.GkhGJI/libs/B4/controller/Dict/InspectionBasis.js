Ext.define('B4.controller.dict.InspectionBasis', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.inspectionbasis.Grid'],

    mainView: 'dict.inspectionbasis.Grid',
    mainViewSelector: 'inspectionbasisgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspectionbasisgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'inspectionbasisgrid',
            permissionPrefix: 'GkhGji.Dict.InspectionBasis'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            gridSelector: 'inspectionbasisgrid',
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