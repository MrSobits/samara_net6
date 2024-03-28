Ext.define('B4.view.dict.service.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.servicedicteditwindow',
    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.TypeCommunalResource',
        'B4.store.dict.UnitMeasure'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    minWidth: 600,
    height: 500,
    minHeight: 400,
    maxHeight: 600,
    bodyPadding: 5,
    title: 'Добавление / редактирование услуги (эталонный справочник услуг)',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    height: 120,
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '5 0 5 0',
                                    anchor: '100%',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        allowBlank: false,
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            minValue: 1,
                                            allowDecimals: false,
                                            name: 'Code',
                                            allowBlank: false,
                                            fieldLabel: 'Код услуги'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'TypeService',
                                            fieldLabel: 'Тип услуги',
                                            enumName: 'B4.enums.TypeServiceGis',
                                            includeEmpty: false,
                                            enumItems: [],
                                            hideTrigger: false,
                                            ownerform: me,
                                            listeners: {
                                                change: function() {
                                                    var me = this;
                                                    var typeCommResourseComboBox = me.ownerform.down('#TypeCommunalResource');

                                                    if (me.value == 10) {
                                                        typeCommResourseComboBox.setDisabled(false);
                                                        typeCommResourseComboBox.allowBlank = false;
                                                    }
                                                    else {
                                                        typeCommResourseComboBox.setDisabled(true);
                                                        typeCommResourseComboBox.allowBlank = true;
                                                    }
                                                }
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Name',
                                            allowBlank: false,
                                            flex: 1,
                                            fieldLabel: 'Наименование'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 5 0',
                                    anchor: '100%',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        allowBlank: false,
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            itemId: 'TypeCommunalResource',
                                            name: 'TypeCommunalResource',
                                            fieldLabel: 'Вид коммунального ресурса',
                                            enumName: 'B4.enums.TypeCommunalResource',
                                            includeEmpty: true,
                                            hideTrigger: false
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'UnitMeasure',
                                            fieldLabel: 'Единица измерения',
                                            anchor: '100%',
                                            store: 'B4.store.dict.UnitMeasure',
                                            editable: false,
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsProvidedForAllHouseNeeds',
                                    margin: '2 0 0 10',
                                    boxLabel: 'Услуга предоставляется на общедомовые нужды',
                                    labelWidth: 200
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'gisServiceDictTabPanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'bilservicedictgrid'
                        },
                        {
                            xtype: 'bilnormativstoragedictgrid'
                        },
                        {
                            xtype: 'biltarifstoragedictgrid'
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