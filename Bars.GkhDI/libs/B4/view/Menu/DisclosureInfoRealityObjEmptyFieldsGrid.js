Ext.define('B4.view.menu.DisclosureInfoRealityObjEmptyFieldsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.Url',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.DiFieldPathType'
    ],
    title: 'Журнал заполнения полей объектов в управлении',
    store: 'menu.DisclosureInfoRealityObjEmptyFields',
    itemId: 'disclosureInfoRealityObjEmptyFieldsGrid',
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
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 3,
                    text: 'Адрес объекта недвижимости',
                    filter: { xtype: 'textfield' }
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