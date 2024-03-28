Ext.define('B4.view.manorg.contract.WorkServiceEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeWork',
        'B4.enums.WorkAssignment'
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
    height: 220,
    minHeight: 220,
    bodyPadding: 5,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Service',
                    fieldLabel: 'Работа/услуга',
                    width: 150,
                    store: 'B4.store.dict.contractservice.AgreementContractService',
                    editable: true,
                    allowBlank: false,
                    textProperty: 'Name',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Единица измерения',
                            dataIndex: 'UnitMeasureName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.WorkAssignment',
                            text: 'Назначение работ',
                            dataIndex: 'WorkAssignment',
                            flex: 1,
                            filter: true
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.TypeWork',
                            text: 'Тип работ',
                            dataIndex: 'TypeWork',
                            flex: 1,
                            filter: true
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    itemId: 'Type',
                    fieldLabel: 'Тип работ',
                    readOnly: true 
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'PaymentAmount',
                    fieldLabel: 'Размер платы (цена) за услуги, работы по управлению домом',
                    decimalSeparator: ',',
                    minValue: 0,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата начала предоставления',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    fieldLabel: 'Дата окончания',
                    format: 'd.m.Y',
                    allowBlank: false
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