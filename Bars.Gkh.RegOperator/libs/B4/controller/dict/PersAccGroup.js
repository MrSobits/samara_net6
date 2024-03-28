Ext.define('B4.controller.dict.PersAccGroup', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    stores: ['dict.PersAccGroup'],
    models: ['dict.PersAccGroup'],

    views: [
        'dict.persaccgroup.Grid'
    ],

    mainView: 'dict.persaccgroup.Grid',
    mainViewSelector: 'persaccgroupGrid',

    mixins: { context: 'B4.mixins.Context' },

    refs: [
        {
            ref: 'mainView',
            selector: 'persaccgroupGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'persaccgroupGrid',
            permissionPrefix: 'GkhRegOp.Settings.PersAccGroup'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'persAccGroupInlineGridAspect',
            storeName: 'dict.PersAccGroup',
            modelName: 'dict.PersAccGroup',
            gridSelector: 'persaccgroupGrid',
            listeners: {
                beforesave: function(asp, rec) {
                    var errorProps = [],
                        errorMsg = 'Не заполнены обязательные поля:',
                        modifiedRecs = rec.getModifiedRecords(),
                        anyEmptyName = false;

                    modifiedRecs.forEach(function(item) {
                        if (!item.data.Name || item.data.Name.length === 0) {
                            anyEmptyName = true;
                        }
                    });

                    if (anyEmptyName) {
                        errorProps.push(' Наименование');
                    }

                    if (errorProps.length > 0) {
                        Ext.Msg.alert('Предупреждение!', errorMsg + errorProps);
                        return false;
                    }
                    return true;
                }
            },
            otherActions: function(actions) {
                actions['persaccgroupGrid b4deletecolumn'] = {
                    click: { fn: this.onDeleteRecord, scope: this }
                };
            },

            onDeleteRecord: function(a, b, t, y, r, rec) {
                var me = this;
                window.asp = this;

                if (rec.get('IsSystem') == B4.enums.YesNo.Yes) {
                    Ext.Msg.alert('Ошибка удаления', 'Нельзя удалять системные группы');
                    return;
                }

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
            onError: function(result) {
                var me = this;
                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                me.unmask();
            }
        }
    ],

    init: function() {
        var me = this;

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('persaccgroupGrid'),
            store = view.getStore();
        me.bindContext(view);
        me.application.deployView(view);
        store.load();
    }
});