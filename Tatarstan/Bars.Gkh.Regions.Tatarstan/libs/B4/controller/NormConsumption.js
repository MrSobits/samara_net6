Ext.define('B4.controller.NormConsumption', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.view.normconsumption.AddWindow'

    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['normconsumption.NormConsumption'],
    stores: ['normconsumption.NormConsumption'],
    views: [
        'normconsumption.Grid',
        'normconsumption.AddWindow'
    ],

    mainView: 'normconsumption.Grid',
    mainViewSelector: 'normconsumptiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'normconsumptiongrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'normconsStateTransferAspect',
            gridSelector: 'normconsumptiongrid',
            menuSelector: 'normconsumptiongridStateMenu',
            stateType: 'gkh_morm_consumption'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'NormConsumptionEditFormAspect',
            gridSelector: 'normconsumptiongrid',
            editFormSelector: 'normconsumptionAddWindow',
            modelName: 'normconsumption.NormConsumption',
            editWindowView: 'B4.view.normconsumption.AddWindow',
            editRecord: function (record) {
                var me = this,
                    objectId = record ? record.get('Id') : null,
                    model = me.controller.getModel(me.modelName),
                    type = record ? record.get('Type') : null;

                if (!objectId) {
                    me.setFormData(new model({ Id: 0 }));
                } else {
                    me.redirectToEdit(objectId, type);
                }
            },

            redirectToEdit: function (objectId, type) {
                switch (type) {
                    case B4.enums.NormConsumptionType.ColdWater:
                        Ext.History.add('normconscoldwater/' + objectId + '/');
                        break;
                    case B4.enums.NormConsumptionType.HotWater:
                        Ext.History.add('normconshotwater/' + objectId + '/');
                        break;
                    case B4.enums.NormConsumptionType.Firing:
                        Ext.History.add('normconsfiring/' + objectId + '/');
                        break;
                    case B4.enums.NormConsumptionType.Heating:
                        Ext.History.add('normconsheating/' + objectId + '/');
                        break;
                }
            },

            otherActions: function(actions) {
                var me = this;

                actions[me.gridSelector + ' b4updatebutton'] = {
                    click: {
                        fn: function () {
                            me.controller.getMainView().getStore().load();
                        },
                        scope: me
                    }
                };
            }
        }
    ],

    init: function () {
        var me = this;
        this.control({
            'normconsumptiongrid': {
                'afterrender': {
                    fn: function(grid) {
                        grid.getStore().on('beforeload', me.onBeforeLoad, me);
                    },
                    scope: this
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('normconsumptiongrid'),
            store = view.getStore();


        store.on('beforeload', me.onBeforeLoad, me);

        me.bindContext(view);
        me.application.deployView(view);
        store.load();
    },

    onBeforeLoad: function (store, operation) {

        var mainView = this.getMainView();
        if (mainView) {
            operation.params.muId = mainView.down('#municipality').getValue();
            operation.params.periodId = mainView.down('#period').getValue();
        }
    }
});