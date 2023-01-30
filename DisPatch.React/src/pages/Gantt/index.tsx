import React, { Component, useState, useEffect, useRef } from 'react';
import Highcharts from 'highcharts/highcharts-gantt';
import HighchartsReact from 'highcharts-react-official';
import { Table, Segmented, message, Button, Form, Input, Row, Col, Spin, DatePicker, Divider } from 'antd';
import { BarsOutlined, AppstoreOutlined, FundViewOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import request from 'umi-request';
import moment from 'moment';

let _url = 'http://127.0.0.1:5225';
export default class IndexPage extends Component<any, any> {

  constructor(props: any) {
    super(props);

    this.state = {
      loading: false,           // 全局loading
      segmentedType: "Gantt",   // 显示方式

      EquipmentNo: "",          // 查询条件 - 设备号
      LotNo: "",                // 查询条件 - 生产批
      DispStartTime: "",        // 查询条件 - 开始时间
      DispEndTime: "",          // 查询条件 - 结束时间

      ganttData: [],            // 查询数据
      equipmentNoList: [],      // y 和 设备号的对比

      options: {
        // title: { text: '快速派工-Gantt' },
        xAxis: [{
          currentDateIndicator: true,
          tickPixelInterval: 70,
          grid: {
            borderWidth: 1,
            cellHeight: 35,
            borderColor: 'rgba(0,0,0,0.3)',
          },
          labels: {
            align: 'center',
            style: { "font-weight": "bold", },
            formatter: function () {
              return `${dayjs(this.value).format('M月D日')}  `;
            }
          },
        }, {
          labels: {
            align: 'center',
            style: { "font-weight": "bold", "font-size": "20px", "text-anchor": "middle", "alignment-baseline": "central" },
            formatter: function () {
              return `${dayjs(this.value).format('YYYY年M月')}`;
            }
          },
        }],
        yAxis: [{
          type: 'category',
          grid: {
            enabled: true,
            borderColor: 'rgba(0,0,0,0.3)',
            borderWidth: 1,
            columns: [
              {
                title: { text: '设备号', style: { "font-weight": "bold", } },
                labels: {
                  //format: '{point.equipmentNo}'
                  formatter: function () {
                    if (this.point) {
                      return this.point.equipmentNo;
                    }
                    else if (this.value != undefined) {
                      return "第" + (this.value + 1) + "行暂无数据";
                    }
                    else {
                      return "/";
                    }
                  }
                }
              },
            ]
          }
          // title: '设备号',
          // type: 'category',
          // categories: []
        }],
        tooltip: {
          formatter: function () {
            return `
           <div>
             设备: ${this.point.equipmentNo}<br/>
             生产批: ${this.point.lotNo}<br/>
             作业站: ${this.point.opNo}<br/>
             开始时间: ${dayjs(this.point.start).format('YYYY-MM-DD')}<br/>
             结束时间: ${dayjs(this.point.end).format('YYYY-MM-DD')}<br/>
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
    }
  }

  componentDidMount() {

    // 初始化chart实例
    this.chartRef = React.createRef();

    Highcharts.setOptions({
      // global: {useUTC: false},
      lang: {
        noData: '暂无数据',
        weekdays: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
        months: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']
      }
    });

    //this.dataSelectDemo();
    this.setState({ ganttData: [], equipmentNoList: [] });
    this.chartBindData([], []);
  }

  //#region highChart 事件

  chartdragStart = () => {
    //console.log("chartdragStart", this.chartRef)
  }

  chartdrag = (e: any) => {
    //console.log("chartdrag", e.target)    
  }

  chartdrop = (e: any) => {
    console.log("chartdrop.target", e.target)
    console.log("chartdrop.newPoint", e.newPoint)

    if (e.newPoint && e.target) {
      let list = [];

      //找到原数据 并 修改
      list = this.state.ganttData.map((item: any) => {
        item.isChange = true;
        if (item.uid == e.target.uid) {
          if (e.newPoint.y >= 0) {
            console.log("行变换【" + item.y + "】【" + e.newPoint.y + "】");
            console.log("设备变换【" + item.equipmentNo + "】【" + this.state.equipmentNoList[e.newPoint.y] + "】");

            //item.y = e.newPoint.y
            item.equipmentNo = this.state.equipmentNoList[e.newPoint.y]
          }
          return item
        }
        else {
          return item
        }
      })

      //更新gantt和表
      this.setState({ ganttData: list });
      this.chartBindData(list, this.state.equipmentNoList);
    }
  }

  chartSelect = (e: any) => {
    //console.log("chartSelect", e.target)
  }

  getEquipmentNo = (e: any) => {

  }

  //#endregion

  //#region 按钮事件

  // 查询按钮
  dataSelect = () => {
    // 后台查询值，返回给前台
    let sel = this;
    sel.setState({
      loading: true,
    }, () => {
      request.post(`${_url}/Gantt/GetGantt`, {
        data: {
          key:"",
          userNo:"",
          equipmentNo: this.state.EquipmentNo,
          lotNo: this.state.LotNo,
          dispStartTime: this.state.DispStartTime,
          dispEndTime: this.state.DispEndTime
        },
      }).then(function (response) {
        console.log(response);
        sel.setState({
          ganttData: response.content.ganttData,
          equipmentNoList: response.content.equipmentNoList,
          loading: false,
        })
        sel.chartBindData(response.content.ganttData, response.content.equipmentNoList);
      }).catch(function (error) {
        console.log(error);
        sel.setState({ loading: false })
        message.error("查询数据异常：" + error.message);
        //sel.dataSelectDemo();
      });
    })
  }
  dataSelectDemo = () => {
    // 此处模拟数据
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

    this.setState({ ganttData: data, equipmentNoList: equipmentNoList });
    this.chartBindData(data, equipmentNoList);
  }

  // 保存按钮
  dataSave = () => {
    // 后台查询值，返回给前台
    let sel = this;
    sel.setState({
      loading: true,
    }, () => {
      request.post(`${_url}/Gantt/SetGantt`, {
        data: {
          ganttInfos: sel.state.ganttData
        },
      }).then(function (response) {
        console.log(response);
        sel.setState({
          loading: false,
        })
      }).catch(function (error) {
        console.log(error);
        sel.setState({
          loading: false
        })
        message.error("存储数据异常：" + error.message);
      });
    })

  }


  //#endregion

  //#region 其他

  // chart数据变化
  chartBindData = (ganttData: any, equipmentNoList: any) => {
    // 1.日期格式化
    let newData = ganttData.map((item: any) => {
      item.start = dayjs(item.start).valueOf();
      item.end = dayjs(item.end).valueOf();
      return item
    });

    // 2.数据变化
    this.state.options.series[0].data = newData;
    this.chartRef?.current?.chart?.series[0]?.setData(newData);

    // this.state.options.yAxis[0].categories = equipmentNoList;
    // this.chartRef?.current?.chart?.yAxis[0]?.setCategories(equipmentNoList);

    // this.state.options.yAxis[0].title = "设备号";
    // this.chartRef?.current?.chart?.yAxis[0]?.setTitle(
    //   {
    //     text: '设备号',
    //     style: {
    //       "font-weight": "bold",
    //       "font-size": "large",
    //       "border": "5px"
    //     }
    //   });
  }

  // 控制器变换
  SegmentedChange = (value: any) => {
    this.setState({
      segmentedType: value
    })
  }

  //#endregion

  render() {
    const ganttPgeColumns = [
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
      <Spin spinning={this.state.loading}>
        <div style={{ margin: "0px 5px 0 5px", backgroundColor: '#FDFFFF' }}>

          {/* 查询条件 */}
          <Row gutter={[24, 24]} style={{ backgroundColor: '#FDFFFF', paddingTop: "15px" }}>
            <Col span={5}>
              <Form.Item name="EquipmentNo" label="设备号" labelCol={{ span: 6 }}>
                <Input onChange={(e) => { this.setState({ EquipmentNo: e.target.value }); }}></Input >
              </Form.Item>
            </Col>
            <Col span={5}>
              <Form.Item name="LotNo" label="生产批" labelCol={{ span: 6 }}>
                <Input onChange={(e) => { this.setState({ LotNo: e.target.value }); }}></Input >
              </Form.Item>
            </Col>
            <Col span={5}>
              <Form.Item name="DispStartTime" label="开始时间" labelCol={{ span: 6 }}>
                <DatePicker onChange={(date, datestring) => { this.setState({ DispStartTime: datestring }); }} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={5}>
              <Form.Item name="DispEndTime" label="结束时间" colon={true} labelCol={{ span: 6 }}>
                <DatePicker onChange={(date, datestring) => { this.setState({ DispEndTime: datestring }); }} style={{ width: '100%' }} />
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
          <Segmented
            value={this.state.segmentedType}
            options={[
              { label: '所有', value: 'All', icon: <AppstoreOutlined />, },
              { label: '清单', value: 'List', icon: <BarsOutlined />, },
              { label: '甘特图', value: 'Gantt', icon: <FundViewOutlined />, },
            ]}
            onChange={e => this.SegmentedChange(e)} />

          {/* 数据表格 */}
          <div style={{ display: this.state.segmentedType === "All" || this.state.segmentedType === "List" ? "block" : "none", padding: "10px 10px 10px 10px" }}>
            <Divider orientation="center">{"数据表格"}</Divider>
            <Table columns={ganttPgeColumns} dataSource={this.state.ganttData} />
          </div>

          {/* 甘特图 */}
          <div style={{ display: this.state.segmentedType === "All" || this.state.segmentedType === "Gantt" ? "block" : "none", padding: "10px 10px 10px 10px" }}>
            <Divider orientation="center" >{"甘特图"}</Divider>
            <HighchartsReact highcharts={Highcharts} constructorType={'ganttChart'} options={this.state.options} ref={this.chartRef} />
          </div>
        </div>
      </Spin>
    );
  }
}