Ext.define('B4.view.realityobj.technicalmonitoring.TechnicalMonitoringEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.technicalmonitoringeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    minWidth: 400,
    minHeight: 250,
    autoScroll: true,
    bodyPadding: 10,
    title: 'Технический мониторинг',
    itemId: 'technicalmonitoringEditWindow',

    requires: [
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    allowBlank: false,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.MonitoringTypeDict',
                    name: 'MonitoringTypeDict',
                    fieldLabel: 'Тип мониторинга',
                    allowBlank: false,
                    editable: false,
                    itemId: 'sfMonitoringTypeDict'
                },
                {
                    xtype: 'numberfield',
                    name: 'TotalBuildingVolume',
                    fieldLabel: 'Общий строительный объем (куб.м.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    itemId: 'nfTotalBuildingVolume',
                    hidden: true,
                    minValue: 0,
                    flex: 1
                },
                {
                    xtype: 'numberfield',
                    name: 'AreaMkd',
                    fieldLabel: 'Общая площадь (кв.м.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    itemId: 'nfAreaMkd',
                    hidden: true,
                    minValue: 0,
                    flex: 1
                },
                {
                    xtype: 'numberfield',
                    name: 'AreaLiving',
                    fieldLabel: 'Площадь жилых помещений (кв.м.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    itemId: 'nfAreaLiving',
                    decimalSeparator: ',',
                    hidden: true,
                    minValue: 0,
                    flex: 1
                },
                {
                    xtype: 'numberfield',
                    name: 'AreaNotLiving',
                    fieldLabel: 'Площадь нежилых помещений (кв.м.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    itemId: 'nfAreaNotLiving',
                    decimalSeparator: ',',
                    hidden: true,
                    minValue: 0,
                    flex: 1
                },
                {
                    xtype: 'numberfield',
                    name: 'AreaLivingNotLivingMkd',
                    fieldLabel: 'Общая площадь жилых и нежилых помещений (кв.м.)',
                    editable: false,
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    itemId: 'nfAreaLivingNotLivingMkd',
                    decimalSeparator: ',',
                    hidden: true,
                    minValue: 0,
                    flex: 1
                },
                {
                    xtype: 'numberfield',
                    name: 'AreaNotLivingFunctional',
                    fieldLabel: 'Общая площадь помещений, входящих в состав общего имущества (кв.м.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    itemId: 'nfAreaNotLivingFunctional',
                    hidden: true,
                    minValue: 0,
                    flex: 1
                },
                {
                    xtype: 'numberfield',
                    name: 'Floors',
                    fieldLabel: 'Количество этажей',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    itemId: 'nfFloors',
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'NumberApartments',
                    fieldLabel: 'Количество квартир',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    itemId: 'nfNumberApartments',
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.WallMaterial',
                    name: 'WallMaterial',
                    fieldLabel: 'Материал стен',
                    editable: false,
                    hidden: true,
                    itemId: 'sfWallMaterial'
                },
                {
                    xtype: 'numberfield',
                    name: 'PhysicalWear',
                    fieldLabel: 'Физический износ',
                    itemId: 'nfPhysicalWear',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                //{
                //    xtype: 'numberfield',
                //    name: 'TotalWear',
                //    fieldLabel: 'Общий износ',
                //    itemId: 'nfTotalWear',
                //    editable: false,
                //    hideTrigger: true,
                //    keyNavEnabled: false,
                //    mouseWheelEnabled: false,
                //    allowDecimals: false,
                //    hidden: true,
                //    minValue: 0,
                //    maxValue: 100
                //},
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.CapitalGroup',
                    name: 'CapitalGroup',
                    itemId: 'sfCapitalGroup',
                    editable: false,
                    hidden: true,
                    fieldLabel: 'Группа капитальности',
                },
                {
                    xtype: 'numberfield',
                    name: 'WearFoundation',
                    fieldLabel: 'Износ фундамента',
                    itemId: 'nfWearFoundation',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearWalls',
                    fieldLabel: 'Износ стен',
                    itemId: 'nfWearWalls',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearRoof',
                    fieldLabel: 'Износ крыши',
                    itemId: 'nfWearRoof',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearInnerSystems',
                    fieldLabel: 'Износ внутриинженерных систем',
                    itemId: 'nfWearInnerSystems',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearHeating',
                    fieldLabel: 'Износ теплоснабжения',
                    itemId: 'nfWearHeating',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearWater',
                    fieldLabel: 'Износ водоснабжения',
                    itemId: 'nfWearWater',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearWaterCold',
                    fieldLabel: 'Износ холодного водоснабжения',
                    itemId: 'nfWearWaterCold',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearWaterHot',
                    fieldLabel: 'Износ горячего водоснабжения',
                    itemId: 'nfWearWaterHot',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearSewere',
                    fieldLabel: 'Износ водоотведения',
                    itemId: 'nfWearSewere',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearElectric',
                    fieldLabel: 'Износ электроснабжения',
                    itemId: 'nfWearElectric',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearLift',
                    fieldLabel: 'Износ лифта (-ов)',
                    itemId: 'nfWearLift',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'numberfield',
                    name: 'WearGas',
                    fieldLabel: 'Износ газоснабжения',
                    itemId: 'nfWearGas',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    hidden: true,
                    minValue: 0,
                    maxValue: 100
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    allowBlank: false,
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'b4combobox',
                    name: 'UsedInExport',
                    fieldLabel: 'Выводить на портал',
                    displayField: 'Display',
                    items: B4.enums.YesNo.getItems(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});