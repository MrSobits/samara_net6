Ext.define('B4.controller.fssp.CourtOrderGkuPanel', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonImportAspect'
    ],

    refs: [
        {
            ref: 'litigationGrid',
            selector: 'litigationpanel'
        },
        {
            ref: 'showAllCheckbox',
            selector: 'litigationpanel checkbox[name=ShowAll]'
        },
        {
            ref: 'uploadDownloadInfoGrid',
            selector: 'uploaddownloadinfopanel'
        }
    ],

    views: [
        'fssp.courtordergku.CourtOrderGkuPanel',
        'fssp.courtordergku.LitigationPanel',
        'fssp.courtordergku.AddressMatchingForm',
        'fssp.courtordergku.UploadFileWindow'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.Fssp.CourtOrderGku.Litigation.Edit',
                    applyTo: 'litigationpanel',
                    selector: 'courtordergkupanel',
                    applyBy: function(component, allowed) {
                        component.isAllowed = allowed;
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.UploadDownloadInfo.Add',
                    applyTo: '[dataIndex=LogFile]',
                    selector: 'uploaddownloadinfopanel',
                    applyBy: function(component, allowed) {
                        component.recVisible = allowed;
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.UploadDownloadInfo.Add',
                    applyTo: '[dataIndex=ReloadFile]',
                    selector: 'uploaddownloadinfopanel',
                    applyBy: function (component, allowed) {
                        component.recVisible = allowed;
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.UploadDownloadInfo.Add',
                    applyTo: '[dataIndex=FollowToMatchingAddress]',
                    selector: 'uploaddownloadinfopanel',
                    applyBy: function (component, allowed) {
                        component.recVisible = allowed;
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.UploadDownloadInfo.Add',
                    applyTo: '[dataIndex=UploadFile]',
                    selector: 'uploaddownloadinfopanel',
                    applyBy: function (component, allowed) {
                        component.recVisible = allowed;
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.UploadDownloadInfo.Add',
                    applyTo: '[itemId=btnImport]',
                    selector: 'uploaddownloadinfopanel',
                    applyBy: function (component, allowed) {
                        if (!allowed) {
                            component.disable();
                        }
                    }
                },
            ]
        },
        {
            xtype: 'gkhbuttonimportaspect',
            buttonSelector: 'uploaddownloadinfopanel #btnImport',
            windowImportView: 'fssp.courtordergku.UploadFileWindow',
            windowImportSelector: 'uploadfilewindow',
            loadImportList: false,
            getUrl: function () {
                return B4.Url.action('/UploadDownloadInfo/Import');
            }
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'fssp.courtordergku.CourtOrderGkuPanel',
    mainViewSelector: 'courtordergkupanel',

    init: function() {
        var me = this;

        me.control({
            'litigationpanel': {
                rowaction: me.onLitigationGridRowAction,
                afterrender: function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeLitigationStoreLoad, me);
                }
            },
            'litigationpanel checkbox[name=ShowAll]': {
                change: me.onCheckBoxChange
            },
            'addressmatchingform pgmuaddressgrid': {
                'pgmuAddressStore.load': me.onAfterPgmuAddressStoreLoad,
                'pgmuAddressGrid.select': me.onPgmuAddressSelect
            },
            'addressmatchingform pgmuaddressgrid b4savebutton': {
                click: me.onAddressMatchingSave
            },
            'uploaddownloadinfopanel': {
                rowaction: me.onUploadDownloadInfoGridRowAction
            },
            'uploaddownloadinfopanel b4updatebutton': {
                 click: me.updateGrid
            },
            'uploaddownloadinfopanel button[action=ImportFile]': {
                'click': {
                    fn: me.onImportFromFile,
                    scope: me
                }
            }
        });
        
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('courtordergkupanel');

        me.bindContext(view);
        me.application.deployView(view);
        
        if (me.addressMatchingForm === undefined){
            var matchingForm = Ext.create('B4.view.fssp.courtordergku.AddressMatchingForm');
            matchingForm.on('close', me.onAddressMatchingFormClose);
            me.addressMatchingForm = matchingForm;
        }
        
        if (me.addressMatchingForm.hidden){
            me.getLitigationGrid().getStore().load();
            me.getUploadDownloadInfoGrid().getStore().load();
        }
    },
    
    onLitigationGridRowAction: function (grid, action, record) {
        if (action === 'addressmatch') {
            var me = this,
                view = me.addressMatchingForm;
            
            view.debtorFsspAddress = Ext.create('B4.model.fssp.courtordergku.FsspAddress', record.data.DebtorFsspAddress);
            view.show();
            view.down('pgmuaddressgrid').getStore().load();
        }
    },

    onUploadDownloadInfoGridRowAction: function (grid, action, record) {
        if (action === 'matchaddresspanel') {
            var me = this,
                view = me.getMainView(),
                tabPanel = view.down('tabpanel'),
                addressMatchingPanel = tabPanel.down('fsspaddressmatchingpanel');

            tabPanel.setActiveTab(addressMatchingPanel);
        }

        if (action === 'reloadFile') {
            B4.Ajax.request({
                url: B4.Url.action('Import', 'UploadDownloadInfo'),
                params: {
                    id: record.getId()
                },
                timeout: 9999999
            })
                .next(function (response) {
                    var json = Ext.JSON.decode(response.responseText);
                    if (json.success) {
                        Ext.Msg.alert('Повторный импорт!', 'Файл добавлен в очередь загрузки');
                    } else {
                        Ext.Msg.alert('Ошибка!', json.message);
                    }
                });
        }
    },
    
    onBeforeLitigationStoreLoad: function (store, options) {
        options.params.showAll = this.getShowAllCheckbox().getValue();
    },
    
    onCheckBoxChange: function () {
        this.getLitigationGrid().getStore().load();
    },

    onAfterPgmuAddressStoreLoad: function () {
        var me = this,
            view = me.addressMatchingForm,
            grid = view.down('pgmuaddressgrid'),
            pgmuAddress = view.debtorFsspAddress.data.PgmuAddress;

        if (pgmuAddress && (pgmuAddress.Id || 0) > 0) {
            var record = grid.getStore().getById(pgmuAddress.Id);

            if (record !== null) {
                grid.getSelectionModel().doSelect(record);
            }
        }
    },
    
    onPgmuAddressSelect: function (checkbox, record) {
        var me = this,
            debtorFsspAddress = me.addressMatchingForm.debtorFsspAddress;

        if (debtorFsspAddress) {
            debtorFsspAddress.set('PgmuAddress', record.getData() || {});
        }
    },

    onAddressMatchingSave: function () {
        var me = this,
            view = me.addressMatchingForm,
            grid = me.getLitigationGrid(),
            record = view.debtorFsspAddress;
        
        if (record && record.data.PgmuAddress){
            record.save().next(function () {
                grid.getStore().load();
            }).error(function (err) {
                Ext.Msg.alert('Ошибка', err.message || err);
            });
        }
        
        view.close();
    },

    onAddressMatchingFormClose: function () {
        var view = this,
            grid = view.down('pgmuaddressgrid');
        
        grid.clearHeaderFilters();
        grid.getStore().currentPage = 1;
    },

    updateGrid: function (btn) {
        btn.up('uploaddownloadinfopanel').getStore().load();
    },

    onImportFromFile: function() {
        var window = Ext.create('B4.view.fssp.courtordergku.UploadFileWindow');
        window.show();
    }
});