Ext.define('B4.controller.tatarstanprotocolgji.TatarstanProtocolGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.tatarstanprotocolgji.TatarstanProtocolGji'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'tatarstanprotocolgji.TatarstanProtocolGji'
    ],

    stores: [
        'tatarstanprotocolgji.TatarstanProtocolGji'
    ],

    views: [
        'tatarstanprotocolgji.MainPanel',
        'tatarstanprotocolgji.FilterPanel',
        'tatarstanprotocolgji.Grid',
        'tatarstanprotocolgji.AddWindow'
    ],

    mainView: 'tatarstanprotocolgji.MainPanel',
    mainViewSelector: 'tatarstanprotocolgjimainpanel',

    refs: [
        {
            ref: 'TatarstanProtocolGjiFilterPanel',
            selector: 'tatarstanprotocolgjifilterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'tatarstanprotocolgjiperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'tatarstanProtocolGjiGridWindowAspect',
            gridSelector: 'tatarstanprotocolgjigrid',
            editFormSelector: 'tatarstanprotocolgjiaddwindow',
            storeName: 'tatarstanprotocolgji.TatarstanProtocolGji',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGji',
            editWindowView: 'tatarstanprotocolgji.AddWindow',
            controllerEditName: 'B4.controller.tatarstanprotocolgji.Navigation',

            onSaveSuccess: function (aspect, rec) {
                //Закрываем окно после добавления новой записи
                aspect.getForm().close();

                //загружаем добавленный объект
                var model = this.controller.getModel(this.modelName);

                model.load(rec.getId(), {
                    success: function (record) {
                        //После загрузки объекта подменяем параметр и открываем вкладку
                        this.editRecord(record);
                    },
                    scope: this
                });
            },

            deleteRecord: function (record) {
                var me = this,
                    gisUin = record.get('GisUin'),
                    message;

                if(!gisUin) {
                    message = 'Вы действительно хотите удалить запись?';
                }
                else{
                    message = 'Начисление было отправлено в ГИС ГМП.</br><p style="text-align:center">Удалить документ?</p>'
                }

                Ext.Msg.confirm('Удаление записи!', message, function (result) {
                    if (result == 'yes') {
                            var model = this.getModel(record),
                                rec = new model({ Id: record.getId() });
                            
                            me.mask('Удаление', B4.getBody());
                            rec.destroy()
                                .next(function () {
                                    this.fireEvent('deletesuccess', this);
                                    me.updateGrid();
                                    me.unmask();
                                }, this)
                                .error(function (result) {
                                    Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                    me.unmask();
                                }, this);
                    }
                }, me);
            },

            listeners: {
                beforerowaction: function (asp, grid, action, record) {
                    if (action.toLowerCase() === 'doubleclick') {
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'tatarstanProtocolGjiStateTransferAspect',
            gridSelector: 'tatarstanprotocolgjimainpanel tatarstanprotocolgjigrid',
            menuSelector: 'tatarstanprotocolgjigridStateMenu',
            stateType: 'gji_document_protocol_gji_rt'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'tatarstanProtocolGjiuttonExportAspect',
            gridSelector: 'tatarstanprotocolgjigrid',
            buttonSelector: 'tatarstanprotocolgjigrid [name=btnExport]',
            controllerName: 'TatarstanProtocolGji',
            actionName: 'ExportToExcel'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'tatarstanprotocolgjigrid': { 'tatarstanprotocolgjistore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanprotocolgjifilterpanel [action=updateGrid]': {
                'click': { fn: me.updateMainGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        me.updateMainGrid();
    },

    updateMainGrid: function () {
        this.getMainView().down('tatarstanprotocolgjigrid').getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getTatarstanProtocolGjiFilterPanel();
        if (filterPanel) {
            operation.params.realityObjectId = filterPanel.down('[name=RealityObject]').getValue();
            operation.params.dateStart = filterPanel.down('[name=DateStart]').getValue();
            operation.params.dateEnd = filterPanel.down('[name=DateEnd]').getValue();
        }
    }
});