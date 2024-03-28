Ext.define('B4.view.gisGkh.MassSignWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.gisGkhMassSignWindow',

    requires: [
        'B4.mixins.MaskBody',
        'B4.form.SelectField'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    closeAction: 'close',
    modal: true,
    rec: undefined,
    signer: undefined,

    layout: {
        type: 'fit',
        align: 'stretch'
    },

    width: 800,
    height: 200,

    title: 'Подписать запросы',

    maximizable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            xtype: 'container',
            layout: 'vbox',
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            flex: 1,
                            name: 'GisGkhRequests',
                            fieldLabel: 'Запросы в ГИС ЖКХ',
                            selectionMode: 'MULTI',
                            idProperty: 'Id',
                            textProperty: 'Id',
                            itemId: 'dfRequests',
                            store: 'B4.store.gisGkh.TaskSignStore',
                            //listeners: { windowcreated: me.onWindowCreated },
                            editable: false,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Id',
                                    flex: 1,
                                    text: 'Id',
                                    filter: {
                                        xtype: 'textfield',
                                    },
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'MessageGUID',
                                    flex: 1,
                                    text: 'MessageGUID',
                                    filter: {
                                        xtype: 'textfield',
                                    },
                                },
                                //{
                                //    xtype: 'b4enumcolumn',
                                //    enumName: 'B4.enums.YesNo',
                                //    dataIndex: 'IsExport',
                                //    text: 'Экспорт из ГИС ЖКХ',
                                //    flex: 1,
                                //    filter: true,
                                //},
                                {
                                    xtype: 'b4enumcolumn',
                                    enumName: 'B4.enums.GisGkhTypeRequest',
                                    dataIndex: 'TypeRequest',
                                    text: 'Тип запроса',
                                    flex: 1,
                                    filter: true,
                                },
                                //{
                                //    xtype: 'b4enumcolumn',
                                //    enumName: 'B4.enums.GisGkhRequestState',
                                //    dataIndex: 'RequestState',
                                //    text: 'Статус запроса',
                                //    flex: 1,
                                //    filter: true,
                                //},
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'ObjectCreateDate',
                                    flex: 1,
                                    text: 'ObjectCreateDate',
                                    format: 'd.m.Y',
                                    filter: {
                                        xtype: 'datefield',
                                        operand: CondExpr.operands.eq,
                                        format: 'd.m.Y H:i:s'
                                    },
                                },
                            ],
                            listeners: {
                                'beforeload': function (store, operation) {
                                    operation = operation || {};
                                    operation.params = operation.params || {};

                                    operation.params.RequestState = 10;
                                }
                            }
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            xtype: 'combo',
                            flex: 1,
                            editable: false,
                            queryMode: 'local',
                            itemId: 'dfCert',
                            displayField: 'SubjectName',
                            valueField: 'Certificate',
                            emptyItem: { SubjectName: '-' },
                        },
                        {
                            xtype: 'button',
                            text: 'Подписать и отправить',
                            iconCls: 'icon-accept',
                            handler: function (b) {
                                var win = b.up('gisGkhMassSignWindow');
                                win.fireEvent('createsignature', win);
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    },
});