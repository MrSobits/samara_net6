Ext.define('B4.view.manorg.contract.CommunalServiceEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.CommunalServiceName',
        'B4.enums.TypeCommunalResource',
        'B4.store.dict.contractservice.CommunalContractService',
        'B4.ux.grid.filter.YesNo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closeAction: 'hide',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 300,
    maxWidth: 800,
    width: 500,
    height: 170,
    minHeight: 170,
    bodyPadding: 5,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Service',
                    fieldLabel: 'Коммунальная услуга организации',
                    width: 150,
                    store: 'B4.store.dict.contractservice.CommunalContractService',
                    editable: true,
                    allowBlank: false,
                    textProperty: 'Name',
                    columns: [
                        {
                            dataIndex: 'Name',
                            flex: 1,
                            text: 'Наименование',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'CommunalResource',
                            flex: 1,
                            text: 'Коммунальный ресурс',
                            enumName: 'B4.enums.TypeCommunalResource',
                            filter: true
                        },
                        {
                            xtype: 'booleancolumn',
                            falseText: 'Нет',
                            trueText: 'Да',
                            dataIndex: 'IsHouseNeeds',
                            flex: 1,
                            text: 'Услуга предоставляется на ОДН',
                            filter: { xtype: 'b4dgridfilteryesno' }
                        }
                    ],
                    /*updateDisplayedText: function (data) {
                        var me = this,
                            text;

                        if (Ext.isString(data)) {
                            text = data;
                        }
                        else {
                            text = B4.enums.CommunalServiceName.displayRenderer(data && data.Name);
                        }

                        me.setRawValue.call(me, text);
                    }*/
                },
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата начала предоставления',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    fieldLabel: 'Дата окончания',
                    format: 'd.m.Y'
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