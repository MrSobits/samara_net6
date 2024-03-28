Ext.define('B4.view.rosregextract.OrgGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Юр лица',
    store: 'RosRegExtractOrg',
    alias: 'widget.rosregextractorggrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
              
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Org_Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Org_Inn',
                    flex: 1,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Org_Code_OGRN',
                    flex: 1,
                    text: 'ОГРН',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Org_Content',
                    flex: 1,
                    text: 'Полные данные',
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