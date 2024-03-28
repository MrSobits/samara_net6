Ext.define('B4.view.devices.ViewPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'border',
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    style: 'background: none repeat scroll 0 0 #DFE9F6',
    title: 'Приборы учёта',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging'
    ],
    alias: 'widget.disinfodevicespanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти в Реестр жилых домов/паспорт дома/Состояние приборного учёта в здании</span>'
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsDevicesGrid',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Общедомовые приборы учета',
                            items: [
                                {
                                    xtype: 'grid',
                                    store:
                                        {
                                            fields: [
                                                'TypeCommResourse',
                                                'ExistMeterDevice',
                                                'InterfaceType',
                                                'UnutOfMeasure',
                                                'InstallDate',
                                                'CheckDate',
                                                'Number'
                                            ]
                                        },
                                    border: false,
                                    columnLines: true,
                                    itemId: 'disinfodevicegrid',
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Номер прибора учета',
                                            dataIndex: 'Number',
                                            flex: 1,
                                            width: 150
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Вид коммунального ресурса',
                                            dataIndex: 'TypeCommResourse',
                                            flex: 1,
                                            width: 250
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Наличие прибора учета',
                                            dataIndex: 'ExistMeterDevice',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Тип интерфейса',
                                            dataIndex: 'InterfaceType',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Единица измерения',
                                            dataIndex: 'UnutOfMeasure',
                                            flex: 1,
                                            width: 100
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            text: 'Дата ввода в эксплуатацию',
                                            dataIndex: 'InstallDate',
                                            flex: 1,
                                            width: 130,
                                            format: 'd.m.Y',
                                            filter: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            text: 'Даты поверки/замены',
                                            dataIndex: 'CheckDate',
                                            flex: 1,
                                            width: 130,
                                            format: 'd.m.Y',
                                            filter: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                operand: CondExpr.operands.eq
                                            }
                                        }
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
                                                    columns: 1,
                                                    items: [
                                                        {
                                                            xtype: 'b4updatebutton'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});
