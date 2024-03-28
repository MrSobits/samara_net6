Ext.define('B4.view.dict.municipalitytree.Grid', {
    extend: 'B4.form.Window',
    layout: {
        type: 'border',
        padding: 5
    },
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.store.dict.MunicipalityTree'
    ],

    title: 'Дерево муниципальных образований',
    alias: 'widget.municipalityGridTree',
    closable: true,

    height: 600,
    width: 1000,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'center',
                    displayField: 'text',
                    rootVisible: false,
                    xtype: 'treepanel',
                    animate: true,
                    autoScroll: true,
                    useArrows: true,
                    containerScroll: true,
                    loadMask: true,
                    rowLines: true,
                    columnLines: true,
                    treetype: 'parttree',
                    store: Ext.create('B4.store.dict.MunicipalityTree'),
                    columns: [
                        {
                            text: 'Наименование',
                            xtype: 'treecolumn',
                            flex: 2,
                            dataIndex: 'Name',
                            sortable: false
                        },
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            sortable: false
                        },
                        {
                            text: 'Группа',
                            flex: 1,
                            dataIndex: 'Group',
                            sortable: false
                        },
                        {
                            text: 'Федеральный номер',
                            flex: 1,
                            dataIndex: 'FederalNumber',
                            sortable: false
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    tbar: [
                        {                             
                            
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});