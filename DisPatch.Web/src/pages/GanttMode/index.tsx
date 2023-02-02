import React, { Component } from 'react';
import dayjs from 'dayjs';
import { PageContainer } from '@ant-design/pro-components';
import Highcharts from 'highcharts/highcharts-gantt';
import HighchartsReact from 'highcharts-react-official';
import { Button, Col, DatePicker, Divider, Form, Input, Row, Segmented, Spin, Table } from 'antd';
import { AppstoreOutlined, BarsOutlined, FundViewOutlined } from '@ant-design/icons';
import moment from 'moment';


export default class GanttMode extends Component<any, any> {

    constructor(props: any) {
        super(props);

        //let sel = this;
        this.state = {
            loading: false,         // 全局loading
            viewType: "Gantt",      // 显示方式
            searchInfo: {},         // 查询条件
            ganttData: [],          // 查询数据
            eqpInfo: [],            // 设备集合
            options: {
                // title: { text: '快速派工-Gantt' },
                xAxis: [
                    {
                        currentDateIndicator: true,
                        tickPixelInterval: 70,
                        grid: { borderWidth: 1, cellHeight: 35, borderColor: 'rgba(0,0,0,0.3)', },
                        labels: { align: 'center', style: { "font-weight": "bold", }, formatter: function () { return `${dayjs(this.value).format('M月D日')}  `; } },
                    },
                    {
                        labels: { align: 'center', style: { "font-weight": "bold", "font-size": "20px", "text-anchor": "middle", "alignment-baseline": "central" }, formatter: function () { return `${dayjs(this.value).format('YYYY年M月')}`; } },
                    }],
                yAxis: [
                    {
                        type: 'category',
                        grid: {
                            enabled: true, borderColor: 'rgba(0,0,0,0.3)', borderWidth: 1,
                            columns: [
                                {
                                    title: { text: '设备号', style: { "font-weight": "bold", } },
                                    labels: {
                                        formatter: function () {
                                            if (this.point) { return this.point.equipmentNo; }
                                            else if (this.value !== undefined) { return "第" + (this.value + 1) + "行暂无数据"; }
                                            else { return "/"; }
                                        }
                                    }
                                },
                            ]
                        }
                    }],
                tooltip: {
                    formatter: function () {
                        return `<div> 设备: ${this.point?.equipmentNo}<br/>
                      生产批: ${this.point?.lotNo}<br/>
                      作业站: ${this.point?.opNo}<br/>
                      开始时间: ${dayjs(this.point?.start).format('YYYY-MM-DD')}<br/>
                      结束时间: ${dayjs(this.point?.end).format('YYYY-MM-DD')}<br/>
                </div>`
                    }
                },
                series: [{
                    data: [], name: 'gantt', cursor: 'move', type: "gantt",
                }],
                plotOptions: {
                    series: {
                        animation: false,     // Do not animate dependency connectors
                        dragDrop: {
                            draggableX: true,          // 横向拖拽              
                            draggableY: true,          // 纵向拖拽
                            dragMinY: 0,               // 纵向拖拽下限
                            dragMaxY: 9,               // 纵向拖拽上限
                            dragPrecisionX: 86400000,   // 横向拖拽精度，单位毫秒
                            //dragPrecisionX: 'day',
                            liveRedraw: false,
                        },
                        dataLabels: {
                            enabled: true,
                            format: '{point.job}',
                            style: { cursor: 'default', pointerEvents: 'none' }
                        },
                        allowPointSelect: true,
                        point: {
                            events: {
                                dragStart: this.chartdragStart,
                                drag: this.chartdrag,
                                drop: this.chartdrop,
                                select: this.chartSelect
                            }
                        }
                    }
                },
                exporting: { sourceWidth: 1000 },
                credits: { enabled: false },
            }
        };
    }

    /** highchart 实例 */
    chartRef: any;

    componentDidMount() {

        /** 取得 highCharts 实例 */
        this.chartRef = React.createRef();

        /** state初始值 */
        this.setState({ searchInfo: {}, ganttData: [], eqpInfo: [] });

        /** highCharts全局配置 */
        this.initGnattOptions();

        /** gantt图默认绑定空数据 */
        this.chartBindData([], []);

    }

    //#region highChrt相关

    /** 单笔数据 */
    value: any;
    /** 选中点 */
    point: any;

