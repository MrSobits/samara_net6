Ext.define('B4.controller.normconsumption.NormConsumptionHeating', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhCtxButtonDataExport',
        'B4.aspects.StateContextButton',
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['normconsumption.NormConsumptionHeating'],
    stores: ['normconsumption.NormConsumptionHeating'],
    views: [
        'normconsumption.Grid',
        'normconsumption.HeatingGrid'
    ],

    mainView: 'normconsumption.HeatingGrid',
    mainViewSelector: 'normconsheatinggrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'normconsheatinggrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkheditpanel',
            modelName: 'normconsumption.NormConsumption',
            editPanelSelector: 'normconsheatinggrid',
            name: 'normconseditpanelaspect'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'heatingGridAspect',
            storeName: 'normconsumption.NormConsumptionHeating',
            modelName: 'normconsumption.NormConsumptionHeating',
            gridSelector: 'normconsheatinggrid gridpanel',
            saveButtonSelector: 'normconsheatinggrid b4savebutton',
            save: function() {
                var me = this;
                var store = this.getStore();

                var mormConsId = me.controller.getContextValue(me.controller.getMainView(), 'normConsId');

                var modifiedRecs = store.getModifiedRecords();

                Ext.Array.each(modifiedRecs, function(el) {
                    el.set('RealityObject', el.get('Id'));
                    el.set('Id', el.get('ObjectId'));
                    el.set('NormConsumption', mormConsId);
                    el.phantom = !el.getId();
                });

                if (modifiedRecs.length > 0) {
                    if (this.fireEvent('beforesave', this, store) !== false) {
                        me.mask('Сохранение', this.getGrid());
                        store.sync({
                            callback: function() {
                                me.unmask();
                                store.load();
                            },
                            // выводим сообщение при ошибке сохранения
                            failure: function(result) {
                                me.unmask();
                                if (result && result.exceptions[0] && result.exceptions[0].response) {
                                    Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                                }
                            }
                        });
                    }
                }
            },
            otherActions: function (actions) {
                actions['normconsheatinggrid b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this } };
            },
            onUpdateBtnClick: function () {
                this.getStore().load();
            }
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'normconsStateButtonAspect',
            buttonSelector: '#btnState',
            stateType: 'gkh_norm_consumption',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    asp.controller.getAspect('normconseditpanelaspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkhctxbuttondataexportaspect',
            name: 'NormConsButtonExportExcelAspect',
            gridSelector: 'normconsheatinggrid gridpanel',
            buttonSelector: 'normconsheatinggrid [action="Export"]',
            controllerName: 'NormConsumptionHeating',
            actionName: 'Export',
            usePost: true,

            btnAction: function () {
                var grid = this.getGrid();

                if (grid) {
                    var store = grid.getStore();
                    var columns = grid.columns;

                    var headers = [];
                    var dataIndexes = [];

                    Ext.each(columns, function (res) {
                        if (!res.hidden && res.header != "&#160;" && (res.dataIndex || res.dataExportAlias)) {
                            var dataIndex = res.dataIndex || res.dataExportAlias,
                                index = dataIndex.indexOf(".");
                            headers.push(res.text || res.header);
                            dataIndexes.push(index >= 0 ? dataIndex.substring(0, index) : dataIndex);
                        }
                    });

                    var params = {};

                    if (headers.length > 0) {
                        Ext.apply(params, { headers: Ext.encode(headers), dataIndexes: Ext.encode(dataIndexes) });
                    }

                    if (store.sortInfo != null) {
                        Ext.apply(params, {
                            sort: store.sortInfo.field,
                            dir: store.sortInfo.direction
                        });
                    }

                    Ext.apply(params, store.lastOptions.params);

                    if (this.usePost) {
                        this.downloadViaPost(params);
                    } else {
                        this.downloadViaGet(params);
                    }
                }
            }
        },
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function (id) {

        var me = this,
            view = me.getMainView() || Ext.widget('normconsheatinggrid'),
            store = view.down('grid').getStore();
        me.bindContext(view);
        me.application.deployView(view);
        me.getAspect('normconseditpanelaspect').setData(id);
        me.setContextValue(view, 'normConsId', id);
        store.clearFilter(true);
        store.filter('normConsId', id);
    }
});