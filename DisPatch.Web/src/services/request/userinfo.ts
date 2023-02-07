import { http } from "./request";

const LOCAL_USER_KEY = "current-user";
const localStorage = window.localStorage;

const SESSION_USER_KEY = "current-user";
const sessionStorage = window.sessionStorage;

/** 设置当前用户信息*/
export function setLoginUser(currentUser: any) {
    // 将用户属性在这里展开，方便查看系统都用到了那些用户属性
    const { userid, name, avatar, token, permissions, password, ...rest } = currentUser;
    const userStr = JSON.stringify({
        userid,       // 用户id 必须
        name,         // 用户名 必须
        avatar,       // 用头像 非必须
        token,        // 登录凭证 非必须 ajax请求有可能会用到，也许是cookie
        permissions,  // 用户权限
        password,
        ...rest,
    });

    localStorage.setItem(LOCAL_USER_KEY, userStr);
    sessionStorage.setItem(SESSION_USER_KEY, userStr);
}

/** 清除当前用户信息 */
export function clearLoginUser() {
    localStorage.removeItem(LOCAL_USER_KEY)
    sessionStorage.removeItem(SESSION_USER_KEY);
}

/** 获取当前用户信息*/
export function getUserInfo() {
    const loginUser = localStorage.getItem(LOCAL_USER_KEY);
    return loginUser ? JSON.parse(loginUser) : null;
}
export function getLoginUser() {
    const localUser: any = getUserInfo();
    if (localUser) {
        return { data: localUser?.userInfo, token: localUser?.token }
    } else {
        const loginUser = sessionStorage.getItem(SESSION_USER_KEY);
        const sessionUser = loginUser ? JSON.parse(loginUser) : null;
        return { data: sessionUser?.userInfo, token: sessionUser?.token }
    }
}

/** 登录接口 */
export async function userLogin(body: API.LoginParams) {
    return http.post('/User/login', { userNo: body.username, password: body.password }, { headers: { Authorization: 'login' } })
        .then(res => { setLoginUser({ userInfo: res.userInfo, token: res.token }) });
}
/** 自动登录 */
export async function userAutoLogin(userInfo: any) {
    return http.post('/User/login', { userNo: userInfo.data.userid, password: userInfo.data.password, autoLogin: true }, { headers: { Authorization: userInfo.token } })
        .then(res => { console.log(res) });
}

/** 登录退出接口 */
export async function userLoginOut() {
    const localUser: any = getLoginUser();
    console.log("localuser", localUser);
    return http.post('/User/loginout', { userid: localUser.data.userid, token: localUser.token }, { headers: { Authorization: 'login' } })
        .then(res => { console.log(res); clearLoginUser() });
}

