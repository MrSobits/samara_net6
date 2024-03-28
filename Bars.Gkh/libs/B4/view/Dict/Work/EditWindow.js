Ext.define('B4.view.dict.work.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,
    itemId: 'workEditWindow',
    title: 'Форма редактирования работы',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.unitmeasure.Grid',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.TypeWork'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'b4selectfield',
                    name: 'UnitMeasure',
                    fieldLabel: 'Ед. измерения',
                    store: 'B4.store.dict.UnitMeasure',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 10
                },
                {
                    xtype: 'numberfield',
                    name: 'ReformCode',
                    fieldLabel: 'Код реформы',
                    minValue: 0,
                    maxLength: 10,
                    hideTrigger: true,
                    allowDecimals: false,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'textfield',
                    name: 'GisCode',
                    fieldLabel: 'Код ГИС ЖКХ'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Normative',
                    fieldLabel: 'Норматив',
                    minValue: 0
                },
                {
                    xtype: 'checkbox',
                    name: 'Consistent185Fz',
                    fieldLabel: 'Соответствие 185 ФЗ'
                },
                {
                    xtype: 'checkbox',
                    name: 'IsPSD',
                    fieldLabel: 'Работа ПСД в КПКР'
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Тип работ',
                    store: B4.enums.TypeWork.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeWork'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    flex: 1,
                    maxLength: 500
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