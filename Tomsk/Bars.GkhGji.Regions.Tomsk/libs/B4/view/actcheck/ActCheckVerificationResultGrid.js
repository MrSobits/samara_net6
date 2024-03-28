Ext.define('B4.view.actcheck.ActCheckVerificationResultGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.actcheckverificationresultgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Ход проведения проверки',
    store: 'actcheck.ActCheckVerificationResult',
    itemId: 'actCheckVerificationResultGrid',

    initComponent: function() {
        var me = this;
        
        var sm = Ext.create('Ext.selection.CheckboxModel', {
            mode: 'SINGLE',
            showHeaderCheckbox: false,
            headerWidth: 60,

            getHeaderConfig: function () {
                var me = this;
                
                return {
                    isCheckerHd: false,
                    text: 'Выявлено',
                    clickTargetName: 'el',
                    width: me.headerWidth,
                    sortable: false,
                    draggable: false,
                    resizable: false,
                    hideable: false,
                    menuDisabled: true,
                    dataIndex: '',
                    cls: '',
                    renderer: Ext.Function.bind(me.renderer, me),
                    editRenderer: me.editRenderer || me.renderEmpty,
                    locked: me.hasLockedHeader()
                };
            }
        });

        Ext.applyIf(me, {
            columnLines: true,
            selModel: sm,
            columns: [
                {
                    xtype: 'gridcolumn',
                    sortable: false,
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                }
            ],
            //plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                 {
                                     xtype: 'button',
                                     text: 'Сохранить',
                                     cmd: 'saveValidationSubject',
                                     tooltip: 'Сохранить',
                                     iconCls: 'icon-accept'
                                 }
                            ]
                        }
                    ]
                }/*,
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }*/
            ]
        });

        me.callParent(arguments);
    }
});