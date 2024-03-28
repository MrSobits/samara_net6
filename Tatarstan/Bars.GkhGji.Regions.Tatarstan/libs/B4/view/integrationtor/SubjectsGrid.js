Ext.define('B4.view.integrationtor.SubjectsGrid',
    {
        extend: 'B4.ux.grid.Panel',
        alias: 'widget.integrationtorsubjectsgrid',

        requires: [
            'B4.ux.button.Update',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.toolbar.Paging',
            'B4.store.integrationtor.Subject'
        ],

        title: 'Отправленные субъекты',
        closable: true,

        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.integrationtor.Subject');
            Ext.applyIf(me, {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'TorId',
                        text: 'Внешний ID',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Name',
                        text: 'Наименование',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'JuridicalAddress',
                        text: 'Юридический адрес',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Inn',
                        text: 'ИНН',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Kpp',
                        text: 'КПП',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Ogrn',
                        text: 'ОГРН',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    }
                ],
                plugins: [
                    Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                                columns: 2,
                                items: [
                                    {
                                        xtype: 'b4updatebutton',
                                        listeners: {
                                            click: function () {
                                                store.load();
                                            }
                                        }
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