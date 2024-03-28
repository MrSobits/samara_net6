Ext.define('B4.ux.grid.column.State', {
    extend: 'Ext.grid.column.Action',
    alias: ['widget.statecolumn'],
    
    dataIndex: 'State',
    text: 'Статус',
    menuText: 'Статус',
    canChange: true,
    typeId: '',

    constructor: function() {
        var me = this,
            processEvent = Ext.emptyFn,
            renderer = function(v) {
                return v && v.Name ? v.Name : '';
            };

        me.callParent(arguments);

        if (Ext.isEmpty(me.typeId)) {
            Ext.Error.raise('Не передан тип статусной сущности');
        }

        if (me.canChange === true) {
            processEvent = function(type, view, cell, recordIndex) {
                if (type == 'click') {
                    var record = view.getStore().getAt(recordIndex);
                    view.ownerCt.fireEvent('rowaction', view.ownerCt, 'statechange', record);
                }
            };
            renderer = function (v) {
                return '<div style="float: left;">' + v + '</div><img data-qtip="Перевести статус" style="display:block; float: right;" src="content/img/icons/arrow_switch_bluegreen.png">';
            };
        }

        Ext.apply(me, {
            processEvent: processEvent,
            renderer: renderer,
            filter: {
                xtype: 'b4combobox',
                url: '/State/GetListByType',
                editable: false,
                storeAutoLoad: false,
                operand: CondExpr.operands.eq,
                listeners: {
                    storebeforeload: function (field, store, options) {
                        options.params.typeId = me.typeId;
                    },
                    storeloaded: {
                        fn: function (col) {
                            col.getStore().insert(0, { Id: null, Name: '-' });
                            col.select(col.getStore().data.items[0]);
                        }
                    }
                }
            }
        });
    }
});