Ext.define('B4.controller.dict.ContragentRole', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: [
        'dict.ContragentRole'
    ],
    stores: [
        'dict.ContragentRole'
    ],
    views: [
        'dict.contragentrole.Grid'
    ],

    mainView: 'dict.contragentrole.Grid',
    mainViewSelector: 'contragentrolegrid',

    refs: [{
        ref: 'mainView',
        selector: 'contragentrolegrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'contragentrole',
            storeName: 'dict.ContragentRole',
            modelName: 'dict.ContragentRole',
            gridSelector: 'contragentrolegrid',
            listeners: {
                beforesave : function(asp, rec) {
                    var errorProps = [],
                        errorMsg = 'Не заполнены обязательные поля:',
                        modifiedRecs = rec.getModifiedRecords(),
                        anyEmptyCode = false,
                        anyEmptyName = false,
                        anyEmptyShortName = false;

                    modifiedRecs.forEach(function(item) {
                        if (item.data.Name.length === 0) {
                            anyEmptyName = true;
                        }
                        if (item.data.Code.length === 0) {
                            anyEmptyCode = true;
                        }
                        if (item.data.ShortName.length === 0) {
                            anyEmptyShortName = true;
                        }
                    });

                    if (anyEmptyCode) {
                        errorProps.push(' Код');
                    }
                    if (anyEmptyName){
                        errorProps.push(' Полное наименование');
                    }
                    if (anyEmptyShortName) {
                        errorProps.push(' Краткое наименование');
                    }

                    if (errorProps.length > 0) {
                        Ext.Msg.alert('Предупреждение', errorMsg  + errorProps);
                        return false;
                    }
                    return true;
                }
            },
            otherActions: function(actions) {
                actions['contragentrolegrid b4deletecolumn'] = {
                    click: { fn: this.onDeleteRecord, scope: this }
                }
            },
            onDeleteRecord: function(a, b, t, y, r, rec) {
                var me = this;
                window.asp = this;
                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                    if (result == 'yes') {
                        if (!rec.phantom) {
                            me.mask("Удаление");
                            rec.destroy().next(me.onSuccess, me).error(me.onError, me);
                        } else {
                            me.getGrid().getStore().remove(rec);
                        }
                   }
                });
            },
            onSuccess: function() {
                var me = this;
                me.unmask();
                me.updateGrid();
            },
            onError: function (result) {
                var me = this;
                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                me.unmask();
            }
        },
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'contragentrolegrid',
            permissionPrefix: 'Gkh.Dictionaries.ContragentRole'
        }
    ],

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget('contragentrolegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});