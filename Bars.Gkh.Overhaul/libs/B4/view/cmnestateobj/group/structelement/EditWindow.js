Ext.define('B4.view.cmnestateobj.group.structelement.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.groupelementseditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.view.dict.unitmeasure.Grid',
        'B4.store.dict.UnitMeasure',

        'B4.view.dict.normativedoc.Grid',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.NormativeDocOverhaul',
        'B4.enums.PriceCalculateBy',
        'B4.form.SelectField',
        'B4.form.ComboBox'
    ],

    layout: 'form',
    modal: true,
    width: 550,
    bodyPadding: 5,
    title: 'Добавление/редактирование конструктивного элемента',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 165
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'numberfield',
                    name: 'ReformCode',
                    fieldLabel: 'Код Реформы ЖКХ',
                    minValue: 0,
                    maxLength: 10,
                    hideTrigger: true,
                    allowDecimals: false,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Единица измерения',
                    name: 'UnitMeasure',
                    

                    store: 'B4.store.dict.UnitMeasure',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'LifeTime',
                    fieldLabel: 'Срок эксплуатации',
                    minValue: 0,
                    hideTrigger: true,
                    allowDecimals: true,
                    negativeText: 'Значение не может быть отрицательным',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'LifeTimeAfterRepair',
                    fieldLabel: 'Срок эксплуатации после ремонта',
                    minValue: 0,
                    hideTrigger: true,
                    allowDecimals: true,
                    negativeText: 'Значение не может быть отрицательным',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Нормативный документ',
                    name: 'NormativeDoc',
                    

                    store: 'B4.store.dict.NormativeDocOverhaul',
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'MutuallyExclusiveGroup',
                    fieldLabel: 'Группа взаимоисключаемости',
                    maxLength: 300
                },
                {
                     xtype: 'b4combobox',
                     name: 'CalculateBy',
                     fieldLabel: 'Считать по',
                     labelWidth: 167,
                     items: B4.enums.PriceCalculateBy.getItems(),
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