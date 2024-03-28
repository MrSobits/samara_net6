Ext.define('B4.view.integrations.gis.TaskTree', {
    extend: 'Ext.tree.Panel',

    alias: 'widget.gistasktree',

    requires: [
        'B4.store.integrations.gis.TaskTreeStore',
        'B4.view.wizard.preparedata.Wizard',
        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    region: 'west',
    rootVisible: false,
    //animate: true,
    autoScroll: true,
    useArrows: true,
    loadMask: true,
    viewConfig: {
        loadMask: true
    },

    filterByStartTime: true,

    title: 'Выполнение задач',
    columns: [
        {
            xtype: 'treecolumn',
            text: 'Наименование',
            flex: 1,
            sortable: true,
            dataIndex: 'Name'
        }, {
            text: 'Автор',
            flex: 1,
            sortable: true,
            dataIndex: 'Author',
            align: 'center'
        }, {
            text: 'Время начала',
            width: 130,
            dataIndex: 'StartTime',
            sortable: true
        }, {
            text: 'Время окончания',
            width: 130,
            dataIndex: 'EndTime',
            sortable: true
        }, {
            text: 'Статус',
            width: 200,
            dataIndex: 'State',
            sortable: true
        },
        {
            text: 'Процент выполнения',
            width: 120,
            dataIndex: 'Percent',
            sortable: true
        },        
        {
            xtype: 'actioncolumn',
            align: 'center',
            text: 'Результат',
            width: 60,
            dataIndex: 'Result',
            defaultRenderer: function (v, meta, record, rowIdx, colIdx, store, view) {
                var me = this,
                    prefix = Ext.baseCSSPrefix,
                    scope = me.origScope || me,
                    items = me.items,
                    item = items[0],
                    origValue = v;


                // Allow a configured renderer to create initial value (And set the other values in the "metadata" argument!)
                v = Ext.isFunction(me.origRenderer) ? me.origRenderer.apply(scope, arguments) || '' : '';

                meta.tdCls += ' ' + Ext.baseCSSPrefix + 'action-col-cell';

                // Only process the item action setup once.
                if (!item.hasActionConfiguration) {

                    // Apply our documented default to all items
                    item.stopSelection = me.stopSelection;
                    item.disable = Ext.Function.bind(me.disableAction, me, [0], 0);
                    item.enable = Ext.Function.bind(me.enableAction, me, [0], 0);
                    item.hasActionConfiguration = true;
                }

                var icon = undefined;
                var tooltip = undefined;

                if (record.data.Type === 'Task' && origValue === true) {
                    icon = B4.Url.content('content/icon/sign_send.png');
                    tooltip = 'Подписать и отправить';
                } else if ((record.data.Type === 'PreparingDataTrigger' || record.data.Type === 'SendingDataTrigger' || record.data.Type === 'Package') && origValue === true) {
                    icon = B4.Url.content('content/icon/view.png');
                    tooltip = 'Посмотреть результат';
                }

                if (icon) {
                    v += '<img alt="' + (item.altText || me.altText) + '" src="' + (icon) +
                        '" class="' + prefix + 'action-col-icon ' + prefix + 'action-col-' + String(0) + ' ' + (item.disabled ? prefix + 'item-disabled' : ' ') +
                        ' ' + (Ext.isFunction(item.getClass) ? item.getClass.apply(item.scope || scope, arguments) : (item.iconCls || me.iconCls || '')) + '"' +
                        ((tooltip) ? ' data-qtip="' + tooltip + '"' : '') + ' />';
                }

                return v;
            }
        },
        {
            xtype: 'actioncolumn',
            align: 'center',
            text: 'Протокол',
            width: 60,
            dataIndex: 'Protocol',
            defaultRenderer: function (v, meta, record, rowIdx, colIdx, store, view) {
                var me = this,
                    prefix = Ext.baseCSSPrefix,
                    scope = me.origScope || me,
                    items = me.items,
                    item = items[0],
                    origValue = v;


                // Allow a configured renderer to create initial value (And set the other values in the "metadata" argument!)
                v = Ext.isFunction(me.origRenderer) ? me.origRenderer.apply(scope, arguments) || '' : '';

                meta.tdCls += ' ' + Ext.baseCSSPrefix + 'action-col-cell';

                // Only process the item action setup once.
                if (!item.hasActionConfiguration) {

                    // Apply our documented default to all items
                    item.stopSelection = me.stopSelection;
                    item.disable = Ext.Function.bind(me.disableAction, me, [0], 0);
                    item.enable = Ext.Function.bind(me.enableAction, me, [0], 0);
                    item.hasActionConfiguration = true;
                }

                var icon = undefined;
                var tooltip = undefined;

                if ((record.data.Type === 'PreparingDataTrigger' || record.data.Type === 'SendingDataTrigger') && origValue === true) {
                    icon = B4.Url.content('content/icon/view.png');
                    tooltip = 'Посмотреть протокол';
                }

                if (icon) {
                    v += '<img alt="' + (item.altText || me.altText) + '" src="' + (icon) +
                        '" class="' + prefix + 'action-col-icon ' + prefix + 'action-col-' + String(0) + ' ' + (item.disabled ? prefix + 'item-disabled' : ' ') +
                        ' ' + (Ext.isFunction(item.getClass) ? item.getClass.apply(item.scope || scope, arguments) : (item.iconCls || me.iconCls || '')) + '"' +
                        ((tooltip) ? ' data-qtip="' + tooltip + '"' : '') + ' />';
                }

                return v;
            }
        },
        {
            text: 'Сообщение',
            flex: 1,
            dataIndex: 'Message',
            sortable: true
        }
    ],

    listeners: {
        beforeload: function (store, operation, eOpts) {
            var params = operation.params,
                nodeId = params.node,
                node = store.getById(nodeId),                   
                me = this;

            if (me.filterByStartTime === true) {

                var dateFromCmp = me.down('[name="DateFrom"]'),
                    timeFromCmp = me.down('[name="TimeFrom"]'),
                    dateTimeFrom = me.getDateTime(dateFromCmp, timeFromCmp),
                        
                    dateToCmp = me.down('[name="DateTo"]'),
                    timeToCmp = me.down('[name="TimeTo"]'),
                    dateTimeTo = me.getDateTime(dateToCmp, timeToCmp);
                
                Ext.apply(params, {
                    dateTimeFrom: dateTimeFrom,
                    dateTimeTo: dateTimeTo
                });
            }

            if (nodeId === 'root') {
                Ext.apply(params, {
                    nodeType: 'root',
                    nodeId: '0'
                });
            } else {
                Ext.apply(params, {
                    nodeType: node.data.Type,
                    nodeId: node.data.Id
                });
            }            
        }
    },

    initComponent: function() {
        var me = this,
        store = Ext.create('B4.store.integrations.gis.TaskTreeStore');

        Ext.applyIf(me, {
            store: store
        });

        if (me.filterByStartTime === true) {
            me.dockedItems[0].items.push({
                xtype: 'panel',
                padding: '0 0 5 0',
                bodyStyle: Gkh.bodyStyle,
                border: false,
                layout: 'hbox',
                defaults: {                    
                    labelAlign: 'right',
                    labelWidth: 200,
                    flex: 1
                },
                items: [
                    {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        name: 'DateFrom',
                        fieldLabel: 'Время начала выполнения задачи с',
                        value: new Date(),
                        maxValue: new Date()
                    },
                    {
                        xtype: 'timefield',
                        name: 'TimeFrom',
                        fieldLabel: '',
                        format: 'H:i',
                        value: '00:00',
                        increment: 30
                    },
                    {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        name: 'DateTo',
                        fieldLabel: 'по',
                        labelWidth: 50,
                        value: new Date(),
                        maxValue: new Date()
                    },
                    {
                        xtype: 'timefield',
                        name: 'TimeTo',
                        fieldLabel: '',
                        format: 'H:i',
                        value: '23:30',
                        increment: 30
                    }
                ]
            });
        }

        me.callParent(arguments);
    },

    dockedItems: [
        {
            xtype: 'toolbar',
            dock: 'top',
            items: [
                {
                    xtype: 'buttongroup',
                    items: [
                        {
                            xtype: 'b4updatebutton'
                        },
                        {
                            xtype: 'b4addbutton',
                            name: 'AddTask'
                        }
                    ]
                }
            ]
        }
    ],

    getDateTime: function (dateCmp, timeCmp) {

        var dateValue = dateCmp.getValue(),
            dateStr = Ext.Date.format(dateValue, 'd.m.Y'),
            timeValue = timeCmp.getValue(),
            timeStr = Ext.Date.format(timeValue, 'H:i'),
            dateTime = Ext.Date.parse(dateStr + ' ' + timeStr, 'd.m.Y H:i');

        return dateTime;
    }
});
