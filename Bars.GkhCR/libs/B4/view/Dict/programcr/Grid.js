Ext.define('B4.view.dict.programcr.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        
        'B4.enums.TypeVisibilityProgramCr',
        'B4.enums.TypeProgramCr',
        'B4.enums.TypeProgramStateCr'
    ],

    title: 'Программы капитального ремонта',
    store: 'dict.ProgramCr',
    alias: 'widget.programCrGrid',
    closable: true,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                  xtype: 'b4editcolumn',
                  scope: me
                },
                {
                    xtype: 'actioncolumn',
                    align: 'center',
                    width: 20,
                    icon: B4.Url.content('content/img/icons/page_copy.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'copy', rec);
                    },
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 3,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodName',
                    flex: 2,
                    text: 'Период',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeVisibilityProgramCr',
                    flex: 2,
                    text: 'Видимость',
                    renderer: function (val) { return B4.enums.TypeVisibilityProgramCr.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeVisibilityProgramCr.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeProgramStateCr',
                    flex: 2,
                    text: 'Состояние',
                    renderer: function (val) { return B4.enums.TypeProgramStateCr.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeProgramStateCr.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
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