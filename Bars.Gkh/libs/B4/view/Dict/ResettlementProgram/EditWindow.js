Ext.define('B4.view.dict.resettlementprogram.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.resettlementProgramEditWindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    height: 300,
    minWidth: 500,
    minHeight: 300,
    bodyPadding: 5,
    title: 'Программа переселения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Period',
        'B4.ux.button.Close',
        'B4.ux.button.Save',        
        'B4.enums.StateResettlementProgram',
        'B4.enums.TypeResettlementProgram',
        'B4.enums.VisibilityResettlementProgram'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 90,
                labelAlign: 'right',
                anchor: '100%'
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
                    name: 'Period',
                    fieldLabel: 'Период',
                    store: 'B4.store.dict.Period',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Состояние',
                    store: B4.enums.StateResettlementProgram.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'StateProgram'
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Тип',
                    store: B4.enums.TypeResettlementProgram.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeProgram'
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Видимость',
                    store: B4.enums.VisibilityResettlementProgram.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'Visibility'
                },
                {
                    xtype: 'checkboxfield',
                    fieldLabel: 'Используется при экспорте',
                    labelWidth: 165,
                    name: 'UseInExport',
                    boxLabelAlign: 'before'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100% -155',
                    maxLength: 1000
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