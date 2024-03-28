Ext.define('B4.controller.RequestToExamRegister', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    views: [
        'requesttoexamregister.Grid'
    ],

    mainView: 'requesttoexamregister.Grid',
    mainViewSelector: 'requesttoexamregistergrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'requesttoexamregistergrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'requestToExamRegisterStateTransferAspect',
            gridSelector: 'requesttoexamregistergrid',
            menuSelector: 'requesttoexamregistergridStateMenu',
            stateType: 'gkh_person_request_exam'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'requestToExamRegisterExportAspect',
            gridSelector: 'requesttoexamregistergrid',
            buttonSelector: 'requesttoexamregistergrid [action=Export]',
            controllerName: 'PersonRequestToExam',
            actionName: 'Export'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'requesttoexamregistergrid b4updatebutton': {
                click: { fn: me.updateGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('requesttoexamregistergrid'),
            store = view.getStore();

        me.bindContext(view);
        me.application.deployView(view);

        store.clearFilter(true);
        store.filter('showAll', true);
    },
    
    updateGrid: function (btn) {
        btn.up('requesttoexamregistergrid').getStore().load();
    }
});