Ext.define('B4.view.dict.accountoperation.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.accountoperationwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.SelectField',
        'B4.enums.AccountOperationType'
    ],

    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Редактирование',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Код',
                    name: 'Code',
                    hideTrigger: true,
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'Type',
                    fieldLabel: 'Тип',
                    displayField: 'Display',
                    store: B4.enums.AccountOperationType.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    itemId: 'cbAccountOperationType',
                    maxWidth: 553
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