Ext.define('B4.view.servorg.RoContractEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.servorgrocontracteditwindow',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.ServiceOrganization',
        'B4.store.servorg.RealityObject',
        'B4.store.servorg.RealityObjectContract',
        'B4.view.servorg.RealityObjectGrid',
        'B4.store.RealityObject',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.store.realityobj.ByServOrg'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closable: 'hide',
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 800,
    maxWidth: 800,
    width: 800,
    height: 250,
    bodyPadding: 3,
    
    title: 'Договор с жилым домом',
    trackResetOnLoad: true,
    autoScroll: true,
    store: 'servorg.RealityObjectContract',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 160
            },
            items: [
                
                   {
                       xtype: 'b4selectfield',
                       name: 'RealityObjectId',
                       itemId: 'sfRealityObject',
                       allowBlank: false,
                       fieldLabel: 'Жилой дом',
                       labelWidth: 150,
                      

                       store: 'B4.store.realityobj.ByServOrg',
                       textProperty: 'Address',
                       editable: false,
                       columns: [
                           {
                               text: 'Муниципальный район',
                               dataIndex: 'Municipality',
                               flex: 1,
                               filter: {
                                   xtype: 'b4combobox',
                                   operand: CondExpr.operands.eq,
                                   storeAutoLoad: false,
                                   hideLabel: true,
                                   editable: false,
                                   valueField: 'Name',
                                   emptyItem: { Name: '-' },
                                   url: '/Municipality/ListMoAreaWithoutPaging'
                               }
                           },
                           {
                               text: 'Адрес',
                               dataIndex: 'Address',
                               flex: 1,
                               filter: { xtype: 'textfield' }
                           }
                       ]
                   },
                   {
                       xtype: 'container',
                       padding: '0 0 5 0',
                       layout: 'hbox',
                       defaults: {
                           labelAlign: 'right',
                           //allowBlank: false,
                           flex: 1
                       },
                       items: [
                           {
                               xtype: 'textfield',
                               name: 'DocumentNumber',
                               fieldLabel: 'Номер',
                               labelWidth: 150,
                               maxLength: 300
                           },
                           {
                               xtype: 'datefield',
                               name: 'DocumentDate',
                               fieldLabel: 'от',
                               labelWidth: 50,
                               format: 'd.m.Y',
                               maxWidth: 150
                           }
                       ]
                   },
                   {
                       xtype: 'container',
                       layout: 'hbox',
                       padding: '0 0 5 0',
                       defaults: {
                           xtype: 'datefield',
                           format: 'd.m.Y',
                           labelAlign: 'right',
                           flex: 1
                       },
                       items: [
                           {
                               name: 'DateStart',
                               fieldLabel: 'Дата начала действия',
                               labelWidth: 150,
                               allowBlank: false
                           },
                           {
                               name: 'DateEnd',
                               fieldLabel: 'Дата окончания действия',
                               labelWidth: 170
                           }
                       ]
                   },
                   {
                       xtype: 'b4filefield',
                       name: 'FileInfo',
                       labelWidth: 150,
                       fieldLabel: 'Файл'
                   },
                   {
                       xtype: 'textarea',
                       name: 'Note',
                       fieldLabel: 'Примечание',
                       height: 60,
                       labelWidth: 150,
                       maxLength: 300
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});