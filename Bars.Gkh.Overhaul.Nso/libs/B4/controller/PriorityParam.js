Ext.define('B4.controller.PriorityParam', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhEditPanel'
    ],

    stores: [
        'priorityparam.multi.Select',
        'priorityparam.multi.Selected',
        'priorityparam.Multi',
        'priorityparam.Quant',
        'priorityparam.Quality'
    ],

    models: [
        'priorityparam.Multi',
        'priorityparam.Quant',
        'priorityparam.Quality',
        'priorityparam.Addition'
    ],

    views: [
        'priorityparam.multi.SelectGrid',
        'priorityparam.multi.SelectedGrid',
        'priorityparam.multi.EditWindow',
        'priorityparam.multi.Grid',
        'priorityparam.Panel',
        'priorityparam.Grid',
        'priorityparam.QualityGrid',
        'priorityparam.QuantGrid',
        'priorityparam.QualityEditWindow',
        'priorityparam.QuantEditWindow',
        'priorityparam.AdditionPanel'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainPanel', selector: 'priorityparampanel' },
        { ref: 'quantGrid', selector: 'priorityparamquantgrid' },
        { ref: 'qualityGrid', selector: 'priorityparamqualitygrid' },
        { ref: 'multiGrid', selector: 'priorityparammultigrid' },
        { ref: 'multiEditWindow', selector: '#priorityparammultiwindow' },
        { ref: 'selectedGrid', selector: '#priorityparammultiselectedgrid' },
        { ref: 'selectGrid', selector: '#priorityparammultiselectgrid' },
        { ref: 'additionPanel', selector: 'priorityparamadditionpanel' }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.PriorityParam.Create', applyTo: 'b4addbutton', selector: '#priorityparamquantgrid' },
                { name: 'Ovrhl.PriorityParam.Delete', applyTo: 'b4deletecolumn', selector: '#priorityparamquantgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Ovrhl.PriorityParam.Edit', applyTo: 'b4savebutton', selector: '#priorparamquantwindow' },
                { name: 'Ovrhl.PriorityParam.Edit', applyTo: 'b4savebutton', selector: '#priorparamqualitywindow' },
                { name: 'Ovrhl.PriorityParam.Edit', applyTo: 'b4savebutton', selector: '#priorparammultiwindow' },
                { name: 'Ovrhl.PriorityParam.Create', applyTo: 'b4addbutton', selector: '#priorityparammultigrid' },
                { name: 'Ovrhl.PriorityParam.Delete', applyTo: 'b4deletecolumn', selector: '#priorityparammultigrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'roAdditionPanelAspect',
            editPanelSelector: 'priorityparamadditionpanel',
            modelName: 'priorityparam.Addition'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'quantaspect',
            gridSelector: '#priorityparamquantgrid',
            editFormSelector: '#priorparamquantwindow',
            storeName: 'priorityparam.Quant',
            modelName: 'priorityparam.Quant',
            editWindowView: 'priorityparam.QuantEditWindow',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (id) {
                    me.setFormData(record);
                } else {
                    model = me.getModel(record);
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            listeners: {
                beforesave: function(me, rec) {
                    rec.set('Code', me.controller.codeParam);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'qualityaspect',
            gridSelector: '#priorityparamqualitygrid',
            editFormSelector: '#priorparamqualitywindow',
            storeName: 'priorityparam.Quality',
            modelName: 'priorityparam.Quality',
            editWindowView: 'priorityparam.QualityEditWindow',
            editRecord: function(record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (id) {
                    me.setFormData(record);
                } else {
                    model = me.getModel(record);
                    me.setFormData(new model({ Id: 0, Value: record.get('Value'), EnumDisplay: record.get('EnumDisplay') }));
                }
            },
            listeners: {
                beforesave: function (me, rec) {
                    rec.set('Code', me.controller.codeParam);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'multiaspect',
            gridSelector: '#priorityparammultigrid',
            editFormSelector: '#priorityparammultiwindow',
            storeName: 'priorityparam.Multi',
            modelName: 'priorityparam.Multi',
            editWindowView: 'priorityparam.multi.EditWindow',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (id) {
                    me.setFormData(record);
                } else {
                    model = this.getModel(record);
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var storeSelect = form.down('priorityparammultiselectgrid').getStore(),
                        storeSelected = form.down('priorityparammultiselectedgrid').getStore();

                    storeSelect.load();
                    if (record.getId()) {
                        storeSelected.load();
                    } else {
                        storeSelected.removeAll();
                    }
                },
                beforesave: function (me, rec) {
                    var storeSelected = me.getForm().down('priorityparammultiselectedgrid').getStore(),
                        records = storeSelected.data.items,
                        value = [];

                    if (records.length == 0) {
                        B4.QuickMsg.msg('Предупреждение', 'Необходимо выбрать записи', 'warning');
                        return false;
                    }

                    Ext.each(records, function (r) {
                        value.push({
                            Id: r.get('Id'),
                            Name: r.get('Name'),
                            Code: r.get('Code')
                        });
                    });

                    rec.set('Code', me.controller.codeParam);
                    rec.set('StoredValues', value);

                    return true;
                }
            }
        }
    ],

    codeParam: null,

    init: function() {
        var me = this,
            actions = {
                'priorityparamgrid': {
                    select: { fn: me.onSelectRow, scope: me }
                },
                'priorityparammultigrid b4updatebutton': {
                    click: {
                        fn: function(btn) {
                            btn.up('priorityparammultigrid').getStore().load();
                        }
                    }
                },
                'priorityparammultiselectgrid': {
                    afterrender: function (grid) {
                        grid.getStore().on('beforeload', function (store, operation) {
                            if (me.codeParam == 'CeoPointPriorityParam') {
                                operation.params.hideNotIncluded = true;
                            }
                        });
                    },
                    select: {
                        fn: function(grid, record) {
                            var selectedStore = Ext.ComponentQuery.query('#priorityparammultiselectedgrid')[0].getStore();
                            
                            if (!selectedStore.findRecord('Code', record.get('Code'), 0, false, true, true)) {
                                selectedStore.insert(0, record);
                            }
                        }
                    }
                },
                '#priorityparammultiselectedgrid' : {
                    rowaction: {
                        fn: function (grid, action, record) {
                            if (action.toLowerCase() === 'delete') {
                                grid.getStore().remove(record);
                            }
                        }
                    }
                },
                '#priorityparammultiselectgrid b4updatebutton': {
                    click: {
                        fn: function (btn) {
                            btn.up('#priorityparammultiselectgrid').getStore().load();
                        }
                    }
                },
                '#priorityparammultiselectedgrid b4updatebutton': {
                    click: {
                        fn: function (btn) {
                            btn.up('#priorityparammultiselectedgrid').getStore().load();
                        }
                    }
                }
            };

        me.control(actions);

        me.getStore('priorityparam.Quant').on('beforeload', me.onBeforeLoad, me);
        me.getStore('priorityparam.Quality').on('beforeload', me.onBeforeLoad, me);
        me.getStore('priorityparam.Multi').on('beforeload', me.onBeforeLoad, me);
        me.getStore('priorityparam.multi.Select').on('beforeload', me.onBeforeLoad, me);
        me.getStore('priorityparam.multi.Selected').on('beforeload', me.onBeforeLoadSelected, me);

        me.callParent(arguments);
    },

    index: function() {
        var view = this.getMainPanel() || Ext.widget('priorityparampanel');

        this.bindContext(view);
        this.application.deployView(view);
    },

    onBeforeLoad: function(store, operation) {
        operation.params.code = this.codeParam;
    },

    onBeforeLoadSelected: function(store, operation) {
        operation.params.id = Ext.ComponentQuery.query('#priorityparammultiwindow')[0].getRecord().getId();
    },

    onSelectRow: function(grid, record) {
        var qualityGrid = this.getQualityGrid(),
            quantGrid = this.getQuantGrid(),
            multiGrid = this.getMultiGrid(),
            name = record.get('Name');

        this.codeParam = record.get('Id');

        var additionPanelAspect = this.getAspect('roAdditionPanelAspect');
        var additionPanel = this.getAdditionPanel();
        var me = this;
        me.mask('Загрузка...', additionPanel);
        B4.Ajax.request({
            url: B4.Url.action('GetValue', 'PriorityParamAddition'),
            params: { Code: this.codeParam },
            timeout: 9999999
        }).next(function (resp) {
            var respData = Ext.decode(resp.responseText);
            additionPanelAspect.setData(respData.Id);
            me.unmask();
        }).error(function (e) {
            this.unmask();
            Ext.Msg.alert('Ошибка!', e.message);
        });
        
        if (record.get('Type') === 10 /*quant*/) {
            this.hideGrid(qualityGrid);
            this.hideGrid(multiGrid);

            this.showGrid(quantGrid, name);
        } else if (record.get('Type') === 20 /*quality*/) {
            this.hideGrid(quantGrid);
            this.hideGrid(multiGrid);

            this.showGrid(qualityGrid, name);
        } else if (record.get('Type') === 30 /*multi*/) {
            this.hideGrid(quantGrid);
            this.hideGrid(qualityGrid);

            this.showGrid(multiGrid, name);
        }
    },
    
    hideGrid: function(grid) {
        grid.getStore().removeAll();
        grid.hide();
    },
    
    showGrid: function(grid, name) {
        grid.getStore().load();
        grid.show();
        grid.setTitle(name);
    }
});