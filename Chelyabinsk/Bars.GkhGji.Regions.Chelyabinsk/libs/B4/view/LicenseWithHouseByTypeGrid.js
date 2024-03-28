Ext.define('B4.view.LicenseWithHouseByTypeGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.LicenseWithHouseByType',
        'B4.enums.KindKND'
    ],

    title: 'Субъекты проверок ЖН ',
    store: 'LicenseWithHouseByType',
    alias: 'widget.licensewithhousebytypegrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Управляющая организация',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'JuridicalAddress',
                     flex: 1,
                     text: 'Юридический адрес',
                     filter: {
                         xtype: 'textfield'
                     }
                 },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'Inn',
                      flex: 1,
                      text: 'ИНН',
                      filter: {
                          xtype: 'textfield'
                      }
                  },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'ShortName',
                       flex: 1,
                       text: 'Наименование',
                       filter: {
                           xtype: 'textfield'
                       }
                   },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'KindKND',
                        flex: 1,
                        text: 'Вид КНД',
                        renderer: function (val) {
                            return B4.enums.KindKND.displayRenderer(val);
                        }
                    },
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4updatebutton'
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});