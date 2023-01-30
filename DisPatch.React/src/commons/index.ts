import {message} from 'antd';

const CURRENT_USER_KEY = "current-user";
const localStorage = window.localStorage;
const sessionStorage = window.sessionStorage;

/**
 * 获取系统设定
 */
export function getSystemSetting() {
    let settingData = localStorage.getItem("system-setting");
    settingData = settingData ? JSON.parse(settingData) : {};
    return settingData;
}
/**
 * 判断是否有权限
 * @param code
 */
export function hasPermission(code: any) {
    const loginUser = getLoginUser();
    return loginUser?.permissions?.includes(code);
}

/**
 * 保存用户数据
 * @param {*} currentUser
 */
export function saveUserInfo(currentUser = {}) {
    const userStr = JSON.stringify(currentUser);

    localStorage.setItem("user-login-info", userStr); //供保持登录用
    localStorage.setItem("last-login-info", userStr); //供记住信息用
}
/**
 * 取出上次用户登陆数据
 */
export function getLastLoginInfo() {
    const loginUser = localStorage.getItem("last-login-info");
    return loginUser ? JSON.parse(loginUser) : null;
}
/**
 * 取出用户登陆数据
 */
export function getUserInfo() {
    const loginUser = localStorage.getItem("user-login-info");
    return loginUser ? JSON.parse(loginUser) : null;
}
/**
 * 设置当前用户信息
 */
export function setLoginUser(currentUser: any) {
    // 将用户属性在这里展开，方便查看系统都用到了那些用户属性
    const { id, name, avatar, token, permissions, ...rest } = currentUser;
    const userStr = JSON.stringify({
        id, // 用户id 必须
        name, // 用户名 必须
        avatar, // 用头像 非必须
        token, // 登录凭证 非必须 ajax请求有可能会用到，也许是cookie
        permissions, // 用户权限
        ...rest,
    });

    sessionStorage.setItem(CURRENT_USER_KEY, userStr);
}



/**
 * 获取当前用户信息
 */
export function getLoginUser() {
    const localUser = getUserInfo();
    if (localUser) {
        return localUser;
    } else {
        const loginUser = sessionStorage.getItem(CURRENT_USER_KEY);

        return loginUser ? JSON.parse(loginUser) : null;
    }
}

/**
 * 判断用户是否登录
 */
export function isAuthenticated() {
    // 如果当前用户存在，就认为已经登录了
    return !!getLoginUser();
}

export function write2clipboard(val:string){
    try{
        navigator.clipboard.writeText(val).then(()=>{
            message.success('已复制！');
        }).catch(ex=>{
            if(ex?.name === 'NotAllowedError'){
                const createInput = document.createElement('input');
                createInput.value = val;
                document.body.appendChild(createInput);
                createInput.select();
                if(document.execCommand('Copy')){
                    message.success('已复制！');
                }
                createInput.remove();
            }else{
                console.log('剪贴板写入异常',ex);
            }
        });
    }catch(ex){
        const createInput = document.createElement('input');
        createInput.value = val;
        document.body.appendChild(createInput);
        createInput.select();
        if(document.execCommand('Copy')){
            message.success('已复制！');
        }
        createInput.remove();
    }    
}