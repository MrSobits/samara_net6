Ext.define('B4.view.passport.house.info.PassportPanel', {
    extend: 'Ext.panel.Panel',
    title: 'Сводный паспорт',
    
    alias: 'widget.infohousepassportpanel',
    
    requires: [
        'B4.view.passport.house.info.CombinedPassportGrid',
        'B4.ux.button.Update',
        'B4.form.ComboBox'
    ],
    
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function() {
        var me = this;
       
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'combinedpassportgrid',
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
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'State',
                            fieldLabel: "Статус паспортов",
                            editable: false,
                            width: 300,
                            url: '/State/GetListByType',
                            storeAutoLoad: false,
                            operand: CondExpr.operands.eq,
                            listeners: {
                                storebeforeload: function(dfdf, store, options) {
                                    options.params.typeId = 'houseproviderpassport';
                                },
                                storeloaded: {
                                    fn: function(field) {
                                        field.getStore().insert(0, { Id: null, Name: '-' });
                                    }
                                }
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});