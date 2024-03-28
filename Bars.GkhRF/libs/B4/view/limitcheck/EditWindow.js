Ext.define('B4.view.limitcheck.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.limitcheck.FinSourceGrid',
        
        'B4.enums.TypeProgramRequest'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minWidth: 600,
    height: 500,
    minHeight: 400,
    maxHeight: 600,
    bodyPadding: 5,
    itemId: 'limitCheckRfEditWindow',
    title: 'Настройка проверки на наличие лимитов',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'combobox', editable: false,
                    editable: false,
                    fieldLabel: 'Тип программы',
                    store: B4.enums.TypeProgramRequest.getStore(),
                    name: 'TypeProgram',
                    valueField: 'Value',
                    displayField: 'Display',
                    labelAlign: 'right',
                    labelWidth: 150
                },
                {
                    xtype: 'limitcheckfinsourcegrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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