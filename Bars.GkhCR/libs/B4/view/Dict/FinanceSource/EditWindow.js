Ext.define('B4.view.dict.financesource.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 600,
    minWidth: 300,
    maxWidth: 600,
    height: 500,

    bodyPadding: 5,
    itemId: 'financeSourceEditWindow',
    title: 'Разрез финансирования',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.dict.financesource.WorkGrid',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.TypeFinanceGroup',
        'B4.enums.TypeFinance'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    height: 120,
                    layout: 'form',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 200
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Группа финансирования',
                            store: B4.enums.TypeFinanceGroup.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeFinanceGroup'
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Тип разреза',
                            store: B4.enums.TypeFinance.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeFinance'
                        }
                    ]
                },
                {
                    xtype: 'financesourceworkgrid',
                    anchor: '100% -120'
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