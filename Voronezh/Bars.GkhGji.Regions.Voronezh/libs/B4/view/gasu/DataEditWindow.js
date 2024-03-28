Ext.define('B4.view.gasu.DataEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 600,
    bodyPadding: 10,
    itemId: 'gasuDataEditWindow',
    title: 'Показатель',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'IndexUid',
                    fieldLabel: 'Код показателя',
                    maxLength: 20
                },
                {
                    xtype: 'textfield',
                    name: 'Indexname',
                    fieldLabel: 'Наименование показателя',
                    maxLength: 500
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'UnitMeasure',
                    fieldLabel: 'Единица измерения',
                    store: 'B4.store.dict.UnitMeasure',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 },
                    { text: 'Описание', dataIndex: 'Description', flex: 1 }]
                },
                {
                    xtype: 'numberfield',
                    name: 'Value',
                    itemId: 'nfValue',
                    fieldLabel: 'Значение',
                    decimalSeparator: ',',
                    minValue: 0,
                    allowBlank: false,
                },   
              
                
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