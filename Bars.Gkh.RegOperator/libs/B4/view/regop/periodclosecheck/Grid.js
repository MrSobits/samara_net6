Ext.define('B4.view.regop.periodclosecheck.Grid',
{
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox'
    ],

    title: 'Проверки перед закрытием месяца',

    alias: 'widget.periodclosecheckgrid',

    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.period.CloseCheck');

        Ext.applyIf(me,
        {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    tooltip: 'История изменений',
                    icon: B4.Url.content('content/img/icons/zoom.png')
                },
                {
                    text: 'Код',
                    dataIndex: 'Code',
                    width: 100
                },
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1
                },
                {
                    text: 'Обязательность',
                    dataIndex: 'IsCritical',
                    width: 100,
                    editor: {
                        xtype: 'b4combobox',
                        items: [[false, 'Нет'], [true, 'Да']],
                        editable: false
                    },
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Добавить',
                                    iconCls: 'icon-add',
                                    actionName: 'add'
                                },
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing',
                {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});