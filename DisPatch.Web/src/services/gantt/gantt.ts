// @ts-ignore
/* eslint-disable */

declare namespace Gantt {

  type ganttInfo = {
    /** 设备号 */
    equipmentNo?: string;
    /** 设备号-记录 */
    equipmentNoOld?: string;
    /** 生产批 */
    lotNo?: string;
    /** 作业站 */
    opNo?: string;
    /** 工作日期 */
    workDate?: string;
    /** 执行任务 */
    job?: string;
    /** 开始时间 */
    start?: string;
    /** 结束时间 */
    end?: string;
    /** 是否调整 */
    isChange?: boolean;
    /** ID */
    uid?: number;
    /** 行号 */
    y?: number;
  };

  type Options = {
    title?: string,
    xAxis?: Options_xAxis[],
    yAxis?: Options_yAxis[],
    tooltip?: Options_tooltip,
    series?: Options_series[],
    plotOptions?: Options_plotOptions,
    exporting?: { sourceWidth?: number },
    credits?: { enabled?: boolean },
  };

  type Options_xAxis = {
    currentDateIndicator?: boolean,
    tickPixelInterval?: number,
    grid?: {
      borderWidth?: number,
      cellHeight?: number,
      borderColor?: string,
    },
    labels: {
      align?: string,
      style?: any,
      formatter?: Function
    }
  }

  type Options_yAxis = {
    type?: string,
    grid?: {
      enabled?: boolean,
      borderColor?: string,
      borderWidth?: number,
      columns?: [
        {
          title?: { text?: string, style?: { "font-weight": "bold", } },
          labels: {
            formatter?: Function
          }
        },
      ]
    }
  }

  type Options_tooltip = {
    formatter?: Function
  }

  type Options_series = {
    data?: any[],
    name?: string,
    cursor?: string,
    type?: string,
  }

  type Options_plotOptions = {
    series: {
      animation: false,
      dragDrop: {
        draggableX: true,          // 横向拖拽              
        draggableY: true,          // 纵向拖拽
        dragMinY: 0,               // 纵向拖拽下限
        dragMaxY: 9,               // 纵向拖拽上限
        dragPrecisionX: 86400000,   // 横向拖拽精度，单位毫秒             
        liveRedraw: false,
      },
      dataLabels: {
        enabled: true,
        format?: string,
        style: { cursor: 'default', pointerEvents: 'none' }
      },
      allowPointSelect: true,
      point: {
        events: {
          dragStart: Function,
          drag: Function,
          drop: Function,
          select: Function
        }
      }
    }
  }
}