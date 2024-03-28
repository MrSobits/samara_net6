Ext.define('B4.controller.RealityObject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.RealityObject',
        'B4.aspects.StateContextMenu',
        'B4.aspects.fieldrequirement.RealityObject'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['RealityObject'],
    stores: ['view.ViewRealityObject'],
    views: [
        'realityobj.Grid',
        'realityobj.AddWindow'
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
            deleteWithRelatedEntities: true,
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
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('realityobjectedit/{0}', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },

            otherActions: function (actions) {
                var me = this;

                actions[me.gridSelector + ' #cbShowDemolished'] = { 'change': { fn: me.cbChange, scope: me } };
                actions[me.gridSelector + ' #cbShowIndividual'] = { 'change': { fn: me.cbChange, scope: me } };
                actions[me.gridSelector + ' #cbShowBlockedBuilding'] = { 'change': { fn: me.cbChange, scope: me } };
            },

            cbChange: function() {
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
            exportName: 'RealityObjectDataExport',
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
        }
    ],

    init: function () {
        var me = this;
        me.getStore('view.ViewRealityObject').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function() {
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
                var json = Ext.JSON.decode(response.responseText),
                    colSettlement = view.down('gridcolumn[dataIndex=Settlement]'),
                    colDistrict = view.down('gridcolumn[dataIndex=District]');
                if (colSettlement) {
                    if (!json.ShowStlRealityGrid) {
                        colSettlement.hide();
                    } else {
                        colSettlement.show();
                    }
                }

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
        var mainView = this.getMainView();
        if (mainView) {
            operation.params.filterShowDemolished = mainView.down('#cbShowDemolished').checked;
            operation.params.filterShowIndividual = mainView.down('#cbShowIndividual').checked;
            operation.params.filterShowBlockedBuilding = mainView.down('#cbShowBlockedBuilding').checked;
        }
    }
});