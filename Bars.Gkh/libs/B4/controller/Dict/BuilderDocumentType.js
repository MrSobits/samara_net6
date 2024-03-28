Ext.define('B4.controller.dict.BuilderDocumentType', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: [
        'dict.BuilderDocumentType'
    ],
    stores: [
        'dict.BuilderDocumentType'
    ],
    views: [
        'dict.builderdocumenttype.Grid'
    ],

    mainView: 'dict.builderdocumenttype.Grid',
    mainViewSelector: 'builderdocumenttype',

    refs: [{
        ref: 'mainView',
        selector: 'builderdocumenttypegrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'builderdocumenttype',
            storeName: 'dict.BuilderDocumentType',
            modelName: 'dict.BuilderDocumentType',
            gridSelector: 'builderdocumenttypegrid',
            listeners: {
                beforesave : function(asp, rec) {
                    var errorProps = [],
                        errorMsg = 'Не заполнены обязательные поля:',
                        modifiedRecs = rec.getModifiedRecords(),
                        anyEmptyCode = false,
                        anyEmptyName = false;

                    modifiedRecs.forEach(function(item) {
                        if (item.data.Name.length === 0) {
                            anyEmptyName = true;
                        }
                        if (item.data.Code.length === 0) {
                            anyEmptyCode = true;
                        }
                    });

                    if (anyEmptyName){
                        errorProps.push(' Наименование');
                    }
                    if (anyEmptyCode) {
                        errorProps.push(' Код');
                    }

                    if (errorProps.length > 0) {
                        Ext.Msg.alert('Предупреждение!', errorMsg  + errorProps);
                        return false;
                    }
                    return true;
                }
            },
            otherActions: function(actions) {
                actions['builderdocumenttypegrid b4deletecolumn'] = {
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
            gridSelector: 'builderdocumenttypegrid',
            permissionPrefix: 'Gkh.Dictionaries.BuilderDocumentType'
        }
    ],

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget('builderdocumenttypegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});