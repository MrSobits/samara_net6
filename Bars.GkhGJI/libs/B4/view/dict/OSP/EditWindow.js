Ext.define('B4.view.dict.OSP.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipality.Grid',
        'B4.store.CreditOrg',
        //   'B4.form.FiasSelectAddress',
        'B4.form.SelectField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'oSPEditWindow',
    title: 'Отделение судебных приставов',
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
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    name: 'ShortName',
                    fieldLabel: 'Краткое наименование',
                    maxLength: 300
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальный район',
                    anchor: '100%',
                    store: 'B4.store.dict.Municipality',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Town',
                            fieldLabel: 'Населенный пункт',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            name: 'Street',
                            fieldLabel: 'Улица',
                            maxLength: 100
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.CreditOrg',
                    textProperty: 'Name',
                    name: 'CreditOrg',
                    fieldLabel: 'Банк',
                    editable: false,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'BankAccount',
                            fieldLabel: 'Расчетный счет',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            name: 'KBK',
                            fieldLabel: 'КБК',
                            maxLength: 100
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