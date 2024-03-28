Ext.define('B4.view.service.repair.ProviderServiceEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Новый поставщик',
    itemId: 'repairProviderServiceEditWindow',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.service.ContragentForProvider',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Provider',
                    itemId: 'sflProvider',
                    fieldLabel: 'Поставщик',
                   

                    store: 'B4.store.service.ContragentForProvider',
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'NumberContract',
                    flex: 2,
                    fieldLabel: 'Номер договора',
                    itemId: 'tfNumberContract'
                },
                {
                    xtype: 'datefield',
                    name: 'DateStartContract',
                    allowBlank: false,
                    itemId: 'dfDateStartContract',
                    fieldLabel: 'Дата заключения договора',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textareafield',
                    name: 'Description',
                    padding: '10 0 0 0',
                    fieldLabel: 'Примечание'
                },
                {
                    xtype: 'checkbox',
                    name: 'IsActive',
                    padding: '10 0 0 0',
                    fieldLabel: 'Активен'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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
