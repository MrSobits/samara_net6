Ext.define('B4.view.suggestion.rubric.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.rubricwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 750,
    height: 400,
    bodyPadding: 5,
    
    title: 'Форма редактирования рубрики',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ExecutorType',
        'B4.view.suggestion.rubric.TypeProblemGrid',
        'B4.view.suggestion.TransitionGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 160,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                    {
                        xtype: 'hidden',
                        name: 'Id'
                    },
                    {
                        xtype: 'numberfield',
                        name: 'Code',
                        fieldLabel: 'Код',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: false,
                        minValue: 0,
                        allowBlank: false
                    },
                    {
                        xtype: 'textfield',
                        name: 'Name',
                        fieldLabel: 'Наименование',
                        allowBlank: false,
                        maxLength: 300
                    },
                    {
                        xtype: 'combobox',
                        editable: false,
                        fieldLabel: 'Тип исполнителя',
                        store: B4.enums.ExecutorType.getStore(),
                        displayField: 'Display',
                        valueField: 'Value',
                        name: 'FirstExecutorType'
                    },
                    {
                        xtype: 'checkbox',
                        fieldLabel: 'Признак актуальности',
                        name: 'isActual'
                    }]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'rubricTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'rubrictypeproblemgrid',
                            flex: 1
                        },
                        {
                            xtype: 'transitiongrid',
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