Ext.define('B4.view.Extract.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.enums.YesNo',
        'B4.enums.ExtractType',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'Росреестр',
    store: 'Extract',
    alias: 'widget.extractgrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.Extract');
        yesNoStore = Ext.create('Ext.data.Store', {
            fields: ['Display', 'Value'],
            data: [
                { "Display": 'Нет', "Value": false },
                { "Display": 'Да', "Value": true }
            ]
        });
        Ext.applyIf(me, {
            columnLines: true,
            //selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'actioncolumn',
                    action: 'getExtract',
                    width: 20,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/book_go.png'),
                    tooltip: 'Скачать выписку'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: 1,
                    text: 'Id',
                    hidden: false,
                    filter:
                    {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CreateDate',
                    text: 'Дата создания',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    text: 'Тип',
                    //enumName: 'B4.enums.ExtractType',
                    flex: 1,
                    renderer: function(val) {
                        return B4.enums.ExtractType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ExtractType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsParsed',
                    text: 'Обработана',
                    flex: 1,
                    renderer: function(val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsActive',
                    text: 'Активна',
                    flex: 1,
                    hidden: true,
                    renderer: function(val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                    
    },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                /*{
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }*/
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
                            xtype: 'b4updatebutton'
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});