Ext.define('B4.controller.PriorityParam', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect'
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
        'priorityparam.Quality'
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
        'priorityparam.QuantEditWindow'
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
        { ref: 'selectGrid', selector: '#priorityparammultiselectgrid' }
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
                    model = this.getModel(record);
                    me.setFormData(new model({ Id: 0, Value: record.get('Value') }));
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
                    }
                },
                beforesave: function (me, rec) {
                    var storeSelected = me.getForm().down('priorityparammultiselectedgrid').getStore(),
                        records = storeSelected.data.items,
                        result = true,
                        value = '';

                    if (records.length == 0) {
                        B4.QuickMsg.msg('Предупреждение', 'Необходимо выбрать записи', 'warning');
                        return false;
                    }

                    Ext.each(records, function (record) {
                        var code = record.get('Code');
                        if (Ext.isEmpty(code)) {
                            return result = false;
                        }

                        if (!Ext.isEmpty(value)) {
                            value += ',';
                        }
                        value += code;

                        return true;
                    });

                    if (!result) {
                        B4.QuickMsg.msg('Предупреждение', 'Необходимо выбрать записи с непустыми кодами', 'warning');
                        return false;
                    }

                    rec.set('Code', me.controller.codeParam);
                    rec.set('Value', value);
                    
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
                'priorityparammultiselectgrid' : {
                    select: {
                        fn: function(grid, record) {
                            var selectedStore = Ext.ComponentQuery.query('#priorityparammultiselectedgrid')[0].getStore();
                            
                            if (!selectedStore.findRecord('Code', record.get('Code'), 0, false, true, true)) {
                                selectedStore.insert(0, record);
                            }
                        }
                    }
                },
                '#priorityparammultiselectedgrid': {
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

        if (record.get('Type') === 10) {
            qualityGrid.getStore().removeAll();
            qualityGrid.hide();
            multiGrid.getStore().removeAll();
            multiGrid.hide();
            
            quantGrid.getStore().load();
            quantGrid.show();
            quantGrid.setTitle(name);
        } else if (record.get('Type') === 20) {
            quantGrid.getStore().removeAll();
            quantGrid.hide();
            multiGrid.getStore().removeAll();
            multiGrid.hide();
            
            qualityGrid.getStore().load();
            qualityGrid.show();
            qualityGrid.setTitle(name);
        } else if (record.get('Type') === 30) {
            quantGrid.getStore().removeAll();
            quantGrid.hide();
            qualityGrid.getStore().removeAll();
            qualityGrid.hide();

            multiGrid.getStore().load();
            multiGrid.show();
            multiGrid.setTitle(name);
        }
    }
});