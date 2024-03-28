Ext.define('B4.controller.dict.StateDuty', {
    extend: 'B4.base.Controller',

    views: ['dict.stateduty.Grid'],
    
    stores: ['dict.StateDuty'],
    
    models: ['StateDuty'],

    requires: [
        'B4.mixins.MaskBody',
        'B4.mixins.Context',
        'B4.Ajax', 'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        { ref: 'mainView', selector: 'statedutygrid' }
    ],

    init: function () {
        var me = this;

        me.control({
            'statedutygrid': {
                rowaction: function(g, action, record) {
                    switch(action.toLowerCase()) {
                        case 'edit':
                            me.gotoEdit(record.get('Id'));
                            break;
                        case 'delete':
                            me.onDeleteDuty(g, record);
                            break;
                    }
                }
            },
            'statedutygrid b4addbutton': {
                'click': function() {
                    me.gotoEdit(0);
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            mainView = me.getMainView() || Ext.widget('statedutygrid');

        me.bindContext(mainView);
        me.application.deployView(mainView);

        mainView.getStore().load();
    },
    
    gotoEdit: function(id) {
        Ext.History.add('stateduty_edit/' + id);
    },
    
    onDeleteDuty: function (grid, record) {
        var me = this;
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
            if (result === 'yes') {
                me.mask('Удаление', B4.getBody());
                record.destroy()
                    .next(function() {
                        grid.getStore().load();
                        me.unmask();
                    })
                    .error(function(res) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(res.responseData) ? res.responseData : res.responseData.message);
                        me.unmask();
                    });
            }
        });
    }
});