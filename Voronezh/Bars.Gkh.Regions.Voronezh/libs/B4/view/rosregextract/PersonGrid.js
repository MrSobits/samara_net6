Ext.define('B4.view.rosregextract.PersonGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Физ лица',
    store: 'RosRegExtractPers',
    alias: 'widget.rosregextractpersongrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
            
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pers_FIO_Surname',
                    flex: 1,
                    text: 'Фамилия',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pers_FIO_First',
                    flex: 1,
                    text: 'Имя',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pers_FIO_Patronymic',
                    flex: 1,
                    text: 'Отчество',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reg_ShareText',
                    flex: 0.5,
                    text: 'Доля',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pers_DateBirth',
                    flex: 0.5,
                    text: 'Дата рождения',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pers_Place_Birth',
                    flex: 1,
                    text: 'Место рождения',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pers_Sex',
                    flex: 0.5,
                    text: 'Пол',
                    filter: { xtype: 'textfield' },
                    hidden: false
                }
               
      
      

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