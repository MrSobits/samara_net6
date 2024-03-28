Ext.define('B4.controller.dict.ControlListTypicalQuestion', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ControlListTypicalQuestion'],
    stores: ['dict.ControlListTypicalQuestion'],

    views: ['dict.ControlListTypicalQuestion.Grid'],

    mainView: 'dict.ControlListTypicalQuestion.Grid',
    mainViewSelector: 'controllisttypicalquestiongrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'controllisttypicalquestiongrid',
            permissionPrefix: 'GkhGji.Dict.ControlListTypicalQuestion'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'controllisttypicalquestiongrid',
            storeName: 'dict.ControlListTypicalQuestion',
            modelName: 'dict.ControlListTypicalQuestion',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validQuestion = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (validQuestion && !me.validate(rec, 'Question')) {
                            validQuestion = false;
                            return false;
                        }
                    });

                    if (!validQuestion) {
                        var commonMessagePart = ' Это поле обязательно для заполнения<br>'
                        var errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (validQuestion ? '' : '<b>Вопрос проверочного листа:</b>' + commonMessagePart);

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