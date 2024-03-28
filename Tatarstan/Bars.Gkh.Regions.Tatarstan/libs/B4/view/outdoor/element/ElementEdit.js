Ext.define('B4.view.outdoor.element.ElementEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.outdoorelementeditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Enum',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.ConditionElementOutdoor',
        'B4.store.dict.ElementOutdoor'
    ],

    layout: 'anchor',
    width: 500,
    minWidth: 400,
    minHeight: 250,
    bodyPadding: 5,
    closeAction: 'destroy',
    trackResetOnLoad: true,

    mixins: ['B4.mixins.window.ModalMask'],

    title: 'Форма редактирования элемента двора',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Element',
                    fieldLabel: 'Элемент двора',
                    textProperty: 'Name',
                    store: 'B4.store.dict.ElementOutdoor',
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'ElementGroup',
                            text: 'Группа элемента',
                            enumName: 'B4.enums.ElementOutdoorGroup',
                            filter: true
                        },
                        {
                            text: 'Ед. измерения',
                            dataIndex: 'UnitMeasure',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        flex: 2
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Measure',
                            fieldLabel: 'Ед. измерения',
                            anchor: '100%',
                            readOnly: true
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            fieldLabel: 'Объем',
                            anchor: '100%',
                            allowBlank: false,
                            minValue: 0,
                            maxLength: 100,
                            regex: /^[0-9]{1,9}(,[0-9]{2})?/

                        }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    editable: false,
                    allowBlank: false,
                    fieldLabel: 'Состояние',
                    store: B4.enums.ConditionElementOutdoor.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'Condition'
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