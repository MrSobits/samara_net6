Ext.define('B4.view.protocolrso.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    itemId: 'protocolRSOAddWindow',
    title: 'Протокол РСО',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.TypeSupplierProtocol',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.Municipality'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                   {
                       xtype: 'combobox', editable: false,
                       name: 'TypeSupplierProtocol',
                       fieldLabel: 'Тип РСО',
                       allowBlank: false,
                       displayField: 'Display',
                       store: B4.enums.TypeSupplierProtocol.getStore(),
                       valueField: 'Value'
                   },
                  {
                      xtype: 'b4selectfield',
                      store: 'B4.store.Contragent',
                      textProperty: 'ShortName',
                      name: 'GasSupplier',
                      fieldLabel: 'РСО, оформившая протокол',
                      editable: false,
                      allowBlank: false,
                      columns: [
                          {
                              header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                              filter: {
                                  xtype: 'b4combobox',
                                  operand: CondExpr.operands.eq,
                                  storeAutoLoad: false,
                                  hideLabel: true,
                                  editable: false,
                                  valueField: 'Name',
                                  emptyItem: { Name: '-' },
                                  url: '/Municipality/ListWithoutPaging'
                              }
                          },
                          { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                          { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                          { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                      ]
                  },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4combobox',
                    itemId: 'cbExecutant',
                    name: 'Executant',
                    editable: false,
                    allowBlank: false,
                    fieldLabel: 'Тип исполнителя',
                    fields: ['Id', 'Name', 'Code'],
                    url: '/ExecutantDocGji/List',
                    queryMode: 'local',
                    triggerAction: 'all'
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