Ext.define('B4.controller.competition.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.StateContextMenu',
        'B4.aspects.StateContextButton',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.view.competition.EditPanel',
        'B4.aspects.BackForward'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'Competition'
    ],

    views: [
        'competition.EditPanel'
    ],

    mainView: 'competition.EditPanel',
    mainViewSelector: 'competitionEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'competitionEditPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'competitionStatePermAspect',
            permissions: [
                { name: 'GkhCr.Competition.Edit', applyTo: 'b4savebutton', selector: 'competitionEditPanel' }
            ]
        },
        {
            xtype: 'backforwardaspect',
            panelSelector: 'competitionEditPanel',
            backForwardController: 'Competition'
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'competitionStateButtonAspect',
            stateButtonSelector: 'competitionEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    asp.controller.getAspect('competitionEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'competitionEditPanelAspect',
            editPanelSelector: 'competitionEditPanel',
            modelName: 'Competition',
            listeners: {
                'aftersetpaneldata': function (asp, rec, panel) {
                    asp.controller.getAspect('competitionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    asp.controller.getAspect('competitionStatePermAspect').setPermissionsByRecord({ getId: function () { return rec.get('Id'); } });
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('competitionEditPanel');

        me.bindContext(view);
        view.params = {};
        view.params.competitionId = id;
        me.setContextValue(view, 'competitionId', id);
        me.application.deployView(view, 'competition_info');

        me.getAspect('competitionEditPanelAspect').setData(id);
    },

    init: function () {
        var me = this,
            actions = {};

        this.control(actions);

        this.callParent(arguments);
    },

    hasChanges: function () {
        return this.getMainComponent().getForm().isDirty();
    },

    getCurrent: function () {

        var me = this;
        return me.getContextValue(me.getMainComponent(), 'competitionId');
    }
});