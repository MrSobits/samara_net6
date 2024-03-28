Ext.define('B4.view.rosregextract.GovGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Муниципальные',
    store: 'RosRegExtractGov',
    alias: 'widget.rosregextractgovgrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
              
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Gov_Code_SP',
                    flex: 1,
                    text: 'Код СП',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Gov_Content',
                    flex: 1,
                    text: 'Содержание',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Gov_Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Gov_OKATO_Code',
                    flex: 1,
                    text: 'ОКАТО',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Gov_Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
              
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Desc_Level1Name',
                //    flex: 0.5,
                //    text: 'Дом',
                //    filter: { xtype: 'textfield' },
                //    hidden: false
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Desc_Level2Name',
                //    flex: 0.5,
                //    text: 'Корпус',
                //    filter: { xtype: 'textfield' },
                //    hidden: false
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Desc_ApartmentName',
                //    flex: 0.5,
                //    text: 'Помещение',
                //    filter: { xtype: 'textfield' },
                //    hidden: false
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Reg_RegDate',
                //    flex: 0.5,
                //    text: 'Дата регистрации',
                //    filter: { xtype: 'textfield' },
                //    hidden: false
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Reg_RegNumber',
                //    flex: 0.5,
                //    text: 'Номер регистрации',
                //    filter: { xtype: 'textfield' },
                //    hidden: false
                //}      
      
      

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