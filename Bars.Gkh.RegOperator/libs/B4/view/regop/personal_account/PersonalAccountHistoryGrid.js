Ext.define('B4.view.regop.personal_account.PersonalAccountHistoryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paccounthistorygrid',

    title: 'История изменений',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.regop.personal_account.PersonalAccountOperationLog',
        'B4.view.Control.GkhFileColumn'
    ],

    cls: 'x-large-head',

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.regop.personal_account.PersonalAccountOperationLog');

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ParameterName',
                    flex: 1,
                    text: 'Наименование параметра',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PropertyDescription',
                    flex: 1,
                    text: 'Описание измененного атрибута',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PropertyValue',
                    flex: 1,
                    text: 'Значение',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateActualChange',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата начала действия значения',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'DateApplied',
                    flex: 1,
                    text: 'Дата установки значения',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'User',
                    flex: 1,
                    text: 'Пользователь',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Причина',
                    editor: {
                        xtype: 'textfield'
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gkhfilecolumn',
                    dataIndex: 'Document',
                    flex: 1
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
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});

