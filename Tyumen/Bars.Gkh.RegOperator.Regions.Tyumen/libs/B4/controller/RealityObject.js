Ext.define('B4.controller.RealityObject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.RealityObject',
        'B4.aspects.StateContextMenu',
        'B4.aspects.fieldrequirement.RealityObject',
        'Ext.ux.data.PagingMemoryProxy'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },
    models: ['RealityObject', 'paysize.RetPreview'],
    stores: ['view.ViewRealityObject', 'paysize.RetPreview'],
    views: [
        'realityobj.Grid',
        'realityobj.AddWindow',
        'paysize.UpdateRetPreviewGrid',
        'paysize.UpdateRetPreviewWindow',
        'SendAccessRequestWindow',
    ],

    mainView: 'realityobj.Grid',
    mainViewSelector: 'realityobjGrid',
    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjGrid'
        },
        {
            ref: 'sendRequestPanel',
            selector: 'sendaccessrequestwin'
        },
    ],

    aspects: [
        {
            xtype: 'realityobjfieldrequirement'
        },
        {
            xtype: 'realityobjperm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'realityobjStateTransferAspect',
            gridSelector: 'realityobjGrid',
            menuSelector: 'realityobjGridStateMenu',
            stateType: 'gkh_real_obj'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'realityobjGridWindowAspect',
            gridSelector: 'realityobjGrid',
            editFormSelector: '#realityobjAddWindow',
            storeName: 'view.ViewRealityObject',
            modelName: 'RealityObject',
            editWindowView: 'realityobj.AddWindow',
            controllerEditName: 'B4.controller.realityobj.Navi',

            listeners: {
                rendermap: function(asp) {
                    asp.controller.loadMap(asp);
                }
            },

            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                    case 'doubleclick':
                    case 'edit':
                        this.editRecord(record);
                        break;
                    case 'delete':
                        this.deleteRecord(record);
                        break;
                    case 'map':
                        this.onClickActionMap(record);
                        break;
                    }
                }
            },

            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;


                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('realityobjectedit/{0}', id));
                    }
                    else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                }
                else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },

            otherActions: function (actions) {
                var me = this;
                actions[me.gridSelector + ' #cbShowDemolished'] = { 'change': { fn: me.cbChange, scope: me } };
                actions[me.gridSelector + ' #cbShowIndividual'] = { 'change': { fn: me.cbChange, scope: me } };
                actions[me.gridSelector + ' #cbShowBlockedBuilding'] = { 'change': { fn: me.cbChange, scope: me } };
                actions[me.gridSelector + ' #cbShowWoDeliveryAgent'] = { 'change': { fn: me.cbShowWoDeliveryAgentChange, scope: me } };
                actions[me.gridSelector + ' #deliveryAgentSelect'] = { 'change': { fn: me.deliveryAgentSelectChange, scope: me } };
            },

            cbChange: function() {
                this.controller.getStore('view.ViewRealityObject').load();
            },

            cbShowWoDeliveryAgentChange: function (checkbox, newValue) {
                var grid = Ext.ComponentQuery.query('realityobjGrid')[0];
                if (newValue == true) {
                    grid.down('#cbShowDemolished').setValue(true);
                    grid.down('#cbShowIndividual').setValue(true);
                    grid.down('#cbShowBlockedBuilding').setValue(true);
                    grid.down('#deliveryAgentSelect').setValue(null);
                    grid.down('#deliveryAgentSelect').setDisabled(true);
                } else {
                    grid.down('#deliveryAgentSelect').setDisabled(false);
                }
                this.controller.getStore('view.ViewRealityObject').load();
            },

            deliveryAgentSelectChange: function() {
                this.controller.getStore('view.ViewRealityObject').load();
            },

            onClickActionMap: function (record) {
                var me = this;

                me.record = record;
                me.controller.application.redirectTo(Ext.String.format('map/{0}', record.get('Id')));
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'realityObjectButtonExportAspect',
            gridSelector: 'realityobjGrid',
            buttonSelector: 'realityobjGrid #btnExport',
            controllerName: 'RealityObject',
            actionName: 'Export',
            usePost: true,
            exportName: 'Regop RealityObjectDataExport',
            downloadViaPost: function (params) {

                if (this.exportName) {
                    params.exportName = this.exportName;
                }

                var action = B4.Url.action('/' + this.controllerName + '/' + this.actionName) + '?_dc=' + (new Date().getTime()),
                    form,
                    r = /"/gm,
                    inputs = [];

                Ext.iterate(params, function (key, value) {
                    if (!value) {
                        return;
                    }

                    if (Ext.isArray(value)) {
                        Ext.each(value, function (item) {
                            inputs.push({ tag: 'input', type: 'hidden', name: key, value: item.toString().replace(r, "&quot;") });
                        });
                    } else {
                        inputs.push({ tag: 'input', type: 'hidden', name: key, value: value.toString().replace(r, "&quot;") });
                    }
                });

                form = Ext.DomHelper.append(document.body, { tag: 'form', action: action, method: 'POST', target: '_blank' });
                Ext.DomHelper.append(form, inputs);

                form.submit();
                form.remove();
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'updateTypesPrevButtonExportAspect',
            gridSelector: 'updateretpreviewgrid',
            buttonSelector: 'updateretpreviewwindow #btnExport',
            controllerName: 'RealEstateType',
            actionName: 'UpdateTypesPreviewExport'
        }
    ],

    init: function () {
        var me = this;
        me.getStore('view.ViewRealityObject').on('beforeload', me.onBeforeLoad, me);

        me.control({
            'realityobjGrid button[action=UpdateRoTypes]': {
                'click': {
                    fn: me.onClickUpdateRoTypesPreview,
                    scope: me
                }
            },
            'updateretpreviewwindow[location=ro] button[action=UpdateRoTypes]': {
                'click': {
                    fn: me.onClickUpdateRoTypes,
                    scope: me
                }
            },
            'sendaccessrequestwin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('sendaccessrequestwin').close();
                    },
                    scope: me
                }
            },
            'realityobjGrid actioncolumn[action="sendrequestemail"]': { click: { fn: this.showrequestdialog, scope: this } },
            'sendaccessrequestwin button[action="SendRequest"]': { click: { fn: this.sendrequestemail, scope: this } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView(),
            firstTime = view === null;

        view = view || Ext.widget('realityobjGrid');

        me.bindContext(view);
        me.application.deployView(view);

        if (firstTime) {
            view.getStore('view.ViewRealityObject').load();

            B4.Ajax.request({
                url: B4.Url.action('GetParams', 'GkhParams')
            }).next(function (response) {

                var json = Ext.JSON.decode(response.responseText);

                var colSettlement = view.down('gridcolumn[dataIndex=Settlement]');
                if (colSettlement) {
                    if (!json.ShowStlRealityGrid) {
                        colSettlement.hide();
                    } else {
                        colSettlement.show();
                    }
                }

                var colDistrict = view.down('gridcolumn[dataIndex=District]');
                if (colDistrict) {
                    if (!json.UseAdminOkrug) {
                        colDistrict.hide();
                    } else {
                        colDistrict.show();
                    }
                }

            }).error(function () {
                Log('Ошибка получения параметров приложения');
            });
        }
    },

    onBeforeLoad: function(store, operation) {
        var me = this,
            mainView = me.getMainView();
        if (mainView) {
            operation.params.filterShowDemolished = me.getMainView().down('#cbShowDemolished').checked;
            operation.params.filterShowIndividual = me.getMainView().down('#cbShowIndividual').checked;
            operation.params.filterShowBlockedBuilding = me.getMainView().down('#cbShowBlockedBuilding').checked;
            operation.params.filterShowWoDeliveryAgent = me.getMainView().down('#cbShowWoDeliveryAgent').checked;
            operation.params.deliveryAgentSelectChange = me.getMainView().down('#deliveryAgentSelect').value;
        }
    },

    onClickUpdateRoTypesPreview: function () {
        var me = this;

        var window = Ext.create('B4.view.paysize.UpdateRetPreviewWindow');
        window.location = 'ro';
        window.show();

        me.mask('Загрузка...', window);

        B4.Ajax.request(
            {
                url: B4.Url.action('UpdateTypesPreview', 'RealEstateType'),
                params: me.params,
                timeout: 9999999
            }
        ).next(function (response) {
            var data = Ext.JSON.decode(response.responseText),
                proxy = Ext.create('Ext.ux.data.PagingMemoryProxy', {
                    enablePaging: true,
                    data: data
                }),
                grid = window.down('updateretpreviewgrid'),
                store = grid.getStore();

            store.currentPage = 1;
            store.setProxy(proxy);
            store.load();

            me.unmask();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при получении типов');
        });
    },

    onClickUpdateRoTypes: function () {
        var me = this;

        me.mask('Обновление типов домов...', B4.getBody().getActiveTab().getEl());

        B4.Ajax.request(
            {
                url: B4.Url.action('UpdateTypes', 'RealEstateType'),
                params: me.params,
                timeout: 9999999
            }).next(function () {
                var grid = Ext.ComponentQuery.query('updateretpreviewgrid')[0],
                        store = grid.getStore();
                store.removeAll();

                Ext.Msg.alert('Успешно выполнено', 'Типы домов обновлены');

                me.unmask();
            }).error(function (e) {
                Ext.Msg.alert('Ошибка', e.message || 'Ошибка при обновлении типов');
                me.unmask();
            });
    },
    showrequestdialog: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var win = Ext.widget('sendaccessrequestwin', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        win.RealityObject = rec;
        win.show();
    },
    sendrequestemail: function (btn) {

        var me = this,
            panel = me.getSendRequestPanel(),
            form = panel.getForm();

        me.mask('Отправка...', panel);
        form.submit({
            url: 'action/RequestStateContoller/SendEmail',
            params: {
                realityObjectId: panel.RealityObject.data.Id
            },
            success: function (form, action) {
                me.unmask();
                Ext.Msg.alert('Отправка', 'Запрос отправлен');

            },
            failure: function (form, action) {
                me.unmask();
                Ext.Msg.alert('Ошибка отправки!', action.result.message);
            }
        });
    }
});