    /** 全局配置 */
    initGnattOptions = () => {

        Highcharts.setOptions({
            // global: {useUTC: false},
            lang: {
                noData: '暂无数据',
                weekdays: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
                months: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']
            }
        });

        // let _options = {
        //     // title: { text: '快速派工-Gantt' },
        //     xAxis: [
        //         {
        //             currentDateIndicator: true,
        //             tickPixelInterval: 70,
        //             grid: { borderWidth: 1, cellHeight: 35, borderColor: 'rgba(0,0,0,0.3)', },
        //             labels: { align: 'center', style: { "font-weight": "bold", }, formatter: function () { return `${dayjs(this.value).format('M月D日')}  `; } },
        //         },
        //         {
        //             labels: { align: 'center', style: { "font-weight": "bold", "font-size": "20px", "text-anchor": "middle", "alignment-baseline": "central" }, formatter: function () { return `${dayjs(this.value).format('YYYY年M月')}`; } },
        //         }],
        //     yAxis: [
        //         {
        //             type: 'category',
        //             grid: {
        //                 enabled: true, borderColor: 'rgba(0,0,0,0.3)', borderWidth: 1,
        //                 columns: [
        //                     {
        //                         title: { text: '设备号', style: { "font-weight": "bold", } },
        //                         labels: {
        //                             formatter: function () {
        //                                 if (this.point) { return this.point.equipmentNo; }
        //                                 else if (this.value !== undefined) { return "第" + (this.value + 1) + "行暂无数据"; }
        //                                 else { return "/"; }
        //                             }
        //                         }
        //                     },
        //                 ]
        //             }
        //         }],
        //     tooltip: {
        //         formatter: function () {
        //             return `<div> 设备: ${this.point?.equipmentNo}<br/>
        //                           生产批: ${this.point?.lotNo}<br/>
        //                           作业站: ${this.point?.opNo}<br/>
        //                           开始时间: ${dayjs(this.point?.start).format('YYYY-MM-DD')}<br/>
        //                           结束时间: ${dayjs(this.point?.end).format('YYYY-MM-DD')}<br/>
        //                     </div>`
        //         }
        //     },
        //     series: [{
        //         data: [], name: 'gantt', cursor: 'move', type: "gantt",
        //     }],
        //     plotOptions: {
        //         series: {
        //             animation: false,     // Do not animate dependency connectors
        //             dragDrop: {
        //                 draggableX: true,          // 横向拖拽              
        //                 draggableY: true,          // 纵向拖拽
        //                 dragMinY: 0,               // 纵向拖拽下限
        //                 dragMaxY: 9,               // 纵向拖拽上限
        //                 dragPrecisionX: 86400000,   // 横向拖拽精度，单位毫秒
        //                 //dragPrecisionX: 'day',
        //                 liveRedraw: false,
        //             },
        //             dataLabels: {
        //                 enabled: true,
        //                 format: '{point.job}',
        //                 style: { cursor: 'default', pointerEvents: 'none' }
        //             },
        //             allowPointSelect: true,
        //             point: {
        //                 events: {
        //                     dragStart: this.chartdragStart,
        //                     drag: this.chartdrag,
        //                     drop: this.chartdrop,
        //                     select: this.chartSelect
        //                 }
        //             }
        //         }
        //     },
        //     exporting: { sourceWidth: 1000 },
        //     credits: { enabled: false },
        // };
        // this.setState({ options: _options });
    }

    /** 绑定数据 */
    chartBindData = (data: Gantt.ganttInfo[], equipmentNoList: string[]) => {

        this.setState({ ganttData: data, eqpInfo: equipmentNoList });

        // 1.日期格式化
        let newData = data.map((item: any) => {
            item.start = dayjs(item.start).valueOf();
            item.end = dayjs(item.end).valueOf();
            return item
        });

        // 2.数据变化
        let _options = this.state.options;
        if (_options?.series !== undefined)
            _options.series[0].data = newData;
        this.setState({ options: _options });

        if (this.chartRef?.current?.chart?.series !== undefined)
            this.chartRef?.current?.chart?.series[0]?.setData(newData);
    }

    chartdragStart = () => {
        console.log("chartdragStart", this.chartRef)
    }

    chartdrag = (e: any) => {
        console.log("chartdrag", e.target)
    }

