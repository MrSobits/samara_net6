Ext.define('B4.controller.regop.statusPDH.StatusPaymentDocumentHouses', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.regop.statusPDH.StatusPaymentDocumentHousesGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realtyloangrid'
        },
        { ref: 'period', selector: 'statuspaymentdocumenthousesgrid b4selectfield[name=ChargePeriod]' }
    ],

  //  periodId: null,
    
    init: function() {
        var me = this;

        me.control({            

            'statuspaymentdocumenthousesgrid': { 'beforeload': me.beforeSnapshotsLoad },
            'statuspaymentdocumenthousesgrid #periodSelect': { 'change': me.onChangePeriod }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('statuspaymentdocumenthousesgrid'),
            store = view.getStore();

        me.bindContext(view);
        me.application.deployView(view);

        view.down('b4selectfield[name = ChargePeriod]').getStore().load();
    },

    onChangePeriod: function (field, newValue) {
        
        var me = this;
        if (newValue) {
            var grid = field.up('statuspaymentdocumenthousesgrid'),
                store = grid.getStore();
            store.on('beforeload',
                    function (store, operation) {
                        operation.params.periodId = newValue.Id;
                    },
                    me);
            store.load();
        }
      
    },

    beforeSnapshotsLoad: function (store, opts) {
        
        var periodId = this.getPeriod().getValue();
        if (!periodId) {
            Ext.Msg.alert('Ошибка', 'Не выбран период!');
            return false;
        }

        Ext.apply(opts.params, {
            periodId: periodId
        });
    }
});