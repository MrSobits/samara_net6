Ext.define('B4.view.builder.WorkforceEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.Specialty',
        'B4.store.dict.Institutions',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    minWidth: 500,
    minHeight: 410,
    maxHeight: 410,
    bodyPadding: 5,
    itemId: 'builderWorkforceEditWindow',
    title: 'Состав трудовых ресурсов',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    title: 'Трудовые ресурсы',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Fio',
                            fieldLabel: 'ФИО',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Specialty',
                            fieldLabel: 'Специальность',
                            store: 'B4.store.dict.Specialty',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'Position',
                            fieldLabel: 'Должность',
                            maxLength: 100
                        },
                        {
                            xtype: 'datefield',
                            name: 'EmploymentDate',
                            fieldLabel: 'Дата приема на работу',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    title: 'Документ подтверждающий квалификацию',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentQualification',
                            fieldLabel: 'Наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата окончания',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Institutions',
                            fieldLabel: 'Учебное заведение',
                            store: 'B4.store.dict.Institutions',
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 50
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл',
                            editable: false
                        }
                    ]
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});