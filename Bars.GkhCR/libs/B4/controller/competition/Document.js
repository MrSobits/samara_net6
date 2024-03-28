Ext.define('B4.controller.competition.Document', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'Competition',
        'competition.Document'
    ],
    
    stores: [
        'competition.Document'
    ],
    
    views: [
        'competition.DocumentGrid',
        'competition.DocumentEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'competitiondocumentgrid'
        }
    ],

    mainView: 'competition.DocumentGrid',
    mainViewSelector: 'competitiondocumentgrid',
    
    parentCtrlCls: 'B4.controller.competition.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'competitiondocpermission',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'GkhCr.Competition.Document.Edit', applyTo: 'b4savebutton', selector: 'competitiondocumenteditwindow' },
                { name: 'GkhCr.Competition.Document.Delete', applyTo: 'b4deletecolumn', selector: 'competitiondocumentgrid' },
                { name: 'GkhCr.Competition.Document.Create', applyTo: 'b4addbutton', selector: 'competitiondocumentgrid' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'competitionDocumentGridWindowAspect',
            gridSelector: 'competitiondocumentgrid',
            editFormSelector: 'competitiondocumenteditwindow',
            modelName: 'competition.Document',
            editWindowView: 'competition.DocumentEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Competition', me.controller.getContextValue(me.controller.getMainComponent(), 'competitionId'));
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        me.control(actions);
        
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('competitiondocumentgrid'),
            model = me.getModel('Competition'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'competitionId', id);
        me.application.deployView(view, 'competition_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('competitionId', id);

        me.getAspect('competitiondocpermission').setPermissionsByRecord(new model({ Id: id }));
    }
});