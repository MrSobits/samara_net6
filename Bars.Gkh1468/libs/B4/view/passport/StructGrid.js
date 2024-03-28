Ext.define('B4.view.passport.StructGrid', {
    extend: 'Ext.grid.Panel',
    requires: [
       'B4.ux.button.Add',
       'B4.ux.button.Update',
       'B4.ux.grid.column.Delete',
       'B4.ux.grid.column.Edit',
       'B4.ux.grid.plugin.HeaderFilters',
       'B4.ux.grid.toolbar.Paging',
       'B4.store.passport.PassportStruct'
    ],

    alias: 'widget.structgrid',

    title: 'Реестр структур паспортов',
    closable: true,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.passport.PassportStruct');

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                { xtype: 'b4editcolumn', scope: me },
                {
                    xtype: 'actioncolumn',
                    name: 'CopyPassportBtn',
                    icon: B4.Url.content('content/img/icons/page_copy.png'),
                    width: 20
                },
                { text: 'Наименование', dataIndex: 'Name', flex: 2 },
                {
                    text: 'Тип паспорта',
                    dataIndex: 'PassportType',
                    flex: 1,
                    renderer: function (value) {
                        switch (value) {
                            case 10:
                                return 'Паспорт МКД';
                            case 20:
                                return 'Паспорт Жилого дома';
                            case 30:
                                return 'Паспорт ОКИ';
                        }
                    }
                },
                { text: 'Год начала действия', dataIndex: 'ValidFromYear', width: 140 },
                { text: 'Месяц начала действия', dataIndex: 'ValidFromMonth', width: 140 },
                { xtype: 'b4deletecolumn', scope: me }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    text: 'Экспорт',
                                    iconCls: 'icon-application-put',
                                    disabled: true,
                                    action: 'export'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Импорт',
                                    iconCls: 'icon-application-get',
                                    action: 'import'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});