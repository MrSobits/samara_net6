Ext.define('B4.view.objectcr.ArchiveMultiplyContragentSmrEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'archiveMultiplyContragentSmrEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
//  layout: { type: 'hbox', align: 'stretch' },
    width: 750,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    title: 'Ход выполнения работ контрагентом',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    editable: false,
                    fieldLabel: 'Исполнитель',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'gkhdecimalfield',
                        labelWidth: 150,
                        labelAlign: 'right',
                        margin: '0 0 5 0'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'VolumeOfCompletion',
                            fieldLabel: 'Объем выполнения'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'CostSum',
                            fieldLabel: 'Сумма расходов'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'PercentOfCompletion',
                            fieldLabel: 'Процент выполнения',
                            maxValue: 100
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});