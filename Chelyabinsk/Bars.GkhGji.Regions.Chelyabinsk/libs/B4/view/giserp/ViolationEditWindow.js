Ext.define('B4.view.giserp.ViolationEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ERPVLawSuitType'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 600,
    bodyPadding: 10,
    itemId: 'giserpViolationEditWindow',
    title: 'Нарушение',
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
                    xtype: 'textarea',
                    name: 'VIOLATION_NOTE',
                    itemId: 'tfVIOLATION_NOTE',
                    fieldLabel: 'Нарушение',
                    allowBlank: false,
                    disabled: false,
                    flex: 1,
                    editable: true
                },
                {
                    xtype: 'textfield',
                    name: 'VIOLATION_ACT',
                    fieldLabel: 'Реквизиты акта',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    name: 'CODE',
                    fieldLabel: 'Реквизиты предписания',
                    maxLength: 255
                },
                {
                    xtype: 'datefield',
                    flex: 1,
                    name: 'DATE_APPOINTMENT',
                    fieldLabel: 'Дата предписания',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'combobox',
                    name: 'VLAWSUIT_TYPE_ID',
                    fieldLabel: 'Вид сведений',
                    displayField: 'Display',
                    itemId: 'cbHasViolations',
                    flex: 1,
                    store: B4.enums.ERPVLawSuitType.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false
                }  
                //
                
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