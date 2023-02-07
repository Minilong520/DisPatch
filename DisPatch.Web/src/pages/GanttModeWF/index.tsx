import React, { Component } from 'react';
import dayjs from 'dayjs';
import { PageContainer } from '@ant-design/pro-components';
import Highcharts from 'highcharts/highcharts-gantt';
import HighchartsReact from 'highcharts-react-official';
import { Button, Col, DatePicker, Divider, Form, Input, message, Row, Segmented, Spin, Table } from 'antd';
import { AppstoreOutlined, BarsOutlined, FundViewOutlined } from '@ant-design/icons';
import moment from 'moment';
import { http } from '@/services/request/request';


export default class GanttModeWF extends Component<any, any> {

    constructor(props: any) {
        super(props);

        //let sel = this;
        this.state = {
            loading: false,         // 全局loading
            viewType: "Gantt",      // 显示方式
            searchInfo: {},         // 查询条件
            ganttData: [],          // 查询数据
            workCenterInfo: [],     // 工作中心集合
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
                                    title: { text: '工作中心', style: { "font-weight": "bold", } },
                                    labels: {
                                        formatter: function () {
                                            if (this.point) { return this.point.workCenter; }
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
                        return `<div> 工作中心: ${this.point?.workCenter}<br/>
                                      计划批号: ${this.point?.lotNo}<br/>                                      
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
        this.setState({ searchInfo: {}, ganttData: [], workCenterInfo: [] });

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

    }

    /** 绑定数据 */
    chartBindData = (data: Gantt.ganttWFInfo[], workCenterList: string[]) => {

        this.setState({ ganttData: data, workCenterInfo: workCenterList });

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
                        console.log("工作中心变换【" + item.workCenter + "】【" + this.state.workCenterInfo[e.newPoint.y] + "】");

                        //item.y = e.newPoint.y
                        item.workCenter = this.state.workCenterInfo[e.newPoint.y]
                    }
                    return item
                }
                else {
                    return item
                }
            })

            //更新gantt和表
            this.setState({ ganttData: list });
            this.chartBindData(list, this.state.workCenterInfo);
        }
    }

    chartSelect = (e: any) => {
        console.log("chartSelect", e.target)
    }

    //#endregion

    //#region 控件change

    /** 查询条件变化 */
    searchChange = (workCenter?: string, lotNo?: string, start?: string, end?: string) => {
        let _searchInfo: Gantt.ganttWFInfo = {};
        if (this.state.searchInfo)
            _searchInfo = this.state.searchInfo;

        if (workCenter)
            _searchInfo.workCenter = workCenter;

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
        let sel = this;
        sel.setState({
            loading: true,
        }, () => {
            http.post(`/WFGantt/GetGantt`,
                {
                    workCenter: this.state.searchInfo.workCenter,
                    lotNo: this.state.searchInfo.lotNo,
                    dispStartTime: this.state.searchInfo.start,
                    dispEndTime: this.state.searchInfo.end
                })
                .then(function (response) {
                    console.log(response);
                    sel.setState({
                        ganttData: response.ganttData,
                        workCenterInfo: response.workCenterList,
                        loading: false,
                    })
                    sel.chartBindData(response.ganttData, response.workCenterList);
                    message.success("查询数据成功");
                }).catch(function (error) {
                    console.log(error);
                    sel.setState({ loading: false })
                    message.error("查询数据异常：" + error.message);
                    //sel.dataSelectDemo();
                });
        })
    }

    /** 保存方法 */
    dataSave = () => {
        let sel = this;
        sel.setState({
            loading: true,
        }, () => {
            // 1.日期格式化
            let newData = sel.state.ganttData.map((item: any) => {
                item.start = new Date(item.start).toLocaleString();
                item.end = new Date(item.end).toLocaleString()
                return item
            });

            http.post(`/WFGantt/SetGantt`,
                { ganttInfo: newData })
                .then(function (response) {
                    console.log(response);
                    sel.setState({ loading: false })
                    message.success("保存数据成功");
                }).catch(function (error) {
                    console.log(error);
                    sel.setState({ loading: false })
                    message.error("保存数据异常：" + error.message);
                });
        })
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
                title: "工作中心",
                dataIndex: 'workCenter',
                key: 'workCenter',
                width: "20%",
            },
            {
                title: "生产批",
                dataIndex: 'lotNo',
                key: 'lotNo',
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
                    <Row gutter={[24, 24]} style={{ paddingTop: "15px" }}>
                        <Col span={5}>
                            <Form.Item name="WorkCenter" label="工作中心" labelCol={{ span: 6 }}>
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