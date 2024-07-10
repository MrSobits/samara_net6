Ext.define('B4.controller.ExtractEgrn', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],
    selectedRo:null,
    models: ['ExtractEgrn',
        'ExtractEgrnRightInd'],
    stores: ['ExtractEgrn',
        'ExtractEgrnRightInd'],
    views: [
        'ExtractEgrn.EditWindow',
        'ExtractEgrn.Grid',
        'ExtractEgrn.IndGrid'
    ],
    parentId: null,
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'ExtractEgrn.Grid',
    mainViewSelector: 'extractegrngrid',
    editWindowSelector: 'extractegrnEditWindow',
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'extractegrnGridWindowAspect',
            gridSelector: 'extractegrngrid',
            editFormSelector: '#extractegrnEditWindow',
            storeName: 'ExtractEgrn',
            modelName: 'ExtractEgrn',
            editWindowView: 'ExtractEgrn.EditWindow',
            otherActions: function (actions) {
                actions['#extractegrnEditWindow #sfPersAcc'] = { 'beforeload': { fn: this.onBeforeLoadPersAcc, scope: this } };
                actions['#extractegrnEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRo, scope: this } };
               
            },
            onChangeRo: function (cmp, newValue) {
                var me = this;
                
                if (newValue)
                {
                    me.controller.selectedRo = newValue.Id;
                }
               

            },
            onBeforeLoadPersAcc: function (store, operation) {
                var me = this;

                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.parentId = parentId;
                operation.params.selectedRo = me.controller.selectedRo;
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                    parentId = rec.getId();
                    extractId = rec.raw.ExtractId ? rec.raw.ExtractId.Id: 0;
                    me.controller.selectedRo = null;
                    var grid = form.down('extractegrnindgrid'),
                        store = grid.getStore();
                    store.filter('parentId', rec.getId());
                }
            }
        }
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'extractegrngrid'
        }
    ],
    init: function() {
        var me = this;
        actions = [];
        actions['extractegrneditwindow button[action=getExtract]'] = { 'click': { fn: this.getExtract, scope: this } };
        actions['extractegrngrid actioncolumn[action=getExtract]'] = { 'click': { fn: this.getExtractFromGrid, scope: this } };
        actions['extractegrngrid button[action=mergeRooms]'] = { 'click': { fn: this.mergeRooms, scope: this } };
       
        me.control(actions);
        me.callParent(arguments);
    },
    getExtractFromGrid: function (grid, row, col, p1, p2, record, asp) {
        
        var asp = this,
            rec = record.data.ExtractId ? record.data.ExtractId.Id : 0;

        //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
        B4.Ajax.request({
            //method: 'POST',
            url: B4.Url.action('DownloadExtract', 'ExtractActions'),
            params: {
                Id: rec
            }
        }).next(function (resp) {
            var tryDecoded;

            //asp.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;
            if (tryDecoded.success == false) {
                throw new Error(tryDecoded.message);
            }
            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });

                //me.fireEvent('onprintsucess', me);
            }
        }).error(function (err) {
            //asp.unmask();
            Ext.Msg.alert('Ошибка', err.message);
        });
    },
    getExtract: function (btn) {
        var asp = this,
            rec = extractId;

        //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
        B4.Ajax.request({
            //method: 'POST',
            url: B4.Url.action('DownloadExtract', 'ExtractActions'),
            params: {
                Id: rec
            }
        }).next(function (resp) {
            var tryDecoded;

            //asp.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;
            if (tryDecoded.success == false) {
                throw new Error(tryDecoded.message);
            }
            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });

                //me.fireEvent('onprintsucess', me);
            }
        }).error(function (err) {
            //asp.unmask();
            Ext.Msg.alert('Ошибка', err.message);
        });
    },
    mergeRooms: function (btn) {
        var asp = this;
        B4.Ajax.request({
            //method: 'POST',
            url: B4.Url.action('RunTask', 'DRosRegExtractMerger')           
        }).next(function (resp) {
            Ext.Msg.alert('Внимание', 'Задача успешно поставлена');
            }).error(function (err) {
            //asp.unmask();
            Ext.Msg.alert('Ошибка', err.message);
        });
    },
    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('extractegrngrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});