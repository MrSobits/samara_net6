Ext.define('B4.view.dict.elementoutdoor.EditWindow', {
    extend: 'B4.form.Window',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    alias: 'widget.elementoutdoorwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    width: 800,
    height: 500,
    bodyPadding: 5,
    title: 'Элемент двора',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.unitmeasure.Grid',
        'B4.view.Control.GkhDecimalField',
        'B4.view.dict.workselementoutdoor.Grid',
        'B4.enums.ElementOutdoorGroup'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
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
                    name: 'ElementGroup',
                    editable: false,
                    fieldLabel: 'Группа элемента',
                    store: B4.enums.ElementOutdoorGroup.getStore(),
                    displayField: 'Display',
                    valueField: 'Value'
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    minHeight: 200,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'workselementoutdoorgrid',
                            flex: 1
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