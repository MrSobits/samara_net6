Ext.define('B4.view.actcheck.actioneditwindowbaseitem.IdentityDocInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.actcheckactionidentitydocinfofieldset',

    requires: [
        'B4.form.SelectField'
    ],
    
    title: 'Документ, удостоверяющий личность',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'b4selectfield',
            store: 'B4.store.dict.IdentityDocumentType',
            name: 'IdentityDocType',
            fieldLabel: 'Тип документа',
            editable: false,
            modalWindow: true,
            textProperty: 'Name',
            labelWidth: 90,
            labelAlign: 'right',
            columns: [
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1
                },
                {
                    text: 'Код',
                    dataIndex: 'Code',
                    flex: 1
                }
            ]
        },
        {
            xtype: 'container',
            layout: 'hbox',
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 90,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Серия',
                            name: 'IdentityDocSeries',
                            maxLength: 25
                        },
                        {
                            xtype: 'datefield',
                            name: 'IdentityDocIssuedOn',
                            fieldLabel: 'Дата выдачи',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 90,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер',
                            name: 'IdentityDocNumber',
                            maxLength: 25
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Кем выдан',
                            name: 'IdentityDocIssuedBy',
                            maxLength: 255
                        }
                    ]
                }
            ]
        }
    ]
});