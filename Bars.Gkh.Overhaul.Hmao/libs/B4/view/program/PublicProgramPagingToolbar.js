Ext.define('B4.view.program.PublicProgramPagingToolbar', {
    extend: 'Ext.toolbar.Paging',
    alias: 'widget.publicprogrampagingtoolbar',

    initComponent: function () {
        var me = this;

        me.items = [
            {
                xtype: 'tbseparator'
            },
            {
                xtype: 'combo',
                editable: false,
                fieldLabel: 'Записей',
                width: 125,
                labelWidth: 50,
                mode: 'local',
                triggerAction: 'all',
                store: new Ext.data.SimpleStore({
                    fields: ['count'],
                    data: [[25], [50], [100], [200], [500]],
                    autoLoad: false
                }),
                valueField: 'count',
                displayField: 'count',
                listeners: {
                    select: function (cb) {
                        me.store.currentPage = 1;
                        me.store.pageSize = parseInt(cb.getRawValue(), 10);
                        me.doRefresh();
                    },
                    afterrender: function () {
                        this.setValue(me.store.pageSize);
                    }
                }
            },
            {
                xtype: 'tbfill'
            },
            {
                xtype: 'tbtext',
                name: 'Summary',
                tpl: Ext.create('Ext.Template','<span>Количество домов: в программе - {total}, включенных - {included}, исключенных - {excluded}</span>')
            }
        ];

        me.callParent(arguments);
    }
});