Ext.define('B4.view.rosregextract.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
         'B4.enums.YesNoNotSet'
    ],

    title: 'Реестр выписок',
    store: 'RosRegExtractDesc',
    alias: 'widget.rosregextractgrid',
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
                    dataIndex: 'Desc_CadastralNumber',
                    flex: 1,
                    text: 'Кадастровый номер',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_ObjectTypeText',
                    flex: 0.5,
                    text: 'Тип объекта',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_AreaText',
                    flex: 0.5,
                    text: 'Площадь',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_CityName',
                    flex: 0.5,
                    text: 'МО',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_Locality',
                    flex: 0.5,
                    text: 'Населенный пункт',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_StreetName',
                    flex: 0.5,
                    text: 'Улица',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_Level1Name',
                    flex: 0.5,
                    text: 'Дом',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_Level2Name',
                    flex: 0.5,
                    text: 'Корпус',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Desc_ApartmentName',
                    flex: 0.5,
                    text: 'Помещение',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YesNoNotSet',
                    flex: 0.5,
                    text: 'Сопоставлено',
                    renderer: function (val) {
                        return B4.enums.YesNoNotSet.displayRenderer(val);
                    },
                    filter: {
                    xtype: 'b4combobox',
                    items: B4.enums.YesNoNotSet.getItemsWithEmpty([null, '-']),
                    editable: false,
                    operand: CondExpr.operands.eq,
                    valueField: 'Value',
                    displayField: 'Display'
                        },
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