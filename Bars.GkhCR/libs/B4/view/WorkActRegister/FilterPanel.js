Ext.define('B4.view.workactregister.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.workactregfilterpnl',
    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'workActFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        
        'B4.view.dict.programcr.Grid',
        'B4.store.dict.ProgramCr'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    border: false,
                    width: 650,
                    defaults: {
                        labelWidth: 130,
                        labelAlign: 'right'
                    },
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    items: [
                        {
                            itemId: 'sfProgram',
                            editable: false,
                            xtype: 'b4selectfield',
                            name: 'Program',
                           

                            store: 'B4.store.dict.ProgramCr',
                            width: 500,
                            fieldLabel: 'Программа'
                        },
                        {
                            width: 10,
                            xtype: 'component'
                        },
                        {
                            width: 100,
                            itemId: 'updateGrid',
                            xtype: 'b4updatebutton'
                        }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipality',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальное образование',
                    width: 500
                },
                {
                    itemId: 'sfRealityObj',
                    editable: false,
                    xtype: 'b4selectfield',
                    name: 'Address',
                    textProperty: 'Address',
                   

                    store: 'B4.store.RealityObject',
                    width: 500,
                    columns: [
                        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    fieldLabel: 'Жилой дом'
                }
            ]
        });

        me.callParent(arguments);
    }
});