Ext.define('B4.view.gischarge.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.store.GisCharge',
        'B4.ux.grid.filter.YesNo'
    ],

    alias: 'widget.gischargegrid',
    title: 'Отправка начислений в ГИС ГМП',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.GisCharge');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    name: 'showJson',
                    width: 20,
                    icon: 'content/img/icons/cog_go.png',
                    tooltip: 'Отобразить передаваемый json'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер документа',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    text: 'Дата документа',
                    width: 120,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateSend',
                    format: 'd.m.Y H:i',
                    width: 120,
                    text: 'Дата отправки'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsSent',
                    width: 100,
                    text: 'Отправлено',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'actioncolumn',
                    name: 'showLog',
                    text: 'Лог',
                    width: 50,
                    icon: 'content/img/searchfield-icon.png',
                    defaultRenderer: function(v, meta, record, rowIdx, colIdx, store, view) {
                        // Отображать иконку в поле, только если есть лог
                        if (Ext.isEmpty(record.get('SendLog'))) {
                            return null;
                        }
                        
                        var me = this,
                            scope = me.origScope || me,
                            v = Ext.isFunction(me.origRenderer)
                                ? me.origRenderer.apply(scope, arguments) || ''
                                : '';

                        var icon = B4.Url.content('content/img/searchfield-icon.png');
                        if (icon) {
                            v += '<img src="' + icon + '" class="x-action-col-icon x-action-col-0">';
                        }
                        return v;
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    hidden: true
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
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить сейчас',
                                    action: 'SendNow'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Загрузить сейчас',
                                    action: 'UploadNow'
                                }/*,
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            fieldLabel: 'С',
                                            name: 'DateStart',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'datefield',
                                            fieldLabel: 'По',
                                            name: 'DateEnd',
                                            format: 'd.m.Y'
                                        }
                                    ]
                                }*/
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