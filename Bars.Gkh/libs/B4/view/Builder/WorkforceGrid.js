Ext.define('B4.view.builder.WorkforceGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Состав трудовых ресурсов',
    store: 'builder.Workforce',
    itemId: 'builderWorkforceGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 50,
                    text: 'Номер'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 75
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentQualification',
                    flex: 1,
                    text: 'Документ квалификации'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EmploymentDate',
                    text: 'Дата приема на работу',
                    format: 'd.m.Y',
                    width: 90
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 2,
                    text: 'ФИО'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    text: 'Должность'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SpecialtyName',
                    flex: 1,
                    text: 'Специальность'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InstitutionsName',
                    flex: 1,
                    text: 'Учебное заведение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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