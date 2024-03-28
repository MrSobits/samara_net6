Ext.define('B4.view.version.RecordEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    alias: 'widget.versionrecordeditwin',
    title: 'Редактировать',
    modal: true,
    bodyPadding: '5 5 0 0',
    width: 400,
    height: 225,
    layout: 'form',
    closable: false,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    minValue: 2013,
                    maxValue: 2100,
                    fieldLabel: 'Плановый год',
                    name: 'Year'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование документа',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNum',
                    fieldLabel: 'Номер',
                    maxLength: 50,
                    allowBlank: true
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'От',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    editable: false,
                    possibleFileExtensions: 'pdf'
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
