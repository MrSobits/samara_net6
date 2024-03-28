Ext.define('B4.controller.RealityObject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.RealityObject',
        'B4.aspects.StateContextMenu',
        'B4.aspects.fieldrequirement.RealityObject',
        'Ext.ux.data.PagingMemoryProxy',
        'B4.aspects.GridEditWindow'
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
        'paysize.UpdateRetPreviewWindow'
    ],

    mainView: 'realityobj.Grid',
    mainViewSelector: 'realityobjGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjGrid'
        }
    ],

    aspects: [
        {
            xtype: 'realityobjfieldrequirement'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [                            
                {
                    name: 'Ovrhl.LongProgram.CalcFecsability.Calc', applyTo: '#btnCreateCalcFecabililty', selector: 'realityobjGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
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
            'realityobjGrid button[action=CreateCalcFecabililty]': {
                'click': {
                    fn: me.onClickCreateCalcFesabililty,
                    scope: me
                }
            },
            'updateretpreviewwindow[location=ro] button[action=UpdateRoTypes]': {
                'click': {
                    fn: me.onClickUpdateRoTypes,
                    scope: me
                }
            },
            'calculatefesabilitywindow button[action=ExecuteCalculationCalcFesabiliti]': {
                'click': {
                    fn: me.onClickExecuteCalculationCalcFesabiliti,
                    scope: me
                }
            },
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

        // Инвертируем значения фильтра, так как поле отображается, как признак УЧАСТИЯ дома в КР
        var isNotInvolvedCrFilterField = view.down('gridcolumn[dataIndex=IsNotInvolvedCr]');
        if (isNotInvolvedCrFilterField) {
            isNotInvolvedCrFilterField.filter.store.getRange().forEach(function (x) {
                var value = x.data.value;
                if (value != null) {
                    x.data.value = !value;
                }
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

    onClickCreateCalcFesabililty: function () {
        var me = this;
        var window = Ext.create('B4.view.realityobj.CalculateWindow');
        window.location = 'ro';
        window.show();
        
    },
    onClickExecuteCalculationCalcFesabiliti: function (btn) {
        var me = this,
            win = btn.up('calculatefesabilitywindow'),
           
            yearStart = win.down('#yearStart').value,
            yearEnd = win.down('#yearEnd').value;
        win.hide();

        if (!yearStart || !yearEnd) {
            Ext.Msg.alert('Сообщение', 'Годы начала и конца расчета обязательны к заполнению!');
            return;
        }

        B4.Ajax.request(
            {
                url: B4.Url.action('ExecuteCalculation', 'EconFesabilityCalc'),
                params: { yearStart: yearStart, yearEnd: yearEnd },
                timeout: 9999999
            }
        ).next(function (response) {
            me.unmask();
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);
            f
            return true;
        }).error(function () {
            me.unmask();

        });
        
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
    }
});