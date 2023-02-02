import { extend } from 'umi-request'
import { history } from 'umi';
import { notification, Modal } from 'antd'
import { getLoginUser } from './userinfo';

//const baseUrl = process.env.BASE_URL
//const userInfo = getLoginUser() || {};
const codeMessage: any = {
    200: '服务器成功返回请求的数据。',
    201: '新建或修改数据成功。',
    202: '一个请求已经进入后台排队（异步任务）。',
    204: '删除数据成功。',
    400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
    401: '用户没有权限（令牌、用户名、密码错误）。',
    403: '用户得到授权，但是访问是被禁止的。',
    404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
    406: '请求的格式不可得。',
    410: '请求的资源被永久删除，且不会再得到的。',
    422: '当创建一个对象时，发生一个验证错误。',
    500: '服务器发生错误，请检查服务器。',
    502: '网关错误。',
    503: '服务不可用，服务器暂时过载或维护。',
    504: '网关超时。',
}

const prefix = "http://127.0.0.1:5225"

const errorHandler = (error: any) => {
    console.log(error);
    const { response = {} } = error;
    console.log(response);
    if (response && response.status) {
        const { status, url } = response
        const errorText = codeMessage[status] || response.statusText;
        if (status !== 200) {
            console.error(url);
            switch (response.status) {
                case 401:
                    notification.error({
                        message: errorText,
                        description: 'token异常,请重新登录!'
                    });
                    history.push('/login');
                    break;
                default:
                    // eslint-disable-next-line no-case-declarations
                    let msg = error.data?.Exception || errorText;
                    Modal.error({
                        title: '后台处理异常',
                        content: msg,
                    });
                    break;
            }
        }
        return Promise.reject(response);
    } else if (!response) {
        const errorText = '网络异常，无法连接服务器';
        notification.error({
            description: errorText,
            message: '网络异常',
        });
        const error = new Error(errorText);
        error.message = errorText;

        return Promise.reject(error);
    } else {
        //可能是超时
        notification.error({
            message: error.message,
            description: error.stack
        });
        return Promise.reject(error);
    }    
}

const request = extend(
    {
        //prefix: prefix + "/api",   // 统一的请求前缀        
        prefix: prefix,   // 统一的请求前缀        
        timeout: 500000,           // 超时时间      
    })

const callback = (response: any) => {
    if (response.isSuccess === false) {
        if (response.code === 1) {
            notification.error({ message: response.message, description: "程式异常" })            
        }
        else if (response.code === 2) {
            notification.warning({ message: response.message, description: "业务提示" })
        }
    }
    else{
        return Promise.resolve(response.content);
    }    
}
const errBack = (err: any) => {   
    return Promise.reject(err);
}


request.interceptors.request.use((url, options) => {
    console.log("url",url)
    let userInfo = getLoginUser() || {};   
    return {
        url: `${url}`,
        options: {
            ...options,
            interceptors: true,
            headers: {
                Authorization: userInfo.token || "",
            },
        },
    }
})

request.interceptors.response.use(async (response, options) => {
    console.log(`${response.url} 请求结果为↓↓↓↓↓↓↓↓`);
    console.log(response);
    console.log(options);
    console.log(`${response.url} 请求结果为↑↑↑↑↑↑↑↑`);
    return response
})

export const http = {
    get: (url: string, data?: any, options?: any) => {
        return request.get(url, { ...options, params: data })
            .then(callback, errBack)
            .catch(errorHandler)
    },
    put: (url: string, data?: any, options?: any) => {
        return request.put(url, { ...options, data })
            .then(callback)
            .catch(errorHandler)
    },
    post: (url: string, data?: any, options?: any) => {
        return request.post(url, { ...options, data })
            .then(callback)
            .catch(errorHandler)
    },
    delete: (url: string, data?: any, options?: any) => {
        return request.delete(url, { ...options, data })
            .then(callback)
            .catch(errorHandler)
    },
    getUrl: () => {
        return prefix
    }
    // patch: (url: string, data?: any, options?: any) => {
    //     return request.patch(url, { ...options, data })
    //         .then(callback)
    //         .catch(errorHandler)
    // },
}

export default request

