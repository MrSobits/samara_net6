Ext.define('B4.view.manorg.CommunalServiceEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorgcommunalserviceeditwindow',
    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.filter.YesNo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 600,
    closeAction: 'hide',

    bodyPadding: 5,
    title: 'Коммунальная услуга',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'BilService',
                    fieldLabel: 'Главная коммунальная услуга',
                    width: 150,
                    store: 'B4.store.dict.service.BilServiceDictionaryCommunal',
                    editable: true,
                    allowBlank: false,
                    textProperty: 'ServiceName',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'ServiceName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'booleancolumn',
                            falseText: 'Нет',
                            trueText: 'Да',
                            dataIndex: 'IsOdnService',
                            flex: 1,
                            text: 'Услуга предоставляется на ОДН',
                            filter: { xtype: 'b4dgridfilteryesno' }
                        },
                        {
                            text: 'Порядок сортировки',
                            dataIndex: 'OrderNumber',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'OrderNumber',
                    fieldLabel: 'Порядок сортировки',
                    readOnly: true
                },
                {
                    xtype: 'checkbox',
                    name: 'IsOdnService',
                    fieldLabel: 'На общедомовые нужды',
                    itemId: 'isOdnService',
                    readOnly: true
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'Name',
                    width: 150,
                    allowBlank: false,
                    fieldLabel: 'Наименование',
                    enumName: 'B4.enums.CommunalServiceName'
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'Resource',
                    width: 150,
                    allowBlank: false,
                    fieldLabel: 'Коммунальный ресурс',
                    enumName: 'B4.enums.CommunalServiceResource'
                }
            ],
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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