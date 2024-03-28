Ext.define('B4.view.menu.DisclosureInfoEmptyFieldsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.Url',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.DiFieldPathType'
    ],
    title: 'Журнал заполнения полей управляющей организации',
    store: 'menu.DisclosureInfoEmptyFields',
    itemId: 'disclosureInfoEmptyFieldsGrid',
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FieldName',
                    flex: 1,
                    text: 'Наименование поля',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'PathId',
                    text: 'Путь для заполнения поля',
                    flex: 4,
                    enumName: 'B4.enums.DiFieldPathType',
                    filter: true
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            dockedItems: [
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