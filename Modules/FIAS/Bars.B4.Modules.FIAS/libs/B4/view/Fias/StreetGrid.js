Ext.define('B4.view.Fias.StreetGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        
        'B4.enums.FiasActualStatusEnum',
        'B4.enums.FiasTypeRecordEnum'
    ],

    store: 'FiasStreet',
    itemId: 'fiasStreetGrid',

    initComponent: function () {
        var me = this;

        //-----получаем статусы актуалньости и в начало добавляем элемент чтобы выбирать все записи
        var actStatus = B4.enums.FiasActualStatusEnum.getItemsWithEmpty([null, '-']);
        var actStatusFilter = Ext.create('B4.form.ComboBox', {
            operand: CondExpr.operands.eq,
            hideLabel: true,
            editable: false,
            items: actStatus,
            valueField: 'Value',
            displayField: 'Display',
            queryMode: 'local',
            triggerAction: 'all'
        });
        //-----
        
        //-----получаем типы записи и в начало добавляем элемент чтобы выбирать все записи
        var storeTypes = B4.enums.FiasTypeRecordEnum.getItemsWithEmpty([null, '-']);
        var typeRecordFilter = Ext.create('B4.form.ComboBox', {
            operand: CondExpr.operands.eq,
            hideLabel: true,
            editable: false,
            items: storeTypes,
            valueField: 'Value',
            displayField: 'Display',
            queryMode: 'local',
            triggerAction: 'all'
        });
        //-----

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                { xtype: 'b4editcolumn', scope: me },
                { xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, text: 'Наименование', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'AOGuid', width: 150, text: 'ФИАС код', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'CodeRecord', width: 150, text: 'Полный код', filter: { xtype: 'textfield'} },
                { xtype: 'gridcolumn', dataIndex: 'TypeRecord', width: 170, text: 'Тип записи', filter: typeRecordFilter,
                    renderer: function (v) { return B4.enums.FiasTypeRecordEnum.displayRenderer(v); }
                },
                { xtype: 'gridcolumn', dataIndex: 'ActStatus', width: 120, text: 'Статус актуальности', filter: actStatusFilter,
                    renderer: function (v) { return B4.enums.FiasActualStatusEnum.displayRenderer(v); }
                },
                { xtype: 'b4deletecolumn', scope: me }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        { xtype: 'b4addbutton' },
                        { xtype: 'b4updatebutton' }
                    ]
                }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: this.store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});