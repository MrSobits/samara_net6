Ext.define('B4.view.riskorientedmethod.KindKNDDictEditWindow', {
    extend: 'B4.form.Window',
 //   alias: 'widget.kindknddicteditwindow',
    itemId: 'kindKNDDictEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.KindKND',
        'B4.view.riskorientedmethod.KindKNDDictArtLawGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    title: 'Статьи закона для вида КНД',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                 
                        {
                            xtype: 'combobox',
                            name: 'KindKND',
                            fieldLabel: 'Вид КНД',
                            displayField: 'Display',
                            store: B4.enums.KindKND.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateFrom',
                            fieldLabel: 'Дата начала',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateTo',
                            fieldLabel: 'Дата окончания',
                            format: 'd.m.Y'
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
                                 xtype: 'kindknddictartlawgrid',
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