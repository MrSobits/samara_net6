Ext.define('B4.view.manorg.contract.AdditionServiceEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.contractservice.AdditionalContractService'
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
                    fieldLabel: 'Дополнительная услуга организации',
                    width: 150,
                    store: 'B4.store.dict.contractservice.AdditionalContractService',
                    editable: true,
                    allowBlank: false,
                    textProperty: 'Name',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Единица измерения', dataIndex: 'UnitMeasureName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
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