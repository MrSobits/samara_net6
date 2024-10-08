﻿Ext.define('B4.controller.normconsumption.NormConsumptionColdWater', {
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

    models: ['normconsumption.NormConsumptionColdWater'],
    stores: ['normconsumption.NormConsumptionColdWater'],
    views: [
        'normconsumption.Grid',
        'normconsumption.ColdWaterGrid'
    ],

    mainView: 'normconsumption.ColdWaterGrid',
    mainViewSelector: 'normconsumptiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'normconscoldwatergrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkheditpanel',
            modelName: 'normconsumption.NormConsumption',
            editPanelSelector: 'normconscoldwatergrid',
            name: 'normconscoldwatereditpanelaspect',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    asp.controller.getAspect('normconsStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'coldWaterGridAspect',
            storeName: 'normconsumption.NormConsumptionColdWater',
            modelName: 'normconsumption.NormConsumptionColdWater',
            gridSelector: 'normconscoldwatergrid gridpanel',
            saveButtonSelector: 'normconscoldwatergrid b4savebutton',
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
                actions['normconscoldwatergrid b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this } };
            },
            onUpdateBtnClick: function () {
                this.getStore().load();
            }
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'normconsStateButtonAspect',
            stateButtonSelector: '#btnState',
            stateType: 'gkh_norm_consumption',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    asp.controller.getAspect('normconscoldwatereditpanelaspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkhctxbuttondataexportaspect',
            name: 'NormConsButtonExportExcelAspect',
            gridSelector: 'normconscoldwatergrid gridpanel',
            buttonSelector: 'normconscoldwatergrid [action="Export"]',
            controllerName: 'NormConsumptionColdWater',
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
        }
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('normconscoldwatergrid'),
            store = view.down('grid').getStore();
        me.bindContext(view);
        me.application.deployView(view);
        me.getAspect('normconscoldwatereditpanelaspect').setData(id);
        me.setContextValue(view, 'normConsId', id);
        store.clearFilter(true);
        store.filter('normConsId', id);
    }
});