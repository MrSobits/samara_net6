Ext.define('B4.view.lifts.ViewPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'border',
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    style: 'background: none repeat scroll 0 0 #DFE9F6',
    title: 'Лифты',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging'
    ],
    alias: 'widget.disinfoliftspanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти в Реестр жилых домов/паспорт дома/Специальное инженерное оборудование/Общие сведения о лифтах</span>'
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsLiftInfoGrid',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Лифты',
                            items: [
                                {
                                    xtype: 'grid',
                                    border: false,
                                    store:
                                        {
                                            fields: [
                                                'EntranceNumber',
                                                'Type',
                                                'CommissioningYear'
                                            ]
                                        },
                                    columnLines: true,
                                    itemId: 'disinfoliftgrid',
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Номер подъезда',
                                            dataIndex: 'EntranceNumber',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Тип лифта',
                                            dataIndex: 'Type',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            text: 'Год ввода в эксплуатацию (год)',
                                            dataIndex: 'CommissioningYear',
                                            flex: 1
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
