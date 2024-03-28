Ext.define('B4.view.program.thirddetails.CommonEstateGrid', {    
    extend: 'Ext.tree.Panel',
    
    alias: 'widget.thirddetailscommonestatetree',
    
    requires: ['B4.store.program.thirddetails.CommonEstate'],
    
    rootVisible: false,
    
    cls: 'x-large-head',
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.program.thirddetails.CommonEstate');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { text: 'Наименование', dataIndex: 'Name', xtype: 'treecolumn', flex: 2 },
                { text: 'Ед. измерения', dataIndex: 'Measure'},
                { text: 'Объем', dataIndex: 'Volume' },
                { text: 'Год ремонта', dataIndex: 'Year' },
                {
                    text: 'Сумма по работам (руб.)', dataIndex: 'WorkSum',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    text: 'Сумма по услугам (руб.)', dataIndex: 'ServiceSum',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    text: 'Сумма по работам, в т.ч. услуги (руб.)', dataIndex: 'ServiceAndWorkSum',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});