Ext.define('B4.controller.dict.ControlListTypicalAnswer', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ControlListTypicalAnswer'],
    stores: ['dict.ControlListTypicalAnswer'],

    views: ['dict.ControlListTypicalAnswer.Grid'],

    mainView: 'dict.ControlListTypicalAnswer.Grid',
    mainViewSelector: 'controllisttypicalanswergrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'controllisttypicalanswergrid',
            permissionPrefix: 'GkhGji.Dict.ControlListTypicalAnswer'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'controllisttypicalanswergrid',
            storeName: 'dict.ControlListTypicalAnswer',
            modelName: 'dict.ControlListTypicalAnswer',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validTorId = true,
                        validAnswer = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (validTorId && !me.validate(rec, 'TorId')) {
                            validTorId = false;
                        }
                        if (validAnswer && !me.validate(rec, 'Answer')) {
                            validAnswer = false;
                        }
                    });

                    if (!(validTorId && validAnswer)) {
                        var commonMessagePart = ' Это поле обязательно для заполнения<br>'
                        var errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (validTorId ? '' : '<b>Уникальный идентификатор:</b>' + commonMessagePart) +
                            (validAnswer ? '' : '<b>Ответ на вопрос проверочного листа:</b>' + commonMessagePart);

                        Ext.Msg.alert('Ошибка сохранения!', errormessage);
                        return false;
                    }

                    return true;
                }
            },

            validate: function (rec, field) {
                return !Ext.isEmpty(rec.get(field));
            }
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