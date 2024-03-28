Ext.define('B4.controller.dict.ConfigurationReferenceInformationKndTor', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ConfigurationReferenceInformationKndTor'],
    stores: ['dict.ConfigurationReferenceInformationKndTor'],

    views: ['dict.ConfigurationReferenceInformationKndTor.Grid'],

    mainView: 'dict.ConfigurationReferenceInformationKndTor.Grid',
    mainViewSelector: 'configurationreferenceinformationkndtorgrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'configurationreferenceinformationkndtorgrid',
            permissionPrefix: 'GkhGji.Dict.ConfigurationReferenceInformationKndTor'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'configurationreferenceinformationkndtorgrid',
            storeName: 'dict.ConfigurationReferenceInformationKndTor',
            modelName: 'dict.ConfigurationReferenceInformationKndTor',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validType = true,
                        validValue = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (validType && !me.validate(rec, 'Type')) {
                            validType = false;
                        }
                        if (validValue && !me.validate(rec, 'Value')) {
                            validValue = false;
                        }
                    });

                    if (!(validType && validValue)) {
                        var commonMessagePart = ' Это поле обязательно для заполнения<br>'
                        var errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (validType ? '' : '<b>Тип справочника:</b>' + commonMessagePart) +
                            (validValue ? '' : '<b>Значение:</b>' + commonMessagePart);

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