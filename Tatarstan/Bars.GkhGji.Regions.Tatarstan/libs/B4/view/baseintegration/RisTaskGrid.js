Ext.define('B4.view.baseintegration.RisTaskGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.integrationristaskgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeDocumentGji',
        'B4.store.baseintegration.RisTask'
    ],

    title: 'Интеграция с ЕРКНМ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.baseintegration.RisTask');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MethodName',
                    text: 'Метод',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartTime',
                    text: 'Время начала',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndTime',
                    text: 'Время окончания',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserName',
                    text: 'Пользователь',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TaskState',
                    flex: 1,
                    text: 'Статус',
                    enumName: 'B4.enums.TaskState',
                    filter: true
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Протокол',
                    name: 'showProtocol',
                    tooltip: 'Показать протокол',
                    width: 65,
                    icon: 'content/img/searchfield-icon.png',
                    align: 'center'
                },
                {
                    xtype: 'actioncolumn',
                    text: 'XML-файл<br> запроса',
                    name: 'showRequestXml',
                    width: 75,
                    align: 'center',
                    defaultRenderer: function (v, meta, record) {
                        if (record && record.get('RequestXmlFileId') !== 0) {
                            v = me.xmlColumnRender(this, v, meta, 'Показать XML запроса');
                        }

                        return v;
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'XML-файл<br> ответа',
                    name: 'showResponseXml',
                    width: 75,
                    align: 'center',
                    defaultRenderer: function (v, meta, record) {
                        if (record && record.get('ResponseXmlFileId') !== 0) {
                            v = me.xmlColumnRender(this, v, meta, 'Показать XML ответа');
                        }

                        return v;
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Лог',
                    name: 'showLog',
                    tooltip: 'Скачать лог',
                    width: 60,
                    align: 'center',
                    icon: 'content/img/icons/disk.png'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    },

    xmlColumnRender: function(me, v, meta, tooltip) {
        var prefix = Ext.baseCSSPrefix,
            scope = me.origScope || me,
            items = me.items,
            item = items[0],
            icon = B4.Url.content('content/img/searchfield-icon.png');

        // Allow a configured renderer to create initial value (And set the other values in the "metadata" argument!)
        v = Ext.isFunction(me.origRenderer)
            ? me.origRenderer.apply(scope, arguments) || ''
            : '';

        meta.tdCls += ' ' + Ext.baseCSSPrefix + 'action-col-cell';

        // Only process the item action setup once.
        if (!item.hasActionConfiguration) {

            // Apply our documented default to all items
            item.stopSelection = me.stopSelection;
            item.disable = Ext.Function.bind(me.disableAction, me, [0], 0);
            item.enable = Ext.Function.bind(me.enableAction, me, [0], 0);
            item.hasActionConfiguration = true;
        }

        if (icon) {
            v += '<img alt="' +
                (item.altText || me.altText) +
                '" src="' +
                (icon) +
                '" class="' +
                prefix +
                'action-col-icon ' +
                prefix +
                'action-col-' +
                String(0) +
                ' ' +
                (item.disabled ? prefix + 'item-disabled' : ' ') +
                ' ' +
                (Ext.isFunction(item.getClass)
                    ? item.getClass.apply(item.scope || scope, arguments)
                    : (item.iconCls || me.iconCls || '')) +
                '"' +
                ((tooltip) ? ' data-qtip="' + tooltip + '"' : '') +
                ' />';
        }

        return v;
    }
});