Ext.define('B4.view.actionisolated.motivatedpresentation.InspectionAddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.inspectioaddwindow',
    itemId: 'inspectionAddWindow',
    title: 'Проверка по мероприятию без взаимодействия с контролируемым лицом',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeObjectAction',
        'B4.enums.TypeJurPerson',
        'B4.enums.TatarstanInspectionFormType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                allowBlank: false,
                readOnly: true
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'NumberAndDocumentDate'
                },
                {
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.TypeObjectAction',
                    name: 'TypeObject',
                    fieldLabel: 'Объект проверки',
                    listeners: {
                        change: function (field, newValue) {
                            var form = field.up('#inspectionAddWindow'),
                                typeJurPerson = form.down('textfield[name=TypeJurPerson]'),
                                jurPersonField = form.down('textfield[name=JurPerson]'),
                                personNameField = form.down('textfield[name=PersonName]'),
                                isIndividual = newValue === B4.enums.TypeObjectAction.Individual;

                            typeJurPerson.allowBlank = isIndividual;
                            jurPersonField.allowBlank = isIndividual;
                            personNameField.allowBlank = !isIndividual;
                            
                            [
                                typeJurPerson,
                                jurPersonField,
                                personNameField
                            ].forEach(function (field) {
                                field.validate();
                                field.allowBlank ? field.hide() : field.show();
                            })
                        }
                    }
                },
                {
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.TypeJurPerson',
                    name: 'TypeJurPerson',
                    fieldLabel: 'Тип контрагента'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Юридическое лицо',
                    name: 'JurPerson'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'ФИО',
                    name: 'PersonName'
                },
                {
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.TatarstanInspectionFormType',
                    name: 'TypeForm',
                    fieldLabel: 'Форма проверки',
                    includeNull: false,
                    readOnly: false
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