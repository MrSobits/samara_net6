/**
* Контроллер раздела протокола
*/
Ext.define('B4.controller.objectcr.HousekeeperReport', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateContextMenu'
    ],

    models: ['objectcr.HousekeeperReport',
    'objectcr.HousekeeperReportFile'],
    stores: ['objectcr.HousekeeperReport',
        'objectcr.HousekeeperReportFile'],
    
    views: [
        'objectcr.HousekeeperReportGrid',
        'objectcr.HousekeeperReportEditWindow',
        'objectcr.HousekeeperReportFileEditWindow',
        'objectcr.HousekeeperReportFileGrid'
    ],

    params: {},
    reportId:null,

    mainView: 'objectcr.HousekeeperReportGrid',
    mainViewSelector: 'housekeeperreportgrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [    
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'housekeeperreportgridStateTransferAspect',
            gridSelector: 'housekeeperreportgrid',
            stateType: 'housekeeper_report',
            menuSelector: 'housekeeperreportgridStateMenu'
        },       
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования протоколов
            */
            xtype: 'grideditwindowaspect',
            name: 'housekeeperreportgridWindowAspect',
            gridSelector: 'housekeeperreportgrid',
            editFormSelector: 'housekeeperreporteditwindow',
            modelName: 'objectcr.HousekeeperReport',
           storeName: 'objectcr.HousekeeperReport',
            editWindowView: 'objectcr.HousekeeperReportEditWindow',         

            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.ObjectCr = asp.controller.getContextValue(asp.controller.getMainComponent(), 'objectcrId');
                    }                   
                },
                aftersetformdata: function (asp, record) {
                    var me = this,
                        grid = me.getGrid();
                    debugger;
                    me.controller.reportId = record.getId();

                    var form = asp.getForm(),
                        archGrid = form.down('housekeeperreportfilegrid'),
                        archStore = archGrid.getStore();

                    archStore.on('beforeload', function (store, operation) {
                        operation.params.reportId = me.controller.reportId;
                    },
                        me);
                    archStore.load();
                }
             
            },            
            tryGetForm: function() {
                return this.componentQuery(this.editFormSelector);
            }
           
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела, грид в окне
            */
            xtype: 'grideditwindowaspect',
            name: 'housekeeperreportfileGridAspect',
            modelName: 'objectcr.HousekeeperReportFile',
            storeName: 'objectcr.HousekeeperReportFile',
            gridSelector: 'housekeeperreportfilegrid',
            editFormSelector: '#housekeeperReportFileEditWindow',
            editWindowView: 'objectcr.HousekeeperReportFileEditWindow',

            listeners: {
                getdata: function (asp, record) {
                    var me = this, store;
                    if (!record.get('Id')) {
                        record.set('HousekeeperReport', me.controller.reportId);
                    }
                }
            }
        }
        
    ],
    init: function () {
        var me = this;
        me.getStore('objectcr.HousekeeperReport').on('beforeload', me.onBeforeLoad, me);
        //me.callParent(arguments);
        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('housekeeperreportgrid'),
            store;

        me.params.id = id;
        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        me.getStore('objectcr.HousekeeperReport').load();

        //store = view.getStore();
        //store.clearFilter(true);
        //store.filter('objectCrId', id);
    },
    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectCrId = me.getContextValue(me.getMainView(), 'objectcrId');
    }
});