Ext.define('B4.view.person.TechnicalMistakeGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.technicalmistakegrid',
    requires: [
        'B4.store.Person',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.grid.SelectFieldEditor',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Информация о тех. ошибках',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.person.TechnicalMistake');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StatementNumber',
                    width: 100,
                    text: '№ заявления',
                    editor: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FixInfo',
                    flex: 1,
                    text: 'Описание технической ошибки',
                    editor: { xtype: 'textfield', maxLength: 500 }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'FixDate',
                    text: 'Дата исправления',
                    format: 'd.m.Y',
                    width: 100,
                    editor: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IssuedDate',
                    text: 'Дата получения',
                    format: 'd.m.Y',
                    width: 100,
                    editor: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    width: 150,
                    text: 'Должностное лицо',
                    renderer: function (val) {
                        if (val && val.Fio) {
                            return val.Fio;
                        }
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.dict.Inspector',
                        modalWindow: true,
                        textProperty: 'Fio',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'ФИО',
                                dataIndex: 'Fio',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            },
                            {
                                text: 'Должность',
                                dataIndex: 'Position',
                                width: 200,
                                filter: { xtype: 'textfield' }
                            },
                        ]
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DecisionNumber',
                    text: '№ решения',
                    width: 100,
                    editor: { xtype: 'textfield'}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DecisionDate',
                    text: 'Дата решения',
                    format: 'd.m.Y',
                    width: 100,
                    editor: { xtype: 'datefield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing',
                {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                { xtype: 'b4savebutton' }
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