Ext.define('B4.view.actcheck.docrequestaction.RequestInfoEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.RequestInfoType'
    ],
    
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    itemId: 'docRequestActionRequestInfoEditWindow',
    title: 'Сведение',
    
    width: 500,
    bodyPadding: 5,
    modal: true,
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'RequestInfoType',
                    enumName: 'B4.enums.RequestInfoType',
                    fieldLabel: 'Тип сведений',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 50,
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    editable: false
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