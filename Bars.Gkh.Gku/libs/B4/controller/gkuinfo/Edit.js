Ext.define('B4.controller.gkuinfo.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GkhEditPanel'
    ],

    mixins: {
        loader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['GkuInfo'],
    views: ['gkuinfo.EditPanel'],

    mainView: 'gkuinfo.EditPanel',
    mainViewSelector: 'gkuinfoeditpanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'gkuinfoEditPanelAspect',
            editPanelSelector: 'gkuinfoeditpanel',
            modelName: 'GkuInfo'
        }
    ],

    onLaunch: function () {
        if (this.params) {
            this.getAspect('gkuinfoEditPanelAspect').setData(this.params.realityObjectId);
        }
    }
});