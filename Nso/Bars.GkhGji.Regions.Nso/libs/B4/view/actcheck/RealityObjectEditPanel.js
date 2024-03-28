Ext.define('B4.view.actcheck.RealityObjectEditPanel', {
    extend: 'Ext.panel.Panel',
    itemId: 'actCheckRealityObjectEditPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    alias: 'widget.actCheckRealityObjectEditPanel',
    border: false,
    requires: [
        'B4.form.ComboBox',
        'B4.enums.YesNoNotSet',
        'B4.view.actcheck.ViolationGrid'
    ],

    autoScroll: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    margin: '7 5 5 5',
                    itemId: 'taRealityObjAddress',
                    fieldLabel: 'Адрес',
                    readOnly: true
                },
                {
                    xtype: 'b4combobox',
                    itemId: 'cbHaveViolation',
                    margin: 5,
                    floating: false,
                    editable: false,
                    fieldLabel: 'Нарушения выявлены',
                    displayField: 'Display',
                    items: B4.enums.YesNoNotSet.getItems(),
                    valueField: 'Value',
                    name: 'HaveViolation',
                    maxWidth: 500
                },
                {
                    margin: 5,
                    height: 70,
                    itemId: 'taNotRevealedViolations',
                    xtype: 'textarea',
                    name: 'NotRevealedViolations',
                    fieldLabel: 'Нарушения не выявлены',
                    maxLength: 500,
                    readOnly: true
                },
                {
                    xtype: 'tabtextarea',
                    itemId: 'taDescription',
                    margin: 5,
                    height: 70,
                    fieldLabel: 'Описание',
                    readOnly: true,
                    name: 'Description',
                    maxLength: 2000
                },
                {
                    xtype: 'textarea',
                    height: 70,
                    labelAlign: 'top',
                    fieldLabel: 'Сведения, свидетельствующие что нарушения допущены в результате виновных действий (бездействия) должностных лиц и (или) работников проверяемого лица',
                    name: 'OfficialsGuiltyActions',
                    itemId: 'taOfficialsGuiltyActions',
                    margin: '5 5 5 160',
                    maxLength: 2000,
                    labelStyle: "margin: 0 0 0 -100px"
                },
                {
                    xtype: 'textarea',
                    margin: 5,
                    height: 70,
                    fieldLabel: 'Сведения о лицах, допустивших нарушения',
                    name: 'PersonsWhoHaveViolated',
                    itemId: 'taPersonsWhoHaveViolated',
                    maxLength: 2000
                },
                {
                    xtype: 'container',
                    margin: '5 5 10 25',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Если в поле "Описание нарушения" необходимо внести текст размером более трех строк, то воспользуйтесь кнопкой "Редактор"</span>'
                },
                {
                    xtype: 'actCheckViolationGrid',
                    border: false,
                    flex: 1,
                    minHeight: 342
                }
            ]
        });

        me.callParent(arguments);
    }
});