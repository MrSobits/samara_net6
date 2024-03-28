Ext.define('B4.view.dict.auditpurposesurveysubjectgji.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.auditPurposeSurveySubjectGjiGrid',
    title: 'Предметы проведения проверки',
    store: 'dict.AuditPurposeSurveySubjectGji',
    itemId: 'auditPurposeSurveySubjectGjiGrid',

    initComponent: function() {
        var me = this;

        var sm = Ext.create('Ext.selection.CheckboxModel', {
            mode: 'MULTI',
            showHeaderCheckbox: false,
            headerWidth: 60,

            getHeaderConfig: function () {
                var me = this;

                return {
                    isCheckerHd: false,
                    text: '',
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
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код'
                }
            ],
            viewConfig: {
                loadMask: true
            },
                dockedItems: [
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