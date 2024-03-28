Ext.define('B4.view.specialaccount.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.specialaccountEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Отчет по спецсчетам',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
         'B4.enums.AmmountMeasurement',
        'B4.form.SelectField',
        'B4.form.FileField',
    ],

    initComponent: function() {
        var me = this;
        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                       
                        
                    ]
                }
            ],

            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                         {
                             xtype: 'textfield',
                            name: 'ContragentName',
                             labelAlign: 'right',
                             labelWidth: 200,
                             readOnly: true,
                             fieldLabel: 'Контрагент',
                             flex: 1
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
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                         {
                             xtype: 'textfield',
                             name: 'Inn',
                             labelAlign: 'right',
                             labelWidth: 200,
                             readOnly: true,
                             fieldLabel: 'ИНН Контрагента',
                             flex: 1
                         },
                          {
                              xtype: 'combobox',
                              name: 'AmmountMeasurement',
                              fieldLabel: 'Рубли/тысячи',
                              displayField: 'Display',
                              store: B4.enums.AmmountMeasurement.getStore(),
                              valueField: 'Value',
                              itemId: 'ammountMeasurement',
                              editable: false,
                              allowBlank: false,
                              // value: 30,
                              width: 230
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
                         padding: '0 0 5 0',
                         labelAlign: 'right'
                     },
                     items: [
                          {
                              xtype: 'textfield',
                              name: 'OP',
                              labelAlign: 'right',
                              labelWidth: 200,
                              fieldWidth: 100,
                              readOnly: true,
                              fieldLabel: 'Отчетный период',                             
                         },
                         {
                             xtype: 'datefield',
                             flex: 1,
                             name: 'DateAccept',
                             itemId: 'tfDateAccept',
                             fieldLabel: 'Дата отчета',
                             format: 'd.m.Y'
                         },

                     ]
                 },
                

                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                       {
                           xtype: 'specialaccountrowgrid',
                           flex: 1
                       }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});