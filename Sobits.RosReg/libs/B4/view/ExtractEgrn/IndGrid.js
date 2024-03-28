Ext.define('B4.view.ExtractEgrn.IndGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Физ. лица',
    store: 'ExtractEgrnRightInd',
    alias: 'widget.extractegrnindgrid',
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Surname',
                    flex: 1,
                    text: 'Фамилия',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstName',
                    flex: 1,
                    text: 'Имя',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Patronymic',
                    flex: 1,
                    text: 'Отчество',
                    hidden: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'BirthDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата рождения',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BirthPlace',
                    flex: 1,
                    text: 'Место рождения',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Snils',
                    flex: 1,
                    text: 'СНИЛС',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocIndName',
                    flex: 1,
                    text: 'Название документа',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocIndSerial',
                    flex: 1,
                    text: 'Серия',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocIndNumber',
                    flex: 1,
                    text: 'Номер',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Номер права',
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Share',
                    flex: 1,
                    text: 'Доля собственности',
                    hidden: false
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