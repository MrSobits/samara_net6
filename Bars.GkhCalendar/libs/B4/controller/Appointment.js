Ext.define('B4.controller.Appointment', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],
  
    models: [
        'AppointmentGridModel',
        'AppointmentDiffDayGridModel',
        'AppointmentTimeGridModel'
    ],
    stores: [
        'AppointmentGridStore',
        'AppointmentDiffDayGridStore',
        'AppointmentTimeGridStore'
    ],
    views: [

        'AppointmentGrid',
        'AppointmentDiffDayGrid',
        'AppointmentDiffDayEditWindow',
        'AppointmentTimeGrid',
        'AppointmentEditWindow',
        'AppointmentTimeEditWindow'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'appointmentGridAspect',
            gridSelector: 'appointmentgrid',
            editFormSelector: '#appointmentEditWindow',
            storeName: 'AppointmentGridStore',
            modelName: 'AppointmentGridModel',
            editWindowView: 'AppointmentEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {

            },

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    //var timeGrid = form.down('appointmenttimegrid'),
                    //    timeStore = timeGrid.getStore();
                    //timeStore.filter('Id', record.getId());

                    var diffDayGrid = form.down('appointmentdiffdaygrid'),
                        diffDayStore = diffDayGrid.getStore();
                    diffDayStore.filter('Id', record.getId());

                }


                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appointmentTimeGridAspect',
            gridSelector: 'appointmenttimegrid',
            editFormSelector: '#appointmentTimeEditWindow',
            storeName: 'AppointmentTimeGridStore',
            modelName: 'AppointmentTimeGridModel',
            editWindowView: 'AppointmentTimeEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {

            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appointmentDiffDayGridAspect',
            gridSelector: 'appointmentdiffdaygrid',
            editFormSelector: '#appointmentDiffDayEditWindow',
            storeName: 'AppointmentDiffDayGridStore',
            modelName: 'AppointmentDiffDayGridModel',
            editWindowView: 'AppointmentDiffDayEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {

            }
        },
    ],

    mainView: 'AppointmentGrid',
    mainViewSelector: 'appointmentgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'appointmentgrid'
        }
        
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({

        });

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('appointmentgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('AppointmentGridStore').load();
    }
});