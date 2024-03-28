Ext.define('B4.view.entityloglight.EntityLogLightGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.UtcDate',
        'B4.store.EntityLogLight'
    ],

    title: 'История изменений',
    alias: 'widget.entityloglightgrid',

    cls: 'x-large-head',

    valueColumnConfig: null,

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.EntityLogLight');

        me.valueColumnConfig = Ext.applyIf(me.valueColumnConfig || {}, {
            xtype: 'gridcolumn',
            dataIndex: 'PropertyValue',
            flex: 1,
            text: 'Значение',
            filter: { xtype: 'textfield' }
        });

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

                me.valueColumnConfig,

                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateActualChange',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата начала действия значения',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'utcdatecolumn',
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
                    dataIndex: 'Document',
                    flex: 1,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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