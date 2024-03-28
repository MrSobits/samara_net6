Ext.define('B4.view.realestatetype.CommonParamGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.commonparameditgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.column.Delete',
        'B4.store.RealEstateTypeCommonParam',
        'Ext.grid.plugin.CellEditing',
        'B4.enums.CommonParamType',

        'B4.form.ComboBox'
    ],
   
    store: 'RealEstateTypeCommonParam',

    initComponent: function () {
        var me = this,
            store = me.commonParamStore = Ext.create('B4.base.Store', {
                fields: ['Code', 'Name', 'CommonParamType'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'CommonParams'
                },
                autoLoad: true
            });
        store.load();

        var editors = {};
        editors[me.getEditorKey('Date')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Date', {
                selectOnFocus: true
            })
        });
        editors[me.getEditorKey('Decimal')]=  Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Number', {
                selectOnFocus: true,
                allowDecimals: true,
                decimalPrecision: 2
            })
        });
        editors[me.getEditorKey('Integer')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Number', {
                selectOnFocus: true
            })
        });

        me.editors = editors;

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonParamCode',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'b4combobox',
                        store: store,
                        displayField: 'Name',
                        valueField: 'Code',
                        editable: false
                    },
                    renderer: function (val) {
                        var record;
                        if (val) {
                            record = store.findRecord('Code', val);
                            return record.get('Name');
                        }
                        return "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Min',
                    flex: 1,
                    text: 'Минимальное значение',
                    renderer: Ext.Function.bind(me.renderCell, me),
                    getEditor: Ext.Function.bind(me.getCellEditor, me)
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Max',
                    flex: 1,
                    text: 'Максимальное значение',
                    renderer: Ext.Function.bind(me.renderCell, me),
                    getEditor: Ext.Function.bind(me.getCellEditor, me)
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                {
                    ptype: 'cellediting',
                    pluginId: 'cellEditing',
                    clicksToEdit: 1
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    },

    renderCell: function (val, meta, rec) {
        var result = val;
        if (val) {
            if (Ext.isDate(val)) {
                result = Ext.util.Format.date(val, 'd.m.Y');
            } else if (this.commonParamStore.findRecord('Code', rec.get('CommonParamCode')).get('CommonParamType') == 30) {
                result = Ext.util.Format.date(Ext.Date.parse(val.substr(0, 10), 'd.m.Y'), 'd.m.Y');
            }
        }
        
        return Ext.util.Format.htmlEncode(result);
    },

    getCellEditor: function (record, column) {
        var commonParam = record.get('CommonParamCode'),
            type;
        if (Ext.isObject(commonParam)) {
            type = commonParam.CommonParamType;
        } else {
            type = this.commonParamStore.findRecord("Code",commonParam).get('CommonParamType');
        }
        return this.editors[type] || Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Text', {
                selectOnFocus: true
            })
        });
    },

    getEditorKey: function (typeName) {
        return B4.enums.CommonParamType.getStore().findRecord('Name', typeName).get('Value');
    }
});