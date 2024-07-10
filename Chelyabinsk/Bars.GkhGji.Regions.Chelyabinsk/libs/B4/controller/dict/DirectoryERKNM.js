Ext.define('B4.controller.dict.DirectoryERKNM', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    erknmId: null,

    models: [
        'dict.erknm.DirectoryERKNM',
        'dict.erknm.RecordDirectoryERKNM',
        'smev.ERKNMDictFile'
    ],
    stores: [
        'dict.erknm.DirectoryERKNM',
        'dict.erknm.RecordDirectoryERKNM',
        'smev.ERKNMDictFile'
    ],
    views: [

        'dict.erknm.Grid',
        'dict.erknm.EditWindow',
        'dict.erknm.RecordGrid',
        'dict.erknm.FileGrid'
    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'erknmDirectoryERKNMGridAspect',
            gridSelector: 'directoryERKNMGrid',
            editFormSelector: '#directoryERKNMEditWindow',
            storeName: 'dict.erknm.DirectoryERKNM',
            modelName: 'dict.erknm.DirectoryERKNM',
            editWindowView: 'dict.erknm.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    
                    asp.controller.erknmId = record.getId();
                    var grid = form.down('recordDirectoryERKNMGrid'),
                        fileGrid = form.down('erknmDictFileGrid'),
                        store = grid.getStore(),
                        fileStore = fileGrid.getStore();
                    store.filter('Id', record.getId());
                    fileStore.filter('ERKNMDictId', record.getId());
                    /*asp.controller.setTypeKNDDictId(erknmId);*/
                    if (record.getId() > 0) {
                        form.down('recordDirectoryERKNMGrid').setDisabled(false);
                        form.down('erknmDictFileGrid').setDisabled(false);
                    } else {
                        form.down('recordDirectoryERKNMGrid').setDisabled(true);
                        form.down('erknmDictFileGrid').setDisabled(true);
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'erknmRecordDirectoryERKNMGridAspect',
            storeName: 'dict.erknm.RecordDirectoryERKNM',
            modelName: 'dict.erknm.RecordDirectoryERKNM',
            gridSelector: 'recordDirectoryERKNMGrid',
            otherActions: function (actions) {
                actions[this.gridSelector + ' #compareDict'] = { 'click': { fn: this.compareDict, scope: this } };
            },
            compareDict: function () {
                var me = this,
                    grid = me.getGrid(),
                    code = grid.up('#directoryERKNMEditWindow').down('#tfCodeERKNM').value;

                    //record = grid.getForm().getRecord();

                me.mask('Обмен информацией со СМЭВ');
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('SendCompareDict', 'ERKNMExecute'),
                    params: {
                        dictGuid: code,
                    }
                }).next(function (response) {
                    
                    me.unmask();
                    var data = Ext.decode(response.responseText);
                    Ext.Msg.alert('Сообщение', data.data);
                }).error(function (err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err);
                });;
            },
            listeners: {
                beforesave: function (asp, store) {
                    var me = this;
                    if (me.controller.erknmId != null) {
                        Ext.each(store.data.items, function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('DirectoryERKNM', me.controller.erknmId);
                            }
                        });
                    }
                }
            }
        }
    ],

    mainView: 'dict.erknm.Grid',
    mainViewSelector: 'directoryERKNMGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'directoryERKNMGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

   

    index: function () {
        var view = this.getMainView() || Ext.widget('directoryERKNMGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.erknm.DirectoryERKNM').load();
    }
});