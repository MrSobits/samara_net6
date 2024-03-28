Ext.define('B4.controller.GeneralInfoRealityObj', {
    extend: 'B4.base.Controller',
 views: [ 'generalinforealityobj.EditPanel' ], 

    requires:
    [
        'B4.aspects.GkhEditPanel'
    ],

    models:
    [
        'DisclosureInfoRealityObj',
        'RealityObject'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    mainView: 'generalinforealityobj.EditPanel',
    mainViewSelector: '#generalInfoRealityObjEditPanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'generalInfoRealityObjEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: '#generalInfoRealityObjEditPanel'
        }
    ],

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('generalInfoRealityObjEditPanelAspect').setData(me.params.disclosureInfoRealityObjId);
        }
    }
});
