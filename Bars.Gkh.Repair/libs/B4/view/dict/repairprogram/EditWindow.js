Ext.define('B4.view.dict.repairprogram.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.repairProgramEditWindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 600,
    height: 505,
    bodyPadding: 5,
    itemId: 'repairProgramEditWindow',
    title: 'Программа текущего ремонта',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Period',
        
        'B4.enums.TypeProgramRepairState',
        'B4.enums.TypeVisibilityProgramRepair',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.view.dict.repairprogram.MunicipalityGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {            
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 100,
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
                            name: 'Period',
                            fieldLabel: 'Период',
                            store: 'B4.store.dict.Period',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Видимость',
                            store: B4.enums.TypeVisibilityProgramRepair.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeVisibilityProgramRepair'
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramRepairState.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgramRepairState'
                        }
                    ]
                },
                {
                    xtype: 'repprogrammunicipalitygrid',
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