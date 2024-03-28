Ext.define('B4.view.specaccowner.SPAccOwnerRealityObjectEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipality.Grid',
        'B4.store.specaccowner.RealityObjectOnSpecAcc',
        'B4.store.CreditOrg',
        'B4.store.Contragent',
        'B4.enums.OrgStateRole',
        'B4.enums.GroundsTermination',
        'B4.form.SelectField',
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 800,
    bodyPadding: 10,
    itemId: 'specialAccountOwnerRealityObjectEditWindow',
    title: 'Cпецсчет',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.CreditOrg',
                    textProperty: 'Name',
                    name: 'CreditOrg',
                    fieldLabel: 'Банк',
                    flex: 1,
                    editable: false,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    allowBlank: false
                },
                  {
                      xtype: 'textfield',
                      name: 'SpacAccNumber',
                      fieldLabel: 'Номер спецсчета',
                      maxLength: 30,
                      allowBlank: false,
                      flex: 1
                  },
                  {
                      xtype: 'b4selectfield',
                      store: 'B4.store.specaccowner.RealityObjectOnSpecAcc',
                      textProperty: 'Address',
                      name: 'RealityObject',
                      fieldLabel: 'МКД',
                      flex: 1,
                      editable: false,
                      columns: [
                          { header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                          { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                      ],
                      allowBlank: false
                  },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                          {
                              xtype: 'datefield',
                              name: 'DateStart',
                              fieldLabel: 'Дата открытия',
                              format: 'd.m.Y',
                              flex: 0.5,
                              labelWidth: 150,
                              allowBlank: false
                          },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            fieldLabel: 'Дата закрытия',
                            format: 'd.m.Y',
                            flex: 0.5,
                            labelWidth: 150,
                            allowBlank: true
                        }
                    
                      
                    ]
                },
                 
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