    chartdrop = (e: any) => {
        console.log("chartdrop.target", e.target)
        console.log("chartdrop.newPoint", e.newPoint)

        if (e.newPoint && e.target) {
            let list = [];

            //找到原数据 并 修改
            list = this.state.ganttData.map((item: any) => {               
                if (item.uid === e.target.uid) {
                    item.isChange = true;                    
                    if (e.newPoint.y >= 0) {
                        console.log("行变换【" + item.y + "】【" + e.newPoint.y + "】");
                        console.log("设备变换【" + item.equipmentNo + "】【" + this.state.eqpInfo[e.newPoint.y] + "】");

                        //item.y = e.newPoint.y
                        item.equipmentNo = this.state.eqpInfo[e.newPoint.y]
                    }
                    return item
                }
                else {
                    return item
                }
            })

            //更新gantt和表
            this.setState({ ganttData: list });
            this.chartBindData(list, this.state.eqpInfo);
        }
    }

    chartSelect = (e: any) => {
        console.log("chartSelect", e.target)
    }

    //#endregion

    //#region 控件change

    /** 查询条件变化 */
    searchChange = (eqp?: string, lotNo?: string, start?: string, end?: string) => {
        let _searchInfo: Gantt.ganttInfo = {};
        if (this.state.searchInfo)
            _searchInfo = this.state.searchInfo;

        if (eqp)
            _searchInfo.equipmentNo = eqp;

        if (lotNo)
            _searchInfo.lotNo = lotNo;

        if (start)
            _searchInfo.start = start;

        if (end)
            _searchInfo.end = end;

        this.setState({ searchInfo: _searchInfo });
    }

    //#endregion

    //#region 后台交互

    /** 查询方法 */
    dataSelect = () => {
        this.dataSelectDemo();
    }

    /** 此处模拟数据展示 */
    dataSelectDemo = () => {
        const data = [
            {
                "equipmentNo": "A-1-P01-1",
                "equipmentNoOld": "A-1-P01-1",
                "lotNo": "MO301045-001",
                "opNo": "BZ01",
                "workDate": "2023-01-04T08:00:00",
                "job": "MO301045-001(BZ01)",
                "start": "2023-01-05T08:00:00",
                "end": "2023-01-09T08:00:00",
                "isChange": false,
                "uid": 1,
                "y": 0
            },
            {
                "equipmentNo": "A-2-A01-1",
                "equipmentNoOld": "A-2-A01-1",
                "lotNo": "MO301045-001",
                "opNo": "ASSY-IN",
                "workDate": "2023-01-04T08:00:00",
                "job": "MO301045-001(ASSY-IN)",
                "start": "2023-01-04T08:00:00",
                "end": "2023-01-06T08:00:00",
                "isChange": false,
                "uid": 2,
                "y": 1
            },
            {
                "equipmentNo": "YY-1-B01-1",
                "equipmentNoOld": "YY-1-B01-1",
                "lotNo": "MO301044-001",
                "opNo": "YY-ASSY-02",
                "workDate": "2023-01-04T08:00:00",
                "job": "MO301044-001(YY-ASSY-02)",
                "start": "2023-01-04T08:00:00",
                "end": "2023-01-06T08:00:00",
                "isChange": false,
                "uid": 3,
                "y": 2
            },
            {
                "equipmentNo": "YY-1-B01-2",
                "equipmentNoOld": "YY-1-B01-2",
                "lotNo": "MO301047-001",
                "opNo": "YY-ASSY-02",
                "workDate": "2023-01-04T08:00:00",
                "job": "MO301047-001(YY-ASSY-02)",
                "start": "2023-01-05T08:00:00",
                "end": "2023-01-07T08:00:00",
                "isChange": false,
                "uid": 4,
                "y": 3
            },
            {
                "equipmentNo": "YY-1-B01-3",
                "equipmentNoOld": "YY-1-B01-3",
                "lotNo": "MO301048-001",
                "opNo": "YY-ASSY-02",
                "workDate": "2023-01-04T08:00:00",
                "job": "MO301048-001(YY-ASSY-02)",
                "start": "2023-01-06T08:00:00",
                "end": "2023-01-08T08:00:00",
                "isChange": false,
                "uid": 5,
                "y": 4
            },
            {
                "equipmentNo": "YY-1-S01-1",
                "equipmentNoOld": "YY-1-S01-1",
                "lotNo": "MO301056-001",
                "opNo": "YY-SMT-BOT",
                "workDate": "2023-01-05T08:00:00",
                "job": "MO301056-001(YY-SMT-BOT)",
                "start": "2023-01-05T08:00:00",
                "end": "2023-01-06T08:00:00",
                "isChange": false,
                "uid": 6,
                "y": 5
            },
            {
                "equipmentNo": "YY-1-S01-2",
                "equipmentNoOld": "YY-1-S01-2",
                "lotNo": "MO301056-001",
                "opNo": "YY-SMT-TOP",
                "workDate": "2023-01-05T08:00:00",
                "job": "MO301056-001(YY-SMT-TOP)",
                "start": "2023-01-06T08:00:00",
                "end": "2023-01-09T08:00:00",
                "isChange": false,
                "uid": 7,
                "y": 6
            }
        ];
        const equipmentNoList = ["A-1-P01-1", "A-2-A01-1", "YY-1-B01-1", "YY-1-B01-2", "YY-1-S01-1", "YY-1-S01-2"]
        this.chartBindData(data, equipmentNoList);
    }

