Ext.define('B4.view.report.HouseTechPassportPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'houseTechPassportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.realityobj.Grid',
        'B4.store.RealityObject'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                 {
                     xtype: 'b4selectfield',
                     editable: false,

                     store: 'B4.store.RealityObject',
                     idProperty: 'Id',
                     textProperty: 'Address',
                     columns: [
                         {
                             text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                         {
                             text: 'Адрес',
                             dataIndex: 'Address',
                             flex: 1,
                             filter: { xtype: 'textfield' }
                         }
                     ],                    
                     name: 'HouseField',
                     itemId: 'sfHouse',
                     fieldLabel: 'Жилой дом',
                     allowBlank: false
                 }
            ]
        });

        me.callParent(arguments);
    }
});