Ext.define('B4.view.desktop.portlet.Map', {
    extend: 'B4.view.desktop.portal.Portlet',

    requires: ['B4.view.Control.YMap'],
    alias: 'widget.maplet',
    ui: 'b4portlet',
    cls: 'x-portlet',
    title: 'Карта',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'maportlet',
    collapsible: false,
    closable: false,
    header: true,
    draggable: false,
    months: {
        1: 'Январь',
        2: 'Февраль',
        3: 'Март',
        4: 'Апрель',
        5: 'Май',
        6: 'Июнь',
        7: 'Июль',
        8: 'Август',
        9: 'Сентябрь',
        10: 'Октябрь',
        11: 'Ноябрь',
        12: 'Декабрь'
    },

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                    {
                        layout: 'card',
                        itemId: 'topPanel',
                        height: document.height - 200 || 600,
                        activeItem: 0,
                        items: [
                            {
                                xtype: 'panel',
                                border: false,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                items: [
                                    {
                                        xtype: 'panel',
                                        itemId: 'controlPanel',
                                        border: false,
                                        hidden: true,
                                        padding: '10 10 0',
                                        defaults: {
                                            scale: 'medium'
                                        },
                                        items: [
                                            {
                                                xtype: 'button',
                                                text: 'Назад',
                                                margin: '0 70px 0 0',
                                                handler: function () {
                                                    B4.YMap.showRegions();
                                                    this.up('panel').hide();
                                                    this.up('#maportlet').setTitle('Карты');
                                                }
                                            },
                                            {
                                                xtype: 'button',
                                                itemId: 'hsId',
                                                text: 'В разрезе домов',
                                                pressed: true,
                                                toggleGroup: 'rq'
                                            },
                                            {
                                                xtype: 'button',
                                                text: 'В разрезе поставщиков информации',
                                                itemId: 'infId',
                                                enabledToogle: true,
                                                toggleGroup: 'rq',
                                                margin: '0 70px 0 0',
                                                handler: function () {
                                                    var layout = me.down('#topPanel').getLayout();
                                                    layout.setActiveItem(1);
                                                    this.up('#maportlet').setTitle('Муниципальное образовние');
                                                }
                                            }
                                            //,
                                            //{
                                            //    xtype: 'button',
                                            //    text: 'Паспорт ОКИ',
                                            //    handler: function () {
                                            //        console.log(this);
                                            //    }
                                            //}
                                        ]
                                    },
                                    {
                                        xtype: 'component',
                                        flex: 1,
                                        renderTpl: new Ext.XTemplate(
                                            '<div class="y-content">',
                                                '<div id="y-map" class="y-map">',
                                                '</div>',
                                            '</div>'
                                            ),

                                        afterRender: function () {
                                            B4.view.Control.YMap.init({
                                                renderTo: 'y-map'
                                            });
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'panel',
                                border: false,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                items: [
                                    {
                                        xtype: 'panel',
                                        border: false,
                                        padding: '10 10 5',
                                        defaults: {
                                            scale: 'medium'
                                        },
                                        items: [
                                            {
                                                xtype: 'button',
                                                text: 'Назад',
                                                margin: '0 68px 0 0',
                                                handler: function () {
                                                    me.down('#topPanel').getLayout().setActiveItem(0);
                                                    me.down('#hsId').toggle(true);
                                                    this.up('#maportlet').setTitle('Муниципалитет');
                                                }
                                            }
                                            //,
                                            //{
                                            //    xtype: 'button',
                                            //    text: 'Управляющие организация',
                                            //    toggleGroup: 'rq',
                                            //    pressed: true,
                                            //    handler: function () {
                                            //        console.log(this);
                                            //    }
                                            //},
                                            //{
                                            //    xtype: 'button',
                                            //    text: 'Ресурсо-снабжающие организации',
                                            //    enabledToogle: true,
                                            //    toggleGroup: 'rq',
                                            //    handler: function () {
                                            //        console.log(this);
                                            //    }
                                            //},
                                            //{
                                            //    xtype: 'button',
                                            //    text: 'Поставщики комунальных услуг',
                                            //    enabledToogle: true,
                                            //    toggleGroup: 'rq',
                                            //    handler: function () {
                                            //        console.log(this);
                                            //    }
                                            //},
                                        ]
                                    },
                                    {
                                        xtype: 'grid',
                                        padding: '10 10',
                                        flex: 1,
                                        itemId: 'pgrid',
                                        store: Ext.create('B4.store.Maplet'),
                                        columnLines: true,
                                        columns: [
                                            {
                                                text: 'Имя',
                                                flex: 1,
                                                dataIndex: 'Name'
                                            },
                                            //{
                                            //    text: 'Месяц',
                                            //    flex: 1,
                                            //    width: 150,
                                            //    dataIndex: 'ReportMonth',
                                            //    renderer: function(val) {
                                            //        return me.months[val];
                                            //    }
                                            //},
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'Percent',
                                                align: 'center',
                                                text: '%',
                                                tdCls: 'x-progress-cell',
                                                renderer: function (value) {
                                                    return value + '%';
                                                },
                                                width: 100
                                            },

                                            {
                                                xtype: 'actioncolumn',
                                                text: 'Действия',
                                                width: 174,
                                                items: [{
                                                    tooltip: 'Перейти к паспортам',
                                                    icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKgAAAAWCAYAAABUi9exAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NjRCRTAyNkUxMzFGMTFFM0E1OEE4MzMwREIzQkFCRDUiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NjRCRTAyNkYxMzFGMTFFM0E1OEE4MzMwREIzQkFCRDUiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo2NEJFMDI2QzEzMUYxMUUzQTU4QTgzMzBEQjNCQUJENSIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo2NEJFMDI2RDEzMUYxMUUzQTU4QTgzMzBEQjNCQUJENSIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PvsEoDoAAAX6SURBVHja7FtNTBtH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',
                                                    handler: function (grid, index) {
                                                        var store = Ext.ComponentQuery.query('#tgrid')[0].getStore(),
                                                            rec = Ext.ComponentQuery.query('#pgrid')[0].getStore().getAt(index);
                                                        store.getProxy().extraParams = {
                                                            muId: B4.YMap.districtId,
                                                            contragentId: rec.data.Id
                                                        };
                                                        store.load();
                                                        me.down('#topPanel').getLayout().setActiveItem(2);
                                                    }
                                                }]
                                            }
                                        ],
                                        viewConfig: {
                                            loadMask: true,
                                            getRowClass: function (record, index) {
                                                var c = parseFloat(record.get('Percent'));
                                                if (c == isNaN) {
                                                    return;
                                                }

                                                if (c == 100) {
                                                    return 'x-percent-100';
                                                }

                                                if (c <= 10) {
                                                    return 'x-percent-10';
                                                } else if (c > 10 && c <= 20) {
                                                    return 'x-percent-20';
                                                } else if (c > 20 && c <= 40) {
                                                    return 'x-percent-30';
                                                } else if (c > 40 && c <= 70) {
                                                    return 'x-percent-70';
                                                } else if (c > 70 && c <= 99) {
                                                    return 'x-percent-90';
                                                };
                                                return;
                                            }
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'panel',
                                border: false,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                items: [
                                    {
                                        xtype: 'panel',
                                        border: false,
                                        padding: '10 10 5',
                                        defaults: {
                                            scale: 'medium'
                                        },
                                        items: [
                                            {
                                                xtype: 'button',
                                                text: 'Назад',
                                                margin: '0 68px 0 0',
                                                handler: function () {
                                                    me.down('#topPanel').getLayout().setActiveItem(1);
                                                }
                                            }
                                            //,
                                            //{
                                            //    xtype: 'button',
                                            //    text: 'Многоквартирные дома',
                                            //    toggleGroup: 'rq',
                                            //    pressed: true,
                                            //    handler: function () {
                                            //        console.log(this);
                                            //    }
                                            //},
                                            //{
                                            //    xtype: 'button',
                                            //    text: 'Жилые дома',
                                            //    enabledToogle: true,
                                            //    toggleGroup: 'rq',
                                            //    handler: function () {
                                            //        console.log(this);
                                            //    }
                                            //}
                                        ]
                                    },
                                    {
                                        xtype: 'grid',
                                        itemId: 'tgrid',
                                        padding: '10 10',
                                        flex: 1,
                                        store: Ext.create('B4.store.MapletPass'),
                                        columnLines: true,
                                        columns: [
                                            {
                                                text: 'Имя',
                                                flex: 1,
                                                dataIndex: 'Name'
                                            },
                                            {
                                                text: 'Месяц',
                                                width: 100,
                                                dataIndex: 'ReportMonth',
                                                renderer: function (val) {
                                                    return me.months[val];
                                                }
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'Percent',
                                                align: 'center',
                                                text: '%',
                                                tdCls: 'x-progress-cell',
                                                renderer: function (value) {
                                                    return value + '%';
                                                },
                                                width: 100
                                            },
                                            {
                                                xtype: 'actioncolumn',
                                                text: 'Действия',
                                                width: 86,
                                                items: [
                                                    {
                                                        tooltip: 'Перейти к паспорту',
                                                        icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFEAAAAWCAYAAAC40nDiAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RkI1MzY2NkMxMzIwMTFFM0I1MDhCQzNFOUM0MjU1MTMiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RkI1MzY2NkQxMzIwMTFFM0I1MDhCQzNFOUM0MjU1MTMiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpGQjUzNjY2QTEzMjAxMUUzQjUwOEJDM0U5QzQyNTUxMyIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpGQjUzNjY2QjEzMjAxMUUzQjUwOEJDM0U5QzQyNTUxMyIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PkGk5owAAANpSURBVHja7JlLSBtBGMf/s9ltEo2PihilEBoIEWEpVrC914N6KIJQvJSWHIteqsFj6Tmk7cXSo4e2h1wCpYcmh3hvBfUglFohJbZoJI2vaBLzmM5sspvEiKapgin7h1zm253d+c332gyhlOZ+7STJq09rZG0rga29NHSdre42I5zdFjwdddIb182U/Iwf5R+++UKyMMBgENnPoFM6R7lcDtlsFhLJ4d2TO9SAWw+eR3YyMBqNEARBJ1SDOCdRFHGczWEjliDC0o9dSJKkk6lDoijhO0uBwmE6p3tgneKpj9cQnd5FeGR9t9kx7+6E7VRbEkHvKjw6xFqUxcrCMmaWyoYGevHhnqR74oVKgdpKLaURElldhCtQZadI7eP13DfBrxiseDFlQ7/pxHyFa3DiGlpmI34tUtoRXz2CQ1afr0QI8ZTuy6vvVDY/Lb3DlYDIFsIAxVcX344F8IiPzLoGMSzLmA3wcNfsGAtYBb6wxxNW6vdFtUVVAB+REXKoliKIwxiG5sKkODednOql0ECK6JclBm6RstQijE/cppNumYKBnJmLKp1KEXYivrBsKUYUqavluSSCzDvCcHkXiasIkMuzmVR3/xgjzSkbktGvAf7iUczMMZhlAM/28HY4TMyz5sPakGc+RiKmVnJ/BCulTVBys7JGv2+XRGAmfSNFz22YcK4uQgIPKw55vE0yIXUkrtczZZcESyqDynuPEE8BjjZri5qz49un2sE3rUEgluAlNiIY8kV5SNKQXF/IXOkvmMv6vGThChtP9iwnjfmqdl7072XyMEkdjnpm384gYZJQeW8TOlghiu9Fb6r+0dF1qp00CsSChxcWSrVKLJvZAiRYB2BA4FBgOUpgOUoRS/wIuez5mmZf2sV6yoxhl10bmnV1Kpv2MQDtHxQbL2LV9gZqcVgFDvawauweFIYVkEkS9MbQx0K84CGs8CxcYy3OIA9x3oLQoDdc46byQgSlQofcnWqLQ0rtT7Gw/AZ/Poar2qMKNf/rUsngsxBtaWn5z7JUIR+jvEW6JB0cHOjfzheSE5uuCaB5qpOoQ/l8DtZWI8j0+xX6ObwPo9GkU/lLpdMp3LW3QpgedVKRdSTpdFohq6s2D+QAOTfOj6gHVS/5QdVmAtF9/aDqPPEQdvZYFID8oOqPAAMA/XxvRFTNlYsAAAAASUVORK5CYII=',
                                                        handler: function (grid, rowIndex, colIndex) {
                                                            var record = grid.getStore().getAt(rowIndex);
                                                            if (record) {
                                                                        Ext.History.add('housepasspeditor/' + record.raw.Id + '/');

                                                            }
                                                        }
                                                    }
                                                    //,
                                                    //{
                                                    //    tooltip: 'Перейти к архиву',
                                                    //    icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAAAWCAIAAAD/3A1jAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NUIyRjYwQkYxMzIxMTFFM0FEQTBEMzhBRDNEQ0I5MTgiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NUIyRjYwQzAxMzIxMTFFM0FEQTBEMzhBRDNEQ0I5MTgiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo1QjJGNjBCRDEzMjExMUUzQURBMEQzOEFEM0RDQjkxOCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo1QjJGNjBCRTEzMjExMUUzQURBMEQzOEFEM0RDQjkxOCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PlMx3K0AAAMBSURBVHja1JdBTNNQGMe/13ZP2nUMSBYQSchoIqexRFAOhouExANkuBMH44mbBiNeOe6sySI3T0YTT9NlHIgGL5yIjoDEGFRclkyBLEFGu1W7ts/XdsOySAIRiH3Z4TVr0+/7/7/f9/ohQoghf6+8e2jsfCKVLfDCQkIH23ZRGLjHBjqRvvdtb+4mQgi4JsRwnkiAmDroP6n0zaNP0d6b+/r2CoP94LVlajLXfonRt5aRjwcPLsTxxs5nBvQyQownE2A4Cq0nQ3evY1OLh57xIdHIx5Tsf5HAcR2YwiGRaAp7PoE96UB/LwsF7eMPXzTsk0DbqGUlxgdJPsd2R5B1WdBSt1XHq2BOnZvRYMw/Osn9Wii9BjE+DHX3bDN3rZulRHPUedZeMr0zeSoO8G1dpPhK3VgyNJHtHHP9I3LdrdVUrJSKqcUWHJ+lfU1bXDJwBPcDSIMsfFAbYpISTSHRda3oq7HSi/HS6hoJDPql0yghKeELKMbmHECmWlJQ6Lq7+xr5O6q9seKGLpbGDUklX2Db7vI9EXOT+nBQi56wUSycKQO4M4y0XPWLQff625V6lDX9yC6p73NEA0aIWdvsvH7uGoYFJWs2agFL5Qoc8DCaDt54GYxGoPi8vHHyDFj4AkT4ePqP8BbKjdLuH5S2PhKisONe+ojaIP/XGWiZPVBC72+VLXUoUZNif+aIXY45Hr52ldd++QJgC2VHP9SyD2GYBm1WMpZpVy8zxceUCt+QCxjBlv9QjXNQpQaOnXAJUXxBXneraJUHRfnCuHPFdj9ynOFHhlltTcs6mO5WFzOUCjM0sc8lG2qtHt5kECuBr5b/yZWQja++2vBWivIEFxrhYd2CWAYcT1uHg7Zmt05aJxEkL9g5Jw05jXumwFadFOfVv7xD5PpSwb56Gz3yKYl2nlxh/B3/1gmm3N39TD9Iy97/FmKAE0zT9GLopqkjod0ZaJYZHPDqQCMMTAMhplamCXlIexo9DZsGj+pD/QN7qN/2yFDfbg/103So/y3AAJziVGS9B3eEAAAAAElFTkSuQmCC',
                                                    //    handler: function (grid, rowIndex, colIndex) {
                                                    //        B4.QuickMsg.msg('asd', 'asd', 'success')
                                                    //    }
                                                    //}
                                                ]
                                            }
                                        ],
                                        viewConfig: {
                                            loadMask: true,
                                            getRowClass: function (record, index) {
                                                var c = parseFloat(record.get('Percent'));
                                                if (c == isNaN) {
                                                    return;
                                                }

                                                if (c == 100) {
                                                    return 'x-percent-100';
                                                }

                                                if (c <= 10) {
                                                    return 'x-percent-10';
                                                } else if (c > 10 && c <= 20) {
                                                    return 'x-percent-20';
                                                } else if (c > 20 && c <= 40) {
                                                    return 'x-percent-30';
                                                } else if (c > 40 && c <= 70) {
                                                    return 'x-percent-70';
                                                } else if (c > 70 && c <= 99) {
                                                    return 'x-percent-90';
                                                };
                                                return;
                                            }
                                        }
                                    }
                                ]
                            }
                        ]

                    }

            ]
        });

        me.callParent(arguments);
    }



});