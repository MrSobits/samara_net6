Ext.define('B4.ux.config.BasisOverhaulDocKindSelectField', {
    extend: 'B4.ux.config.TriggerSingleSelectEditor',
    requires: [
        'B4.store.dict.BaseDict'
    ],
    alias: 'widget.basisoverhauldockindselectfield',

    textProperty: 'Name',
    columnsGridSelect: [
        {
            xtype: 'gridcolumn',
            dataIndex: 'Name',
            flex: 5,
            text: 'Наименование',
            filter: {xtype: 'textfield'}
        },
        {
            xtype: 'gridcolumn',
            dataIndex: 'Code',
            flex: 1,
            text: 'Код',
            filter: {xtype: 'textfield'}
        }
    ],

    getStore: function() {
        return Ext.create('B4.store.dict.BaseDict', {
            proxy: {
                type: 'b4proxy',
                controllerName: 'BasisOverhaulDocKind'
            }
        })
    }
});