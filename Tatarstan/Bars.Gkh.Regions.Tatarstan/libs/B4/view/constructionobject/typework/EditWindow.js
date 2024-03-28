Ext.define('B4.view.constructionobject.typework.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.constructionobjeditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,
    title: 'Вид работы',
    closeAction: 'destroy',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.TypeWork',
        'B4.model.dict.Work'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Work',
                    fieldLabel: 'Вид работы',                  
                    model: 'B4.model.dict.Work',
                    allowBlank: false,
                    listeners: {
                        beforeload: function(_, opts) {
                            opts.params.isConstructionWorks = true;
                        }
                    }
                },
                {
                    xtype: 'b4enumcombo',
                    disabled: true,
                    name: 'TypeWork',
                    enumName: 'B4.enums.TypeWork',
                    fieldLabel: 'Тип работы'
                },
                {
                    xtype: 'textfield',
                    name: 'UnitMeasureName',
                    disabled: true,
                    fieldLabel: 'Единица измерения'
                },
                {
                    xtype: 'numberfield',
                    name: 'YearBuilding',
                    fieldLabel: 'Год строительства',
                    minValue: 0,
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'Deadline',
                    fieldLabel: 'Контрольный срок',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'HasPsd',
                            fieldLabel: 'Наличие ПСД'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            fieldLabel: 'Объем'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'HasExpertise',
                            fieldLabel: 'Наличие экспертизы'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма (руб.)'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    maxLength: 2000,
                    flex: 1
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