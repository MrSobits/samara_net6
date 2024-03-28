Ext.define('B4.controller.dict.IdentityDocumentType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.IdentityDocumentType'],
    stores: ['dict.IdentityDocumentType'],

    views: ['dict.identitydocumenttype.Grid'],

    mainView: 'dict.identitydocumenttype.Grid',
    mainViewSelector: 'identitydocumenttypegrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'identitydocumenttypegrid',
            permissionPrefix: 'Gkh.Dictionaries.IdentityDocumentType'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'identitydocumenttypegrid',
            storeName: 'dict.IdentityDocumentType',
            modelName: 'dict.IdentityDocumentType',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validCode = true,
                        validName = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (validCode && !me.validate(rec, 'Code')) {
                            validCode = false;
                        }
                        if (validName && !me.validate(rec, 'Name')) {
                            validName = false;
                        }
                    });

                    if (!(validCode && validName)) {
                        var commonMessagePart = ' Это поле обязательно для заполнения<br>'
                        var errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (validCode ? '' : '<b>Код:</b>' + commonMessagePart) +
                            (validName ? '' : '<b>Тип документа:</b>' + commonMessagePart);

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