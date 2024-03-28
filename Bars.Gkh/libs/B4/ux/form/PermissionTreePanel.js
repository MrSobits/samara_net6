Ext.define('B4.ux.form.PermissionTreePanel', {
    extend: 'Ext.tree.Panel',

    alias: 'widget.permissiontreepanel',

    requires: [
        'B4.form.SearchField'
    ],

    animate: false,
    overflowY: 'auto',
    useArrows: true,

    rootVisible: false,
    loadMask: true,
    viewConfig: {
        loadMask: true
    },

    tbar: [
        { iconCls: 'icon-arrow-refresh', text: 'Обновить', name: 'Update' },
        { iconCls: 'icon-disk', text: 'Сохранить', name: 'Save' },
        { xtype: 'tbseparator' },
        { iconCls: 'icon-collapse-all', text: 'Свернуть все', name: 'CollapseAll' },
        { iconCls: 'icon-collapse-all', text: 'Раскрыть все', name: 'ExpandAll' },
        { xtype: 'tbseparator' },
        { iconCls: 'icon-accept', text: 'Отметить все', name: 'MarkAll' },
        { iconCls: 'icon-decline', text: 'Снять все отметки', name: 'UnmarkAll' },
        { xtype: 'tbseparator' },
        { iconCls: 'icon-add', text: 'Копирование', name: 'CopyRole', tooltip: 'Копировать права из роли' },
        {
            xtype: 'tbfill'
        },
        {
            xtype: 'b4searchfield',
            name: 'Search',
            typeAhead: false,
            hideLabel: true,
            emptyText: 'Поиск',
            margin: '0 10 0 0',
            width: 250
        }
    ],

    initComponent: function() {
        var me = this,
            tbar = [
                { iconCls: 'icon-arrow-refresh', text: 'Обновить', name: 'Update' },
                { iconCls: 'icon-disk', text: 'Сохранить', name: 'Save' },
                { xtype: 'tbseparator' },
                { iconCls: 'icon-collapse-all', text: 'Свернуть все', name: 'CollapseAll' },
                { iconCls: 'icon-collapse-all', text: 'Раскрыть все', name: 'ExpandAll' },
                { xtype: 'tbseparator' },
                { iconCls: 'icon-accept', text: 'Отметить все', name: 'MarkAll' },
                { iconCls: 'icon-decline', text: 'Снять все отметки', name: 'UnmarkAll' },
                { xtype: 'tbseparator' },
                { iconCls: 'icon-add', text: 'Копирование', name: 'CopyRole', tooltip: 'Копировать права из роли' },
                {
                    xtype: 'tbfill'
                },
                {
                    xtype: 'b4searchfield',
                    name: 'Search',
                    typeAhead: false,
                    hideLabel: true,
                    emptyText: 'Поиск',
                    margin: '0 10 0 0',
                    width: 250
                }
            ];

        Ext.applyIf(me, {
            tbar: tbar
        });

        me.callParent(arguments);
    }

});