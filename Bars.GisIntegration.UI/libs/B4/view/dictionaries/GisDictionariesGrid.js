Ext.define('B4.view.dictionaries.GisDictionariesGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gisdictionariesgrid',
    columnLines: true,

    requires: [
        'B4.enums.DictionaryGroup',
        'B4.ux.grid.plugin.HeaderLocalFilters'
    ],

    selModel: Ext.create('Ext.selection.CheckboxModel', {
        mode: 'SINGLE'
    }),

    filtrable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.GisDictionary');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'numbercolumn',
                    format: '0',
                    text: 'Реестровый номер',
                    dataIndex: 'RegistryNumber',
                    width: 125,
                    filter: {
                        xtype: 'numberfield'
                    } 
                },
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.DictionaryGroup',
                    text: 'Группа',
                    dataIndex: 'Group',
                    width: 120,
                    filter: true
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    text: 'Дата последнего изменения',
                    dataIndex: 'Modified',
                    width: 160,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    } 
                }
            ],
            plugins: [
                {
                    ptype: 'b4gridheaderlocalfilters'
                }
            ]
        });

        me.callParent(arguments);
    }
});
