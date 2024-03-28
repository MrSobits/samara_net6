Ext.define('B4.view.dateareaowner.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'dateareaowner table',
    store: 'DateAreaOwner',
    alias: 'widget.dateareaownergrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ID_Object',
                    flex: 1,
                    text: 'ID_Object',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CadastralNumber',
                    flex: 1,
                    text: 'CadastralNumber',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectType',
                    flex: 1,
                    text: 'ObjectType',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectTypeText',
                    flex: 1,
                    text: 'ObjectTypeText',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectTypeName',
                    flex: 1,
                    text: 'ObjectTypeName',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AssignationCode',
                    flex: 1,
                    text: 'AssignationCode',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AssignationCodeText',
                    flex: 1,
                    text: 'AssignationCodeText',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
                    flex: 0.3,
                    text: 'Area',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaText',
                    flex: 1,
                    text: 'AreaText',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaUnit',
                    flex: 1,
                    text: 'AreaUnit',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Floor',
                    flex: 0.3,
                    text: 'Floor',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ID_Address',
                    flex: 1,
                    text: 'ID_Address',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressContent',
                    flex: 1,
                    text: 'AddressContent',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegionCode',
                    flex: 1,
                    text: 'RegionCode',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegionName',
                    flex: 1,
                    text: 'RegionName',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OKATO',
                    flex: 1,
                    text: 'OKATO',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KLADR',
                    flex: 1,
                    text: 'KLADR',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CityName',
                    flex: 1,
                    text: 'CityName',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StreetName',
                    flex: 1.2,
                    text: 'StreetName',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Level1Name',
                    flex: 0.5,
                    text: 'Level1Name',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ApartmentName',
                    flex: 0.5,
                    text: 'ApartmentName',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ID_Subject',
                    flex: 1,
                    text: 'ID_Subject',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code_SP',
                    flex: 1,
                    text: 'Code_SP',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonContent',
                    flex: 1,
                    text: 'PersonContent',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Surname',
                    flex: 1,
                    text: 'Surname',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstName',
                    flex: 1,
                    text: 'FirstName',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Patronymic',
                    flex: 1,
                    text: 'Patronymic',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DateBirth',
                    flex: 1,
                    text: 'DateBirth',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Citizenship',
                    flex: 1,
                    text: 'Citizenship',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sex',
                    flex: 1,
                    text: 'Sex',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PassportContent',
                    flex: 1,
                    text: 'PassportContent',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDocument',
                    flex: 1,
                    text: 'TypeDocument',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'DocumentName',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Series',
                    flex: 1,
                    text: 'Series',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Number',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssueDate',
                    flex: 1,
                    text: 'IssueDate',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssueOrgan',
                    flex: 1,
                    text: 'IssueOrgan',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'DeptCode',
                //    flex: 1,
                //    text: 'DeptCode',
                //    filter: { xtype: 'textfield' },
                //    hidden: true
                //},
                //
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ID_AddressL',
                    flex: 1,
                    text: 'ID_AddressL',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressContentL',
                    flex: 1,
                    text: 'AddressContentL',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegionCodeL',
                    flex: 1,
                    text: 'RegionCodeL',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegionNameL',
                    flex: 1,
                    text: 'RegionNameL',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OKATOL',
                    flex: 1,
                    text: 'OKATOL',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KLADRL',
                    flex: 1,
                    text: 'KLADRL',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SNILS',
                    flex: 1,
                    text: 'SNILS',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ID_Record',
                    flex: 1,
                    text: 'ID_Record',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegNumber',
                    flex: 1,
                    text: 'RegNumber',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    flex: 1,
                    text: 'Type',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegName',
                    flex: 1,
                    text: 'RegName',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegDate',
                    flex: 1,
                    text: 'RegDate',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RestorCourt',
                    flex: 1,
                    text: 'RestorCourt',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShareText',
                    flex: 1,
                    text: 'ShareText',
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsProcessed',
                    flex: 1,
                    text: 'IsProcessed',
                    filter: { xtype: 'checkbox' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsImported',
                    flex: 1,
                    text: 'IsImported',
                    filter: { xtype: 'checkbox' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsProcessedWithCollision',
                    flex: 1,
                    text: 'IsProcessedWithCollision',
                    filter: { xtype: 'checkbox' },
                    hidden: true
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
                     
                        {
                            xtype: 'b4updatebutton'
                        },
                        {
                            xtype: 'button',
                            text: 'Сопоставить собственников',
                            iconCls: 'icon-reload',
              //              itemId: 'btnMerge',
                            action: 'Merge'
                        },
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