Ext.define('B4.view.dictionaries.RecordComparisonGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.recordcomparisongrid',
    columnLines: true,

    requires: [
        'B4.grid.SelectFieldEditor',
        'B4.store.RecordComparison',
        'B4.store.GisRecord',

        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.plugin.HeaderLocalFilters'
    ],

    gisRecStore: undefined,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.RecordComparison'),
            gisRecStore = Ext.create('B4.store.GisRecord', { load: Ext.emptyFn });

        Ext.applyIf(me,
            {
                gisRecStore: gisRecStore,
                store: store,
                columns: [
                    {
                        text: 'Запись внешней системы',
                        dataIndex: 'ExternalEntity',
                        flex: 1,
                        renderer: function(val) {
                            return val && val.Name || '';
                        },
                        filter: {
                            xtype: 'textfield',
                            filterFn: function(value, filterValue) {
                                return value && value.Name && value.Name.toLowerCase().indexOf(filterValue.toLowerCase()) > -1;
                            }
                        }
                    },
                    {
                        text: 'Запись ГИС ЖКХ',
                        dataIndex: 'GisRecord',
                        flex: 1,
                        renderer: function (val) {
                            return val && val.Name || '';
                        },
                        editor: {
                            xtype: 'b4selectfieldeditor',
                            isGetOnlyIdProperty: false,
                            store: gisRecStore,
                            editable: false,
                            listView: {
                                plugins: [
                                {
                                    ptype: 'b4gridheaderlocalfilters'
                                }
                                ],
                                columns: [
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'Name',
                                        header: 'Наименование',
                                        flex: 1,
                                        filter: {
                                            xtype: 'textfield'
                                        }
                                    }
                                ],
                                dockedItems:[]
                            }
                        },
                        filter: {
                            xtype: 'textfield',
                            filterFn: function (value, filterValue) {
                                return value && value.Name && value.Name.toLowerCase().indexOf(filterValue.toLowerCase()) > -1;
                            }
                        }
                    }
                ],
                plugins: [
                    Ext.create('Ext.grid.plugin.CellEditing',
                        {
                            clicksToEdit: 1
                        }),
                    {
                        ptype: 'b4gridheaderlocalfilters'
                    }
                ]
            });

        me.callParent(arguments);
    },

    getGisRecStore: function() {
        return this.gisRecStore;
    }
});
