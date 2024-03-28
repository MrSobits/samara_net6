Ext.define('B4.view.prescription.CloseDocAddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.prescrclosedocaddwin',
    requires: [
        'B4.enums.PrescriptionDocType',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo'
    ],

    modal: true,
    width: 400,
    height: 180,

    title: 'Документ',

    defaults: {
        margin: '5 5 5 5',
        width: 380,
        labelAlign: 'right'
    },

    items: [
        {
            xtype: 'hidden',
            name: 'Id',
            margin: '0 0 0 0'
        },
        {
            xtype: 'hidden',
            name: 'Prescription',
            margin: '0 0 0 0'
        },
        {
            xtype: 'b4enumcombo',
            name: 'DocType',
            enumName: 'B4.enums.PrescriptionDocType',
            fieldLabel: 'Тип документа'
        },
        {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: 'Наименование',
            maxLength: 300
        },
        {
            xtype: 'datefield',
            name: 'Date',
            fieldLabel: 'Дата ',
            format: 'd.m.Y'
        },
        {
            xtype: 'b4filefield',
            name: 'File',
            fieldLabel: 'Файл',
            allowBlank: false
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