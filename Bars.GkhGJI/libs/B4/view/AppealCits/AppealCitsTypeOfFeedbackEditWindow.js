Ext.define('B4.view.appealcits.AppealCitsTypeOfFeedbackEditWindow', {
    extend: 'B4.form.Window',
    closable: true,
    width: 500,
    bodyPadding: 10,
    title: 'Обратная связь',
    itemId: 'appealcitsTypeOfFeedbackEditWindow',
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.appealcits.TypeOfFeedback',
        'B4.form.FileField'
    ],    

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'TypeOfFeedback',
                    allowBlank: false,
                    fieldLabel: 'Наименование',
                    maxLength: 100,
                    store: 'B4.store.dict.TypeOfFeedback'
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    allowBlank: false,
                    editable: false,
                    fieldLabel: 'Файл'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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
                            itemId: 'statusButtonGroup',
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
