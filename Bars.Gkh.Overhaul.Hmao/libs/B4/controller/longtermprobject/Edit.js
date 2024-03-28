Ext.define('B4.controller.longtermprobject.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: ['B4.aspects.GkhEditPanel'],

    mixins: {
        loader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['LongTermPrObject', 'RealityObject'],
    views: ['longtermprobject.EditPanel'],

    mainView: 'longtermprobject.EditPanel',
    mainViewSelector: '#longtermprobjectEditPanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'longtermprobjectEditPanelAspect',
            editPanelSelector: '#longtermprobjectEditPanel',
            modelName: 'LongTermPrObject'
        }
    ],

    init: function () {
        this.control({
            'longtermprobjectEditPanel button[name="btnShowHouseDetails"]': { click: { fn: this.onShowHouseDetails, scope: this } }
        });

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('longtermprobjectEditPanelAspect').setData(this.params.longTermObjId);
        }
    },

    onShowHouseDetails: function () {
        var id = this.params.record.get('RealObjId');
        Ext.History.add('realityobjectedit/' + id );
    }
});