    /** 保存方法 */
    dataSave = () => {

    }

    //#endregion

    render() {

        /** 显示模式 */
        const segmentedOptions = [
            { label: '所有', value: 'All', icon: <AppstoreOutlined />, },
            { label: '清单', value: 'List', icon: <BarsOutlined />, },
            { label: '甘特图', value: 'Gantt', icon: <FundViewOutlined />, },
        ];

        /** 表格栏位 */
        const ganttPageColumns = [
            {
                title: "设备号",
                dataIndex: 'equipmentNo',
                key: 'equipmentNo',
                width: "20%",
            },
            {
                title: "生产批",
                dataIndex: 'lotNo',
                key: 'lotNo',
                width: "20%",
            },
            {
                title: "作业站",
                dataIndex: 'opNo',
                key: 'opNo',
                width: "20%",
            },
            {
                title: "开始时间",
                dataIndex: 'start',
                key: 'start',
                width: "20%",
                render: (text: any) => <div>{moment(text).format('yyyy-MM-DD')}</div>,
            },
            {
                title: "结束时间",
                dataIndex: 'end',
                key: 'end',
                width: "20%",
                render: (text: any) => <div>{moment(text).format('yyyy-MM-DD')}</div>,
            },
        ];

        return (
            <PageContainer>
                <Spin spinning={this.state.loading}>
                    {/* 查询条件 */}
                    <Row gutter={[24, 24]} style={{ backgroundColor: '#FDFFFF', paddingTop: "15px" }}>
                        <Col span={5}>
                            <Form.Item name="EquipmentNo" label="设备号" labelCol={{ span: 6 }}>
                                <Input onChange={(e) => { this.searchChange(e.target.value); }}></Input >
                            </Form.Item>
                        </Col>
                        <Col span={5}>
                            <Form.Item name="LotNo" label="生产批" labelCol={{ span: 6 }}>
                                <Input onChange={(e) => { this.searchChange(undefined, e.target.value); }}></Input >
                            </Form.Item>
                        </Col>
                        <Col span={5}>
                            <Form.Item name="DispStartTime" label="开始时间" labelCol={{ span: 6 }}>
                                <DatePicker onChange={(date, datestring) => { this.searchChange(undefined, undefined, datestring); }} style={{ width: '100%' }} />
                            </Form.Item>
                        </Col>
                        <Col span={5}>
                            <Form.Item name="DispEndTime" label="结束时间" colon={true} labelCol={{ span: 6 }}>
                                <DatePicker onChange={(date, datestring) => { this.searchChange(undefined, undefined, undefined, datestring); }} style={{ width: '100%' }} />
                            </Form.Item>
                        </Col>
                        <Col span={4}>
                            <Form.Item name="Do" label="" labelCol={{ span: 3 }}>
                                <Button type='primary' onClick={() => { this.dataSelect() }} style={{ marginRight: 15 }}>查询</Button>
                                <Button type='primary' onClick={() => { this.dataSave() }} style={{ marginRight: 15 }}>保存</Button>
                            </Form.Item>
                        </Col>
                    </Row>

                    {/* 控制显示方式 */}
                    <Segmented value={this.state.viewType} options={segmentedOptions} onChange={e => this.setState({ viewType: e.toString() })} />

                    {/* 数据表格 */}
                    <div style={{ display: this.state.viewType === "All" || this.state.viewType === "List" ? "block" : "none", padding: "10px 10px 10px 10px" }}>
                        <Divider orientation="center">{"数据表格"}</Divider>
                        <Table columns={ganttPageColumns} dataSource={this.state.ganttData} />
                    </div>

                    {/* 甘特图 */}
                    <div style={{ display: this.state.viewType === "All" || this.state.viewType === "Gantt" ? "block" : "none", padding: "10px 10px 10px 10px" }}>
                        <Divider orientation="center" >{"甘特图"}</Divider>
                        <HighchartsReact highcharts={Highcharts} constructorType={'ganttChart'} options={this.state.options} ref={this.chartRef} />
                    </div>
                </Spin>
            </PageContainer>
        )
    }
}