Ext.define('B4.view.program.PublishProgramDateEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    alias: 'widget.publishprogramdateeditwin',
    title: 'Дата опубликования номера',
    modal: true,
    width: 395,
    bodyPadding: 5,
    layout: {
        align: 'stretch',
        type: 'vbox'
    },
    
    closable: false,
    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'datefield',
                    labelAlign: 'right',
                    format: 'd.m.Y',
                    name: 'PublishDate',
                    fieldLabel: 'Дата опубликования',
                    allowBlank: false,
                    labelWidth: 180
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
                                    xtype: 'b4closebutton',
                                    text: 'Отмена'
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