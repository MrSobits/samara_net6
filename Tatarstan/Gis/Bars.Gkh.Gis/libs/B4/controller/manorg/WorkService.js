Ext.define('B4.controller.manorg.WorkService', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.manorg.ServiceWorks'
    ],

    models: [
        'manorg.ManOrgBilWorkService',
        'manorg.ManOrgBilMkdWork'
    ],
    stores: [
        'manorg.ManOrgBilWorkService',
        'manorg.ManOrgBilMkdWork',
        'dict.ContentRepairMkdWorkSelected',
        'dict.ContentRepairMkdWorkForSelect'
    ],
    views: [
        'manorg.WorkServiceEditWindow',
        'manorg.WorkServiceGrid',
        'manorg.ManOrgBilMkdWorkGrid',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    refs: [
        { ref: 'mainView', selector: 'manorgworkservicegrid' },
        { ref: 'editWindow', selector: 'manorgworkserviceeditwindow' }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgserviceworksperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgWorkServiceGridWindowAspect',
            gridSelector: 'manorgworkservicegrid',
            editFormSelector: 'manorgworkserviceeditwindow',
            modelName: 'manorg.ManOrgBilWorkService',
            editWindowView: 'manorg.WorkServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        var view = this.controller.getMainView();
                        record.set('ManagingOrganization', this.controller.getContextValue(view, 'manorgId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var id = record.getId(),
                        grid = form.down('manorgbilmkdworkgrid'),
                        store = grid.getStore(),
                        view = this.controller.getMainView();

                    this.controller.setContextValue(view, 'serviceWorkId', id);
                    this.serviceWorkId = id;
                    grid.setDisabled(!id);

                    if (id) {
                        store.filter('serviceWorkId', id);
                    }
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorgMkdWorkServiceGridAspect',
            gridSelector: 'manorgbilmkdworkgrid',
            modelName: 'manorg.ManOrgBilMkdWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#manorgMultiSelectWindow',
            storeSelect: 'dict.ContentRepairMkdWorkForSelect',
            storeSelected: 'dict.ContentRepairMkdWorkSelected',
            titleSelectWindow: 'Выбор работ МКД',
            titleGridSelect: 'Работы для отбора',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    header: 'Код',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    header: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Наименование'
                }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });
                    if (recordIds[0] > 0) {
                        var view = this.controller.getMainView(),
                            editWindow = this.controller.getEditWindow(),
                            grid = editWindow.down('manorgbilmkdworkgrid'),
                            store = grid.getStore(),
                            serviceId = this.controller.getContextValue(view, 'serviceWorkId');

                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        
                        B4.Ajax.request(
                            B4.Url.action('AddMkdWorks', 'ManagingOrgMkdWork', {
                            mkdWorkIds: Ext.encode(recordIds),
                            serviceWorkId: serviceId
                        })).next(function (response) {
                            store.load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Работы сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    params: {},
   
    init: function () {
        var me = this;

        me.control({
            'manorgworkserviceeditwindow b4selectfield[name=BilService]': {
                change: me.onBilServiceChange
            }
        });

        me.callParent(arguments);
    },

    onBilServiceChange: function (field, val) {
        var editWindow = this.getEditWindow();

        if (val != null) {
            editWindow.down('textfield[name=MeasureName]').setValue(val.MeasureName);
            editWindow.down('textfield[name=ServiceCode]').setValue(val.ServiceCode);
        } else {
            editWindow.down('textfield[name=MeasureName]').setValue('');
            editWindow.down('textfield[name=ServiceCode]').setValue(false);
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgworkservicegrid'),
            editWindow = me.getEditWindow() || Ext.widget('manorgworkserviceeditwindow');

        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        view.getStore().filter('manorgId', id);
        editWindow.down('b4selectfield[name=BilService]').getStore().filter('manorgId', id);
    }
});