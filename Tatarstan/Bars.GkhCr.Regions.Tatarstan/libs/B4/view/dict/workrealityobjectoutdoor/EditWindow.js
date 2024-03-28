Ext.define('B4.view.dict.workrealityobjectoutdoor.EditWindow', {
    extend: 'B4.form.Window',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    alias: 'widget.workrealityobjectoutdoorwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    width: 500,
    bodyPadding: 5,
    title: 'Форма редактирования работы',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.unitmeasure.Grid',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.KindWorkOutdoor'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 90
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 255
                },
                {
                    xtype: 'b4selectfield',
                    name: 'UnitMeasure',
                    fieldLabel: 'Ед. измерения',
                    store: 'B4.store.dict.UnitMeasure',
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    maxLength: 10
                },
                {
                    xtype: 'combobox',
                    name: 'TypeWork',
                    editable: false,
                    fieldLabel: 'Тип работы',
                    store: B4.enums.KindWorkOutdoor.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'checkbox',
                    name: 'IsActual',
                    fieldLabel: 'Актуальна'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    flex: 1,
                    maxLength: 255
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