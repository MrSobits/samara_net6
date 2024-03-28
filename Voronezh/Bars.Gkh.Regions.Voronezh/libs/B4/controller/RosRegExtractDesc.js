Ext.define('B4.controller.RosRegExtractDesc', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
         'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['RosRegExtractDesc',
             'RosRegExtractPers',
             'RosRegExtractGov',
             'RosRegExtractOrg'],
    stores: ['RosRegExtractDesc',
             'RosRegExtractPers',
             'RosRegExtractGov',
             'RosRegExtractOrg'],
    views: [
        'rosregextract.Grid',
        'rosregextract.EditWindow',
        'rosregextract.PersonGrid',
        'rosregextract.OrgGrid',
         'rosregextract.Grid'
    ],
    parentId: null,

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'rosregextract.Grid',
    mainViewSelector: 'rosregextractgrid',
    editWindowSelector: 'rosregextractEditWindow',
    aspects: [

    {
        xtype: 'grideditwindowaspect',
        name: 'rosregextractGridWindowAspect',
        gridSelector: 'rosregextractgrid',
        editFormSelector: '#rosregextractEditWindow',
        storeName: 'RosRegExtractDesc',
        modelName: 'RosRegExtractDesc',
        editWindowView: 'rosregextract.EditWindow',
        otherActions: function (actions) {
            actions['#rosregextractEditWindow #sfPersAcc'] = { 'beforeload': { fn: this.onBeforeLoadPersAcc, scope: this } };
            actions['#rosregextractEditWindow #PrintExtract'] = { 'mouseover': { fn: this.printReport, scope: this } };
        },
        onBeforeLoadPersAcc: function (store, operation) {
            debugger;
            operation = operation || {};
            operation.params = operation.params || {};

            operation.params.parentId = parentId;
        },
        printReport: function (button) {
            //Обновление ссылки при наведении на кнопку выписки
            var Id = parentId;
            button.btnEl.dom.href = B4.Url.action('/ExtractPrinter/PrintExtractForDescription?id=' + Id);
        },
        listeners: {
            aftersetformdata: function (asp, rec, form) {
                var me = this;
                //debugger;
                parentId = rec.getId();
                //Выставляем Id комнаты для кнопки изменения площади
                var roomId = rec.data.Room_id;
                var button = form.down('#ChangeRoomAreaButton');
                button.entityId=roomId;
                
                var grid = form.down('rosregextractpersongrid'),
                store = grid.getStore();
                store.filter('parentId', rec.getId());

                var grid2 = form.down('rosregextractorggrid'),
                store2 = grid2.getStore();
                store2.filter('parentId', rec.getId());

                var grid3 = form.down('rosregextractgovgrid'),
                store3 = grid3.getStore();
                store3.filter('parentId', rec.getId());

                //var grid4 = form.down('vprresolutiongrid'),
                //store4 = grid4.getStore();
                //store4.filter('parentId', rec.getId());



            }
        }
  
    }
],


    refs: [
        {
            ref: 'mainView',
            selector: 'rosregextractgrid'
        }
    ],
    init: function () {
        var me = this;
        

        me.callParent(arguments);
    },

    init2: function () {
        var me = this;
        
        //me.control({
          
        //    'dateareaownergrid button[action="Merge"]': {
        //        click: me.onMerge
        //    },
          
        //});
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('rosregextractgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onMerge: function () {
        debugger;
        B4.Ajax.request({
            url: B4.Url.action('Merge', 'DataAreaOwnerMerger')
        });
    },
});